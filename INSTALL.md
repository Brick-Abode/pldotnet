# 📦 pldotnet Installation Guide

This guide provides concise yet complete instructions for installing **PL/.NET** (or simply **pldotnet**) and its dependencies. For advanced configurations and full troubleshooting, refer to our [GitHub Wiki](https://github.com/Brick-Abode/pldotnet/wiki/).

---

## 1. Docker Build

This section will walk you through the process of building and running PL/.NET using Docker. This is the recommended method for most users, as it simplifies the installation process and ensures a consistent environment.
However, if you prefer to build locally, refer to [Section 2](#2-manual-build-and-installation-debian-based-linux) for manual installation instructions.

### 1.1. Prerequisites

Ensure your system includes the following dependencies **before proceeding**:

- **Docker**
  - 👉 [Install Docker](https://docs.docker.com/desktop/)
- **Docker Compose** (if not included with Docker Desktop)
  - 👉 [Install Docker Compose](https://docs.docker.com/compose/install/)
- **Make**
  - 👉 [Install Make](https://www.gnu.org/software/make/)
- **Git**
  - 👉 [Install Git](https://git-scm.com/book/en/v2/Getting-Started-Installing-Git)
- Clone the repository and initialize the submodules:

```bash
git clone https://github.com/Brick-Abode/pldotnet.git
git submodule update --init --recursive
```

- Create a `.env` file in the root directory of the repository. An example file `.env.example` is provided. You can copy it and modify it as needed.

### 1.2. Building Debian Packages (optional)

If you want to build the `.deb` packages manually using Docker, first navigate to the repository root directory:

```bash
cd pldotnet
```

Then, run the following command to build the package:

```bash
make build-docker
```

This command will build the `.deb` package using Docker and place it in the `debian/packages/` directory.
Remember to change the `.env` file to match the PostgreSQL version you want to build PL/.NET for.

To install it in your local PostgreSQL instance, you can use the following command:

```bash
sudo dpkg -i debian/packages/postgresql-*-pldotnet_0.99-rc1_amd64.deb
```

The instance must be running and the PostgreSQL version must match the one specified in the `.env` file during build.
It also requires .NET SDK 9.0 or higher to be installed on your system.
Your main application does not need to have .NET SDK 9.0 installed, only the PostgreSQL instance where PL/.NET is installed.

### 1.3. Run PostgreSQL with PL/.NET in a docker container

First navigate to the root directory of the repository:

```bash
cd pldotnet
```

Then to run PostgreSQL with PL/.NET, execute the following command:

```bash
docker compose up
```

This command will start a container (name `pldotnet-runtime`) with PostgreSQL + PL/.NET running on the specified port in the `.env` file.

That's it. You now have a PostgreSQL container running with PL/.NET installed and ready to use.

### 1.4. Running Tests

First start the PostgreSQL dev container using the following command:

```bash
make dev
```

Then run the tests in a separate terminal using the following command:

```bash
make test-docker
```

This command will run the tests inside the PostgreSQL container. The results will be displayed in the terminal.
The results will also be displayed in the `automated_test_results/` directory, where you can find the detailed logs of the tests.

If you would like to run only C# or F# tests, you can use one of the following commands:

```bash
make XUNIT_FILTER="CSharp" test-docker
make XUNIT_FILTER="FSharp" test-docker
```

---

## 2. Manual Build and Installation (Debian-based Linux)

This section provides instructions for manually building and installing PL/.NET on Debian-based Linux distributions. This method is recommended for advanced users who want more control over the installation process or need to customize the build.

### 2.1. Prerequisites

- **PostgreSQL** 11 or higher
  - 👉 [Install PostgreSQL](https://www.postgresql.org/download/); OR
- **.NET SDK and Runtime** 9.0 or higher
  - 👉 [Install .NET SDK and Runtime](https://learn.microsoft.com/en-us/dotnet/core/install/)
  - 👉 `sudo apt install -y dotnet-sdk-9.0 dotnet-runtime-9.0` - you need to install both sdk and runtime
- **System Packages** for Debian-based Linux distributions:

```bash
sudo apt install -y libglib2.0 # or libglib2.0-dev depending on your distribution
sudo apt install -y make postgresql-common devscripts build-essential lintian debhelper postgresql-server-dev-all
```

- Clone the repository:

```bash
git clone https://github.com/Brick-Abode/pldotnet.git
```

### 2.2. Building Debian Packages (optional)

Navigate to the root directory of the repository:

```bash
cd pldotnet
```

Modify the `debian/pgversions` file to specify the PostgreSQL versions you want to build for. Each version should be on a new line.
For example:

```plaintext
16
17
```

Leave an empty line at the end of the file.

Then, run the following command to build the package:

```bash
make build-local
```

If the build process fails for any reason, try again with `sudo`. Sometimes permissions can cause issues during the build process.

The packages will be generated in the `debian/packages/` directory.

### 2.3. Install the Package

First, build the package using the command mentioned above or download the pre-built package from the [releases page](https://github.com/Brick-Abode/pldotnet/releases).

To install the PL/.NET `.deb` package, use the following command:

```bash
sudo dpkg -i path/to/postgresql-*-pldotnet_0.99-rc1_amd64.deb
```

Be sure to replace the wildcard with your actual downloaded or generated filename.

### 2.4. Creating the Extension

After installation, you will need to enable the extension inside PostgreSQL.

First, connect to PostgreSQL using the `psql` command:

```bash
psql
```

Then, create the extension using the following command:

```sql
CREATE EXTENSION pldotnet;
```

Confirm it's active:

```sql
SELECT * FROM pg_extension WHERE extname = 'pldotnet';
```

Then restart PostgreSQL to ensure the extension is loaded correctly and the PL/.NET configuration is applied:

```bash
sudo systemctl restart postgresql
```

### 2.5. Using PL/.NET

You can now use PL/.NET to create and run .NET functions in PostgreSQL. For example, to create a simple function:

```sql
CREATE FUNCTION hello_world() RETURNS text AS $$
return "Hello, World!";
$$ LANGUAGE plcsharp STRICT;
```

Then you can call the function:

```sql
SELECT hello_world();
```

This should return `Hello, World!`.
Done, you can now use PL/.NET to create and run .NET functions in PostgreSQL!
