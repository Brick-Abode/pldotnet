/* Not working */

CREATE OR REPLACE FUNCTION retVarCharLua(fname varchar) RETURNS varchar AS $$
return fname .. " Smith";
$$ LANGUAGE pllua;
SELECT retVarCharLua('Jerry') = varchar 'Jerry Smith';

CREATE OR REPLACE FUNCTION retConcatVarCharLua(fname varchar, lname varchar) RETURNS varchar AS $$
return fname .. lname
$$ LANGUAGE pllua;
SELECT retConcatVarCharLua('João ', 'da Silva') = varchar 'João da Silva';

CREATE OR REPLACE FUNCTION retConcatTextLua(fname text, lname text) RETURNS text AS $$
return "Hello " .. fname .. " " .. lname .. "!"
$$ LANGUAGE pllua;
SELECT retConcatTextLua('João', 'da Silva') =  varchar 'Hello João da Silva!';

CREATE OR REPLACE FUNCTION retVarCharTextLua(fname varchar, lname varchar) RETURNS text AS $$
return "Hello " .. fname .. " " .. lname .. "!"
$$ LANGUAGE pllua;
SELECT retVarCharTextLua('Homer Jay', 'Simpson') = varchar 'Hello Homer Jay Simpson!';

CREATE OR REPLACE FUNCTION retCharLua(argchar character) RETURNS character AS $$
return argchar
$$ LANGUAGE pllua;
SELECT retCharLua('R') =  character 'R';

CREATE OR REPLACE FUNCTION retConcatLettersLua(a character, b character) RETURNS varchar AS $$
return a .. b
$$ LANGUAGE pllua;
SELECT retConcatLettersLua('R', 'C') = varchar 'RC';

CREATE OR REPLACE FUNCTION retConcatCharsLua(a char(5), b char(7)) RETURNS char(5) AS $$
return a .. b
$$ LANGUAGE pllua;
SELECT retConcatCharsLua('H.', 'Simpson') = 'H.Simpson';

CREATE OR REPLACE FUNCTION retConcatVarCharsLua(a varchar(5), b varchar(7)) RETURNS varchar(5) AS $$
return a .. b
$$ LANGUAGE pllua;
SELECT retConcatVarCharsLua('H.', 'Simpson') = 'H.Simpson';
SELECT retConcatVarCharsLua('H. あ', 'Simpson') = 'H. あSimpson';

CREATE OR REPLACE FUNCTION retNonRegularEncodingLua(a varchar) RETURNS varchar AS $$
return a
$$ LANGUAGE pllua;
SELECT retNonRegularEncodingLua('漢字') = varchar '漢字';
SELECT retNonRegularEncodingLua('ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ') = varchar 'ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ';
SELECT retNonRegularEncodingLua('ŁĄŻĘĆŃŚŹ') = varchar 'ŁĄŻĘĆŃŚŹ';
SELECT retNonRegularEncodingLua('Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.')
    = varchar 'Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.';
