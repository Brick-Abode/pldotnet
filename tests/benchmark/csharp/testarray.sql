CREATE OR REPLACE FUNCTION sumArrayInt(a integer[]) RETURNS integer AS $$
int[] intArray = (int[])a;
return intArray[0] + intArray[1] + intArray[2];
$$ LANGUAGE plcsharp;
SELECT sumArrayInt( ARRAY[4,1,5] ) = integer '10';

CREATE OR REPLACE FUNCTION sumArrayText(a text[]) RETURNS text AS $$
string[] stringArray = (string[])a;
return stringArray[0] + stringArray[1] + stringArray[2] + stringArray[3];
$$ LANGUAGE plcsharp;
SELECT sumArrayText( ARRAY['Rafael', ' da', ' Veiga', ' Cabral'] ) = varchar 'Rafael da Veiga Cabral';
