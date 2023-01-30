CREATE OR REPLACE FUNCTION retVarCharPg(fname varchar) RETURNS varchar AS $$
DECLARE
BEGIN
    RETURN concat(fname, ' Simpson');
END
$$ LANGUAGE plpgsql;
SELECT retVarCharPg('Homer') = varchar 'Homer Simpson';

CREATE OR REPLACE FUNCTION retConcatVarCharPg(fname varchar, lname varchar) RETURNS varchar AS $$
DECLARE
BEGIN
    RETURN concat(fname, lname);
END
$$ LANGUAGE plpgsql;
SELECT retConcatVarCharPg('João ', 'da Silva') = varchar 'João da Silva';

CREATE OR REPLACE FUNCTION retConcatTextPg(fname text, lname text) RETURNS text AS $$
DECLARE
BEGIN
    RETURN concat('Hello ', fname, lname, '!');
END
$$ LANGUAGE plpgsql;
SELECT retConcatTextPg('João ', 'da Silva') =  varchar 'Hello João da Silva!';

CREATE OR REPLACE FUNCTION retVarCharTextPg(fname varchar, lname varchar) RETURNS text AS $$
DECLARE
BEGIN
    RETURN concat('Hello ', fname, lname, '!');
END
$$ LANGUAGE plpgsql;
SELECT retVarCharTextPg('Homer Jay ', 'Simpson') = varchar 'Hello Homer Jay Simpson!';

CREATE OR REPLACE FUNCTION retCharPg(argchar character) RETURNS character AS $$
DECLARE
BEGIN
    RETURN argchar;
END
$$ LANGUAGE plpgsql;
SELECT retCharPg('R') =  character 'R';

CREATE OR REPLACE FUNCTION retConcatLettersPg(a character, b character) RETURNS varchar AS $$
DECLARE
BEGIN
    RETURN concat(a, b);
END
$$ LANGUAGE plpgsql;
SELECT retConcatLettersPg('R', 'C') = varchar 'RC';

-- Problem here it is neither padding and truncating
CREATE OR REPLACE FUNCTION retConcatCharsPg(a char(5), b char(7)) RETURNS char(5) AS $$
DECLARE
BEGIN
    RETURN concat(a, b);
END
$$ LANGUAGE plpgsql;
SELECT retConcatCharsPg('H.', 'Simpson') = 'H.Simpson';

CREATE OR REPLACE FUNCTION retConcatVarCharsPg(a varchar(5), b varchar(7)) RETURNS varchar(5) AS $$
DECLARE
BEGIN
    RETURN concat(a, b);
END
$$ LANGUAGE plpgsql;
SELECT retConcatVarCharsPg('H.', 'Simpson') = 'H.Simpson';
SELECT retConcatVarCharsPg('H. あ', 'Simpson') = 'H. あSimpson';

CREATE OR REPLACE FUNCTION retNonRegularEncodingPg(a varchar) RETURNS varchar AS $$
DECLARE
BEGIN
    RETURN a;
END
$$ LANGUAGE plpgsql;
SELECT retNonRegularEncodingPg('漢字') = varchar '漢字';
SELECT retNonRegularEncodingPg('ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ') = varchar 'ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ';
SELECT retNonRegularEncodingPg('ŁĄŻĘĆŃŚŹ') = varchar 'ŁĄŻĘĆŃŚŹ';
SELECT retNonRegularEncodingPg('Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.')
    = varchar 'Unicode, которая состоится 10-12 марта 1997 года в Майнце в Германии.';
