\i tests/benchmark/includes.sql

\i tests/benchmark/csharp/testnullintegers.sql
\i tests/benchmark/fsharp/testfsnullintegers.sql
\i tests/benchmark/v8javascript/testnullintegers.sql
\i tests/benchmark/python/testnullintegers.sql
\i tests/benchmark/pgsql/testnullintegers.sql
\i tests/benchmark/java/testnullintegers.sql
\i tests/benchmark/perl/testnullintegers.sql
\i tests/benchmark/lua/testnullintegers.sql
\i tests/benchmark/tcl/testnullintegers.sql
\i tests/benchmark/r/testnullintegers.sql

\set runs NRUNS

SELECT
    'returnNullInt',
    'NullInteger',
    plbench('SELECT returnNullInt()', :runs) as plcsharp,
    plbench('SELECT returnNullIntFSharp()', :runs) as plfsharp,
    plbench('SELECT returnNullIntV8()', :runs) as plv8,
    plbench('SELECT returnNullIntPython()', :runs) as plpython,
    plbench('SELECT returnNullIntPg()', :runs) as plpgsql,
    plbench('SELECT returnNullIntJava()', :runs) as pljava,
    plbench('SELECT returnNullIntPerl()', :runs) as plperl,
    plbench('SELECT returnNullIntLua()', :runs) as pllua,
    plbench('SELECT returnNullIntTcl()', :runs) as pltcl,
    plbench('SELECT returnNullIntR()', :runs) as plr;

SELECT
    'returnNullSmallInt',
    'NullInteger',
    plbench('SELECT returnNullSmallInt()', :runs) as plcsharp,
    plbench('SELECT returnNullSmallIntFSharp()', :runs) as plfsharp,
    plbench('SELECT returnNullSmallIntV8()', :runs) as plv8,
    plbench('SELECT returnNullSmallIntPython()', :runs) as plpython,
    plbench('SELECT returnNullSmallIntPg()', :runs) as plpgsql,
    plbench('SELECT returnNullSmallIntJava()', :runs) as pljava,
    plbench('SELECT returnNullSmallIntPerl()', :runs) as plperl,
    plbench('SELECT returnNullSmallIntLua()', :runs) as pllua,
    plbench('SELECT returnNullSmallIntTcl()', :runs) as pltcl,
    plbench('SELECT returnNullSmallIntR()', :runs) as plr;

SELECT
    'returnNullBigInt',
    'NullInteger',
    plbench('SELECT returnNullBigInt()', :runs) as plcsharp,
    plbench('SELECT returnNullBigIntFSharp()', :runs) as plfsharp,
    plbench('SELECT returnNullBigIntV8()', :runs) as plv8,
    plbench('SELECT returnNullBigIntPython()', :runs) as plpython,
    plbench('SELECT returnNullBigIntPg()', :runs) as plpgsql,
    plbench('SELECT returnNullBigIntJava()', :runs) as pljava,
    plbench('SELECT returnNullBigIntPerl()', :runs) as plperl,
    plbench('SELECT returnNullBigIntLua()', :runs) as pllua,
    plbench('SELECT returnNullBigIntTcl()', :runs) as pltcl,
    plbench('SELECT returnNullBigIntR()', :runs) as plr;

SELECT
    'sumNullArgInt-1',
    'NullInteger',
    plbench('SELECT sumNullArgInt(null,null)', :runs) as plcsharp,
    plbench('SELECT sumNullArgIntFSharp(null,null)', :runs) as plfsharp,
    plbench('SELECT sumNullArgIntV8(null,null)', :runs) as plv8,
    plbench('SELECT sumNullArgIntPython(null,null)', :runs) as plpython,
    plbench('SELECT sumNullArgIntPg(null,null)', :runs) as plpgsql,
    plbench('SELECT sumNullArgIntJava(null,null)', :runs) as pljava,
    plbench('SELECT sumNullArgIntPerl(null,null)', :runs) as plperl,
    plbench('SELECT sumNullArgIntLua(null,null)', :runs) as pllua,
    plbench('SELECT sumNullArgIntTcl(null,null)', :runs) as pltcl,
    plbench('SELECT sumNullArgIntR(null,null)', :runs) as plr;

