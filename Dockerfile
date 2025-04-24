########
# BASE #
########
# This image serves as base images for the next stages
FROM ubuntu:25.04 AS base
ARG DOTNET_VERSION=9.0
ARG POSTGRES_VERSION=17

ENV DOTNET_VERSION=$DOTNET_VERSION
ENV POSTGRES_VERSION=$POSTGRES_VERSION

# Update apt
RUN apt update && apt upgrade -y

# Install .NET SDK
RUN apt install -y dotnet-sdk-$DOTNET_VERSION dotnet-runtime-$DOTNET_VERSION

# Install PostgreSQL
RUN apt install -y postgresql-$POSTGRES_VERSION debhelper postgresql-server-dev-all

# Install dependencies
RUN apt install -y libglib2.0-dev

#########
# BUILD #
#########
# This image is used to build the application
FROM base AS build

## Install builk dependencies
RUN apt install -y devscripts build-essential lintian make

# Copy application source code
WORKDIR /app
COPY . .

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
COPY --from=build /app/debian/packages/postgresql-$POSTGRES_VERSION-pldotnet_0.99-rc1_amd64.deb /app/debian/packages/

# Install the application deb package
RUN dpkg -i /app/debian/packages/postgresql-$POSTGRES_VERSION-pldotnet_0.99-rc1_amd64.deb
# Remove the deb package after installation
RUN rm -rf /app

# Create the Extension on the DB
RUN pg_ctlcluster $POSTGRES_VERSION main start && runuser -u postgres -- psql -c 'CREATE EXTENSION pldotnet;'

# Create a Healthcheck to verify that PostgreSQL is running and the extension is installed
HEALTHCHECK --interval=10s --timeout=5s --start-period=15s --retries=3 \
  CMD runuser -u postgres -- psql -tAc "SELECT extname FROM pg_extension WHERE extname = 'pldotnet';" | grep -q pldotnet || exit 1

# Start the PostgreSQL service and tail the log file
CMD ["/bin/bash", "-c", "pg_ctlcluster $POSTGRES_VERSION main start && tail -f /var/log/postgresql/postgresql-$POSTGRES_VERSION-main.log"]
