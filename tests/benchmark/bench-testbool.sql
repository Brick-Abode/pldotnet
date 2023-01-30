\i tests/benchmark/includes.sql

\i tests/benchmark/csharp/testbool.sql
\i tests/benchmark/fsharp/testfsbool.sql
\i tests/benchmark/v8javascript/testbool.sql
\i tests/benchmark/python/testbool.sql
\i tests/benchmark/pgsql/testbool.sql
\i tests/benchmark/java/testbool.sql
\i tests/benchmark/perl/testbool.sql
\i tests/benchmark/lua/testbool.sql
\i tests/benchmark/tcl/testbool.sql
\i tests/benchmark/r/testbool.sql

\set runs NRUNS

SELECT
    'returnBool',
    'Bool',
    plbench('SELECT returnBool()', :runs) as plcsharp,
    plbench('SELECT returnBoolFSharp()', :runs) as plfsharp,
    plbench('SELECT returnBoolV8()', :runs) as plv8,
    plbench('SELECT returnBoolPython()', :runs) as plpython,
    plbench('SELECT returnBoolPg()', :runs) as plpgsql,
    plbench('SELECT returnBoolJava()', :runs) as pljava,
    plbench('SELECT returnBoolPerl()', :runs) as plperl,
    plbench('SELECT returnBoolLua()', :runs) as pllua,
    plbench('SELECT returnBoolTcl()', :runs) as pltcl,
    plbench('SELECT returnBoolR()', :runs) as plr;

SELECT
    'booleanAnd',
    'Bool',
    plbench('SELECT BooleanAnd(true, true)', :runs) as plcsharp,
    plbench('SELECT BooleanAndFSharp(true, true)', :runs) as plfsharp,
    plbench('SELECT BooleanAndV8(true, true)', :runs) as plv8,
    plbench('SELECT BooleanAndPython(true, true)', :runs) as plpython,
    plbench('SELECT BooleanAndPg(true, true)', :runs) as plpgsql,
    plbench('SELECT BooleanAndJava(true, true)', :runs) as pljava,
    plbench('SELECT BooleanAndPerl(true, true)', :runs) as plperl,
    plbench('SELECT BooleanAndLua(true, true)', :runs) as pllua,
    plbench('SELECT BooleanAndTcl(true, true)', :runs) as pltcl,
    plbench('SELECT BooleanAndR(true, true)', :runs) as plr;

SELECT
    'booleanOr',
    'Bool',
    plbench('SELECT BooleanOr(false, false)', :runs) as plcsharp,
    plbench('SELECT BooleanOrFSharp(false, false)', :runs) as plfsharp,
    plbench('SELECT BooleanOrV8(false, false)', :runs) as plv8,
    plbench('SELECT BooleanOrPython(false, false)', :runs) as plpython,
    plbench('SELECT BooleanOrPg(false, false)', :runs) as plpgsql,
    plbench('SELECT BooleanOrJava(false, false)', :runs) as pljava,
    plbench('SELECT BooleanOrPerl(false, false)', :runs) as plperl,
    plbench('SELECT BooleanOrLua(false, false)', :runs) as pllua,
    plbench('SELECT BooleanOrTcl(false, false)', :runs) as pltcl,
    plbench('SELECT BooleanOrR(false, false)', :runs) as plr;

SELECT
    'booleanXor',
    'Bool',
    plbench('SELECT BooleanXor(false, false)', :runs) as plcsharp,
    plbench('SELECT BooleanXorFSharp(false, false)', :runs) as plfsharp,
    plbench('SELECT BooleanXorV8(false, false)', :runs) as plv8,
    plbench('SELECT BooleanXorPython(false, false)', :runs) as plpython,
    plbench('SELECT BooleanXorPg(false, false)', :runs) as plpgsql,
    plbench('SELECT BooleanXorJava(false, false)', :runs) as pljava,
    plbench('SELECT BooleanXorPerl(false, false)', :runs) as plperl,
    plbench('SELECT BooleanXorLua(false, false)', :runs) as pllua,
    plbench('SELECT BooleanXorTcl(false, false)', :runs) as pltcl,
    plbench('SELECT BooleanXorR(false, false)', :runs) as plr;
