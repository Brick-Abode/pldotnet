\i tests/benchmark/includes.sql

\i tests/benchmark/csharp/testrecursive.sql
\i tests/benchmark/fsharp/testfsrecursive.sql
\i tests/benchmark/v8javascript/testrecursive.sql
\i tests/benchmark/python/testrecursive.sql
\i tests/benchmark/pgsql/testrecursive.sql
\i tests/benchmark/java/testrecursive.sql
\i tests/benchmark/perl/testrecursive.sql
\i tests/benchmark/lua/testrecursive.sql
\i tests/benchmark/tcl/testrecursive.sql
\i tests/benchmark/r/testrecursive.sql

\set runs NRUNS

SELECT
    'Fibonacci',
    'Recursive',
    plbench('SELECT fibbb(4)', :runs) as plcsharp,
    plbench('SELECT fibbbFSharp(4)', :runs) as plfsharp,
    plbench('SELECT fibbbV8(4)', :runs) as plv8,
    plbench('SELECT fibbbPython(4)', :runs) as plpython,
    plbench('SELECT fibbbPg(4)', :runs) as plpgsql,
    plbench('SELECT fibbbJava(4)', :runs) as pljava,
    plbench('SELECT fibbbPerl(4)', :runs) as plperl,
    plbench('SELECT fibbbLua(4)', :runs) as pllua,
    plbench('SELECT fibbbTcl(4)', :runs) as pltcl,
    plbench('SELECT fibbbR(4)', :runs) as plr;

SELECT
    'Factorial',
    'Recursive',
    plbench('SELECT fact(4)', :runs) as plcsharp,
    plbench('SELECT factFSharp(4)', :runs) as plfsharp,
    plbench('SELECT factV8(4)', :runs) as plv8,
    plbench('SELECT factPython(4)', :runs) as plpython,
    plbench('SELECT factPg(4)', :runs) as plpgsql,
    plbench('SELECT factJava(4)', :runs) as pljava,
    plbench('SELECT factPerl(4)', :runs) as plperl,
    plbench('SELECT factLua(4)', :runs) as pllua,
    plbench('SELECT factTcl(4)', :runs) as pltcl,
    plbench('SELECT factR(4)', :runs) as plr;
