# 📦 pldotnet Installation Guide

This guide provides concise yet complete instructions for installing **pldotnet** and its dependencies. For advanced configurations and full troubleshooting, refer to our [GitHub Wiki](https://github.com/Brick-Abode/pldotnet/wiki/).

---

## 1. Prerequisites

Ensure your system includes the following dependencies **before proceeding**:

### Required Software

- **PostgreSQL** 10 or higher
  👉 [Download PostgreSQL](https://www.postgresql.org/download/)

- **.NET SDK** 9.0 or higher
  👉 [Install .NET SDK](https://learn.microsoft.com/en-us/dotnet/core/install/)

- **System Packages** for Debian-based Linux distributions:

```bash
sudo apt install -y libglib2.0 make
```

---

## 2. Installing _pldotnet_

### 2.1. Download the Package

Visit the [Releases Page](https://github.com/Brick-Abode/pldotnet/releases) and download the `.deb` file that matches your installed version of PostgreSQL.

### 2.2. Install the Package

```bash
sudo dpkg -i postgresql-*-pldotnet_0.99-rc1_amd64.deb
```

Be sure to replace the wildcard with your actual downloaded filename.

---

## 3. Building Debian Packages (Optional)

If you want to build the `.deb` packages manually using Docker:

### 3.1. Requirements

- `docker`
- `docker-compose`

👉 Follow the [Docker installation guide](https://docs.docker.com/desktop/) if you haven't installed it yet.

### 3.2. Setup and Build

```bash
# Clone the repository
git clone https://github.com/Brick-Abode/pldotnet.git
cd pldotnet

# Initialize submodules
git submodule update --init --recursive

# Specify PostgreSQL versions to target (add one per line)
echo "15" > debian/pgversions

# Build the package
docker-compose up pldotnet-build
```

The generated `.deb` files will be available in:

```bash
debian/packages/
```

### 3.3. Try it in Docker (Sandboxed Testing)

```bash
docker-compose run --rm pldotnet-build bash
# Then inside the container:
dpkg -i debian/packages/postgres-15-pldotnet_0.9-1_amd64.deb
```

---

## 4. ARM Architecture Builds

To build packages compatible with ARM systems:

1. Specify PostgreSQL versions in `debian/pgversions-arm`.
2. Execute the build:

```bash
docker-compose up pldotnet-build-arm
```

3. Test and install in an isolated container:

```bash
docker-compose run --rm pldotnet-build-arm bash
# Inside the container:
dpkg -i debian/packages/postgresql-17-pldotnet_0.99-rc1_amd64.deb
```

---

## 5. Installing Built Packages

Use `dpkg` to install your local `.deb` build:

```bash
dpkg -i path/to/package.deb
```

---

## 6. Creating the Extension

After installation, enable the extension inside PostgreSQL:

```sql
CREATE EXTENSION pldotnet;
```

Confirm it's active:

```sql
SELECT * FROM pg_extension WHERE extname = 'pldotnet';
```

---

## 7. Running Tests

pldotnet includes two test formats:

- **xUnit Tests**: Validate all C# and F# bindings.
- **SQL Tests**: Direct PostgreSQL SQL-based assertions.

### 7.1. Environment Setup

Set your PostgreSQL connection string for test execution:

```bash
export DATABASE_CONNECTION_STRING="Host=127.0.0.1;Port=5432;Username=postgres;Password=postgres;Database=postgres"
```

You can also prefix commands:

```bash
DATABASE_CONNECTION_STRING="..." make pldotnet-tests
```

### 7.2. Running xUnit Tests

#### 7.2.1. Commands

- All tests:

```bash
make pldotnet-tests
```

- C# only:

```bash
make csharp-tests
```

- F# only:

```bash
make fsharp-tests
```

#### 7.2.2. Understanding Results

- ✅ All pass:

```bash
Passed!  - Failed:     0, Passed:   881, Skipped:     0, Total:   881
```

- ❌ Some fail:

```bash
Failed!  - Failed:    15, Passed:   866, Skipped:     0, Total:   881
```

Failures are categorized:

- **Compilation Error**: Function fails to compile
- **Runtime Error**: Function throws at execution
- **Assertion Error**: Output differs from expected

Each failure includes the failing test name, expected value, actual result, and stack trace.

### 7.3. Running SQL Tests

#### 7.3.1. Commands

- All:

```bash
make pldotnet-tests-sql
```

- C# only:

```bash
make csharp-tests-sql
```

- F# only:

```bash
make fsharp-tests-sql
```

#### 7.3.2. Understanding Results

SQL test output includes:

- A **table** listing features, test names, and pass/fail status
- A **summary** count of passed/failed cases

```
| Feature       | Test Name             | Result |
|---------------|-----------------------|--------|
| c#-bit        | modifybit1            | t      |
| c#-varbit     | concatenatevarbit2    | t      |
...

| Result | Count |
|--------|-------|
| t      | 996   |
| f      | 1     |
```

Detailed logs are saved in:

```bash
automated_test_results/
```

---

For advanced setup and full documentation, visit the [official wiki](https://github.com/Brick-Abode/pldotnet/wiki/).
