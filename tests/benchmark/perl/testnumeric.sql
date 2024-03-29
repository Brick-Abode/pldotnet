CREATE OR REPLACE FUNCTION get_sumPerl(
   a NUMERIC(3,2),
   b NUMERIC)
RETURNS NUMERIC(4,1) AS $$
return $_[0] + $_[1];
$$ LANGUAGE plperl;

SELECT get_sumPerl(1.3333333, 10) =  numeric '11.3333333';
SELECT get_sumPerl(1.33333333, -10.99999999) = '-9.66666666';
SELECT get_sumPerl(1999999999999.555555555555555, -10.99999999) = numeric '1999999999988.56';

CREATE OR REPLACE FUNCTION getbigNumPerl(a NUMERIC) RETURNS NUMERIC AS $$
return $_[0];
$$ LANGUAGE plperl;


SELECT getbigNumPerl(999999999999999999991.9999991) = numeric '999999999999999999991.9999991'; -- Sextllion at 7 scale (10 power 28 precision)

SELECT getbigNumPerl(999999999999999999991.99999999) =  numeric '999999999999999999991.99999999';
