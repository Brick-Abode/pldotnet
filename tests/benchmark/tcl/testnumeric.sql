CREATE OR REPLACE FUNCTION get_sumTcl(
   a NUMERIC(3,2), 
   b NUMERIC) 
RETURNS NUMERIC(4,1) AS $$
return [expr $1 + $2];
$$
LANGUAGE pltcl;
SELECT get_sumTcl(1.3333333, 10) =  numeric '11.3333333';
SELECT get_sumTcl(1.33333333, -10.99999999) = '-9.666666659999999';
SELECT get_sumTcl(1999999999999.555555555555555, -10.99999999) = numeric '1999999999988.5557'; 

CREATE OR REPLACE FUNCTION getbigNumTcl(a NUMERIC) RETURNS NUMERIC AS $$
return $1;
$$
LANGUAGE pltcl;
SELECT getbigNumTcl(999999999999999999991.9999991) = numeric '999999999999999999991.9999991';
SELECT getbigNumTcl(999999999999999999991.99999999) =  numeric '999999999999999999991.99999999';