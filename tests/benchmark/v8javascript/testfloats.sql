-- Float4 (real): 6 digits of precison
CREATE OR REPLACE FUNCTION returnRealV8() RETURNS real AS $$
return 1.50055;
$$ LANGUAGE plv8;
SELECT returnRealV8() = real '1.50055';

CREATE OR REPLACE FUNCTION sumRealV8(a real, b real) RETURNS real AS $$
return a+b;
$$ LANGUAGE plv8;
SELECT sumRealV8(1.50055, 1.50054) = real '3.00109'; -- 3.00109

--- Float8 (double precision): 15 digits of precison
CREATE OR REPLACE FUNCTION returnDoubleV8() RETURNS double precision AS $$
return 11.0050000000005;
$$ LANGUAGE plv8;
SELECT returnDoubleV8() = double precision '11.0050000000005';

CREATE OR REPLACE FUNCTION sumDoubleV8(a double precision, b double precision) RETURNS double precision AS $$
return a+b;
$$ LANGUAGE plv8;
SELECT sumDoubleV8(10.5000000000055, 10.5000000000054) = double precision  '21.0000000000109'; -- 21.0000000000109
