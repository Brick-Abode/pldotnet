-- Float4 (real): 6 digits of precison
CREATE OR REPLACE FUNCTION returnRealLua() RETURNS real AS $$
return 1.50055
$$ LANGUAGE pllua;
SELECT returnRealLua() = real '1.50055';

CREATE OR REPLACE FUNCTION sumRealLua(a real, b real) RETURNS real AS $$
return a + b
$$ LANGUAGE pllua;
SELECT sumRealLua(1.50055, 1.50054) = real '3.00109'; -- 3.00109

--- Float8 (double precision): 15 digits of precison
CREATE OR REPLACE FUNCTION returnDoubleLua() RETURNS double precision AS $$
return 11.0050000000005
$$ LANGUAGE pllua;
SELECT returnDoubleLua() = double precision '11.0050000000005';

CREATE OR REPLACE FUNCTION sumDoubleLua(a double precision, b double precision) RETURNS double precision AS $$
return a + b
$$ LANGUAGE pllua;
SELECT sumDoubleLua(10.5000000000055, 10.5000000000054) = double precision  '21.0000000000109'; -- 21.0000000000109
