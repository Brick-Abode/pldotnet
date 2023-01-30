CREATE OR REPLACE FUNCTION sumArrayIntFSharp(a integer[]) RETURNS integer AS $$
Nullable ((a.GetValue(0) :?> int) + (a.GetValue(1) :?> int) + (a.GetValue(2) :?> int))
$$ LANGUAGE plfsharp;
SELECT sumArrayIntFSharp( ARRAY[4,1,5] ) = integer '10';

CREATE OR REPLACE FUNCTION sumArrayTextFSharp(a text[]) RETURNS text AS $$
(a.GetValue(0) :?> string) + (a.GetValue(1) :?> string) + (a.GetValue(2) :?> string) + (a.GetValue(3) :?> string)
$$ LANGUAGE plfsharp;
SELECT sumArrayTextFSharp( ARRAY['Rafael', ' da', ' Veiga', ' Cabral'] ) = varchar 'Rafael da Veiga Cabral';
