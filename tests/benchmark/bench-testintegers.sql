\i tests/benchmark/includes.sql

\i tests/benchmark/csharp/testintegers.sql
\i tests/benchmark/fsharp/testfsintegers.sql
\i tests/benchmark/v8javascript/testintegers.sql
\i tests/benchmark/python/testintegers.sql
\i tests/benchmark/pgsql/testintegers.sql
\i tests/benchmark/java/testintegers.sql
\i tests/benchmark/perl/testintegers.sql
\i tests/benchmark/lua/testintegers.sql
\i tests/benchmark/tcl/testintegers.sql
\i tests/benchmark/r/testintegers.sql

\set runs NRUNS

SELECT
    'maxSmallInt',
    'Integers',
    plbench('SELECT maxSmallInt()', :runs) as plcsharp,
    plbench('SELECT maxSmallIntFSharp()', :runs) as plfsharp,
    plbench('SELECT maxSmallIntV8()', :runs) as plv8,
    plbench('SELECT maxSmallIntPython()', :runs) as plpython,
    plbench('SELECT maxSmallIntPg()', :runs) as plpgsql,
    plbench('SELECT maxSmallIntJava()', :runs) as pljava,
    plbench('SELECT maxSmallIntPerl()', :runs) as plperl,
    plbench('SELECT maxSmallIntLua()', :runs) as pllua,
    plbench('SELECT maxSmallIntTcl()', :runs) as pltcl,
    plbench('SELECT maxSmallIntR()', :runs) as plr;

SELECT
    'sum2SmallInt',
    'Integers',
    plbench('SELECT sum2SmallInt(CAST(100 AS smallint), CAST(101 AS smallint))', :runs) as plcsharp,
    plbench('SELECT sum2SmallIntFSharp(CAST(100 AS smallint), CAST(101 AS smallint))', :runs) as plfsharp,
    plbench('SELECT sum2SmallIntV8(CAST(100 AS smallint), CAST(101 AS smallint))', :runs) as plv8,
    plbench('SELECT sum2SmallIntPython(CAST(100 AS smallint), CAST(101 AS smallint))', :runs) as plpython,
    plbench('SELECT sum2SmallIntPg(CAST(100 AS smallint), CAST(101 AS smallint))', :runs) as plpgsql,
    plbench('SELECT sum2SmallIntJava(CAST(100 AS smallint), CAST(101 AS smallint))', :runs) as pljava,
    plbench('SELECT sum2SmallIntPerl(CAST(100 AS smallint), CAST(101 AS smallint))', :runs) as plperl,
    plbench('SELECT sum2SmallIntLua(CAST(100 AS smallint), CAST(101 AS smallint))', :runs) as pllua,
    plbench('SELECT sum2SmallIntTcl(CAST(100 AS smallint), CAST(101 AS smallint))', :runs) as pltcl,
    plbench('SELECT sum2SmallIntR(CAST(100 AS smallint), CAST(101 AS smallint))', :runs) as plr;

SELECT
    'maxInteger',
    'Integers',
    plbench('SELECT maxInteger()', :runs) as plcsharp,
    plbench('SELECT maxIntegerFSharp()', :runs) as plfsharp,
    plbench('SELECT maxIntegerV8()', :runs) as plv8,
    plbench('SELECT maxIntegerPython()', :runs) as plpython,
    plbench('SELECT maxIntegerPg()', :runs) as plpgsql,
    plbench('SELECT maxIntegerJava()', :runs) as pljava,
    plbench('SELECT maxIntegerPerl()', :runs) as plperl,
    plbench('SELECT maxIntegerLua()', :runs) as pllua,
    plbench('SELECT maxIntegerTcl()', :runs) as pltcl,
    plbench('SELECT maxIntegerR()', :runs) as plr;

SELECT
    'returnInt',
    'Integers',
    plbench('SELECT returnInt()', :runs) as plcsharp,
    plbench('SELECT returnIntFSharp()', :runs) as plfsharp,
    plbench('SELECT returnIntV8()', :runs) as plv8,
    plbench('SELECT returnIntPython()', :runs) as plpython,
    plbench('SELECT returnIntPg()', :runs) as plpgsql,
    plbench('SELECT returnIntJava()', :runs) as pljava,
    plbench('SELECT returnIntPerl()', :runs) as plperl,
    plbench('SELECT returnIntLua()', :runs) as pllua,
    plbench('SELECT returnIntTcl()', :runs) as pltcl,
    plbench('SELECT returnIntR()', :runs) as plr;

SELECT
    'inc2ToIntToInt',
    'Integers',
    plbench('SELECT inc2ToInt(8)', :runs) as plcsharp,
    plbench('SELECT inc2ToIntFSharp(8)', :runs) as plfsharp,
    plbench('SELECT inc2ToIntV8(8)', :runs) as plv8,
    plbench('SELECT inc2ToIntPython(8)', :runs) as plpython,
    plbench('SELECT inc2ToIntPg(8)', :runs) as plpgsql,
    plbench('SELECT inc2ToIntJava(8)', :runs) as pljava,
    plbench('SELECT inc2ToIntPerl(8)', :runs) as plperl,
    plbench('SELECT inc2ToIntLua(8)', :runs) as pllua,
    plbench('SELECT inc2ToIntTcl(8)', :runs) as pltcl,
    plbench('SELECT inc2ToIntR(8)', :runs) as plr;

