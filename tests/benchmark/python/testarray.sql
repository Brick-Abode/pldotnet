CREATE OR REPLACE FUNCTION sumArrayIntPython(a integer[]) RETURNS integer AS $$
return a[0] + a[1] + a[2]
$$ LANGUAGE plpython3u;
SELECT sumArrayIntPython( ARRAY[4,1,5] ) = integer '10';

CREATE OR REPLACE FUNCTION sumArrayNumPython(a numeric[]) RETURNS numeric AS $$
return a[0] + a[1] + a[2]
$$ LANGUAGE plpython3u;
SELECT sumArrayNumPython( ARRAY[1.00002, 1.00003, 1.00004] ) = numeric '3.00009';

CREATE OR REPLACE FUNCTION sumArrayTextPython(a text[]) RETURNS text AS $$
return a[0] + a[1] + a[2] + a[3]
$$ LANGUAGE plpython3u;
SELECT sumArrayTextPython( ARRAY['Rodrigo', ' Silva', ' Lima', ' Bahia'] ) = varchar 'Rodrigo Silva Lima Bahia';
