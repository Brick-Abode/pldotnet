\i tests/benchmark/includes.sql

\i tests/benchmark/csharp/testnullbool.sql
\i tests/benchmark/fsharp/testfsnullbool.sql
\i tests/benchmark/v8javascript/testnullbool.sql
\i tests/benchmark/python/testnullbool.sql
\i tests/benchmark/pgsql/testnullbool.sql
\i tests/benchmark/java/testnullbool.sql
\i tests/benchmark/perl/testnullbool.sql
\i tests/benchmark/lua/testnullbool.sql
\i tests/benchmark/tcl/testnullbool.sql
\i tests/benchmark/r/testnullbool.sql

\set runs NRUNS

SELECT
    'returnNullBool',
    'NullBool',
    plbench('SELECT returnNullBool()', :runs) as plcsharp,
    plbench('SELECT returnNullBoolFSharp()', :runs) as plfsharp,
    plbench('SELECT returnNullBoolV8()', :runs) as plv8,
    plbench('SELECT returnNullBoolPython()', :runs) as plpython,
    plbench('SELECT returnNullBoolPg()', :runs) as plpgsql,
    plbench('SELECT returnNullBoolJava()', :runs) as pljava,
    plbench('SELECT returnNullBoolPerl()', :runs) as plperl,
    plbench('SELECT returnNullBoolLua()', :runs) as pllua,
    plbench('SELECT returnNullBoolTcl()', :runs) as pltcl,
    plbench('SELECT returnNullBoolR()', :runs) as plr;

SELECT
    'booleanNullAnd-1',
    'NullBool',
    plbench('SELECT BooleanNullAnd(true, null)', :runs) as plcsharp,
    plbench('SELECT BooleanNullAndFSharp(true, null)', :runs) as plfsharp,
    plbench('SELECT BooleanNullAndV8(true, null)', :runs) as plv8,
    plbench('SELECT BooleanNullAndPython(true, null)', :runs) as plpython,
    plbench('SELECT BooleanNullAndPg(true, null)', :runs) as plpgsql,
    plbench('SELECT BooleanNullAndJava(true, null)', :runs) as pljava,
    plbench('SELECT BooleanNullAndPerl(true, null)', :runs) as plperl,
    plbench('SELECT BooleanNullAndLua(true, null)', :runs) as pllua,
    plbench('SELECT BooleanNullAndTcl(true, null)', :runs) as pltcl,
    plbench('SELECT BooleanNullAndR(true, null)', :runs) as plr;

SELECT
    'booleanNullAnd-2',
    'NullBool',
    plbench('SELECT BooleanNullAnd(null, true)', :runs) as plcsharp,
    plbench('SELECT BooleanNullAndFSharp(null, true)', :runs) as plfsharp,
    plbench('SELECT BooleanNullAndV8(null, true)', :runs) as plv8,
    plbench('SELECT BooleanNullAndPython(null, true)', :runs) as plpython,
    plbench('SELECT BooleanNullAndPg(null, true)', :runs) as plpgsql,
    plbench('SELECT BooleanNullAndJava(null, true)', :runs) as pljava,
    plbench('SELECT BooleanNullAndPerl(null, true)', :runs) as plperl,
    plbench('SELECT BooleanNullAndLua(null, true)', :runs) as pllua,
    plbench('SELECT BooleanNullAndTcl(null, true)', :runs) as pltcl,
    plbench('SELECT BooleanNullAndR(null, true)', :runs) as plr;

SELECT
    'booleanNullAnd-3',
    'NullBool',
    plbench('SELECT BooleanNullAnd(false, null)', :runs) as plcsharp,
    plbench('SELECT BooleanNullAndFSharp(false, null)', :runs) as plfsharp,
    plbench('SELECT BooleanNullAndV8(false, null)', :runs) as plv8,
    plbench('SELECT BooleanNullAndPython(false, null)', :runs) as plpython,
    plbench('SELECT BooleanNullAndPg(false, null)', :runs) as plpgsql,
    plbench('SELECT BooleanNullAndJava(false, null)', :runs) as pljava,
    plbench('SELECT BooleanNullAndPerl(false, null)', :runs) as plperl,
    plbench('SELECT BooleanNullAndLua(false, null)', :runs) as pllua,
    plbench('SELECT BooleanNullAndTcl(false, null)', :runs) as pltcl,
    plbench('SELECT BooleanNullAndR(false, null)', :runs) as plr;

