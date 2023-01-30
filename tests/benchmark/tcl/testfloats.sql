-- Float4 (real): 6 digits of precison
CREATE OR REPLACE FUNCTION returnRealTcl() RETURNS real AS $$
return 1.50055;
$$ LANGUAGE pltcl;
SELECT returnRealTcl() = real '1.50055';

CREATE OR REPLACE FUNCTION sumRealTcl(a real, b real) RETURNS real AS $$
return [expr $1 + $2];
$$ LANGUAGE pltcl;
SELECT sumRealTcl(1.50055, 1.50054) = real '3.00109'; -- 3.00109

--- Float8 (double precision): 15 digits of precison
CREATE OR REPLACE FUNCTION returnDoubleTcl() RETURNS double precision AS $$
return 11.0050000000005;
$$ LANGUAGE pltcl;
SELECT returnDoubleTcl() = double precision '11.0050000000005';

CREATE OR REPLACE FUNCTION sumDoubleTcl(a double precision, b double precision) RETURNS double precision AS $$
return [expr $1 + $2];
$$ LANGUAGE pltcl;
SELECT sumDoubleTcl(10.5000000000055, 10.5000000000054) = double precision  '21.0000000000109'; -- 21.0000000000109
