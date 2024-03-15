![PL.NET LOGO](./PL-NET_LOGO.png)

# PL/.NET

pl/dotnet adds full support for C# and F# to PostgreSQL.  0.99 is our public beta release.

- In our benchmarks, pl/dotnet is the fastest PL in PostgreSQL
- We support all Procedural Language (PL) operations: functions, procedures, DO, SPI, triggers, records, SRF, OUT/INOUT, table functions, etc
- We natively support 38 out of 46 standard user types, the most of any non-native PL
- We are the only PL using the native database API; our database access(SPI) is fully NPGSQL-compatible
- We support both SQL-embedded code blocks and also loading functions from DLLs
- We have extensive testing, with 1065 unit tests across both C# and F#
- 100% free software under the PostgreSQL license

Our white paper has extensive discussion of all of these items; check it out.

## Usage examples

Here is an example that returns a set of records in C#:

```sql
CREATE OR REPLACE FUNCTION dynamic_record_generator_srf(lim INT8)
RETURNS SETOF record
AS $$
    var upperLimit = lim.HasValue ? lim : System.Int32.MaxValue;
    for(long i=0;i<upperLimit;i++){ yield return new object?[] { i, $"Number is {i}" }; }
$$ LANGUAGE plcsharp;
select * from dynamic_record_generator_srf(10) as record(a int8, b text);
```

The same example in F#:

```sql
CREATE OR REPLACE FUNCTION dynamic_record_generator_srf_fsharp(lim INT8)
RETURNS SETOF record
AS $$
    let upperLimit = if lim.HasValue then lim.Value else int64 System.Int32.MaxValue
    seq { for i in 0L .. upperLimit - 1L do yield [| box i; $"Number is {i}" |] }
$$ LANGUAGE plfsharp;
select * from dynamic_record_generator_srf_fsharp(10) as record(a int8, b text);
```

The `tests/` folder has a complete suite of unit tests in both C# and F#; we
encourage you to consult it for examples of SQL code for your favorite
datatype or SQL feature.

## Major features

We support  all SQL function modes:
- normal procedures and functions
- full support for trigger functions: trigger arguments, old/new row, row rewriting (where allowed), and all the standard trigger information
- set-returning functions, nicely mapped to iterators in C# and sequences in F#
- table functions, as well as functions returning records or sets of records
- full support for IN/OUT/INOUT functions
- full support for trigger functions: 
    + trigger function arguments, 
    + old row and new row,
    + row rewriting (where allowed by SQL), and 
    + all the standard trigger information: Name, When, Level, Event, Table Name, Table Schema, etc

Data types and SPI are described below.

## Data type support

We support 36 PostgreSQL types, with all mapped to their NPGSQL-standard
dotnet types.  The only notable exceptions are multirange, enum, and
struct types, all of which we hope to add in the future.  All datatypes
are nullable, have full array support, and are fully unit-tested for C#
and F#.

| PostgreSQL type  | Dotnet type                      |
|------------------|----------------------------------|
| BitString        | BitArray                         |
| Bool             | bool                             |
| Box              | NpgsqlBox                        |
| Bytea            | byte[]                           |
| Cidr             | (IPAddress Address, int Netmask) |
| Circle           | NpgsqlCircle                     |
| Date             | DateOnly                         |
| DateRange        | DateOnly, DateHandler            |
| Double           | double                           |
| Float            | float                            |
| Inet             | (IPAddress Address, int Netmask) |
| Interval         | NpgsqlInterval                   |
| Int              | int                              |
| IntRange         | int, IntHandler                  |
| Json             | string                           |
| Line             | NpgsqlLine                       |
| LineSegment      | NpgsqlLSeg                       |
| Long             | long                             |
| LongRange        | long, LongHandler                |
| Macaddr8         | PhysicalAddress                  |
| Macaddr          | PhysicalAddress                  |
| Money            | decimal                          |
| Path             | NpgsqlPath                       |
| Point            | NpgsqlPoint                      |
| Polygon          | NpgsqlPolygon                    |
| Record           | object[]                         |
| Short            | short                            |
| String           | string                           |
| Timestamp        | DateTime                         |
| TimestampRange   | DateTime, TimestampHandler       |
| TimestampTz      | DateTime                         |
| TimestampTzRange | DateTime, TimestampTzHandler     |
| Time             | TimeOnly                         |
| TimeTz           | DateTimeOffset                   |
| Uuid             | Guid                             |
| VarBitString     | BitArray                         |

## SPI

Our SPI leverages the NPGSQL client library to provide a native dotnet
implementation, making it maximally compatible with existing client code.
We intercepted the NPGSQL calls at a very low level to replace the
client protocol handling with SPI calls; NPGSQL was otherwise unmodified.
We imported the NPGSQL test suite as stored procedures and are using
it for our testing, giving us high confidence in our compatibility.

Work remains to improve the compatibility and add features.  Our biggest
category of NPGSQL incompatibility is error mapping, because SPI throws
exceptions differently than NPGSQL does.  Such incompatibilities are
minor, and work continues to improve them.

Here are our currently tested SPI operations:

- Data Manipulation Language (DML) Operations
    - Select
    - Insert
    - Update
    - Delete
- Data Definition Language (DDL) Operations
    - Create Table
    - Alter Table
    - Drop Table
    - Truncate
    - Create Index
    - Drop Index
    - Create View
    - Drop View
    - Create Function
    - Call Function
    - Drop Function
    - Create Procedure
    - Call Procedure
    - Drop Procedure
- Transaction Control
    - Begin Transaction
    - Commit
    - Rollback
- Supported Data Types
    - Basic types
    - Array types
    - Record

## What we don't have

We lack support for multirange, enum, and composite/table types.

Our SPI implementation lacks some minor features like sub-transactions.

We fully support Linux and provide dpkg's for Debian and Ubuntu, but we do not yet have packaging on Windows or OS/X.

Our package build system for dpkg is functional but not as tidy as we would like.

We welcome code submissions to address any of these features, and we hope to improve them all in time.

## Getting started

To get started with pldotnet, you will need to install it on your
PostgreSQL server. Detailed installation instructions can be found in the
pldotnet [Wiki pages](https://github.com/Brick-Abode/pldotnet/wiki), along
with examples and information on the supported PostgreSQL data types.

Feel free to open an issue or a discussion topic on our GitHub repository.

