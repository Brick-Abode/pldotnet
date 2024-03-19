# pl/dotnet: Frequently Asked Questions

[TOC]

## Introduction

### What is pl/dotnet?

The pl/dotnet project extends PostgreSQL to support functions, stored procedures and `DO` blocks for the dotnet platform, including both C# and F#.

### Which version of pl/dotnet does this FAQ cover?

This FAQ is for the version 0.99 beta release of pl/dotnet, released in March of 2024.

### What languages are supported?

pl/dotnet supports the creation of stored procedures, functions, triggers, and `DO` blocks in both C# and F#, which can be used inside of PostgreSQL as the `plcsharp` and `plfsharp` languages, respectively.

### Can I see an example?

Of course.  Here is a basic example:

```sql
CREATE OR REPLACE FUNCTION IntegerTest(a INTEGER) RETURNS INTEGER AS $$
return a+1;
$$ LANGUAGE plcsharp STRICT;
```

### Who is responsible for pl/dotnet?

pl/dotnet was built by the fine people at Brick Abode. We love ASP.NET development and PostgreSQL, and we build pl/dotnet to be the stored procedure environment that we wanted to have for working on our clients' projects. Feel free to talk to us about your ASP.NET development needs. Find out more at [https://www.brickabode.com](https://www.brickabode.com).

### How can I ask questions or request features?

