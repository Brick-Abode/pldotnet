CREATE OR REPLACE FUNCTION get_sumPython(
   a NUMERIC(3,2),
   b NUMERIC)
RETURNS NUMERIC(4,1) AS $$
return a + b;
$$
LANGUAGE plpython3u;
SELECT get_sumPython(1.3333333, 10) =  numeric '11.3333333';
SELECT get_sumPython(1.33333333, -10.99999999) = '-9.66666666';
SELECT get_sumPython(1999999999999.555555555555555, -10.99999999) = numeric '1999999999988.555555565555555'; -- 1999999999988.555555565555555

CREATE OR REPLACE FUNCTION getbigNumPython(a NUMERIC) RETURNS NUMERIC AS $$
return a;
$$
LANGUAGE plpython3u;
SELECT getbigNumPython(999999999999999999991.9999991) = numeric '999999999999999999991.9999991'; -- Sextllion at 7 scale (10 power 28 precision)
/* Failing test case */
SELECT getbigNumPython(999999999999999999991.99999999) =  numeric '999999999999999999992.0000000'; -- It is rounded to 999999999999999999992.0000000
