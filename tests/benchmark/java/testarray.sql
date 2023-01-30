SELECT sumArrayIntJava( ARRAY[4,1,5] ) = integer '10';

SELECT sumArrayNumJava( ARRAY[1.00002, 1.00003, 1.00004] ) = numeric '3.00009';

SELECT sumArrayTextJava( ARRAY['Rafael', ' da', ' Veiga', ' Cabral'] ) = varchar 'Rafael da Veiga Cabral';
