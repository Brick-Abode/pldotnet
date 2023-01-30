\i tests/benchmark/includes.sql

\i tests/benchmark/csharp/testmixedtypes.sql
\i tests/benchmark/fsharp/testfsmixedtypes.sql
\i tests/benchmark/v8javascript/testmixedtypes.sql
\i tests/benchmark/python/testmixedtypes.sql
\i tests/benchmark/pgsql/testmixedtypes.sql
\i tests/benchmark/java/testmixedtypes.sql
\i tests/benchmark/perl/testmixedtypes.sql
\i tests/benchmark/lua/testmixedtypes.sql
\i tests/benchmark/tcl/testmixedtypes.sql
\i tests/benchmark/r/testmixedtypes.sql

\set runs NRUNS

SELECT
    'ageTest-1',
    'Mixed',
    plbench('SELECT ageTest(''Billy'', 10, ''The KID'')', :runs) as plcsharp,
    plbench('SELECT ageTestFSharp(''Billy'', 10, ''The KID'')', :runs) as plfsharp,
    plbench('SELECT ageTestV8(''Billy'', 10, ''The KID'')', :runs) as plv8,
    plbench('SELECT ageTestPython(''Billy'', 10, ''The KID'')', :runs) as plpython,
    plbench('SELECT ageTestPg(''Billy'', 10, ''The KID'')', :runs) as plpgsql,
    plbench('SELECT ageTestJava(''Billy'', 10, ''The KID'')', :runs) as pljava,
    plbench('SELECT ageTestPerl(''Billy'', 10, ''The KID'')', :runs) as plperl,
    plbench('SELECT ageTestLua(''Billy'', 10, ''The KID'')', :runs) as pllua,
    plbench('SELECT ageTestTcl(''Billy'', 10, ''The KID'')', :runs) as pltcl,
    plbench('SELECT ageTestR(''Billy'', 10, ''The KID'')', :runs) as plr;

SELECT
    'ageTest-2',
    'Mixed',
    plbench('SELECT ageTest(''John'', 33, ''Smith'')', :runs) as plcsharp,
    plbench('SELECT ageTestFSharp(''John'', 33, ''Smith'')', :runs) as plfsharp,
    plbench('SELECT ageTestV8(''John'', 33, ''Smith'')', :runs) as plv8,
    plbench('SELECT ageTestPython(''John'', 33, ''Smith'')', :runs) as plpython,
    plbench('SELECT ageTestPg(''John'', 33, ''Smith'')', :runs) as plpgsql,
    plbench('SELECT ageTestJava(''John'', 33, ''Smith'')', :runs) as pljava,
    plbench('SELECT ageTestPerl(''John'', 33, ''Smith'')', :runs) as plperl,
    plbench('SELECT ageTestLua(''John'', 33, ''Smith'')', :runs) as pllua,
    plbench('SELECT ageTestTcl(''John'', 33, ''Smith'')', :runs) as pltcl,
    plbench('SELECT ageTestR(''John'', 33, ''Smith'')', :runs) as plr;

SELECT
    'ageTest-3',
    'Mixed',
    plbench('SELECT ageTest(''Robson'', 41, ''Cruzoe'')', :runs) as plcsharp,
    plbench('SELECT ageTestFSharp(''Robson'', 41, ''Cruzoe'')', :runs) as plfsharp,
    plbench('SELECT ageTestV8(''Robson'', 41, ''Cruzoe'')', :runs) as plv8,
    plbench('SELECT ageTestPython(''Robson'', 41, ''Cruzoe'')', :runs) as plpython,
    plbench('SELECT ageTestPg(''Robson'', 41, ''Cruzoe'')', :runs) as plpgsql,
    plbench('SELECT ageTestJava(''Robson'', 41, ''Cruzoe'')', :runs) as pljava,
    plbench('SELECT ageTestPerl(''Robson'', 41, ''Cruzoe'')', :runs) as plperl,
    plbench('SELECT ageTestLua(''Robson'', 41, ''Cruzoe'')', :runs) as pllua,
    plbench('SELECT ageTestTcl(''Robson'', 41, ''Cruzoe'')', :runs) as pltcl,
    plbench('SELECT ageTestR(''Robson'', 41, ''Cruzoe'')', :runs) as plr;
