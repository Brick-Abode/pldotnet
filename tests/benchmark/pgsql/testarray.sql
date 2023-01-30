CREATE OR REPLACE FUNCTION sumArrayIntPg(a integer[]) RETURNS integer AS $$
DECLARE
BEGIN
    RETURN a[1] + a[2] + a[3];
END
$$ LANGUAGE plpgsql;
SELECT sumArrayIntPg(ARRAY[4,1,5]) = integer '10';

CREATE OR REPLACE FUNCTION sumArrayNumPg(a numeric[]) RETURNS numeric AS $$
DECLARE
BEGIN
    RETURN a[1] + a[2] + a[3];
END
$$ LANGUAGE plpgsql;
SELECT sumArrayNumPg( ARRAY[1.00002, 1.00003, 1.00004] ) = numeric '3.00009';

CREATE OR REPLACE FUNCTION sumArrayTextPg(a text[]) RETURNS text AS $$
DECLARE
BEGIN
    RETURN concat(a[1], a[2], a[3], a[4]);
END
$$ LANGUAGE plpgsql;
SELECT sumArrayTextPg( ARRAY['Rafael', ' da', ' Veiga', ' Cabral'] ) = varchar 'Rafael da Veiga Cabral';
