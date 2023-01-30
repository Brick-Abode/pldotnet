-- Float4 (real): 6 digits of precison
CREATE OR REPLACE FUNCTION returnRealPerl() RETURNS real AS $$
return 1.50055;
$$ LANGUAGE plperl;
SELECT returnRealPerl() = real '1.50055';

CREATE OR REPLACE FUNCTION sumRealPerl(a real, b real) RETURNS real AS $$
return $_[0] + $_[1];
$$ LANGUAGE plperl;
SELECT sumRealPerl(1.50055, 1.50054) = real '3.00109'; -- 3.00109

--- Float8 (double precision): 15 digits of precison
CREATE OR REPLACE FUNCTION returnDoublePerl() RETURNS double precision AS $$
return 11.0050000000005;
$$ LANGUAGE plperl;
SELECT returnDoublePerl() = double precision '11.0050000000005';

CREATE OR REPLACE FUNCTION sumDoublePerl(a double precision, b double precision) RETURNS double precision AS $$
return $_[0] + $_[1]
$$ LANGUAGE plperl;
SELECT sumDoublePerl(10.5000000000055, 10.5000000000054) = double precision  '21.0000000000109'; -- 21.0000000000109