SELECT
    'sumNullArgInt-2',
    'NullInteger',
    plbench('SELECT sumNullArgInt(null,3)', :runs) as plcsharp,
    plbench('SELECT sumNullArgIntFSharp(null,3)', :runs) as plfsharp,
    plbench('SELECT sumNullArgIntV8(null,3)', :runs) as plv8,
    plbench('SELECT sumNullArgIntPython(null,3)', :runs) as plpython,
    plbench('SELECT sumNullArgIntPg(null,3)', :runs) as plpgsql,
    plbench('SELECT sumNullArgIntJava(null,3)', :runs) as pljava,
    plbench('SELECT sumNullArgIntPerl(null,3)', :runs) as plperl,
    plbench('SELECT sumNullArgIntLua(null,3)', :runs) as pllua,
    plbench('SELECT sumNullArgIntTcl(null,3)', :runs) as pltcl,
    plbench('SELECT sumNullArgIntR(null,3)', :runs) as plr;

SELECT
    'sumNullArgInt-3',
    'NullInteger',
    plbench('SELECT sumNullArgInt(3,null)', :runs) as plcsharp,
    plbench('SELECT sumNullArgIntFSharp(3,null)', :runs) as plfsharp,
    plbench('SELECT sumNullArgIntV8(3,null)', :runs) as plv8,
    plbench('SELECT sumNullArgIntPython(3,null)', :runs) as plpython,
    plbench('SELECT sumNullArgIntPg(3,null)', :runs) as plpgsql,
    plbench('SELECT sumNullArgIntJava(3,null)', :runs) as pljava,
    plbench('SELECT sumNullArgIntPerl(3,null)', :runs) as plperl,
    plbench('SELECT sumNullArgIntLua(3,null)', :runs) as pllua,
    plbench('SELECT sumNullArgIntTcl(3,null)', :runs) as pltcl,
    plbench('SELECT sumNullArgIntR(3,null)', :runs) as plr;

SELECT
    'sumNullArgInt-4',
    'NullInteger',
    plbench('SELECT sumNullArgInt(3,3)', :runs) as plcsharp,
    plbench('SELECT sumNullArgIntFSharp(3,3)', :runs) as plfsharp,
    plbench('SELECT sumNullArgIntV8(3,3)', :runs) as plv8,
    plbench('SELECT sumNullArgIntPython(3,3)', :runs) as plpython,
    plbench('SELECT sumNullArgIntPg(3,3)', :runs) as plpgsql,
    plbench('SELECT sumNullArgIntJava(3,3)', :runs) as pljava,
    plbench('SELECT sumNullArgIntPerl(3,3)', :runs) as plperl,
    plbench('SELECT sumNullArgIntLua(3,3)', :runs) as pllua,
    plbench('SELECT sumNullArgIntTcl(3,3)', :runs) as pltcl,
    plbench('SELECT sumNullArgIntR(3,3)', :runs) as plr;

SELECT
    'sumNullArgSmallInt-1',
    'NullInteger',
    plbench('SELECT sumNullArgSmallInt(null,null)', :runs) as plcsharp,
    plbench('SELECT sumNullArgSmallIntFSharp(null,null)', :runs) as plfsharp,
    plbench('SELECT sumNullArgSmallIntV8(null,null)', :runs) as plv8,
    plbench('SELECT sumNullArgSmallIntPython(null,null)', :runs) as plpython,
    plbench('SELECT sumNullArgSmallIntPg(null,null)', :runs) as plpgsql,
    plbench('SELECT sumNullArgSmallIntJava(null,null)', :runs) as pljava,
    plbench('SELECT sumNullArgSmallIntPerl(null,null)', :runs) as plperl,
    plbench('SELECT sumNullArgSmallIntLua(null,null)', :runs) as pllua,
    plbench('SELECT sumNullArgSmallIntTcl(null,null)', :runs) as pltcl,
    plbench('SELECT sumNullArgSmallIntR(null,null)', :runs) as plr;

