CREATE OR REPLACE FUNCTION sumArrayIntLua(a integer[]) RETURNS integer AS $$
return a[1] + a[2] + a[3]
$$ LANGUAGE pllua;
SELECT sumArrayIntLua( ARRAY[4,1,5] ) = integer '10';

-- CREATE OR REPLACE FUNCTION sumArrayNumLua(a NUMERIC[]) RETURNS NUMERIC AS $$
-- return a[1] + a[2] + a[3]
-- $$ LANGUAGE pllua;
-- SELECT sumArrayNumLua( ARRAY[1.00002, 1.00003, 1.00004] ) = NUMERIC '3.00009';

CREATE OR REPLACE FUNCTION sumArrayTextLua(a text[]) RETURNS text AS $$
return a[1] .. a[2] .. a[3] .. a[4]
$$ LANGUAGE pllua;
SELECT sumArrayTextLua( ARRAY['Rodrigo', ' Silva', ' Lima', ' Bahia'] ) = varchar 'Rodrigo Silva Lima Bahia';
