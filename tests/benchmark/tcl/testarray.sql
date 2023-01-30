CREATE OR REPLACE FUNCTION sumArrayIntTcl(a integer[]) RETURNS integer AS $$
set state [split $1 "{,}"]
return [expr {[lindex $state 1] + [lindex $state 2] + [lindex $state 3]}]
$$ LANGUAGE pltcl;
SELECT sumArrayIntTcl( ARRAY[4,1,5] ) = integer '10';

CREATE OR REPLACE FUNCTION sumArrayNumTcl(a numeric[]) RETURNS numeric AS $$
set state [split $1 "{,}"]
return [expr {[lindex $state 1] + [lindex $state 2] + [lindex $state 3]}]
$$ LANGUAGE pltcl;
SELECT sumArrayNumTcl( ARRAY[1.00002, 1.00003, 1.00004] ) = numeric '3.00009';

CREATE OR REPLACE FUNCTION sumArrayTextTcl(a text[]) RETURNS text AS $$
set state [split $1 "{,}"]
set state [split $state "{\"}"]
return [concat [lindex $state 1][lindex $state 2][lindex $state 3][lindex $state 4]]
$$ LANGUAGE pltcl;
SELECT sumArrayTextTcl( ARRAY['Rafael', 'da', 'Veiga', 'Cabral'] ) = varchar 'Rafael da Veiga Cabral';