- You can open a discussion on [our github discussions](https://github.com/Brick-Abode/pldotnet/discussions).
- You can submit a pull request [via github](https://github.com/Brick-Abode/pldotnet/pulls).
- You can email us at `pldotnet@brickabode.com`.

## Project information

### Where can I get the source code for pl/dotnet?

The official repository for pl/dotnet is [https://github.com/Brick-Abode/pldotnet/](https://github.com/Brick-Abode/pldotnet/).

### Where can I read the documentation for pl/dotnet?

Our project wiki is at [https://github.com/Brick-Abode/pldotnet/wiki](https://github.com/Brick-Abode/pldotnet/wiki).

### Is there a white paper explaining the project?

Yes, you can find the [whitepaper on our wiki](https://github.com/Brick-Abode/pldotnet/wiki/pldotnet:-White-Paper).

### Is pl/dotnet fast?

In our benchmarks, pl/csharp is the fastest procedural language in PostgreSQL.  pl/fsharp is second-fastest, less than 1% slower than pl/csharp.

These benchmarks are somewhat arbitrary, merely having been designed for our own needs, but they were not cherry-picked or tuned; we built the tests first and only benchmarked them afterwards. We wish for the benchmarks to be fair and welcome submissions to improve them.

More details of the benchmarks can be found in our [whitepaper](https://github.com/Brick-Abode/pldotnet/wiki/pldotnet:-White-Paper).

### Does pl/dotnet support a lot of types?

We were able to achieve native representation for 38 types, plus their arrays, the widest range of types of any external procedural language in PostgreSQL.

Here is the complete list of type mappings:

| PostgreSQL | pl/dotnet                                   |
|------------|---------------------------------------------|
| BIT        | `BitArray`                                  |
| BOOL       | `bool`                                      |
| BOX        | `NpgsqlBox`                                 |
| BPCHAR     | `string`                                    |
| BYTEA      | `byte[]`                                    |
| CIDR       | `IPAddress Address, int Netmask`            |
| CIRCLE     | `NpgsqlCircle`                              |
| DATE       | `DateOnly`                                  |
| FLOAT4     | `float`                                     |
| FLOAT8     | `double`                                    |
| INET       | `IPAddress Address, int Netmask`            |
| INT2       | `short`                                     |
| INT4       | `int`                                       |
| INT8       | `long`                                      |
| INTERVAL   | `NpgsqlInterval`                            |
| JSON       | `string`                                    |
| LINE       | `NpgsqlLine`                                |
| LSEG       | `NpgsqlLSeg`                                |
| MACADDR    | `PhysicalAddress`                           |
| MACADDR8   | `PhysicalAddress`                           |
| MONEY      | `decimal`                                   |
| PATH       | `NpgsqlPath`                                |
| POINT      | `NpgsqlPoint`                               |
| POLYGON    | `NpgsqlPolygon`                             |
| TEXT       | `string`                                    |
| TIME       | `TimeOnly`                                  |
| TIMESTAMP  | `DateTime`                                  |
| TIMESTAMPTZ| `DateTime`                                  |
| TIMETZ     | `DateTimeOffset`                            |
| UUID       | `Guid`                                      |
| VARBIT     | `BitArray`                                  |
| VARCHAR    | `string`                                    |
| XML        | `string`                                    |
| DATERANGE  | `NpgsqlRange<DateOnly>`                     |
| INT4RANGE  | `NpgsqlRange<int>`                          |
| INT8RANGE  | `NpgsqlRange<long>`                         |
| TSRANGE    | `NpgsqlRange<DateTime>`                     |
| TSTZRANGE  | `NpgsqlRange<DateTime>`                     |

### How does pl/dotnet make use of Npgsql?

Npgsql is an open source ADO.NET Data Provider for PostgreSQL. You can find out more at their website, [https://www.npgsql.org/](https://www.npgsql.org/).

pl/dotnet embraced Npgsql as our PostgreSQL compatibility layer to provide maximum transparency and ease in migrating code between a database client and the database server.

pl/dotnet uses Npgsql to map PostgreSQL data types to .NET data types.  Our implementation of SPI (database access within the stored procedure) is also based on Npgsql.

pl/dotnet incorporates Npgsql with minor, low-level modifications.  We make use of Npgsql's own regression test suite and are working towards perfect compatibility with it.

We are very grateful to the authors of Npgsql, as their work forms an integral piece of our own project.

### How compatible is my Npgsql client code with pl/dotnet?

We are 100% compatible with all supported Npgsql types.

Database access within stored procedures, via SPI, is fully Npgsql-compatible in how you call them.  There are some minor differences, such as raising different exceptions in certain error conditions.

We still have some minor Npgsql features, like subtransactions, which have not been mapped yet; it is our intention to reach full support in time.

### How complete is your F# support compared to C#?

Very complete.  We have complete unit testing for all supported features in both C# and F#.

Thanks to the magic of .NET, everything that works in C# *should* work in F#, and in our experience it does.

### How good is the code quality is pl/dotnet?

- We have complete unit testing in C# and F# for all features and supported data types, their arrays, and nulls for that type.
- Cpplint is a static code checker for C and C++.  pl/dotnet is clean under its checks.
- StyleCop is a static code analysis tool for C#.  pl/dotnet is clean under its checks.
- SonarLint is a code quality and security static analysis tool with almost 5000 rules.  pl/dotnet is clean under its checks.

### Is the code well commented?

All C and C# code is commented using Doxygen, and we encourage you to consult the generated Doxygen documentation.

### What versions of .NET are supported?

We currently support .NET version 6 and hope to add support for other versions soon.

### What versions of PostgreSQL are supported?

We currently support PostgreSQL versions 10, 11, 12, 13, 14, and 15.

## Using pl/dotnet

### How do I install pl/dotnet?

See our `INSTALL` file at the top level of our source code.

You also can download our debian packages or use our Docker image on Dockerhub.

### How do I use pl/dotnet?

After you install it, you can use the normal Postgresql `CREATE FUNCTION`, `CREATE PROCEDURE`, `CREATE TRIGGER`, or `DO` syntax. The language is `plcsharp` or `plfsharp` depending on your choice of C# or F#.

### How do I see the C#/F# code generated by pl/dotnet?

By default pl/dotnet saves the generated source code in `/tmp/PlDotNET/GeneratedCodes`.

You can control this behavior by modifying pl/dotnet. You can set the `SaveSourceCode` variable in `dotnet_src/Engine.cs` and specify the location by updating the `PathToSaveSourceCode` variable in the same file. This path needs to have mode of `0700`. Alternatively, you can set the `PrintSourceCode` variable in `dotnet_src/Engine.cs` to `true` to print the generated code to the console.  Note that after making changes to the `dotnet_src/Engine.cs` file, you will need to rebuild and reinstall the package.

## NULL and STRICT

### How are NULLs handled?

Null is mapped to `null` in C# and F#.  By default, all types in pl/dotnet are nullable.  You will see that as `T?` in C# and `Nullable<T>` in F#.

### Why does the type mapping change depending on whether my function is `STRICT`?

A normal PostgreSQL function can be passed NULL as an argument. In order to handle this, pl/dotnet maps such types to their optional C# or F# type. For example, a PostgreSQL `INT4` is mapped to a `int?` in C#; this allows NULL to be handled in the standard way in the user function.

Some developers find such handling tedious and do not wish to be passed NULL. Fortunately, PostgreSQL supports this; you can
declare your function to be `STRICT` (or `RETURNS NULL ON NULL INPUT`, which is an alias for `STRICT`.)

Functions declared in this way immediately return NULL if called with NULL input (without actually calling the function), so the user function will never see a NULL value. For this reason, the type mapping is changed; `INT4` arguments are now mapped to `int` instead of `int?` in C#, or `Nullable<int>` in F#,  saving the developer the trouble of handling null values.

We could have always mapped types to `T?`, but we chose to do it this way for a reason; if you have existing code which takes `T` instead of `T?` arguments, then this lets you map them cleanly and appropriately to SQL.

## Arrays

### How are arrays handled?

Because PostgreSQL does not enforce dimensioning on its arrays, NPGSQL uses [`Array`](https://learn.microsoft.com/en-us/dotnet/api/system.array?view=net-8.0), a very generic, boxed version of arrays in its API. You can read more about NPGSQL's handling [here](https://www.npgsql.org/efcore/mapping/array.html). As always, pl/dotnet supports the NPGSQL mapping.

### What is the performance problem with arrays?

Because arrays are boxed, and because you don't know the dimensionality of an array, they behave more like trees than arrays, and navigating them is tedious and slow.

We are forced to do this, because of our Npgsql compatibility.  We have ideas for how this can be improved in future versions; they will be incompatible and will require developers to explicitly opt into them.  We have found some optimizations in our array handling in certain cases and hope to continue improving it.

## Procedures, Functions, and `DO` blocks

### Does pl/dotnet support procedures?

It does.  A reminder (from the PostgreSQL manual)[https://www.postgresql.org/docs/current/xproc.html] about the central elements of procedures:

```quote
- Procedures do not return a function value; hence CREATE PROCEDURE lacks a RETURNS clause. However, procedures can instead return data to their callers via output parameters.
- While a function is called as part of a query or DML command, a procedure is called in isolation using the CALL command.
- A procedure can commit or roll back transactions during its execution (then automatically beginning a new transaction), so long as the invoking CALL command is not part of an explicit transaction block. A function cannot do that.
- Certain function attributes, such as strictness, don't apply to procedures. Those attributes control how the function is used in a query, which isn't relevant to procedures.
```

Here is an example procedure using SPI to modify the database:

```sql
user=# CREATE OR REPLACE PROCEDURE SPITransactionTestCommitFirst() AS $$
    NpgsqlCommand command;
    NpgsqlDataReader reader;
    int i;

    var conn = new NpgsqlConnection();
    conn.Open();
    var batch = conn.CreateBatch();
    batch.BatchCommands.Add(new ("DROP TABLE IF EXISTS TRANSACTION_TEST;"));
    batch.BatchCommands.Add(new ("CREATE TABLE IF NOT EXISTS TRANSACTION_TEST (a INTEGER, b text);"));
    batch.ExecuteReader();
    var transaction = conn.BeginTransaction();
    for(i = 0; i < 10; i++)
    {
        command = new NpgsqlCommand($"INSERT INTO TRANSACTION_TEST (a) VALUES ({i})", conn);
        reader = command.ExecuteReader();
        if (i % 2 == 0)
        {
            transaction.Commit();
        }
        else
        {
            transaction.Rollback();
        }
    }
    command = new NpgsqlCommand("SELECT * FROM TRANSACTION_TEST", conn);
    reader = command.ExecuteReader();
    i = 0;
    while(reader.Read())
    {
        int value = reader.GetInt32(0);
        Elog.Info($"Row[{i++}] = {value}");
    }
$$ LANGUAGE plcsharp;
CREATE PROCEDURE

user=# CALL spitransactiontestcommitfirst();
INFO:  Row[0] = 0
INFO:  Row[1] = 2
INFO:  Row[2] = 4
INFO:  Row[3] = 6
INFO:  Row[4] = 8
CALL
```

The similar plfsharp procedure can be found in the `SPITestTruncateAndAlterFSharp` procedure in `tests/fsharp/testfsspi.sql`.

### Does pl/dotnet support functions?

It does.  Functions in SQL are "pure" in the computational sense: they are expected to be transformations from inputs to outputs, with no externally-visible side effects.

Here is an example in plcsharp:

```sql
user=# CREATE OR REPLACE FUNCTION make_pi_n(n int) RETURNS double precision AS $$
double sum = 0.0;
for(int i=0;i<n;i++){ sum += ((i%2)==0?1.0:-1.0)/(2*i+1); }
return 4.0 * sum;
$$
LANGUAGE plcsharp;
CREATE FUNCTION

user=# select make_pi_n(1000);
     make_pi_n
-------------------
 3.140592653839794
(1 row)
```

Here is the same example in plfsharp:

```sql
user=# CREATE OR REPLACE FUNCTION make_pi_n_fs(a int) RETURNS float8 AS $$
    let mutable sum : float = 0.0
    for i = 0 to (a-1) do
        sum <- sum + ((if i % 2 = 0 then 1.0 else -1.0)/ float(2.0 * float(i) + 1.0))
    Nullable<float>(sum * 4.0);
$$ LANGUAGE plfsharp STRICT;
CREATE FUNCTION

user=# select make_pi_n_fs(1000);
   make_pi_n_fs
-------------------
 3.140592653839794
(1 row)
```

### Does pl/dotnet support `DO` blocks?

It does.  `DO` blocks are treated as though it were the body of a function with no parameters, returning void.  They are parsed and executed a single time.

Here are some examples:

```sql
user=# do $$
    string arabic = "هل تتكلم العربية؟";
    Elog.Info($"Do you speak Arabic? => {arabic}");

    string chinese = "你会说中文吗？";
    Elog.Info($"Do you speak Chinese? => {chinese}");

    string japanese = "あなたは日本語を話しますか？";
    Elog.Info($"Do you speak Japanese? => {japanese}");

    string portuguese = "Você fala português?";
    Elog.Info($"Do you speak Portuguese? => {portuguese}");

    string russian = "а ты говоришь по русски?";
    Elog.Info($"Do you speak Russian? => {russian}");
$$ language plcsharp;
INFO:  Do you speak Arabic? => هل تتكلم العربية؟
INFO:  Do you speak Chinese? => 你会说中文吗？
INFO:  Do you speak Japanese? => あなたは日本語を話しますか？
INFO:  Do you speak Portuguese? => Você fala português?
INFO:  Do you speak Russian? => а ты говоришь по русски?
DO

user=# do $$
    let c = 1450 + 275
    Elog.Info("c = " + c.ToString())
$$ language plfsharp;
INFO:  c = 1725
DO
```

## INOUT and OUT  parameters

### Does pl/dotnet support INOUT or OUT parameters?

Yes, pl/dotnet supports INOUT and OUT parameters for both plcsharp and plfsharp.

### How does pl/csharp support INOUT and OUT parameters?

For plcsharp, OUT parameters are mapped to `out`, and INOUT parameters are mapped to `ref`.

Here is a C# example:

```sql
CREATE OR REPLACE FUNCTION inout_multiarg_1(IN argument_0 INT, INOUT argument_1 INT, IN argument_2 INT, OUT argument_3 INT, OUT argument_4 INT, INOUT argument_5 INT, IN argument_6 INT, OUT argument_7 INT) AS $$
    if(argument_0 != 0){ throw new SystemException($"Failed assertion: argument_0 = {argument_0}");}
    if(argument_1 != 1){ throw new SystemException($"Failed assertion: argument_1 = {argument_1}");}
    argument_1 = 2;
    if(argument_2 != 2){ throw new SystemException($"Failed assertion: argument_2 = {argument_2}");}
    argument_3 = 4;
    argument_4 = 5;
    if(argument_5 != null){ throw new SystemException($"Failed assertion: argument_5 = {argument_5}");}
    argument_5 = 6;
    if(argument_6 != 6){ throw new SystemException($"Failed assertion: argument_6 = {argument_6}");}
    argument_7 = null;
$$ LANGUAGE plcsharp;
```

Here is the generated code:

```csharp
        public static void inout_multiarg_1(int? argument_0, ref int? argument_1, int? argument_2, out int? argument_3, out int? argument_4, ref int? argument_5, int? argument_6, out int? argument_7)
        {
#line 1
            if (argument_0 != 0)
            {
                throw new SystemException($"Failed assertion: argument_0 = {argument_0}");
            }

            if (argument_1 != 1)
            {
                throw new SystemException($"Failed assertion: argument_1 = {argument_1}");
            }

            argument_1 = 2;
            if (argument_2 != 2)
            {
                throw new SystemException($"Failed assertion: argument_2 = {argument_2}");
            }

            argument_3 = 4;
            argument_4 = 5;
            if (argument_5 != null)
            {
                throw new SystemException($"Failed assertion: argument_5 = {argument_5}");
            }

            argument_5 = 6;
            if (argument_6 != 6)
            {
                throw new SystemException($"Failed assertion: argument_6 = {argument_6}");
            }

            argument_7 = null;
        }
    }
```

### How does pl/fsharp support INOUT and OUT parameters?

For plfsharp, IN and INOUT parameters are passed as arguments, and the function returns a tuple containing all INOUT and OUT parameters, in keeping with the functional style.

Here is an F# example:

```sql
user=# CREATE OR REPLACE FUNCTION inout_multiarg_1_fs(IN a0 INT, INOUT a1 INT, IN a2 INT, OUT a3 INT, OUT a4 INT, INOUT a5 INT, IN a6 INT, OUT a7 INT) AS $$
    if a0.HasValue && a0.Value <> 0 then
        raise <| SystemException("Failed assertion: a0")
    if a1.HasValue && a1.Value <> 1 then
        raise <| SystemException("Failed assertion: a1")
    if a2.HasValue && a2.Value <> 2 then
        raise <| SystemException("Failed assertion: a2")
    if a5.HasValue then
        raise <| SystemException("Failed assertion: a5")
    if a6.HasValue && a6.Value <> 6 then
        raise <| SystemException("Failed assertion: a6")

    (Nullable(2), Nullable(4), Nullable(5), Nullable(6), Nullable())
$$ LANGUAGE plfsharp;
CREATE FUNCTION

user=# select inout_multiarg_1_fs(0, 1, 2, null, 6);
 inout_multiarg_1_fs
---------------------
 (2,4,5,6,)
(1 row)
```

Here is the generated code:

```fsharp
    static member inout_multiarg_1_fs (a0: Nullable<int>) (a1: Nullable<int>) (a2: Nullable<int>) (a5: Nullable<int>) (a6: Nullable<int>) : Nullable<int> * Nullable<int> * Nullable<int> * Nullable<int> * Nullable<int> =
            if a0.HasValue && a0.Value <> 0 then
                raise <| SystemException("Failed assertion: a0")
            if a1.HasValue && a1.Value <> 1 then
                raise <| SystemException("Failed assertion: a1")
            if a2.HasValue && a2.Value <> 2 then
                raise <| SystemException("Failed assertion: a2")
            if a5.HasValue then
                raise <| SystemException("Failed assertion: a5")
            if a6.HasValue && a6.Value <> 6 then
                raise <| SystemException("Failed assertion: a6")

            (Nullable(2), Nullable(4), Nullable(5), Nullable(6), Nullable())
```

## Triggers

### Are triggers supported?

Yes, triggers are fully supported.

### How is trigger data passed?

Trigger functions are called with a `TriggerData` argument which has
all of the normal PostgreSQL trigger data:

```
    public class TriggerData
    {
        // Row-level information for operations
        public object?[] OldRow { get; set; }
        public object?[] NewRow { get; set; }
        // Trigger metadata
        public string TriggerName { get; set; }
        public string TriggerWhen { get; set; }
        public string TriggerLevel { get; set; }
        public string TriggerEvent { get; set; }
        // Table-related details
        public int RelationId { get; set; }
        public string TableName { get; set; }
        public string TableSchema { get; set; }
        // Trigger arguments
        public string[] Arguments { get; set; }
    }
```

These fields have the standard PostgreSQL meaning:

- NewRow (Record): new database row for INSERT/UPDATE operations in row-level triggers. Null in statement-level triggers and for DELETE operations.
- OldRow (Record): old database row for UPDATE/DELETE operations in row-level triggers. Null in statement-level triggers and for INSERT operations.
- TriggerName (string): name of the trigger which fired.
- TriggerWhen (string): "BEFORE", "AFTER", or "INSTEAD OF", depending on the trigger's definition.
- TriggerLevel (string): "ROW" or "STATEMENT", depending on the trigger's definition.
- TriggerEvent (string): operation for which the trigger was fired: INSERT, UPDATE, DELETE, or TRUNCATE.
- RelationId (oid): table that caused the trigger invocation. This is now deprecated, and could disappear in a future release. Use TG_TABLE_NAME instead.
- TableName (string): table that caused the trigger invocation.
- TableSchema (string): schema of the table that caused the trigger invocation.
- Arguments (string[]): arguments from the CREATE TRIGGER statement.

### How do I return from a trigger?

Triggers can return `ReturnMode.Normal`, `ReturnMode.TriggerSkip`,
or `ReturnMode.TriggerModify` as appropriate, under the normal
SQL/PostgreSQL trigger rules.  Look in `tests/csharp/testtrigger.sql`
or `tests/fsharp/testfstrigger.sql`.  for sample usage, or look in
`TriggerData.cs` for the definition of `TriggerData`.

### Where can I learn more about trigger functions?

The PostgreSQL manual does not define
trigger functions abstractly, so (the PL/pgSQL
definition)[https://www.postgresql.org/docs/current/plpgsql-trigger.html]
is a good reference, although basically all Procedural Languages in
PostgreSQL do the same thing, including pl/dotnet.

### Where can I find examples of trigger functions?

They are somewhat complicated, so we refer you to `tests/csharp/testtrigger.sql` and

## SPI

### Is SPI supported?

Yes, pl/dotnet supports the Server Programming Interface (SPI) in both plcsharp and plfsharp, allowing execute SQL commands directly from C# and F# code.
All SPI functionality is exported via the Npgsql-standard APIs.

Sample code can be found in `tests/csharp/testspi.sql` and
`tests/fsharp/testfsspi.sql`.

#### Supported Data Definition Language (DDL) Operations

Here are the DDL operations we have tested:

| C#      | F#      | Operation            | Description                                            |
| ------- | ------- | -------------------- | ------------------------------------------------------ |
| ✅  | ✅  | **Create Table**     | Create new tables in the database.                     |
| ✅  | ✅  | **Alter Table**      | Modify the structure of existing tables.               |
| ✅  | ✅  | **Drop Table**       | Remove tables from the database.                       |
| ✅  | ✅  | **Truncate**         | Quickly remove all rows from a table or set of tables. |
| ✅  | ✅  | **Create Index**     | Create indexes on table columns for faster queries.    |
| ✅  | ✅  | **Drop Index**       | Remove indexes from a table.                           |
| ✅  | ✅  | **Create View**      | Create a view based on a select statement.             |
| ✅  | ✅  | **Drop View**        | Drop a view based on a select statement.               |
| ✅  | ✅  | **Create Function**  | Define a new function.                                 |
| ✅  | ✅  | **Call Function**    | Call an existing function.                             |
| ✅  | ✅  | **Drop Function**    | Drop an existing function.                             |
| ✅  | ✅  | **Create Procedure** | Define a new procedure.                                |
| ✅  | ✅  | **Call Procedure**   | Call a procedure.                                      |
| ✅  | ✅  | **Drop Procedure**   | Drop an existing procedure.                            |

Here is a sample procedure:

```sql
user=# CREATE OR REPLACE FUNCTION SPITestingCompoudParameters() RETURNS INTEGER AS $$
    var sb = new StringBuilder();
    sb.Append("DROP TABLE IF EXISTS SPI_COMPOUD_TESTS;");
    sb.Append("CREATE TABLE IF NOT EXISTS SPI_COMPOUD_TESTS (ID INTEGER);");
    sb.Append("INSERT INTO SPI_COMPOUD_TESTS (ID) VALUES(@a);");
    sb.Append("INSERT INTO SPI_COMPOUD_TESTS (ID) VALUES(@b);");
    sb.Append("INSERT INTO SPI_COMPOUD_TESTS (ID) VALUES(@c);");
    sb.Append("INSERT INTO SPI_COMPOUD_TESTS (ID) VALUES(@d);");
    sb.Append("INSERT INTO SPI_COMPOUD_TESTS (ID) VALUES(@e);");
    sb.Append("UPDATE SPI_COMPOUD_TESTS SET ID = 2 * ID;");
    sb.Append("DELETE FROM SPI_COMPOUD_TESTS WHERE ID = @f;");
    sb.Append("SELECT * FROM SPI_COMPOUD_TESTS;");

    using var conn = new NpgsqlConnection();
    conn.Open();
    var cmd = new NpgsqlCommand(sb.ToString(), conn);

    cmd.Parameters.AddWithValue("a", NpgsqlDbType.Integer, 1);
    cmd.Parameters.AddWithValue("b", NpgsqlDbType.Integer, 2);
    cmd.Parameters.AddWithValue("c", NpgsqlDbType.Integer, 3);
    cmd.Parameters.AddWithValue("d", NpgsqlDbType.Integer, 4);
    cmd.Parameters.AddWithValue("e", NpgsqlDbType.Integer, 5);

    cmd.Parameters.AddWithValue("f", NpgsqlDbType.Integer, 8);

    var reader = cmd.ExecuteReader();

    reader.NextResult();
    reader.NextResult();
    reader.NextResult();
    reader.NextResult();
    reader.NextResult();
    reader.NextResult();
    reader.NextResult();
    reader.NextResult();
    reader.NextResult();

    int sum = 0;
    while (reader.Read())
    {
        int _a = reader.GetInt32(0);
        Elog.Info($"Returned value = {_a}");
        sum += _a;
    }
    return sum;
$$ LANGUAGE plcsharp;
CREATE FUNCTION

user=# SELECT 'c#-int-spi-multiquery-compoud', 'SPITestingCompoudParameters', SPITestingCompoudParameters() = 22;
NOTICE:  table "spi_compoud_tests" does not exist, skipping
INFO:  Returned value = 2
INFO:  Returned value = 4
INFO:  Returned value = 6
INFO:  Returned value = 10
           ?column?            |          ?column?           | ?column?
-------------------------------+-----------------------------+----------
 c#-int-spi-multiquery-compoud | SPITestingCompoudParameters | t
(1 row)
```

#### Supported Data Manipulation Language (DML) Operations

Here are the DML operations we have tested:

| C#      | F#      | Operation  | Description                      |
| ------- | ------- | ---------- | -------------------------------- |
| ✅  | ✅  | **Select** | Retrieve data from the database. |
| ✅  | ✅  | **Insert** | Add new rows to a table.         |
| ✅  | ✅  | **Update** | Modify existing data in a table. |
| ✅  | ✅  | **Delete** | Remove data from a table.        |

#### Supported Transaction Control Operations

Here are the DML operations we have tested:

| C#      | F#      | Operation             | Description                       |
| ------- | ------- | --------------------- | --------------------------------- |
| ✅  | ✅  | **Begin Transaction** | Start a new database transaction. |
| ✅  | ✅  | **Commit**            | Commit the current transaction.   |
| ✅  | ✅  | **Rollback**          | Rollback the current transaction. |

## Set-returning functions (SRFs)

### Are set-returning functions (SRFs) supported?

Set returning functions are supported in C# but not F#.  You can have a simple return type, a RECORD return type, or you can define a TABLE function.

Functions returning `SETOF RECORD` are unique in that the RECORDs are flexible; you can return different types in different circumstances.  SQL requires you to provide the expected types when you select it, and pl/dotnet checks to confirm that the types provided match the types expected.  In general, SRF-with-record is a bad idea, and table functions are the preferred way to return tabular data.

You cannot return `SETOF RECORD` using `INOUT` or `OUT` parameters.

You can find plcsharp examples in `tests/csharp/testrecord.sql`, `tests/csharp/testtable.sql`, and `tests/csharp/testsrf.sql`.

### How are set-returning functions (SRFs) supported in plcsharp?

Set returning functions are expressed in C# as `IEnumerable<>`.

Here is an example:

```sql
user=# CREATE OR REPLACE FUNCTION numbers(count int8)
RETURNS SETOF int8 AS
$$
        if(count == null){ for(long i=0;;i++) { yield return i; } }
        else { for(long i=0;i<count;i++) { yield return i; } }
$$
LANGUAGE plcsharp;
CREATE FUNCTION

user=# select numbers(10);
 numbers
---------
       0
       1
       2
       3
       4
       5
       6
       7
       8
       9
(10 rows)
```

Here is the generated code:

```csharp
    public static IEnumerable<long?> numbers(long? count)
    {
#line 1
        if (count == null)
        {
            for (long i = 0;; i++)
            {
                yield return i;
            }
        }
        else
        {
            for (long i = 0; i < count; i++)
            {
                yield return i;
            }
        }
    }
```

### How are set-returning functions (SRFs) supported in plfsharp?

Set returning functions are expressed in F# as `seq`, and you can find helpful examples in `tests/fsharp/testfssrf.sql`

Here is an example:

```sql
user=# CREATE OR REPLACE FUNCTION numbers_fsharp(count INT4)
RETURNS SETOF INT4 AS
$$
    let c = if count.HasValue then count.Value else int 0

    match count.HasValue with
    | false ->
        seq { for i in 0 .. System.Int32.MaxValue do yield i }
    | true ->
        seq { for i in 0 .. count.Value - 1 do yield i }
$$
LANGUAGE plfsharp;
CREATE FUNCTION

user=# select numbers_fsharp(10);
 numbers_fsharp
----------------
              0
              1
              2
              3
              4
              5
              6
              7
              8
              9
(10 rows)
```

Here is the generated code:

```fsharp
    static member numbers_fsharp (count: Nullable<int>) : seq<Nullable<int>> =
        let c = if count.HasValue then count.Value else int 0

        match count.HasValue with
        | false ->
            seq { for i in 0 .. System.Int32.MaxValue do yield i }
        | true ->
            seq { for i in 0 .. count.Value - 1 do yield i }
```

## Records

### Does pl/csharp support records?

Yes, pl/csharp supports records.  Per the PostgreSQL convention,
you cannot accept them as inputs, but you can create them to outputs.
They are mapped in dotnet to arrays of objects.

Here's an example:

```sql
user=# CREATE OR REPLACE FUNCTION dynamic_record_generator(scenario INT4)
RETURNS record
LANGUAGE plcsharp
AS $$
    switch(scenario)
    {
        case 1:
            return new object[]{1, "Alice"};
        case 2:
            var barbara = new NpgsqlParameter
                    {
                        ParameterName = "_",
                        NpgsqlDbType = NpgsqlDbType.Varchar,
                        Value = "Barbara"
                    };
            return new object[]{2, barbara};
        case 3:
           return new object[]{1.5, 2.5, true};
        default:
            throw new SystemException($"Unrecognized scenario: {scenario}");
    }
$$;
CREATE FUNCTION

user=# SELECT * FROM dynamic_record_generator(1) AS (a int4, b text);
 a |   b
---+-------
 1 | Alice
(1 row)

user=# SELECT * FROM dynamic_record_generator(3) AS (a float, b float, c bool);
  a  |  b  | c
-----+-----+---
 1.5 | 2.5 | t
(1 row)
```

You can even see how pl/dotnet will keep you safe from returning the wrong type:

```sql
user=# SELECT * FROM dynamic_record_generator_srf(10) AS t(a int8, b varchar);
ERROR:  Type mismatch on RECORD:  psql OID(25) != pldotnet OID(1043) (Slot 1)",
```

### Does pl/csharp support sets of records?

Yes, although you probably should use a table function.

Here's an example:

```sql
user=# CREATE OR REPLACE FUNCTION dynamic_record_generator_srf(lim INT8)
RETURNS SETOF record
LANGUAGE plcsharp
AS $$
    if (!(lim > 0)){ yield break; }
    for(long i=0;i<lim;i++){ yield return new object?[] { (long)i, $"Number is {i}" }; }
$$;
CREATE FUNCTION

user=# SELECT * FROM dynamic_record_generator_srf(10) AS t(a int8, b text);
 a |      b
---+-------------
 0 | Number is 0
 1 | Number is 1
 2 | Number is 2
 3 | Number is 3
 4 | Number is 4
 5 | Number is 5
 6 | Number is 6
 7 | Number is 7
 8 | Number is 8
 9 | Number is 9
(10 rows)
```

Here is the generated code:

```csharp
    public static IEnumerable<Object? []?> dynamic_record_generator_srf(long? lim)
    {
#line 1
        if (!(lim > 0))
        {
            yield break;
        }

        for (long i = 0; i < lim; i++)
        {
            yield return new object? []{(long)i, $"Number is {i}"};
        }
    }
```

### Does pl/fsharp support records?

Yes, same as C#.  Here's an example:

```sql
user=# CREATE OR REPLACE FUNCTION dynamic_record_generator_fsharp(scenario INT4)
RETURNS record
LANGUAGE plfsharp
AS $$
let barbara =
    let param = new NpgsqlParameter()
    param.ParameterName <- "_"
    param.NpgsqlDbType <- NpgsqlDbType.Varchar
    param.Value <- "Barbara"
    param
let s = if scenario.HasValue then scenario.Value else int 0
match s with
| _ when 1 = s -> [| 1; "Alice" |]
| _ when 2 = s -> [| 2; barbara |]
| _ when 3 = s -> [| 1.5; 2.5; true |]
| _ -> failwithf "Unrecognized scenario: %d" s
$$;
CREATE FUNCTION

user=# SELECT * FROM dynamic_record_generator_fsharp(2) AS (a int4, b varchar);
 a |    b
---+---------
 2 | Barbara
(1 row)
```

This is also a good example of how to control the type mapping, in this case dotnet `string` to PostgreSQL `varchar`, by using

### Does pl/fsharp support sets of records?

Yes, though again you should usually use a table function instead.  Here is an example:

```sql
tlewis=# CREATE OR REPLACE FUNCTION dynamic_record_generator_srf_fsharp(lim INT8)
RETURNS SETOF record
LANGUAGE plfsharp
AS $$
match lim.HasValue with
| false ->
    seq { for i in 0 .. System.Int32.MaxValue do yield [| box i; $"Number is {i}" |] }
| true ->
    if not (lim.Value > 0) then
        seq { () }
    else
        seq { for i in 0L .. lim.Value - 1L do yield [| box i; $"Number is {i}" |] }
$$;
CREATE FUNCTION

tlewis=# SELECT * FROM dynamic_record_generator_srf_fsharp(10) AS t(a int8, b text);
 a |      b
---+-------------
 0 | Number is 0
 1 | Number is 1
 2 | Number is 2
 3 | Number is 3
 4 | Number is 4
 5 | Number is 5
 6 | Number is 6
 7 | Number is 7
 8 | Number is 8
 9 | Number is 9
(10 rows)
```

Here is the generated code:

```fsharp
    static member dynamic_record_generator_srf_fsharp (lim: Nullable<int64>) : seq<obj[]> =
        match lim.HasValue with
        | false ->
            seq { for i in 0 .. System.Int32.MaxValue do yield [| box i; $"Number is {i}" |] }
        | true ->
            if not (lim.Value > 0) then
                seq { () }
            else
                seq { for i in 0L .. lim.Value - 1L do yield [| box i; $"Number is {i}" |] }
```


### Can I control the mapping of types from dotnet to PostgreSQL?

Some dotnet types can be mapped to multiple PostgreSQL types.  In a normal
context, pl/dotnet knows the correct type mapping and will handle it for
you, but in a Record context, you can control the type mapping by using a
`NpgsqlParameter`.

Here is an example using plfsharp.

```sql
user=# CREATE OR REPLACE FUNCTION dynamic_record_generator_fsharp(scenario INT4)
RETURNS record
LANGUAGE plfsharp
AS $$
let barbara =
    let param = new NpgsqlParameter()
    param.ParameterName <- "_"
    param.NpgsqlDbType <- NpgsqlDbType.Varchar
    param.Value <- "Barbara"
    param
let s = if scenario.HasValue then scenario.Value else int 0
match s with
| _ when 1 = s -> [| 1; "Alice" |]
| _ when 2 = s -> [| 2; barbara |]
| _ when 3 = s -> [| 1.5; 2.5; true |]
| _ -> failwithf "Unrecognized scenario: %d" s
$$;
CREATE FUNCTION

user=# SELECT * FROM dynamic_record_generator_fsharp(2) AS (a int4, b varchar);
 a |    b
---+---------
 2 | Barbara
(1 row)
```

Look in `tests/csharp/testrecord.sql` for similar plcsharp examples.

## Table functions

### Does pl/csharp support table functions?

Yes.  Here is an example:

```sql
user=# CREATE OR REPLACE FUNCTION table_array_test(lim int4)
RETURNS TABLE(id integer[], name text) AS
$$
    return lim.HasValue
        ? Enumerable.Range(0, lim.Value).Select(i => ((Array)new int?[] { i, i*i*i, null }, $"The number is {i}"))
        : Enumerable.Empty<(Array? id, string? name)>();
$$
LANGUAGE plcsharp;
CREATE FUNCTION
user=# SELECT * FROM table_array_test(5);
     id      |      name
-------------+-----------------
 {0,0,NULL}  | The number is 0
 {1,1,NULL}  | The number is 1
 {2,8,NULL}  | The number is 2
 {3,27,NULL} | The number is 3
 {4,64,NULL} | The number is 4
(5 rows)
```

They are mapped the same as an SRF, but different from a SRF returning
`RECORD`; here, the return type of the `IEnumerable<>` is properly typed,
while in a RECORD then return type is always `object[]`.

Here is the generated code:

```csharp
    public static IEnumerable<(Array? id, string? name)> table_array_test(int? lim)
    {
#line 1
        return lim.HasValue
            ? Enumerable.Range(0, lim.Value).Select(i => ((Array)new int? []{i, i * i * i, null}, $"The number is {i}"))
            : Enumerable.Empty<(Array? id, string? name)>();
    }
```

### Does pl/fsharp support table functions?

Yes.  Here is an example:

```sql
tlewis=# CREATE OR REPLACE FUNCTION table_array_test_fsharp(lim int4)
RETURNS TABLE(id integer[], name text) AS
$$
    match lim.HasValue with
    | true ->
        seq {
            for i = 0 to lim.Value-1 do
                yield struct([| Nullable(i); Nullable(i*i*i); System.Nullable() |], "The number is "+i.ToString());
            }
    | false ->
        Seq.empty
$$
LANGUAGE plfsharp;
CREATE FUNCTION
tlewis=# SELECT * FROM table_array_test_fsharp(5);
     id      |      name
-------------+-----------------
 {0,0,NULL}  | The number is 0
 {1,1,NULL}  | The number is 1
 {2,8,NULL}  | The number is 2
 {3,27,NULL} | The number is 3
 {4,64,NULL} | The number is 4
(5 rows)
```

Here is the generated code:

```fsharp
    static member table_array_test_fsharp (lim: Nullable<int>) : seq<struct (Array * string)> =
        match lim.HasValue with
        | true ->
            seq {
                for i = 0 to lim.Value-1 do
                    yield struct([| Nullable(i); Nullable(i*i*i); System.Nullable() |], "The number is "+i.ToString());
                }
        | false ->
            Seq.empty
```

## DLL support

### Can I load my code from a DLL?

Yes, you can. Here is an example:

```sql
CREATE OR REPLACE FUNCTION IntegerTest(a INTEGER) RETURNS INTEGER
AS 'Test.dll:Namespace.Class!IntegerTest'
LANGUAGE plcsharp STRICT;
```

To use your assembly with pl/dotnet, place it in the `/var/lib/postgresql/data/` directory. Alternatively, you can specify the full path to the assembly in the `CREATE OR REPLACE FUNCTION` statement, as demonstrated in this example:

```sql
CREATE OR REPLACE FUNCTION IntegerTest(a INTEGER) RETURNS INTEGER
AS '/path/to/Test.dll:Namespace.Class!IntegerTest'
LANGUAGE plcsharp STRICT;
```

### Can I use any C#/F# library in pl/dotnet?

Users cannot currently load libraries into their pl/dotnet programs, but we hope to add this support soon. In the meantime, if you wish to use other libraries, you can create your own Assembly and load it as demonstrated in the question [Q: Can I load my code from a DLL?](#q-can-i-load-my-code-from-a-dll).

## Security

### How safe is pl/dotnet?

Currently, pl/dotnet outputs the source code for each function to `/tmp/PlDotNET/` for debugging.

Each function in pl/dotnet is placed in a separate [AssemblyLoadContext](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.loader.assemblyloadcontext?view=net-8.0) in the .NET runtime (CLR).  This places significant barriers between sharing of data between functions.

.NET was not designed to provide perfect security guarantees for code running inside of it, nor were most of the other runtimes used in PostgreSQL procedural languages. We encourage stored procedure authors to be cautious of the security implications of running code in stored procedures.

### How isolated are functions in pl/dotnet?

Each function in pl/dotnet is placed in a separate AssemblyLoadContext in the .NET runtime (CLR).

We hope in the future to allow different functions loaded from the same DLL to share the same AssemblyLoadContext.

## Coding guidelines

### How do I write efficient and secure C# stored procedures in pl/dotnet?

- Use parameterized queries: Use parameterized queries to prevent SQL injection attacks.
- Use prepared statements: Use prepared statements to avoid parsing the same SQL statement multiple times.
- Use transactions: Use transactions to ensure that your stored procedures are atomic, consistent, isolated, and durable (ACID).
- Use the appropriate data types: Use the appropriate data types when passing data between C# and SQL Server to avoid unnecessary data conversions.
- Avoid using global variables: Global variables can be modified by any part of the code and can cause surprising behavior, so avoid using them where possible.
- Use proper error handling: Use proper error handling to catch errors and prevent your stored procedures from crashing.
- Use the `using` statement when working with database connections: The using statement will automatically handle the closing of the connection.
- Avoid using dynamic SQL if possible: Dynamic SQL can be vulnerable to SQL injection attacks and is harder to debug and maintain.
- Be mindful of memory usage: Be mindful of memory usage when working with large datasets and try to release memory as soon as it is no longer needed.
- Keep your stored procedures simple and focused: Avoid adding unnecessary complexity to your stored procedures, keep them simple, focused, and easy to understand.

It is also important to keep PostgreSQL updated with the latest security patches and use appropriate authentication and authorization methods for accessing your stored procedures.
