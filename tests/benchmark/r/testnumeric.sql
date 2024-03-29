CREATE OR REPLACE FUNCTION get_sumR(
   a NUMERIC(3,2), 
   b NUMERIC) 
RETURNS NUMERIC(4,1) AS $$
return(a + b)
$$
LANGUAGE plr;
SELECT get_sumR(1.3333333, 10) =  numeric '11.3333333';
SELECT get_sumR(1.33333333, -10.99999999) = '-9.66666666';
SELECT get_sumR(1999999999999.555555555555555, -10.99999999) = numeric '1999999999988.56';

CREATE OR REPLACE FUNCTION getbigNumR(a NUMERIC) RETURNS NUMERIC AS $$
return(a)
$$
LANGUAGE plr;
SELECT getbigNumR(999999999999999999991.9999991) = numeric '1000000000000000000000';
SELECT getbigNumR(999999999999999999991.99999999) =  numeric '1000000000000000000000';