SELECT
    'booleanNullAnd-4',
    'NullBool',
    plbench('SELECT BooleanNullAnd(null, false)', :runs) as plcsharp,
    plbench('SELECT BooleanNullAndFSharp(null, false)', :runs) as plfsharp,
    plbench('SELECT BooleanNullAndV8(null, false)', :runs) as plv8,
    plbench('SELECT BooleanNullAndPython(null, false)', :runs) as plpython,
    plbench('SELECT BooleanNullAndPg(null, false)', :runs) as plpgsql,
    plbench('SELECT BooleanNullAndJava(null, false)', :runs) as pljava,
    plbench('SELECT BooleanNullAndPerl(null, false)', :runs) as plperl,
    plbench('SELECT BooleanNullAndLua(null, false)', :runs) as pllua,
    plbench('SELECT BooleanNullAndTcl(null, false)', :runs) as pltcl,
    plbench('SELECT BooleanNullAndR(null, false)', :runs) as plr;

SELECT
    'booleanNullAnd-5',
    'NullBool',
    plbench('SELECT BooleanNullAnd(null, null)', :runs) as plcsharp,
    plbench('SELECT BooleanNullAndFSharp(null, null)', :runs) as plfsharp,
    plbench('SELECT BooleanNullAndV8(null, null)', :runs) as plv8,
    plbench('SELECT BooleanNullAndPython(null, null)', :runs) as plpython,
    plbench('SELECT BooleanNullAndPg(null, null)', :runs) as plpgsql,
    plbench('SELECT BooleanNullAndJava(null, null)', :runs) as pljava,
    plbench('SELECT BooleanNullAndPerl(null, null)', :runs) as plperl,
    plbench('SELECT BooleanNullAndLua(null, null)', :runs) as pllua,
    plbench('SELECT BooleanNullAndTcl(null, null)', :runs) as pltcl,
    plbench('SELECT BooleanNullAndR(null, null)', :runs) as plr;

SELECT
    'booleanNullOr-1',
    'NullBool',
    plbench('SELECT BooleanNullOr(true, null)', :runs) as plcsharp,
    plbench('SELECT BooleanNullOrFSharp(true, null)', :runs) as plfsharp,
    plbench('SELECT BooleanNullOrV8(true, null)', :runs) as plv8,
    plbench('SELECT BooleanNullOrPython(true, null)', :runs) as plpython,
    plbench('SELECT BooleanNullOrPg(true, null)', :runs) as plpgsql,
    plbench('SELECT BooleanNullOrJava(true, null)', :runs) as pljava,
    plbench('SELECT BooleanNullOrPerl(true, null)', :runs) as plperl,
    plbench('SELECT BooleanNullOrLua(true, null)', :runs) as pllua,
    plbench('SELECT BooleanNullOrTcl(true, null)', :runs) as pltcl,
    plbench('SELECT BooleanNullOrR(true, null)', :runs) as plr;

SELECT
    'booleanNullOr-2',
    'NullBool',
    plbench('SELECT BooleanNullOr(null, true)', :runs) as plcsharp,
    plbench('SELECT BooleanNullOrFSharp(null, true)', :runs) as plfsharp,
    plbench('SELECT BooleanNullOrV8(null, true)', :runs) as plv8,
    plbench('SELECT BooleanNullOrPython(null, true)', :runs) as plpython,
    plbench('SELECT BooleanNullOrPg(null, true)', :runs) as plpgsql,
    plbench('SELECT BooleanNullOrJava(null, true)', :runs) as pljava,
    plbench('SELECT BooleanNullOrPerl(null, true)', :runs) as plperl,
    plbench('SELECT BooleanNullOrLua(null, true)', :runs) as pllua,
    plbench('SELECT BooleanNullOrTcl(null, true)', :runs) as pltcl,
    plbench('SELECT BooleanNullOrR(null, true)', :runs) as plr;

