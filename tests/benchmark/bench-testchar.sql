\i tests/benchmark/includes.sql

\i tests/benchmark/csharp/testchar.sql
\i tests/benchmark/fsharp/testfschar.sql
\i tests/benchmark/v8javascript/testchar.sql
\i tests/benchmark/python/testchar.sql
\i tests/benchmark/pgsql/testchar.sql
\i tests/benchmark/java/testchar.sql
\i tests/benchmark/perl/testchar.sql
\i tests/benchmark/lua/testchar.sql
\i tests/benchmark/tcl/testchar.sql
\i tests/benchmark/r/testchar.sql

\set runs NRUNS

SELECT
    'retVarChar',
    'Char',
    plbench('SELECT retVarChar(''Rodrigo'')', :runs) as plcsharp,
    plbench('SELECT retVarCharFSharp(''Rodrigo'')', :runs) as plfsharp,
    plbench('SELECT retVarCharV8(''Rodrigo'')', :runs) as plv8,
    plbench('SELECT retVarCharPython(''Rodrigo'')', :runs) as plpython,
    plbench('SELECT retVarCharPg(''Rodrigo'')', :runs) as plpgsql,
    plbench('SELECT retVarCharJava(''Rodrigo'')', :runs) as pljava,
    plbench('SELECT retVarCharPerl(''Rodrigo'')', :runs) as plperl,
    plbench('SELECT retVarCharLua(''Rodrigo'')', :runs) as pllua,
    plbench('SELECT retVarCharTcl(''Rodrigo'')', :runs) as pltcl,
    plbench('SELECT retVarCharR(''Rodrigo'')', :runs) as plr;

SELECT
    'retConcatVarChar',
    'Char',
    plbench('SELECT retConcatVarChar(''João '', ''da Silva'')', :runs) as plcsharp,
    plbench('SELECT retConcatVarCharFSharp(''João '', ''da Silva'')', :runs) as plfsharp,
    plbench('SELECT retConcatVarCharV8(''João '', ''da Silva'')', :runs) as plv8,
    plbench('SELECT retConcatVarCharPython(''João '', ''da Silva'')', :runs) as plpython,
    plbench('SELECT retConcatVarCharPg(''João '', ''da Silva'')', :runs) as plpgsql,
    plbench('SELECT retConcatVarCharJava(''João '', ''da Silva'')', :runs) as pljava,
    plbench('SELECT retConcatVarCharPerl(''João '', ''da Silva'')', :runs) as plperl,
    plbench('SELECT retConcatVarCharLua(''João '', ''da Silva'')', :runs) as pllua,
    plbench('SELECT retConcatVarCharTcl(''João '', ''da Silva'')', :runs) as pltcl,
    plbench('SELECT retConcatVarCharR(''João '', ''da Silva'')', :runs) as plr;

SELECT
    'retConcatText',
    'Char',
    plbench('SELECT retConcatText(''João '', ''da Silva'')', :runs) as plcsharp,
    plbench('SELECT retConcatTextFSharp(''João '', ''da Silva'')', :runs) as plfsharp,
    plbench('SELECT retConcatTextV8(''João '', ''da Silva'')', :runs) as plv8,
    plbench('SELECT retConcatTextPython(''João '', ''da Silva'')', :runs) as plpython,
    plbench('SELECT retConcatTextPg(''João '', ''da Silva'')', :runs) as plpgsql,
    plbench('SELECT retConcatTextJava(''João '', ''da Silva'')', :runs) as pljava,
    plbench('SELECT retConcatTextPerl(''João '', ''da Silva'')', :runs) as plperl,
    plbench('SELECT retConcatTextLua(''João '', ''da Silva'')', :runs) as pllua,
    plbench('SELECT retConcatTextTcl(''João '', ''da Silva'')', :runs) as pltcl,
    plbench('SELECT retConcatTextR(''João '', ''da Silva'')', :runs) as plr;

SELECT
    'retVarCharText',
    'Char',
    plbench('SELECT retVarCharText(''Homer Jay '', ''Simpson'')', :runs) as plcsharp,
    plbench('SELECT retVarCharTextFSharp(''Homer Jay '', ''Simpson'')', :runs) as plfsharp,
    plbench('SELECT retVarCharTextV8(''Homer Jay '', ''Simpson'')', :runs) as plv8,
    plbench('SELECT retVarCharTextPython(''Homer Jay '', ''Simpson'')', :runs) as plpython,
    plbench('SELECT retVarCharTextPg(''Homer Jay '', ''Simpson'')', :runs) as plpgsql,
    plbench('SELECT retVarCharTextJava(''Homer Jay '', ''Simpson'')', :runs) as pljava,
    plbench('SELECT retVarCharTextPerl(''Homer Jay '', ''Simpson'')', :runs) as plperl,
    plbench('SELECT retVarCharTextLua(''Homer Jay '', ''Simpson'')', :runs) as pllua,
    plbench('SELECT retVarCharTextTcl(''Homer Jay '', ''Simpson'')', :runs) as pltcl,
    plbench('SELECT retVarCharTextR(''Homer Jay '', ''Simpson'')', :runs) as plr;

