CREATE OR REPLACE FUNCTION retVarCharPython(fname varchar) RETURNS varchar AS $$
return fname + " Lima"
$$ LANGUAGE plpython3u;
SELECT retVarCharPython('Rodrigo') = varchar 'Rodrigo Lima';

CREATE OR REPLACE FUNCTION retConcatVarCharPython(fname varchar, lname varchar) RETURNS varchar AS $$
return fname + lname
$$ LANGUAGE plpython3u;
SELECT retConcatVarCharPython('João ', 'da Silva') = varchar 'João da Silva';

CREATE OR REPLACE FUNCTION retConcatTextPython(fname text, lname text) RETURNS text AS $$
return f"Hello {fname} {lname}!"
$$ LANGUAGE plpython3u;
SELECT retConcatTextPython('João', 'da Silva') =  varchar 'Hello João da Silva!';

CREATE OR REPLACE FUNCTION retVarCharTextPython(fname varchar, lname varchar) RETURNS text AS $$
return f"Hello {fname} {lname}!"
$$ LANGUAGE plpython3u;
SELECT retVarCharTextPython('Homer Jay', 'Simpson') = varchar 'Hello Homer Jay Simpson!';

CREATE OR REPLACE FUNCTION retCharPython(argchar character) RETURNS character AS $$
return argchar
$$ LANGUAGE plpython3u;
SELECT retCharPython('R') =  character 'R';

CREATE OR REPLACE FUNCTION retConcatLettersPython(a character, b character) RETURNS varchar AS $$
return a + b
$$ LANGUAGE plpython3u;
SELECT retConcatLettersPython('R', 'C') = varchar 'RC';

-- Problem here it is neither padding and truncating
CREATE OR REPLACE FUNCTION retConcatCharsPython(a char(5), b char(7)) RETURNS char(5) AS $$
return a + b
$$ LANGUAGE plpython3u;
SELECT retConcatCharsPython('H.', 'Simpson') = 'H.Simpson';

CREATE OR REPLACE FUNCTION retConcatVarCharsPython(a varchar(5), b varchar(7)) RETURNS varchar(5) AS $$
return a + b
$$ LANGUAGE plpython3u;
SELECT retConcatVarCharsPython('H.', 'Simpson') = 'H.Simpson';
SELECT retConcatVarCharsPython('H. あ', 'Simpson') = 'H. あSimpson';

CREATE OR REPLACE FUNCTION retNonRegularEncodingPython(a varchar) RETURNS varchar AS $$
return a
$$ LANGUAGE plpython3u;
SELECT retNonRegularEncodingPython('漢字') = varchar '漢字';
SELECT retNonRegularEncodingPython('ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ') = varchar 'ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ';
SELECT retNonRegularEncodingPython('ŁĄŻĘĆŃŚŹ') = varchar 'ŁĄŻĘĆŃŚŹ';
SELECT retNonRegularEncodingPython('Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.')
    = varchar 'Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.';