SELECT
    'booleanNullOr-3',
    'NullBool',
    plbench('SELECT BooleanNullOr(false, null)', :runs) as plcsharp,
    plbench('SELECT BooleanNullOrFSharp(false, null)', :runs) as plfsharp,
    plbench('SELECT BooleanNullOrV8(false, null)', :runs) as plv8,
    plbench('SELECT BooleanNullOrPython(false, null)', :runs) as plpython,
    plbench('SELECT BooleanNullOrPg(false, null)', :runs) as plpgsql,
    plbench('SELECT BooleanNullOrJava(false, null)', :runs) as pljava,
    plbench('SELECT BooleanNullOrPerl(false, null)', :runs) as plperl,
    plbench('SELECT BooleanNullOrLua(false, null)', :runs) as pllua,
    plbench('SELECT BooleanNullOrTcl(false, null)', :runs) as pltcl,
    plbench('SELECT BooleanNullOrR(false, null)', :runs) as plr;

SELECT
    'booleanNullOr-4',
    'NullBool',
    plbench('SELECT BooleanNullOr(null, false)', :runs) as plcsharp,
    plbench('SELECT BooleanNullOrFSharp(null, false)', :runs) as plfsharp,
    plbench('SELECT BooleanNullOrV8(null, false)', :runs) as plv8,
    plbench('SELECT BooleanNullOrPython(null, false)', :runs) as plpython,
    plbench('SELECT BooleanNullOrPg(null, false)', :runs) as plpgsql,
    plbench('SELECT BooleanNullOrJava(null, false)', :runs) as pljava,
    plbench('SELECT BooleanNullOrPerl(null, false)', :runs) as plperl,
    plbench('SELECT BooleanNullOrLua(null, false)', :runs) as pllua,
    plbench('SELECT BooleanNullOrTcl(null, false)', :runs) as pltcl,
    plbench('SELECT BooleanNullOrR(null, false)', :runs) as plr;

SELECT
    'booleanNullOr-5',
    'NullBool',
    plbench('SELECT BooleanNullOr(null, null)', :runs) as plcsharp,
    plbench('SELECT BooleanNullOrFSharp(null, null)', :runs) as plfsharp,
    plbench('SELECT BooleanNullOrV8(null, null)', :runs) as plv8,
    plbench('SELECT BooleanNullOrPython(null, null)', :runs) as plpython,
    plbench('SELECT BooleanNullOrPg(null, null)', :runs) as plpgsql,
    plbench('SELECT BooleanNullOrJava(null, null)', :runs) as pljava,
    plbench('SELECT BooleanNullOrPerl(null, null)', :runs) as plperl,
    plbench('SELECT BooleanNullOrLua(null, null)', :runs) as pllua,
    plbench('SELECT BooleanNullOrTcl(null, null)', :runs) as pltcl,
    plbench('SELECT BooleanNullOrR(null, null)', :runs) as plr;

SELECT
    'booleanNullXor-1',
    'NullBool',
    plbench('SELECT BooleanNullXor(true, null)', :runs) as plcsharp,
    plbench('SELECT BooleanNullXorFSharp(true, null)', :runs) as plfsharp,
    plbench('SELECT BooleanNullXorV8(true, null)', :runs) as plv8,
    plbench('SELECT BooleanNullXorPython(true, null)', :runs) as plpython,
    plbench('SELECT BooleanNullXorPg(true, null)', :runs) as plpgsql,
    plbench('SELECT BooleanNullXorJava(true, null)', :runs) as pljava,
    plbench('SELECT BooleanNullXorPerl(true, null)', :runs) as plperl,
    plbench('SELECT BooleanNullXorLua(true, null)', :runs) as pllua,
    plbench('SELECT BooleanNullXorTcl(true, null)', :runs) as pltcl,
    plbench('SELECT BooleanNullXorR(true, null)', :runs) as plr;

