CREATE OR REPLACE FUNCTION sumArrayIntV8(a integer[]) RETURNS integer AS $$
return a[0] + a[1] + a[2];
$$ LANGUAGE plv8;
SELECT sumArrayIntV8( ARRAY[4,1,5] ) = integer '10';

CREATE OR REPLACE FUNCTION sumArrayNumV8(a numeric[]) RETURNS numeric AS $$
return a[0] + a[1] + a[2];
$$ LANGUAGE plv8;
SELECT sumArrayNumV8( ARRAY[1.00002, 1.00003, 1.00004] ) = numeric '3.00009';

CREATE OR REPLACE FUNCTION sumArrayTextV8(a text[]) RETURNS text AS $$
return a[0] + a[1] + a[2] + a[3];
$$ LANGUAGE plv8;
SELECT sumArrayTextV8( ARRAY['Rodrigo', ' Silva', ' Lima', ' Bahia'] ) = varchar 'Rodrigo Silva Lima Bahia';