SELECT
    'retChar',
    'Char',
    plbench('SELECT retChar(''R'')', :runs) as plcsharp,
    plbench('SELECT retCharFSharp(''R'')', :runs) as plfsharp,
    plbench('SELECT retCharV8(''R'')', :runs) as plv8,
    plbench('SELECT retCharPython(''R'')', :runs) as plpython,
    plbench('SELECT retCharPg(''R'')', :runs) as plpgsql,
    plbench('SELECT retCharJava(''R'')', :runs) as pljava,
    plbench('SELECT retCharPerl(''R'')', :runs) as plperl,
    plbench('SELECT retCharLua(''R'')', :runs) as pllua,
    plbench('SELECT retCharTcl(''R'')', :runs) as pltcl,
    plbench('SELECT retCharR(''R'')', :runs) as plr;

SELECT
    'retConcatLetters',
    'Char',
    plbench('SELECT retConcatLetters(''R'', ''C'')', :runs) as plcsharp,
    plbench('SELECT retConcatLettersFSharp(''R'', ''C'')', :runs) as plfsharp,
    plbench('SELECT retConcatLettersV8(''R'', ''C'')', :runs) as plv8,
    plbench('SELECT retConcatLettersPython(''R'', ''C'')', :runs) as plpython,
    plbench('SELECT retConcatLettersPg(''R'', ''C'')', :runs) as plpgsql,
    plbench('SELECT retConcatLettersJava(''R'', ''C'')', :runs) as pljava,
    plbench('SELECT retConcatLettersPerl(''R'', ''C'')', :runs) as plperl,
    plbench('SELECT retConcatLettersLua(''R'', ''C'')', :runs) as pllua,
    plbench('SELECT retConcatLettersTcl(''R'', ''C'')', :runs) as pltcl,
    plbench('SELECT retConcatLettersR(''R'', ''C'')', :runs) as plr;

SELECT
    'retConcatChars',
    'Char',
    plbench('SELECT retConcatChars(''H.'', ''Simpson'')', :runs) as plcsharp,
    plbench('SELECT retConcatCharsFSharp(''H.'', ''Simpson'')', :runs) as plfsharp,
    plbench('SELECT retConcatCharsV8(''H.'', ''Simpson'')', :runs) as plv8,
    plbench('SELECT retConcatCharsPython(''H.'', ''Simpson'')', :runs) as plpython,
    plbench('SELECT retConcatCharsPg(''H.'', ''Simpson'')', :runs) as plpgsql,
    plbench('SELECT retConcatCharsJava(''H.'', ''Simpson'')', :runs) as pljava,
    plbench('SELECT retConcatCharsPerl(''H.'', ''Simpson'')', :runs) as plperl,
    plbench('SELECT retConcatCharsLua(''H.'', ''Simpson'')', :runs) as pllua,
    plbench('SELECT retConcatCharsTcl(''H.'', ''Simpson'')', :runs) as pltcl,
    plbench('SELECT retConcatCharsR(''H.'', ''Simpson'')', :runs) as plr;

SELECT
    'retConcatVarChars-1',
    'Char',
    plbench('SELECT retConcatVarChars(''H.'', ''Simpson'')', :runs) as plcsharp,
    plbench('SELECT retConcatVarCharsFSharp(''H.'', ''Simpson'')', :runs) as plfsharp,
    plbench('SELECT retConcatVarCharsV8(''H.'', ''Simpson'')', :runs) as plv8,
    plbench('SELECT retConcatVarCharsPython(''H.'', ''Simpson'')', :runs) as plpython,
    plbench('SELECT retConcatVarCharsPg(''H.'', ''Simpson'')', :runs) as plpgsql,
    plbench('SELECT retConcatVarCharsJava(''H.'', ''Simpson'')', :runs) as pljava,
    plbench('SELECT retConcatVarCharsPerl(''H.'', ''Simpson'')', :runs) as plperl,
    plbench('SELECT retConcatVarCharsLua(''H.'', ''Simpson'')', :runs) as pllua,
    plbench('SELECT retConcatVarCharsTcl(''H.'', ''Simpson'')', :runs) as pltcl,
    plbench('SELECT retConcatVarCharsR(''H.'', ''Simpson'')', :runs) as plr;

SELECT
    'retConcatVarChars-2',
    'Char',
    plbench('SELECT retConcatVarChars(''H. あ'', ''Simpson'')', :runs) as plcsharp,
    plbench('SELECT retConcatVarCharsFSharp(''H. あ'', ''Simpson'')', :runs) as plfsharp,
    plbench('SELECT retConcatVarCharsV8(''H. あ'', ''Simpson'')', :runs) as plv8,
    plbench('SELECT retConcatVarCharsPython(''H. あ'', ''Simpson'')', :runs) as plpython,
    plbench('SELECT retConcatVarCharsPg(''H. あ'', ''Simpson'')', :runs) as plpgsql,
    plbench('SELECT retConcatVarCharsJava(''H. あ'', ''Simpson'')', :runs) as pljava,
    plbench('SELECT retConcatVarCharsPerl(''H. あ'', ''Simpson'')', :runs) as plperl,
    plbench('SELECT retConcatVarCharsLua(''H. あ'', ''Simpson'')', :runs) as pllua,
    plbench('SELECT retConcatVarCharsTcl(''H. あ'', ''Simpson'')', :runs) as pltcl,
    plbench('SELECT retConcatVarCharsR(''H. あ'', ''Simpson'')', :runs) as plr;