SELECT
    'sumNullArgSmallInt-2',
    'NullInteger',
    plbench('SELECT sumNullArgSmallInt(null,CAST(101 AS smallint))', :runs) as plcsharp,
    plbench('SELECT sumNullArgSmallIntFSharp(null,CAST(101 AS smallint))', :runs) as plfsharp,
    plbench('SELECT sumNullArgSmallIntV8(null,CAST(101 AS smallint))', :runs) as plv8,
    plbench('SELECT sumNullArgSmallIntPython(null,CAST(101 AS smallint))', :runs) as plpython,
    plbench('SELECT sumNullArgSmallIntPg(null,CAST(101 AS smallint))', :runs) as plpgsql,
    plbench('SELECT sumNullArgSmallIntJava(null,CAST(101 AS smallint))', :runs) as pljava,
    plbench('SELECT sumNullArgSmallIntPerl(null,CAST(101 AS smallint))', :runs) as plperl,
    plbench('SELECT sumNullArgSmallIntLua(null,CAST(101 AS smallint))', :runs) as pllua,
    plbench('SELECT sumNullArgSmallIntTcl(null,CAST(101 AS smallint))', :runs) as pltcl,
    plbench('SELECT sumNullArgSmallIntR(null,CAST(101 AS smallint))', :runs) as plr;

SELECT
    'sumNullArgSmallInt-3',
    'NullInteger',
    plbench('SELECT sumNullArgSmallInt(CAST(101 AS smallint),null)', :runs) as plcsharp,
    plbench('SELECT sumNullArgSmallIntFSharp(CAST(101 AS smallint),null)', :runs) as plfsharp,
    plbench('SELECT sumNullArgSmallIntV8(CAST(101 AS smallint),null)', :runs) as plv8,
    plbench('SELECT sumNullArgSmallIntPython(CAST(101 AS smallint),null)', :runs) as plpython,
    plbench('SELECT sumNullArgSmallIntPg(CAST(101 AS smallint),null)', :runs) as plpgsql,
    plbench('SELECT sumNullArgSmallIntJava(CAST(101 AS smallint),null)', :runs) as pljava,
    plbench('SELECT sumNullArgSmallIntPerl(CAST(101 AS smallint),null)', :runs) as plperl,
    plbench('SELECT sumNullArgSmallIntLua(CAST(101 AS smallint),null)', :runs) as pllua,
    plbench('SELECT sumNullArgSmallIntTcl(CAST(101 AS smallint),null)', :runs) as pltcl,
    plbench('SELECT sumNullArgSmallIntR(CAST(101 AS smallint),null)', :runs) as plr;

SELECT
    'sumNullArgSmallInt-4',
    'NullInteger',
    plbench('SELECT sumNullArgSmallInt(CAST(101 AS smallint),CAST(101 AS smallint))', :runs) as plcsharp,
    plbench('SELECT sumNullArgSmallIntFSharp(CAST(101 AS smallint),CAST(101 AS smallint))', :runs) as plfsharp,
    plbench('SELECT sumNullArgSmallIntV8(CAST(101 AS smallint),CAST(101 AS smallint))', :runs) as plv8,
    plbench('SELECT sumNullArgSmallIntPython(CAST(101 AS smallint),CAST(101 AS smallint))', :runs) as plpython,
    plbench('SELECT sumNullArgSmallIntPg(CAST(101 AS smallint),CAST(101 AS smallint))', :runs) as plpgsql,
    plbench('SELECT sumNullArgSmallIntJava(CAST(101 AS smallint),CAST(101 AS smallint))', :runs) as pljava,
    plbench('SELECT sumNullArgSmallIntPerl(CAST(101 AS smallint),CAST(101 AS smallint))', :runs) as plperl,
    plbench('SELECT sumNullArgSmallIntLua(CAST(101 AS smallint),CAST(101 AS smallint))', :runs) as pllua,
    plbench('SELECT sumNullArgSmallIntTcl(CAST(101 AS smallint),CAST(101 AS smallint))', :runs) as pltcl,
    plbench('SELECT sumNullArgSmallIntR(CAST(101 AS smallint),CAST(101 AS smallint))', :runs) as plr;

