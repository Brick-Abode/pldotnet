CREATE OR REPLACE FUNCTION sumArrayIntPerl(a integer[]) RETURNS integer AS $$
return $_[0][0] + $_[0][1] + $_[0][2];
$$ LANGUAGE plperl;
SELECT sumArrayIntPerl( ARRAY[4,1,5] ) = integer '10';

CREATE OR REPLACE FUNCTION sumArrayNumPerl(a numeric[]) RETURNS numeric AS $$
return $_[0][0] + $_[0][1] + $_[0][2];
$$ LANGUAGE plperl;
SELECT sumArrayNumPerl( ARRAY[1.00002, 1.00003, 1.00004] ) = numeric '3.00009';

CREATE OR REPLACE FUNCTION sumArrayTextPerl(a text[]) RETURNS text AS $$
return "$_[0][0]$_[0][1]$_[0][2]$_[0][3]";
$$ LANGUAGE plperl;
SELECT sumArrayTextPerl( ARRAY['Rodrigo', ' Silva', ' Lima', ' Bahia'] ) = varchar 'Rodrigo Silva Lima Bahia';
