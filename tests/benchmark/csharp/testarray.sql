CREATE OR REPLACE FUNCTION sumArrayInt(a integer[]) RETURNS integer AS $$
return (int)a.GetValue(0) + (int)a.GetValue(1) + (int)a.GetValue(2);
$$ LANGUAGE plcsharp;
SELECT sumArrayInt( ARRAY[4,1,5] ) = integer '10';

CREATE OR REPLACE FUNCTION sumArrayText(a text[]) RETURNS text AS $$
return (string)a.GetValue(0) + (string)a.GetValue(1) + (string)a.GetValue(2) + (string)a.GetValue(3);
$$ LANGUAGE plcsharp;
SELECT sumArrayText( ARRAY['Rafael', ' da', ' Veiga', ' Cabral'] ) = varchar 'Rafael da Veiga Cabral';