SELECT
    'sumNullArgBigInt-1',
    'NullInteger',
    plbench('SELECT sumNullArgBigInt(null,null)', :runs) as plcsharp,
    plbench('SELECT sumNullArgBigIntFSharp(null,null)', :runs) as plfsharp,
    plbench('SELECT sumNullArgBigIntV8(null,null)', :runs) as plv8,
    plbench('SELECT sumNullArgBigIntPython(null,null)', :runs) as plpython,
    plbench('SELECT sumNullArgBigIntPg(null,null)', :runs) as plpgsql,
    plbench('SELECT sumNullArgBigIntJava(null,null)', :runs) as pljava,
    plbench('SELECT sumNullArgBigIntPerl(null,null)', :runs) as plperl,
    plbench('SELECT sumNullArgBigIntLua(null,null)', :runs) as pllua,
    plbench('SELECT sumNullArgBigIntTcl(null,null)', :runs) as pltcl,
    plbench('SELECT sumNullArgBigIntR(null,null)', :runs) as plr;

SELECT
    'sumNullArgBigInt-2',
    'NullInteger',
    plbench('SELECT sumNullArgBigInt(null,100)', :runs) as plcsharp,
    plbench('SELECT sumNullArgBigIntFSharp(null,100)', :runs) as plfsharp,
    plbench('SELECT sumNullArgBigIntV8(null,100)', :runs) as plv8,
    plbench('SELECT sumNullArgBigIntPython(null,100)', :runs) as plpython,
    plbench('SELECT sumNullArgBigIntPg(null,100)', :runs) as plpgsql,
    plbench('SELECT sumNullArgBigIntJava(null,100)', :runs) as pljava,
    plbench('SELECT sumNullArgBigIntPerl(null,100)', :runs) as plperl,
    plbench('SELECT sumNullArgBigIntLua(null,100)', :runs) as pllua,
    plbench('SELECT sumNullArgBigIntTcl(null,100)', :runs) as pltcl,
    plbench('SELECT sumNullArgBigIntR(null,100)', :runs) as plr;

SELECT
    'checkedSumNullArgInt-1',
    'NullInteger',
    plbench('SELECT checkedSumNullArgInt(null,null)', :runs) as plcsharp,
    plbench('SELECT checkedSumNullArgIntFSharp(null,null)', :runs) as plfsharp,
    plbench('SELECT checkedSumNullArgIntV8(null,null)', :runs) as plv8,
    plbench('SELECT checkedSumNullArgIntPython(null,null)', :runs) as plpython,
    plbench('SELECT checkedSumNullArgIntPg(null,null)', :runs) as plpgsql,
    plbench('SELECT checkedSumNullArgIntJava(null,null)', :runs) as pljava,
    plbench('SELECT checkedSumNullArgIntPerl(null,null)', :runs) as plperl,
    plbench('SELECT checkedSumNullArgIntLua(null,null)', :runs) as pllua,
    plbench('SELECT checkedSumNullArgIntTcl(null,null)', :runs) as pltcl,
    plbench('SELECT checkedSumNullArgIntR(null,null)', :runs) as plr;

SELECT
    'checkedSumNullArgInt-2',
    'NullInteger',
    plbench('SELECT checkedSumNullArgInt(null,3)', :runs) as plcsharp,
    plbench('SELECT checkedSumNullArgIntFSharp(null,3)', :runs) as plfsharp,
    plbench('SELECT checkedSumNullArgIntV8(null,3)', :runs) as plv8,
    plbench('SELECT checkedSumNullArgIntPython(null,3)', :runs) as plpython,
    plbench('SELECT checkedSumNullArgIntPg(null,3)', :runs) as plpgsql,
    plbench('SELECT checkedSumNullArgIntJava(null,3)', :runs) as pljava,
    plbench('SELECT checkedSumNullArgIntPerl(null,3)', :runs) as plperl,
    plbench('SELECT checkedSumNullArgIntLua(null,3)', :runs) as pllua,
    plbench('SELECT checkedSumNullArgIntTcl(null,3)', :runs) as pltcl,
    plbench('SELECT checkedSumNullArgIntR(null,3)', :runs) as plr;

