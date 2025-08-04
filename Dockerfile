########
# BASE #
########
# This image serves as base images for the next stages
FROM ubuntu:25.04 AS base
ARG POSTGRES_VERSION=17
ARG POSTGRES_PORT=5432
ARG POSTGRES_PASSWORD=postgres

ENV DOTNET_VERSION=9.0
ENV POSTGRES_VERSION=$POSTGRES_VERSION
ENV POSTGRES_PORT=$POSTGRES_PORT
ENV POSTGRES_PASSWORD=$POSTGRES_PASSWORD
ENV DATABASE_CONNECTION_STRING="Host=127.0.0.1;Port=$POSTGRES_PORT;Username=postgres;Password=$POSTGRES_PASSWORD;Database=postgres"
ENV TargetFramework=net${DOTNET_VERSION}

# Update apt
RUN apt update && apt upgrade -y

# Install make
RUN apt install -y make

# Install PostgreSQL
RUN apt install -y postgresql-common
RUN /usr/share/postgresql-common/pgdg/apt.postgresql.org.sh -y
RUN apt install -y postgresql-$POSTGRES_VERSION

# Install dependencies
RUN apt install -y libglib2.0-dev

# Install .NET SDK
RUN apt install -y dotnet-sdk-$DOTNET_VERSION dotnet-runtime-$DOTNET_VERSION

#########
# BUILD #
#########
# This image is used to build the application
FROM base AS build

## Install build dependencies
RUN apt install -y devscripts build-essential lintian debhelper postgresql-server-dev-all

# Copy application source code
WORKDIR /app
COPY . .

# Replace the content of debian/pgversions with the PostgreSQL version
RUN echo $POSTGRES_VERSION > debian/pgversions

# Build the application
RUN make build-local

#############
# ARTIFACTS #
#############
# This image is used to store the artifacts which are outputted by the build stage.
# It is used to copy the artifacts from the image to the user's local machine when calling `make build`.
FROM scratch AS artifacts

# Copy the built application from the build stage
COPY --from=build /app/debian/packages /

###########
# RUNTIME #
###########
# This image is used to run the application
FROM base AS runtime

# Copy the built application from the build stage
COPY --from=build /app/debian/packages/dotnet-$DOTNET_VERSION-postgresql-$POSTGRES_VERSION-pldotnet_0.99-rc1_amd64.deb /app/debian/packages/

# Initialize PostgreSQL, install the pldotnet extension, and configure the database
RUN pg_ctlcluster $POSTGRES_VERSION main start \
&& dpkg -i /app/debian/packages/dotnet-$DOTNET_VERSION-postgresql-$POSTGRES_VERSION-pldotnet_0.99-rc1_amd64.deb \
&& runuser -u postgres -- psql -c 'CREATE EXTENSION pldotnet;' \
&& runuser -u postgres -- psql -c "ALTER USER postgres WITH PASSWORD '$POSTGRES_PASSWORD';"

# Remove the deb package after installation
RUN rm -rf /app

# Add a message of the day
COPY motd /motd
RUN echo "cat /motd" >> /etc/bash.bashrc

# Start the PostgreSQL service and tail the log file
CMD ["/bin/bash", "-c", "cat /motd && pg_ctlcluster $POSTGRES_VERSION main start && tail -f /var/log/postgresql/postgresql-$POSTGRES_VERSION-main.log"]

FROM runtime AS dev

COPY . /app/pldotnet
WORKDIR /app/pldotnet

COPY motd.dev /motd.dev
RUN cat /motd.dev >> /motd
