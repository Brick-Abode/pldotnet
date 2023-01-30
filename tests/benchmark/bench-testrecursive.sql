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
    plbench('SELECT fibbb(25)', 1000) as plcsharp,
    plbench('SELECT fibbbFSharp(25)', 1000) as plfsharp,
    plbench('SELECT fibbbV8(25)', 1000) as plv8,
    plbench('SELECT fibbbPython(25)', 1000) as plpython,
    plbench('SELECT fibbbPg(25)', 1000) as plpgsql,
    plbench('SELECT fibbbJava(25)', 1000) as pljava,
    plbench('SELECT fibbbPerl(25)', 1000) as plperl,
    plbench('SELECT fibbbLua(25)', 1000) as pllua,
    plbench('SELECT fibbbTcl(25)', 1000) as pltcl,
    plbench('SELECT fibbbR(25)', 1000) as plr;

SELECT
    'Factorial',
    'Recursive',
    plbench('SELECT fact(12)', :runs) as plcsharp,
    plbench('SELECT factFSharp(12)', :runs) as plfsharp,
    plbench('SELECT factV8(12)', :runs) as plv8,
    plbench('SELECT factPython(12)', :runs) as plpython,
    plbench('SELECT factPg(12)', :runs) as plpgsql,
    plbench('SELECT factJava(12)', :runs) as pljava,
    plbench('SELECT factPerl(12)', :runs) as plperl,
    plbench('SELECT factLua(12)', :runs) as pllua,
    plbench('SELECT factTcl(12)', :runs) as pltcl,
    plbench('SELECT factR(12)', :runs) as plr;
