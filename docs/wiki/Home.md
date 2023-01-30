![PL.NET LOGO](https://i.imgur.com/QLGhps2.png)

# 1. What is pl/dotnet?

PL/.NET, also known as *pldotnet* is a powerful and
free add-on module that enhances the functionality of
[PostgreSQL:tm:](https://www.postgresql.org/) by integrating Microsoft's
[.NET framework](https://dotnet.microsoft.com/en-us/). It enables
developers to use C# and F#, two popular programming languages, as
loadable procedural languages, allowing them to create stored procedures
and triggers on the .NET platform. The development of *pldotnet* was led
by the team at Brick Abode and was officially released in January 2023.

With *pldotnet*, developers can write code directly in the database
using the .NET framework's robust application development environment,
providing them with the full benefits of the .NET framework. One of
the major advantages of *pldotnet* is its ability to perform complex
operations in the database, making it more efficient and effective
than other external procedural languages. It is an excellent option for
developers who are looking to extend the capabilities of their PostgreSQL
database while taking advantage of the latest technologies.

# 2. Features

## 2.1 Current support

- The languages C# and F# are both fully supported for functions,
procedures, and `DO` blocks.
- 38 out of 45 PostgreSQL user (non-system) data types are supported,
along with their arrays. All data types are nullable.
- You can enter code directly through the `CREATE FUNCTION` command or
load it from a pre-compiled assembly.
- Our benchmarks show that performance is very good, surpassing all
other external PL implementations.
- We have 820 unit tests that cover all types, plus their arrays and
nulls, in both C# and F#.
- Security is enhanced through the use of a dotnet Assembly Load Context
for isolating code for each function.

## 2.2 Future suppoort

- We plan to add support for missing data types such as multirange,
enumerated, and composite.
- Basic SPI is currently working privately, but we aim to make it 100%
Npgsql-compatible.
- We have prototypes for triggers and plan to fully implement them.
- Set-Returning Functions will be mapped to `IEnumerable<T>` for both C#
and F#.
- Output parameter support will also be added.

# 3. Other documentation sources

## 3.1 Doxygen documentation

To generate the [Doxygen](https://www.doxygen.nl/) documentation,
you need to have the `doxygen` and `make` packages installed on your
machine. You can install them by running the following command:

```bash
sudo apt-get install -y doxygen make
```

Once the packages are installed, navigate to the project directory and
run `make documentation` to generate the Doxygen documentation in the
`docs/html/` directory.

## 3.2 Whitepaper

Our white paper, titled "The pl/dotnet Extension to PostgreSQL, v0.9",
provides detailed information about the project and can be found in the
`docs/` subdirectory within the source code. Alternatively, you can
download the white paper directly from the Brick Abode website.

To generate your own copy, it is required to have `make` and `plantuml`
packages installed on your machine. ou can install them using the command
`sudo apt-get install -y doxygen plantuml`. Next, navigate to `docs/img`
directory in pldotnet repository and run these commands:

```bash
# Build our performance graphs
make pldotnet-performance-comparison.png

# Build our sequence diagram
make pldotnet-sequence-diagram.png
```

To finalize, you can use the [TeX](https://www.latex-project.org/get/)
distribution of your choice to create a copy of the whitepaper.
