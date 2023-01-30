CREATE OR REPLACE FUNCTION retVarCharFSharp(fname varchar) RETURNS varchar AS $$
fname + " Cabral"
$$ LANGUAGE plfsharp;
SELECT retVarCharFSharp('Rafael') = varchar 'Rafael Cabral';

CREATE OR REPLACE FUNCTION retConcatVarCharFSharp(fname varchar, lname varchar) RETURNS varchar AS $$
fname + lname
$$ LANGUAGE plfsharp;
SELECT retConcatVarCharFSharp('João ', 'da Silva') = varchar 'João da Silva';

CREATE OR REPLACE FUNCTION retConcatTextFSharp(fname text, lname text) RETURNS text AS $$
("Hello " + fname + lname + "!")
$$ LANGUAGE plfsharp;
SELECT retConcatTextFSharp('João ', 'da Silva') =  varchar 'Hello João da Silva!';

CREATE OR REPLACE FUNCTION retVarCharTextFSharp(fname varchar, lname varchar) RETURNS text AS $$
"Hello " + fname + lname + "!"
$$ LANGUAGE plfsharp;
SELECT retVarCharTextFSharp('Homer Jay ', 'Simpson') = varchar 'Hello Homer Jay Simpson!';

CREATE OR REPLACE FUNCTION retCharFSharp(argchar character) RETURNS character AS $$
argchar
$$ LANGUAGE plfsharp;
SELECT retCharFSharp('R') =  character 'R';

CREATE OR REPLACE FUNCTION retConcatLettersFSharp(a character, b character) RETURNS varchar AS $$
a + b
$$ LANGUAGE plfsharp;
SELECT retConcatLettersFSharp('R', 'C') = varchar 'RC';

CREATE OR REPLACE FUNCTION retConcatCharsFSharp(a char(5), b char(7)) RETURNS char(5) AS $$
a + b
$$ LANGUAGE plfsharp;
SELECT retConcatCharsFSharp('H.', 'Simpson') = 'H.Simpson';

CREATE OR REPLACE FUNCTION retConcatVarCharsFSharp(a varchar(5), b varchar(7)) RETURNS varchar(5) AS $$
a + b
$$ LANGUAGE plfsharp;
SELECT retConcatVarCharsFSharp('H.', 'Simpson') = 'H.Simpson';
SELECT retConcatVarCharsFSharp('H. あ', 'Simpson') = 'H. あSimpson';

CREATE OR REPLACE FUNCTION retNonRegularEncodingFSharp(a varchar) RETURNS varchar AS $$
a
$$ LANGUAGE plfsharp;
SELECT retNonRegularEncodingFSharp('漢字') = varchar '漢字';
SELECT retNonRegularEncodingFSharp('ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ') = varchar 'ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ';
SELECT retNonRegularEncodingFSharp('ŁĄŻĘĆŃŚŹ') = varchar 'ŁĄŻĘĆŃŚŹ';
SELECT retNonRegularEncodingFSharp('Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.')
    = varchar 'Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.';
