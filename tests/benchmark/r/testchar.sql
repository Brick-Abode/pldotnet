CREATE OR REPLACE FUNCTION retVarCharR(fname varchar) RETURNS varchar AS $$
return(paste(fname, ' Cabral', sep=""))
$$ LANGUAGE plr;
SELECT retVarCharR('Rafael') = varchar 'Rafael Cabral';

CREATE OR REPLACE FUNCTION retConcatVarCharR(fname varchar, lname varchar) RETURNS varchar AS $$
return(paste(fname, lname, sep=""))
$$ LANGUAGE plr;
SELECT retConcatVarCharR('João ', 'da Silva') = varchar 'João da Silva';

CREATE OR REPLACE FUNCTION retConcatTextR(fname text, lname text) RETURNS text AS $$
return(paste("Hello ", fname, lname, "!", sep=""))
$$ LANGUAGE plr;
SELECT retConcatTextR('João ', 'da Silva') =  varchar 'Hello João da Silva!';

CREATE OR REPLACE FUNCTION retVarCharTextR(fname varchar, lname varchar) RETURNS text AS $$
return(paste("Hello ", fname, lname, "!", sep=""))
$$ LANGUAGE plr;
SELECT retVarCharTextR('Homer Jay ', 'Simpson') = varchar 'Hello Homer Jay Simpson!';

CREATE OR REPLACE FUNCTION retCharR(argchar character) RETURNS character AS $$
return(argchar)
$$ LANGUAGE plr;
SELECT retCharR('R') =  character 'R';

CREATE OR REPLACE FUNCTION retConcatLettersR(a character, b character) RETURNS varchar AS $$
return(paste(a, b, sep=''))
$$ LANGUAGE plr;
SELECT retConcatLettersR('R', 'C') = varchar 'RC';

-- Problem here it is neither padding and truncating
CREATE OR REPLACE FUNCTION retConcatCharsR(a char(5), b char(7)) RETURNS char(5) AS $$
return(paste(a, b, sep=''))
$$ LANGUAGE plr;
SELECT retConcatCharsR('H.', 'Simpson') = 'H.Simpson';

CREATE OR REPLACE FUNCTION retConcatVarCharsR(a varchar(5), b varchar(7)) RETURNS varchar(5) AS $$
return(paste(a, b, sep=''))
$$ LANGUAGE plr;
SELECT retConcatVarCharsR('H.', 'Simpson') = 'H.Simpson';
SELECT retConcatVarCharsR('H. あ', 'Simpson') = 'H. あSimpson';

CREATE OR REPLACE FUNCTION retNonRegularEncodingR(a varchar) RETURNS varchar AS $$
return(a)
$$ LANGUAGE plr;
SELECT retNonRegularEncodingR('漢字') = varchar '漢字';
SELECT retNonRegularEncodingR('ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ') = varchar 'ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ';
SELECT retNonRegularEncodingR('ŁĄŻĘĆŃŚŹ') = varchar 'ŁĄŻĘĆŃŚŹ';
SELECT retNonRegularEncodingR('Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.')
    = varchar 'Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.';
