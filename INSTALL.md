# 📦 pldotnet Installation Guide

This guide provides concise yet complete instructions for installing **pldotnet** and its dependencies. For advanced configurations and full troubleshooting, refer to our [GitHub Wiki](https://github.com/Brick-Abode/pldotnet/wiki/).

---

## 1. Docker Build

This section will walk you through the process of building and running PL/.NET using Docker. This is the recommended method for most users, as it simplifies the installation process and ensures a consistent environment.
However, if you prefer to build locally, refer to [Section 2](#2-manual-build-debian-based-linux) for manual installation instructions.

### 1.1. Prerequisites

Ensure your system includes the following dependencies **before proceeding**:

- **Docker**
  - 👉 [Install Docker](https://docs.docker.com/desktop/)
- **Docker Compose** (if not included with Docker Desktop)
  - 👉 [Install Docker Compose](https://docs.docker.com/compose/install/)
- Clone the repository:

```bash
git clone https://github.com/Brick-Abode/pldotnet.git
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

### 1.3. Run PostgreSQL with PL/.NET

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

First start the PostgreSQL container (as explained on the previous step) using the following command:

```bash
docker compose up
```

Then run the tests using the following command:

```bash
make test-docker
```

This command will run the tests inside the PostgreSQL container. The results will be displayed in the terminal.
The results will also be displayed in the `automated_test_results/` directory, where you can find the detailed logs of the tests.

---

## 2. Manual Build and Installation (Debian-based Linux)

This section provides instructions for manually building and installing PL/.NET on Debian-based Linux distributions. This method is recommended for advanced users who want more control over the installation process or need to customize the build.

### 2.1. Prerequisites

- **PostgreSQL** 11 or higher
  - 👉 [Install PostgreSQL](https://www.postgresql.org/download/)
- **.NET SDK** 9.0 or higher
  - 👉 [Install .NET SDK](https://learn.microsoft.com/en-us/dotnet/core/install/)
- **System Packages** for Debian-based Linux distributions:

```bash
sudo apt install -y libglib2.0 make
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

To build the packages, you will need additional dependencies. Install them using the following command:

```bash
sudo apt install -y devscripts build-essential lintian debhelper postgresql-server-dev-all postgresql-common
```

Then, run the following command to build the package:

```bash
make build-local
```

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

### 2.5. Using PL/.NET

You can now use PL/.NET to create and run .NET functions in PostgreSQL. For example, to create a simple function:

```sql
CREATE FUNCTION hello_world() RETURNS text
AS $$
return "Hello, World!";
$$ LANGUAGE plcsharp STRICT;
```

Then you can call the function:

```sql
SELECT hello_world();
```

This should return `Hello, World!`.
Done, you can now use PL/.NET to create and run .NET functions in PostgreSQL!