SELECT
    'retNonRegularEncoding-1',
    'Char',
    plbench('SELECT retNonRegularEncoding(''漢字'')', :runs) as plcsharp,
    plbench('SELECT retNonRegularEncodingFsharp(''漢字'')', :runs) as plfsharp,
    plbench('SELECT retNonRegularEncodingV8(''漢字'')', :runs) as plv8,
    plbench('SELECT retNonRegularEncodingPython(''漢字'')', :runs) as plpython,
    plbench('SELECT retNonRegularEncodingPg(''漢字'')', :runs) as plpgsql,
    plbench('SELECT retNonRegularEncodingJava(''漢字'')', :runs) as pljava,
    plbench('SELECT retNonRegularEncodingPerl(''漢字'')', :runs) as plperl,
    plbench('SELECT retNonRegularEncodingLua(''漢字'')', :runs) as pllua,
    plbench('SELECT retNonRegularEncodingTcl(''漢字'')', :runs) as pltcl,
    plbench('SELECT retNonRegularEncodingR(''漢字'')', :runs) as plr;

SELECT
    'retNonRegularEncoding-2',
    'Char',
    plbench('SELECT retNonRegularEncoding(''ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ'')', :runs) as plcsharp,
    plbench('SELECT retNonRegularEncodingFSharp(''ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ'')', :runs) as plfsharp,
    plbench('SELECT retNonRegularEncodingV8(''ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ'')', :runs) as plv8,
    plbench('SELECT retNonRegularEncodingPython(''ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ'')', :runs) as plpython,
    plbench('SELECT retNonRegularEncodingPg(''ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ'')', :runs) as plpgsql,
    plbench('SELECT retNonRegularEncodingJava(''ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ'')', :runs) as pljava,
    plbench('SELECT retNonRegularEncodingPerl(''ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ'')', :runs) as plperl,
    plbench('SELECT retNonRegularEncodingLua(''ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ'')', :runs) as pllua,
    plbench('SELECT retNonRegularEncodingTcl(''ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ'')', :runs) as pltcl,
    plbench('SELECT retNonRegularEncodingR(''ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ'')', :runs) as plr;

SELECT
    'retNonRegularEncoding-3',
    'Char',
    plbench('SELECT retNonRegularEncoding(''ŁĄŻĘĆŃŚŹ'')', :runs) as plcsharp,
    plbench('SELECT retNonRegularEncodingFSharp(''ŁĄŻĘĆŃŚŹ'')', :runs) as plfsharp,
    plbench('SELECT retNonRegularEncodingV8(''ŁĄŻĘĆŃŚŹ'')', :runs) as plv8,
    plbench('SELECT retNonRegularEncodingPython(''ŁĄŻĘĆŃŚŹ'')', :runs) as plpython,
    plbench('SELECT retNonRegularEncodingPg(''ŁĄŻĘĆŃŚŹ'')', :runs) as plpgsql,
    plbench('SELECT retNonRegularEncodingJava(''ŁĄŻĘĆŃŚŹ'')', :runs) as pljava,
    plbench('SELECT retNonRegularEncodingPerl(''ŁĄŻĘĆŃŚŹ'')', :runs) as plperl,
    plbench('SELECT retNonRegularEncodingLua(''ŁĄŻĘĆŃŚŹ'')', :runs) as pllua,
    plbench('SELECT retNonRegularEncodingTcl(''ŁĄŻĘĆŃŚŹ'')', :runs) as pltcl,
    plbench('SELECT retNonRegularEncodingR(''ŁĄŻĘĆŃŚŹ'')', :runs) as plr;

SELECT
    'retNonRegularEncoding-4',
    'Char',
    plbench('SELECT retNonRegularEncoding(''Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.'')', :runs) as plcsharp,
    plbench('SELECT retNonRegularEncodingFSharp(''Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.'')', :runs) as plfsharp,
    plbench('SELECT retNonRegularEncodingV8(''Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.'')', :runs) as plv8,
    plbench('SELECT retNonRegularEncodingPython(''Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.'')', :runs) as plpython,
    plbench('SELECT retNonRegularEncodingPg(''Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.'')', :runs) as plpgsql,
    plbench('SELECT retNonRegularEncodingJava(''Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.'')', :runs) as pljava,
    plbench('SELECT retNonRegularEncodingPerl(''Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.'')', :runs) as plperl,
    plbench('SELECT retNonRegularEncodingLua(''Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.'')', :runs) as pllua,
    plbench('SELECT retNonRegularEncodingTcl(''Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.'')', :runs) as pltcl,
    plbench('SELECT retNonRegularEncodingR(''Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.'')', :runs) as plr;
