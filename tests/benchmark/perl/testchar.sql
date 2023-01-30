CREATE OR REPLACE FUNCTION retVarCharPerl(fname varchar) RETURNS varchar AS $$
return "$_[0] Lima";
$$ LANGUAGE plperl;
SELECT retVarCharPerl('Rodrigo') = varchar 'Rodrigo Lima';

CREATE OR REPLACE FUNCTION retConcatVarCharPerl(fname varchar, lname varchar) RETURNS varchar AS $$
return "$_[0]$_[1]";
$$ LANGUAGE plperl;
SELECT retConcatVarCharPerl('João ', 'da Silva') = varchar 'João da Silva';

CREATE OR REPLACE FUNCTION retConcatTextPerl(fname text, lname text) RETURNS text AS $$
return "Hello $_[0] $_[1]!";
$$ LANGUAGE plperl;
SELECT retConcatTextPerl('João', 'da Silva') =  varchar 'Hello João da Silva!';

CREATE OR REPLACE FUNCTION retVarCharTextPerl(fname varchar, lname varchar) RETURNS text AS $$
return "Hello $_[0] $_[1]!";
$$ LANGUAGE plperl;
SELECT retVarCharTextPerl('Homer Jay', 'Simpson') = varchar 'Hello Homer Jay Simpson!';

CREATE OR REPLACE FUNCTION retCharPerl(argchar character) RETURNS character AS $$
return $_[0];
$$ LANGUAGE plperl;
SELECT retCharPerl('R') =  character 'R';

CREATE OR REPLACE FUNCTION retConcatLettersPerl(a character, b character) RETURNS varchar AS $$
return "$_[0]$_[1]"
$$ LANGUAGE plperl;
SELECT retConcatLettersPerl('R', 'C') = varchar 'RC';

-- Problem here it is neither padding and truncating
CREATE OR REPLACE FUNCTION retConcatCharsPerl(a char(5), b char(7)) RETURNS char(5) AS $$
return "$_[0]$_[1]";
$$ LANGUAGE plperl;
SELECT retConcatCharsPerl('H.', 'Simpson') = 'H.Simpson';

CREATE OR REPLACE FUNCTION retConcatVarCharsPerl(a varchar(5), b varchar(7)) RETURNS varchar(5) AS $$
return "$_[0]$_[1]";
$$ LANGUAGE plperl;
SELECT retConcatVarCharsPerl('H.', 'Simpson') = 'H.Simpson';
SELECT retConcatVarCharsPerl('H. あ', 'Simpson') = 'H. あSimpson';

CREATE OR REPLACE FUNCTION retNonRegularEncodingPerl(a varchar) RETURNS varchar AS $$
return $_[0];
$$ LANGUAGE plperl;
SELECT retNonRegularEncodingPerl('漢字') = varchar '漢字';
SELECT retNonRegularEncodingPerl('ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ') = varchar 'ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ';
SELECT retNonRegularEncodingPerl('ŁĄŻĘĆŃŚŹ') = varchar 'ŁĄŻĘĆŃŚŹ';
SELECT retNonRegularEncodingPerl('Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.')
    = varchar 'Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.';
