CREATE OR REPLACE FUNCTION sumArrayIntR(a integer[]) RETURNS integer AS $$
return(a[1] + a[2] + a[3])
$$ LANGUAGE plr;
SELECT sumArrayIntR( ARRAY[4,1,5] ) = integer '10';

CREATE OR REPLACE FUNCTION sumArrayNumR(a numeric[]) RETURNS numeric AS $$
return(a[1] + a[2] + a[3])
$$ LANGUAGE plr;
SELECT sumArrayNumR( ARRAY[1.00002, 1.00003, 1.00004] ) = numeric '3.00009';

CREATE OR REPLACE FUNCTION sumArrayTextR(a text[]) RETURNS text AS $$
return(paste(a[1],a[2],a[3],a[4], sep=""))
$$ LANGUAGE plr;
SELECT sumArrayTextR( ARRAY['Rafael', ' da', ' Veiga', ' Cabral'] ) = varchar 'Rafael da Veiga Cabral';
