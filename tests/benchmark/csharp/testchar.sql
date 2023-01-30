CREATE OR REPLACE FUNCTION retVarChar(fname varchar) RETURNS varchar AS $$
return fname + " Cabral";
$$ LANGUAGE plcsharp;
SELECT retVarChar('Rafael') = varchar 'Rafael Cabral';

CREATE OR REPLACE FUNCTION retConcatVarChar(fname varchar, lname varchar) RETURNS varchar AS $$
return fname + lname;
$$ LANGUAGE plcsharp;
SELECT retConcatVarChar('João ', 'da Silva') = varchar 'João da Silva';

CREATE OR REPLACE FUNCTION retConcatText(fname text, lname text) RETURNS text AS $$
return "Hello " + fname + lname + "!";
$$ LANGUAGE plcsharp;
SELECT retConcatText('João ', 'da Silva') =  varchar 'Hello João da Silva!';

CREATE OR REPLACE FUNCTION retVarCharText(fname varchar, lname varchar) RETURNS text AS $$
return "Hello " + fname + lname + "!";
$$ LANGUAGE plcsharp;
SELECT retVarCharText('Homer Jay ', 'Simpson') = varchar 'Hello Homer Jay Simpson!';

CREATE OR REPLACE FUNCTION retChar(argchar character) RETURNS character AS $$
return argchar;
$$ LANGUAGE plcsharp;
SELECT retChar('R') =  character 'R';

CREATE OR REPLACE FUNCTION retConcatLetters(a character, b character) RETURNS varchar AS $$
return a + b;
$$ LANGUAGE plcsharp;
SELECT retConcatLetters('R', 'C') = varchar 'RC';

-- Problem here it is neither padding and truncating
CREATE OR REPLACE FUNCTION retConcatChars(a char(5), b char(7)) RETURNS char(5) AS $$
return a + b;
$$ LANGUAGE plcsharp;
SELECT retConcatChars('H.', 'Simpson') = 'H.Simpson';

CREATE OR REPLACE FUNCTION retConcatVarChars(a varchar(5), b varchar(7)) RETURNS varchar(5) AS $$
return a + b;
$$ LANGUAGE plcsharp;
SELECT retConcatVarChars('H.', 'Simpson') = 'H.Simpson';
SELECT retConcatVarChars('H. あ', 'Simpson') = 'H. あSimpson';

CREATE OR REPLACE FUNCTION retNonRegularEncoding(a varchar) RETURNS varchar AS $$
return a;
$$ LANGUAGE plcsharp;
SELECT retNonRegularEncoding('漢字') = varchar '漢字';
SELECT retNonRegularEncoding('ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ') = varchar 'ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ';
SELECT retNonRegularEncoding('ŁĄŻĘĆŃŚŹ') = varchar 'ŁĄŻĘĆŃŚŹ';
SELECT retNonRegularEncoding('Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.')
    = varchar 'Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.';
