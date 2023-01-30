# 1. Quick Install

This guide provides quick instructions for installing pldotnet
and its dependencies. You can find more detailed instructions on
how to install pldotnet and run verification tests in the [GitHub
wiki](https://github.com/Brick-Abode/pldotnet/wiki/).

# 2. Requirements

Before installing pldotnet, make sure that you have the following software
installed on your system:

- [PostgreSQL](https://www.postgresql.org/) 10 or greater
  - Refer to the [PostgreSQL download
  page](https://www.postgresql.org/download/) for instructions.
- [.NET](https://learn.microsoft.com/en-us/dotnet/) 6.0 or greater
  - Refer to [Microsoft installation
  page](https://learn.microsoft.com/en-us/dotnet/core/install/) for
  instructions.

*pldotnet* also requires `libglib2.0` and `make`. You can install them
using the following command:

```bash
sudo apt install -y libglib2.0 make
```

# 3. Installing *pldotnet*

1- Download the Debian package for pldotnet from the Brick Abode website,
selecting the package that corresponds to your version of PostgreSQL.

2- Install the package using the following command: `sudo dpkg -i
postgres-*-pldotnet_0.9-1_amd64.deb`

# 4. Building Debian packages

To build Debian packages for pldotnet using Docker, you need to have
`docker` and `docker-compose` installed on your system. Refer to
the [Docker documentation]((https://docs.docker.com/desktop/)) for
installation instructions.

Before building the packages, specify the version of PostgreSQL you are
using in the `pldotnet/debian/pgversions` file.

To build the pldotnet packages, follow these steps:

```bash
# Clone the pldotnet repository:
git clone https://github.com/Brick-Abode/pldotnet.git

# Change into the repository directory:
cd pldotnet

# Create a docker container to build the packages
docker-compose up pldotnet-build
```

The packages will be stored in the `pldotnet/debian/packages` directory.

If you want to try out pldotnet before installing it on your system,
you can use a Docker container by running the following commands:

```bash
# Create a docker container and open the bash terminal
docker-compose run --rm pldotnet-build bash

# Install pldotnet inside the container
dpkg -i debian/packages/postgres-15-pldotnet_0.9-1_amd64.deb
```

## 4.1 Debian packages for ARM processors

To create pldotnet packages for installation on ARM processors,
specify the version of PostgreSQL you are using in the
`pldotnet/debian/pgversions-arm` file. Then, run the following command
from the pldotnet directory:

```bash
# Create a docker container to build the ARM packages
docker-compose up pldotnet-build-arm
```

After building the packages, they will be stored in the
`pldotnet/debian/packages` directory. If you want to install them in a
clean container, you can use the following instructions:

```bash
# Create a docker container and open the bash terminal
docker-compose run --rm pldotnet-build-arm bash

# Install pldotnet inside the container
dpkg -i debian/packages/postgresql-15-pldotnet_0.9-1_arm64.deb
```

# 5. Installing the built packages

To install the pldotnet packages that you have built, use the `dpkg -i`
command and specify the path to the package file:

```bash
dpkg -i path/to/package.deb
```
