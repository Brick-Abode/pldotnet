CREATE OR REPLACE FUNCTION sumArrayInt(a integer[]) RETURNS integer AS $$
return a[0] + a[1] + a[2];
$$ LANGUAGE plcsharp;
SELECT sumArrayInt( ARRAY[4,1,5] ) = integer '10';

CREATE OR REPLACE FUNCTION sumArrayNum(a numeric[]) RETURNS numeric AS $$
return a[0] + a[1] + a[2];
$$ LANGUAGE plcsharp;
SELECT sumArrayNum( ARRAY[1.00002, 1.00003, 1.00004] ) = numeric '3.00009';

CREATE OR REPLACE FUNCTION sumArrayText(a text[]) RETURNS text AS $$
return a[0] + a[1] + a[2] + a[3];
$$ LANGUAGE plcsharp;
SELECT sumArrayText( ARRAY['Rafael', ' da', ' Veiga', ' Cabral'] ) = varchar 'Rafael da Veiga Cabral';
