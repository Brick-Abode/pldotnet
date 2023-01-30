-- Float4 (real): 6 digits of precison
CREATE OR REPLACE FUNCTION returnRealR() RETURNS real AS $$
return(1.50055)
$$ LANGUAGE plr;
SELECT returnRealR() = real '1.50055';

CREATE OR REPLACE FUNCTION sumRealR(a real, b real) RETURNS real AS $$
return(a+b)
$$ LANGUAGE plr;
SELECT sumRealR(1.50055, 1.50054) = real '3.00109'; -- 3.00109

--- Float8 (double precision): 15 digits of precison
CREATE OR REPLACE FUNCTION returnDoubleR() RETURNS double precision AS $$
return(11.0050000000005)
$$ LANGUAGE plr;
SELECT returnDoubleR() = double precision '11.0050000000005';

CREATE OR REPLACE FUNCTION sumDoubleR(a double precision, b double precision) RETURNS double precision AS $$
return(a+b)
$$ LANGUAGE plr;
SELECT sumDoubleR(10.5000000000055, 10.5000000000054) = double precision  '21.0000000000109'; -- 21.0000000000109