SELECT
    'booleanNullXor-2',
    'NullBool',
    plbench('SELECT BooleanNullXor(null, true)', :runs) as plcsharp,
    plbench('SELECT BooleanNullXorFSharp(null, true)', :runs) as plfsharp,
    plbench('SELECT BooleanNullXorV8(null, true)', :runs) as plv8,
    plbench('SELECT BooleanNullXorPython(null, true)', :runs) as plpython,
    plbench('SELECT BooleanNullXorPg(null, true)', :runs) as plpgsql,
    plbench('SELECT BooleanNullXorJava(null, true)', :runs) as pljava,
    plbench('SELECT BooleanNullXorPerl(null, true)', :runs) as plperl,
    plbench('SELECT BooleanNullXorLua(null, true)', :runs) as pllua,
    plbench('SELECT BooleanNullXorTcl(null, true)', :runs) as pltcl,
    plbench('SELECT BooleanNullXorR(null, true)', :runs) as plr;

SELECT
    'booleanNullXor-3',
    'NullBool',
    plbench('SELECT BooleanNullXor(false, null)', :runs) as plcsharp,
    plbench('SELECT BooleanNullXorFSharp(false, null)', :runs) as plfsharp,
    plbench('SELECT BooleanNullXorV8(false, null)', :runs) as plv8,
    plbench('SELECT BooleanNullXorPython(false, null)', :runs) as plpython,
    plbench('SELECT BooleanNullXorPg(false, null)', :runs) as plpgsql,
    plbench('SELECT BooleanNullXorJava(false, null)', :runs) as pljava,
    plbench('SELECT BooleanNullXorPerl(false, null)', :runs) as plperl,
    plbench('SELECT BooleanNullXorLua(false, null)', :runs) as pllua,
    plbench('SELECT BooleanNullXorTcl(false, null)', :runs) as pltcl,
    plbench('SELECT BooleanNullXorR(false, null)', :runs) as plr;

SELECT
    'booleanNullXor-4',
    'NullBool',
    plbench('SELECT BooleanNullXor(null, false)', :runs) as plcsharp,
    plbench('SELECT BooleanNullXorFSharp(null, false)', :runs) as plfsharp,
    plbench('SELECT BooleanNullXorV8(null, false)', :runs) as plv8,
    plbench('SELECT BooleanNullXorPython(null, false)', :runs) as plpython,
    plbench('SELECT BooleanNullXorPg(null, false)', :runs) as plpgsql,
    plbench('SELECT BooleanNullXorJava(null, false)', :runs) as pljava,
    plbench('SELECT BooleanNullXorPerl(null, false)', :runs) as plperl,
    plbench('SELECT BooleanNullXorLua(null, false)', :runs) as pllua,
    plbench('SELECT BooleanNullXorTcl(null, false)', :runs) as pltcl,
    plbench('SELECT BooleanNullXorR(null, false)', :runs) as plr;

SELECT
    'booleanNullXor-5',
    'NullBool',
    plbench('SELECT BooleanNullXor(null, null)', :runs) as plcsharp,
    plbench('SELECT BooleanNullXorFSharp(null, null)', :runs) as plfsharp,
    plbench('SELECT BooleanNullXorV8(null, null)', :runs) as plv8,
    plbench('SELECT BooleanNullXorPython(null, null)', :runs) as plpython,
    plbench('SELECT BooleanNullXorPg(null, null)', :runs) as plpgsql,
    plbench('SELECT BooleanNullXorJava(null, null)', :runs) as pljava,
    plbench('SELECT BooleanNullXorPerl(null, null)', :runs) as plperl,
    plbench('SELECT BooleanNullXorLua(null, null)', :runs) as pllua,
    plbench('SELECT BooleanNullXorTcl(null, null)', :runs) as pltcl,
    plbench('SELECT BooleanNullXorR(null, null)', :runs) as plr;