SELECT
    'checkedSumNullArgInt-3',
    'NullInteger',
    plbench('SELECT checkedSumNullArgInt(3,null)', :runs) as plcsharp,
    plbench('SELECT checkedSumNullArgIntFSharp(3,null)', :runs) as plfsharp,
    plbench('SELECT checkedSumNullArgIntV8(3,null)', :runs) as plv8,
    plbench('SELECT checkedSumNullArgIntPython(3,null)', :runs) as plpython,
    plbench('SELECT checkedSumNullArgIntPg(3,null)', :runs) as plpgsql,
    plbench('SELECT checkedSumNullArgIntJava(3,null)', :runs) as pljava,
    plbench('SELECT checkedSumNullArgIntPerl(3,null)', :runs) as plperl,
    plbench('SELECT checkedSumNullArgIntLua(3,null)', :runs) as pllua,
    plbench('SELECT checkedSumNullArgIntTcl(3,null)', :runs) as pltcl,
    plbench('SELECT checkedSumNullArgIntR(3,null)', :runs) as plr;

SELECT
    'checkedSumNullArgInt-4',
    'NullInteger',
    plbench('SELECT checkedSumNullArgInt(3,3)', :runs) as plcsharp,
    plbench('SELECT checkedSumNullArgIntFSharp(3,3)', :runs) as plfsharp,
    plbench('SELECT checkedSumNullArgIntV8(3,3)', :runs) as plv8,
    plbench('SELECT checkedSumNullArgIntPython(3,3)', :runs) as plpython,
    plbench('SELECT checkedSumNullArgIntPg(3,3)', :runs) as plpgsql,
    plbench('SELECT checkedSumNullArgIntJava(3,3)', :runs) as pljava,
    plbench('SELECT checkedSumNullArgIntPerl(3,3)', :runs) as plperl,
    plbench('SELECT checkedSumNullArgIntLua(3,3)', :runs) as pllua,
    plbench('SELECT checkedSumNullArgIntTcl(3,3)', :runs) as pltcl,
    plbench('SELECT checkedSumNullArgIntR(3,3)', :runs) as plr;

SELECT
    'checkedSumNullArgSmallInt-1',
    'NullInteger',
    plbench('SELECT checkedSumNullArgSmallInt(null,null)', :runs) as plcsharp,
    plbench('SELECT checkedSumNullArgSmallIntFSharp(null,null)', :runs) as plfsharp,
    plbench('SELECT checkedSumNullArgSmallIntV8(null,null)', :runs) as plv8,
    plbench('SELECT checkedSumNullArgSmallIntPython(null,null)', :runs) as plpython,
    plbench('SELECT checkedSumNullArgSmallIntPg(null,null)', :runs) as plpgsql,
    plbench('SELECT checkedSumNullArgSmallIntJava(null,null)', :runs) as pljava,
    plbench('SELECT checkedSumNullArgSmallIntPerl(null,null)', :runs) as plperl,
    plbench('SELECT checkedSumNullArgSmallIntLua(null,null)', :runs) as pllua,
    plbench('SELECT checkedSumNullArgSmallIntTcl(null,null)', :runs) as pltcl,
    plbench('SELECT checkedSumNullArgSmallIntR(null,null)', :runs) as plr;

