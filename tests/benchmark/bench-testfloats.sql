\i tests/benchmark/includes.sql

\i tests/benchmark/csharp/testfloats.sql
\i tests/benchmark/fsharp/testfsfloats.sql
\i tests/benchmark/v8javascript/testfloats.sql
\i tests/benchmark/python/testfloats.sql
\i tests/benchmark/pgsql/testfloats.sql
\i tests/benchmark/java/testfloats.sql
\i tests/benchmark/perl/testfloats.sql
\i tests/benchmark/lua/testfloats.sql
\i tests/benchmark/tcl/testfloats.sql
\i tests/benchmark/r/testfloats.sql

\set runs NRUNS

SELECT
    'returnReal',
    'Float',
    plbench('SELECT returnReal()', :runs) as plcsharp,
    plbench('SELECT returnRealFSharp()', :runs) as plfsharp,
    plbench('SELECT returnRealV8()', :runs) as plv8,
    plbench('SELECT returnRealPython()', :runs) as plpython,
    plbench('SELECT returnRealPg()', :runs) as plpgsql,
    plbench('SELECT returnRealJava()', :runs) as pljava,
    plbench('SELECT returnRealPerl()', :runs) as plperl,
    plbench('SELECT returnRealLua()', :runs) as pllua,
    plbench('SELECT returnRealTcl()', :runs) as pltcl,
    plbench('SELECT returnRealR()', :runs) as plr;

SELECT
    'sumReal',
    'Float',
    plbench('SELECT sumReal(1.50055, 1.50054)', :runs) as plcsharp,
    plbench('SELECT sumRealFSharp(1.50055, 1.50054)', :runs) as plfsharp,
    plbench('SELECT sumRealV8(1.50055, 1.50054)', :runs) as plv8,
    plbench('SELECT sumRealPython(1.50055, 1.50054)', :runs) as plpython,
    plbench('SELECT sumRealPg(1.50055, 1.50054)', :runs) as plpgsql,
    plbench('SELECT sumRealJava(1.50055, 1.50054)', :runs) as pljava,
    plbench('SELECT sumRealPerl(1.50055, 1.50054)', :runs) as plperl,
    plbench('SELECT sumRealLua(1.50055, 1.50054)', :runs) as pllua,
    plbench('SELECT sumRealTcl(1.50055, 1.50054)', :runs) as pltcl,
    plbench('SELECT sumRealR(1.50055, 1.50054)', :runs) as plr;

SELECT
    'returnDouble',
    'Float',
    plbench('SELECT returnDouble()', :runs) as plcsharp,
    plbench('SELECT returnDoubleFSharp()', :runs) as plfsharp,
    plbench('SELECT returnDoubleV8()', :runs) as plv8,
    plbench('SELECT returnDoublePython()', :runs) as plpython,
    plbench('SELECT returnDoublePg()', :runs) as plpgsql,
    plbench('SELECT returnDoubleJava()', :runs) as pljava,
    plbench('SELECT returnDoublePerl()', :runs) as plperl,
    plbench('SELECT returnDoubleLua()', :runs) as pllua,
    plbench('SELECT returnDoubleTcl()', :runs) as pltcl,
    plbench('SELECT returnDoubleR()', :runs) as plr;

SELECT
    'sumDouble',
    'Float',
    plbench('SELECT sumDouble(10.5000000000055, 10.5000000000054)', :runs) as plcsharp,
    plbench('SELECT sumDoubleFSharp(10.5000000000055, 10.5000000000054)', :runs) as plfsharp,
    plbench('SELECT sumDoubleV8(10.5000000000055, 10.5000000000054)', :runs) as plv8,
    plbench('SELECT sumDoublePython(10.5000000000055, 10.5000000000054)', :runs) as plpython,
    plbench('SELECT sumDoublePg(10.5000000000055, 10.5000000000054)', :runs) as plpgsql,
    plbench('SELECT sumDoubleJava(10.5000000000055, 10.5000000000054)', :runs) as pljava,
    plbench('SELECT sumDoublePerl(10.5000000000055, 10.5000000000054)', :runs) as plperl,
    plbench('SELECT sumDoubleLua(10.5000000000055, 10.5000000000054)', :runs) as pllua,
    plbench('SELECT sumDoubleTcl(10.5000000000055, 10.5000000000054)', :runs) as pltcl,
    plbench('SELECT sumDoubleR(10.5000000000055, 10.5000000000054)', :runs) as plr;