SELECT
    'sum2Integer',
    'Integers',
    plbench('SELECT sum2Integer(32770, 100)', :runs) as plcsharp,
    plbench('SELECT sum2IntegerFSharp(32770, 100)', :runs) as plfsharp,
    plbench('SELECT sum2IntegerV8(32770, 100)', :runs) as plv8,
    plbench('SELECT sum2IntegerPython(32770, 100)', :runs) as plpython,
    plbench('SELECT sum2IntegerPg(32770, 100)', :runs) as plpgsql,
    plbench('SELECT sum2IntegerJava(32770, 100)', :runs) as pljava,
    plbench('SELECT sum2IntegerPerl(32770, 100)', :runs) as plperl,
    plbench('SELECT sum2IntegerLua(32770, 100)', :runs) as pllua,
    plbench('SELECT sum2IntegerTcl(32770, 100)', :runs) as pltcl,
    plbench('SELECT sum2IntegerR(32770, 100)', :runs) as plr;

SELECT
    'sum3IntegerInteger',
    'Integers',
    plbench('SELECT sum3Integer(3,2,1)', :runs) as plcsharp,
    plbench('SELECT sum3IntegerFSharp(3,2,1)', :runs) as plfsharp,
    plbench('SELECT sum3IntegerV8(3,2,1)', :runs) as plv8,
    plbench('SELECT sum3IntegerPython(3,2,1)', :runs) as plpython,
    plbench('SELECT sum3IntegerPg(3,2,1)', :runs) as plpgsql,
    plbench('SELECT sum3IntegerJava(3,2,1)', :runs) as pljava,
    plbench('SELECT sum3IntegerPerl(3,2,1)', :runs) as plperl,
    plbench('SELECT sum3IntegerLua(3,2,1)', :runs) as pllua,
    plbench('SELECT sum3IntegerTcl(3,2,1)', :runs) as pltcl,
    plbench('SELECT sum3IntegerR(3,2,1)', :runs) as plr;

SELECT
    'sum4IntegerInteger',
    'Integers',
    plbench('SELECT sum4Integer(4,3,2,1)', :runs) as plcsharp,
    plbench('SELECT sum4IntegerFSharp(4,3,2,1)', :runs) as plfsharp,
    plbench('SELECT sum4IntegerV8(4,3,2,1)', :runs) as plv8,
    plbench('SELECT sum4IntegerPython(4,3,2,1)', :runs) as plpython,
    plbench('SELECT sum4IntegerPg(4,3,2,1)', :runs) as plpgsql,
    plbench('SELECT sum4IntegerJava(4,3,2,1)', :runs) as pljava,
    plbench('SELECT sum4IntegerPerl(4,3,2,1)', :runs) as plperl,
    plbench('SELECT sum4IntegerLua(4,3,2,1)', :runs) as pllua,
    plbench('SELECT sum4IntegerTcl(4,3,2,1)', :runs) as pltcl,
    plbench('SELECT sum4IntegerR(4,3,2,1)', :runs) as plr;

SELECT
    'maxBigInt',
    'Integers',
    plbench('SELECT maxBigInt()', :runs) as plcsharp,
    plbench('SELECT maxBigIntFSharp()', :runs) as plfsharp,
    plbench('SELECT maxBigIntV8()', :runs) as plv8,
    plbench('SELECT maxBigIntPython()', :runs) as plpython,
    plbench('SELECT maxBigIntPg()', :runs) as plpgsql,
    plbench('SELECT maxBigIntJava()', :runs) as pljava,
    plbench('SELECT maxBigIntPerl()', :runs) as plperl,
    plbench('SELECT maxBigIntLua()', :runs) as pllua,
    plbench('SELECT maxBigIntTcl()', :runs) as pltcl,
    plbench('SELECT maxBigIntR()', :runs) as plr;

SELECT
    'mixedInt',
    'Integers',
    plbench('SELECT mixedInt(CAST(32767 AS smallint),  CAST(32767 AS smallint), 100)', :runs) as plcsharp,
    plbench('SELECT mixedIntFSharp(CAST(32767 AS smallint),  CAST(32767 AS smallint), 100)', :runs) as plfsharp,
    plbench('SELECT mixedIntV8(CAST(32767 AS smallint),  CAST(32767 AS smallint), 100)', :runs) as plv8,
    plbench('SELECT mixedIntPython(CAST(32767 AS smallint),  CAST(32767 AS smallint), 100)', :runs) as plpython,
    plbench('SELECT mixedIntPg(CAST(32767 AS smallint),  CAST(32767 AS smallint), 100)', :runs) as plpgsql,
    plbench('SELECT mixedIntJava(CAST(32767 AS smallint),  CAST(32767 AS smallint), 100)', :runs) as pljava,
    plbench('SELECT mixedIntPerl(CAST(32767 AS smallint),  CAST(32767 AS smallint), 100)', :runs) as plperl,
    plbench('SELECT mixedIntLua(CAST(32767 AS smallint),  CAST(32767 AS smallint), 100)', :runs) as pllua,
    plbench('SELECT mixedIntTcl(CAST(32767 AS smallint),  CAST(32767 AS smallint), 100)', :runs) as pltcl,
    plbench('SELECT mixedIntR(CAST(32767 AS smallint),  CAST(32767 AS smallint), 100)', :runs) as plr;