SELECT
    'checkedSumNullArgSmallInt-2',
    'NullInteger',
    plbench('SELECT checkedSumNullArgSmallInt(null,CAST(133 AS smallint))', :runs) as plcsharp,
    plbench('SELECT checkedSumNullArgSmallIntFSharp(null,CAST(133 AS smallint))', :runs) as plfsharp,
    plbench('SELECT checkedSumNullArgSmallIntV8(null,CAST(133 AS smallint))', :runs) as plv8,
    plbench('SELECT checkedSumNullArgSmallIntPython(null,CAST(133 AS smallint))', :runs) as plpython,
    plbench('SELECT checkedSumNullArgSmallIntPg(null,CAST(133 AS smallint))', :runs) as plpgsql,
    plbench('SELECT checkedSumNullArgSmallIntJava(null,CAST(133 AS smallint))', :runs) as pljava,
    plbench('SELECT checkedSumNullArgSmallIntPerl(null,CAST(133 AS smallint))', :runs) as plperl,
    plbench('SELECT checkedSumNullArgSmallIntLua(null,CAST(133 AS smallint))', :runs) as pllua,
    plbench('SELECT checkedSumNullArgSmallIntTcl(null,CAST(133 AS smallint))', :runs) as pltcl,
    plbench('SELECT checkedSumNullArgSmallIntR(null,CAST(133 AS smallint))', :runs) as plr;

SELECT
    'checkedSumNullArgSmallInt-3',
    'NullInteger',
    plbench('SELECT checkedSumNullArgSmallInt(CAST(133 AS smallint),null)', :runs) as plcsharp,
    plbench('SELECT checkedSumNullArgSmallIntFSharp(CAST(133 AS smallint),null)', :runs) as plfsharp,
    plbench('SELECT checkedSumNullArgSmallIntV8(CAST(133 AS smallint),null)', :runs) as plv8,
    plbench('SELECT checkedSumNullArgSmallIntPython(CAST(133 AS smallint),null)', :runs) as plpython,
    plbench('SELECT checkedSumNullArgSmallIntPg(CAST(133 AS smallint),null)', :runs) as plpgsql,
    plbench('SELECT checkedSumNullArgSmallIntJava(CAST(133 AS smallint),null)', :runs) as pljava,
    plbench('SELECT checkedSumNullArgSmallIntPerl(CAST(133 AS smallint),null)', :runs) as plperl,
    plbench('SELECT checkedSumNullArgSmallIntLua(CAST(133 AS smallint),null)', :runs) as pllua,
    plbench('SELECT checkedSumNullArgSmallIntTcl(CAST(133 AS smallint),null)', :runs) as pltcl,
    plbench('SELECT checkedSumNullArgSmallIntR(CAST(133 AS smallint),null)', :runs) as plr;

SELECT
    'checkedSumNullArgSmallInt-4',
    'NullInteger',
    plbench('SELECT checkedSumNullArgSmallInt(CAST(133 AS smallint),CAST(133 AS smallint))', :runs) as plcsharp,
    plbench('SELECT checkedSumNullArgSmallIntFSharp(CAST(133 AS smallint),CAST(133 AS smallint))', :runs) as plfsharp,
    plbench('SELECT checkedSumNullArgSmallIntV8(CAST(133 AS smallint),CAST(133 AS smallint))', :runs) as plv8,
    plbench('SELECT checkedSumNullArgSmallIntPython(CAST(133 AS smallint),CAST(133 AS smallint))', :runs) as plpython,
    plbench('SELECT checkedSumNullArgSmallIntPg(CAST(133 AS smallint),CAST(133 AS smallint))', :runs) as plpgsql,
    plbench('SELECT checkedSumNullArgSmallIntJava(CAST(133 AS smallint),CAST(133 AS smallint))', :runs) as pljava,
    plbench('SELECT checkedSumNullArgSmallIntPerl(CAST(133 AS smallint),CAST(133 AS smallint))', :runs) as plperl,
    plbench('SELECT checkedSumNullArgSmallIntLua(CAST(133 AS smallint),CAST(133 AS smallint))', :runs) as pllua,
    plbench('SELECT checkedSumNullArgSmallIntTcl(CAST(133 AS smallint),CAST(133 AS smallint))', :runs) as pltcl,
    plbench('SELECT checkedSumNullArgSmallIntR(CAST(133 AS smallint),CAST(133 AS smallint))', :runs) as plr;

