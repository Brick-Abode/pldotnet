CREATE OR REPLACE FUNCTION retVarCharTcl(fname varchar) RETURNS varchar AS $$
return [concat $1 Cabral];
$$ LANGUAGE pltcl;
SELECT retVarCharTcl('Rafael') = varchar 'Rafael Cabral';

CREATE OR REPLACE FUNCTION retConcatVarCharTcl(fname varchar, lname varchar) RETURNS varchar AS $$
return [concat $1 $2];
$$ LANGUAGE pltcl;
SELECT retConcatVarCharTcl('João ', 'da Silva') = varchar 'João da Silva';

CREATE OR REPLACE FUNCTION retConcatTextTcl(fname text, lname text) RETURNS text AS $$
return [concat Hello $1 $2!];
$$ LANGUAGE pltcl;
SELECT retConcatTextTcl('João ', 'da Silva') =  varchar 'Hello João da Silva!';

CREATE OR REPLACE FUNCTION retVarCharTextTcl(fname varchar, lname varchar) RETURNS text AS $$
return [concat Hello $1 $2!];
$$ LANGUAGE pltcl;
SELECT retVarCharTextTcl('Homer Jay ', 'Simpson') = varchar 'Hello Homer Jay Simpson!';

CREATE OR REPLACE FUNCTION retCharTcl(argchar character) RETURNS character AS $$
return $1;
$$ LANGUAGE pltcl;
SELECT retCharTcl('R') =  character 'R';

CREATE OR REPLACE FUNCTION retConcatLettersTcl(a character, b character) RETURNS varchar AS $$
return [concat $1$2];
$$ LANGUAGE pltcl;
SELECT retConcatLettersTcl('R', 'C') = varchar 'RC';

-- Problem here it is neither padding and truncating
CREATE OR REPLACE FUNCTION retConcatCharsTcl(a char(5), b char(7)) RETURNS char(5) AS $$
return [concat $1$2];
$$ LANGUAGE pltcl;
SELECT retConcatCharsTcl('H.', 'Simpson') = 'H.Simpson';

CREATE OR REPLACE FUNCTION retConcatVarCharsTcl(a varchar(5), b varchar(7)) RETURNS varchar(5) AS $$
return [concat $1$2];
$$ LANGUAGE pltcl;
SELECT retConcatVarCharsTcl('H.', 'Simpson') = 'H.Simpson';
SELECT retConcatVarCharsTcl('H. あ', 'Simpson') = 'H. あSimpson';

CREATE OR REPLACE FUNCTION retNonRegularEncodingTcl(a varchar) RETURNS varchar AS $$
return $1;
$$ LANGUAGE pltcl;
SELECT retNonRegularEncodingTcl('漢字') = varchar '漢字';
SELECT retNonRegularEncodingTcl('ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ') = varchar 'ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ';
SELECT retNonRegularEncodingTcl('ŁĄŻĘĆŃŚŹ') = varchar 'ŁĄŻĘĆŃŚŹ';
SELECT retNonRegularEncodingTcl('Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.')
    = varchar 'Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.';
