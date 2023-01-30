CREATE OR REPLACE FUNCTION retVarCharV8(fname varchar) RETURNS varchar AS $$
return fname + " Lima";
$$ LANGUAGE plv8;
SELECT retVarCharV8('Rodrigo') = varchar 'Rodrigo Lima';

CREATE OR REPLACE FUNCTION retConcatVarCharV8(fname varchar, lname varchar) RETURNS varchar AS $$
return fname + lname;
$$ LANGUAGE plv8;
SELECT retConcatVarCharV8('João ', 'da Silva') = varchar 'João da Silva';

CREATE OR REPLACE FUNCTION retConcatTextV8(fname text, lname text) RETURNS text AS $$
return "Hello " + fname + lname + "!";
$$ LANGUAGE plv8;
SELECT retConcatTextV8('João ', 'da Silva') =  varchar 'Hello João da Silva!';

CREATE OR REPLACE FUNCTION retVarCharTextV8(fname varchar, lname varchar) RETURNS text AS $$
return "Hello " + fname + lname + "!";
$$ LANGUAGE plv8;
SELECT retVarCharTextV8('Homer Jay ', 'Simpson') = varchar 'Hello Homer Jay Simpson!';

CREATE OR REPLACE FUNCTION retCharV8(argchar character) RETURNS character AS $$
return argchar;
$$ LANGUAGE plv8;
SELECT retCharV8('R') =  character 'R';

CREATE OR REPLACE FUNCTION retConcatLettersV8(a character, b character) RETURNS varchar AS $$
return a + b;
$$ LANGUAGE plv8;
SELECT retConcatLettersV8('R', 'C') = varchar 'RC';

-- Problem here it is neither padding and truncating
CREATE OR REPLACE FUNCTION retConcatCharsV8(a char(5), b char(7)) RETURNS char(5) AS $$
return a + b;
$$ LANGUAGE plv8;
SELECT retConcatCharsV8('H.', 'Simpson') = 'H.Simpson';

CREATE OR REPLACE FUNCTION retConcatVarCharsV8(a varchar(5), b varchar(7)) RETURNS varchar(5) AS $$
return a + b;
$$ LANGUAGE plv8;
SELECT retConcatVarCharsV8('H.', 'Simpson') = 'H.Simpson';
SELECT retConcatVarCharsV8('H. あ', 'Simpson') = 'H. あSimpson';

CREATE OR REPLACE FUNCTION retNonRegularEncodingV8(a varchar) RETURNS varchar AS $$
return a;
$$ LANGUAGE plv8;
SELECT retNonRegularEncodingV8('漢字') = varchar '漢字';
SELECT retNonRegularEncodingV8('ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ') = varchar 'ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ';
SELECT retNonRegularEncodingV8('ŁĄŻĘĆŃŚŹ') = varchar 'ŁĄŻĘĆŃŚŹ';
SELECT retNonRegularEncodingV8('Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.')
    = varchar 'Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.';