SELECT
    'checkedSumNullArgBigInt-1',
    'NullInteger',
    plbench('SELECT checkedSumNullArgBigInt(null,null)', :runs) as plcsharp,
    plbench('SELECT checkedSumNullArgBigIntFSharp(null,null)', :runs) as plfsharp,
    plbench('SELECT checkedSumNullArgBigIntV8(null,null)', :runs) as plv8,
    plbench('SELECT checkedSumNullArgBigIntPython(null,null)', :runs) as plpython,
    plbench('SELECT checkedSumNullArgBigIntPg(null,null)', :runs) as plpgsql,
    plbench('SELECT checkedSumNullArgBigIntJava(null,null)', :runs) as pljava,
    plbench('SELECT checkedSumNullArgBigIntPerl(null,null)', :runs) as plperl,
    plbench('SELECT checkedSumNullArgBigIntLua(null,null)', :runs) as pllua,
    plbench('SELECT checkedSumNullArgBigIntTcl(null,null)', :runs) as pltcl,
    plbench('SELECT checkedSumNullArgBigIntR(null,null)', :runs) as plr;

SELECT
    'checkedSumNullArgBigInt-2',
    'NullInteger',
    plbench('SELECT checkedSumNullArgBigInt(null,100)', :runs) as plcsharp,
    plbench('SELECT checkedSumNullArgBigIntFSharp(null,100)', :runs) as plfsharp,
    plbench('SELECT checkedSumNullArgBigIntV8(null,100)', :runs) as plv8,
    plbench('SELECT checkedSumNullArgBigIntPython(null,100)', :runs) as plpython,
    plbench('SELECT checkedSumNullArgBigIntPg(null,100)', :runs) as plpgsql,
    plbench('SELECT checkedSumNullArgBigIntJava(null,100)', :runs) as pljava,
    plbench('SELECT checkedSumNullArgBigIntPerl(null,100)', :runs) as plperl,
    plbench('SELECT checkedSumNullArgBigIntLua(null,100)', :runs) as pllua,
    plbench('SELECT checkedSumNullArgBigIntTcl(null,100)', :runs) as pltcl,
    plbench('SELECT checkedSumNullArgBigIntR(null,100)', :runs) as plr;

SELECT
    'checkedSumNullArgMixed-1',
    'NullInteger',
    plbench('SELECT checkedSumNullArgMixed(null,null,null)', :runs) as plcsharp,
    plbench('SELECT checkedSumNullArgMixedFSharp(null,null,null)', :runs) as plfsharp,
    plbench('SELECT checkedSumNullArgMixedV8(null,null,null)', :runs) as plv8,
    plbench('SELECT checkedSumNullArgMixedPython(null,null,null)', :runs) as plpython,
    plbench('SELECT checkedSumNullArgMixedPg(null,null,null)', :runs) as plpgsql,
    plbench('SELECT checkedSumNullArgMixedJava(null,null,null)', :runs) as pljava,
    plbench('SELECT checkedSumNullArgMixedPerl(null,null,null)', :runs) as plperl,
    plbench('SELECT checkedSumNullArgMixedLua(null,null,null)', :runs) as pllua,
    plbench('SELECT checkedSumNullArgMixedTcl(null,null,null)', :runs) as pltcl,
    plbench('SELECT checkedSumNullArgMixedR(null,null,null)', :runs) as plr;

SELECT
    'checkedSumNullArgMixed-2',
    'NullInteger',
    plbench('SELECT checkedSumNullArgMixed(null,CAST(1313 as smallint),null)', :runs) as plcsharp,
    plbench('SELECT checkedSumNullArgMixedFSharp(null,CAST(1313 as smallint),null)', :runs) as plfsharp,
    plbench('SELECT checkedSumNullArgMixedV8(null,CAST(1313 as smallint),null)', :runs) as plv8,
    plbench('SELECT checkedSumNullArgMixedPython(null,CAST(1313 as smallint),null)', :runs) as plpython,
    plbench('SELECT checkedSumNullArgMixedPg(null,CAST(1313 as smallint),null)', :runs) as plpgsql,
    plbench('SELECT checkedSumNullArgMixedJava(null,CAST(1313 as smallint),null)', :runs) as pljava,
    plbench('SELECT checkedSumNullArgMixedPerl(null,CAST(1313 as smallint),null)', :runs) as plperl,
    plbench('SELECT checkedSumNullArgMixedLua(null,CAST(1313 as smallint),null)', :runs) as pllua,
    plbench('SELECT checkedSumNullArgMixedTcl(null,CAST(1313 as smallint),null)', :runs) as pltcl,
    plbench('SELECT checkedSumNullArgMixedR(null,CAST(1313 as smallint),null)', :runs) as plr;

SELECT
    'checkedSumNullArgMixed-3',
    'NullInteger',
    plbench('SELECT checkedSumNullArgMixed(1313,null,null)', :runs) as plcsharp,
    plbench('SELECT checkedSumNullArgMixedFSharp(1313,null,null)', :runs) as plfsharp,
    plbench('SELECT checkedSumNullArgMixedV8(1313,null,null)', :runs) as plv8,
    plbench('SELECT checkedSumNullArgMixedPython(1313,null,null)', :runs) as plpython,
    plbench('SELECT checkedSumNullArgMixedPg(1313,null,null)', :runs) as plpgsql,
    plbench('SELECT checkedSumNullArgMixedJava(1313,null,null)', :runs) as pljava,
    plbench('SELECT checkedSumNullArgMixedPerl(1313,null,null)', :runs) as plperl,
    plbench('SELECT checkedSumNullArgMixedLua(1313,null,null)', :runs) as pllua,
    plbench('SELECT checkedSumNullArgMixedTcl(1313,null,null)', :runs) as pltcl,
    plbench('SELECT checkedSumNullArgMixedR(1313,null,null)', :runs) as plr;

SELECT
    'checkedSumNullArgMixed-4',
    'NullInteger',
    plbench('SELECT checkedSumNullArgMixed(null,null,3)', :runs) as plcsharp,
    plbench('SELECT checkedSumNullArgMixedFSharp(null,null,3)', :runs) as plfsharp,
    plbench('SELECT checkedSumNullArgMixedV8(null,null,3)', :runs) as plv8,
    plbench('SELECT checkedSumNullArgMixedPython(null,null,3)', :runs) as plpython,
    plbench('SELECT checkedSumNullArgMixedPg(null,null,3)', :runs) as plpgsql,
    plbench('SELECT checkedSumNullArgMixedJava(null,null,3)', :runs) as pljava,
    plbench('SELECT checkedSumNullArgMixedPerl(null,null,3)', :runs) as plperl,
    plbench('SELECT checkedSumNullArgMixedLua(null,null,3)', :runs) as pllua,
    plbench('SELECT checkedSumNullArgMixedTcl(null,null,3)', :runs) as pltcl,
    plbench('SELECT checkedSumNullArgMixedR(null,null,3)', :runs) as plr;

SELECT
    'checkedSumNullArgMixed-5',
    'NullInteger',
    plbench('SELECT checkedSumNullArgMixed(1313,CAST(1313 as smallint), 1313)', :runs) as plcsharp,
    plbench('SELECT checkedSumNullArgMixedFSharp(1313,CAST(1313 as smallint), 1313)', :runs) as plfsharp,
    plbench('SELECT checkedSumNullArgMixedV8(1313,CAST(1313 as smallint), 1313)', :runs) as plv8,
    plbench('SELECT checkedSumNullArgMixedPython(1313,CAST(1313 as smallint), 1313)', :runs) as plpython,
    plbench('SELECT checkedSumNullArgMixedPg(1313,CAST(1313 as smallint), 1313)', :runs) as plpgsql,
    plbench('SELECT checkedSumNullArgMixedJava(1313,CAST(1313 as smallint), 1313)', :runs) as pljava,
    plbench('SELECT checkedSumNullArgMixedPerl(1313,CAST(1313 as smallint), 1313)', :runs) as plperl,
    plbench('SELECT checkedSumNullArgMixedLua(1313,CAST(1313 as smallint), 1313)', :runs) as pllua,
    plbench('SELECT checkedSumNullArgMixedTcl(1313,CAST(1313 as smallint), 1313)', :runs) as pltcl,
    plbench('SELECT checkedSumNullArgMixedR(1313,CAST(1313 as smallint), 1313)', :runs) as plr;
