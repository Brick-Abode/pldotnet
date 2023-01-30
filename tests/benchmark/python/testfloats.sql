-- Float4 (real): 6 digits of precison
CREATE OR REPLACE FUNCTION returnRealPython() RETURNS real AS $$
return 1.50055
$$ LANGUAGE plpython3u;
SELECT returnRealPython() = real '1.50055';

CREATE OR REPLACE FUNCTION sumRealPython(a real, b real) RETURNS real AS $$
return a + b
$$ LANGUAGE plpython3u;
SELECT sumRealPython(1.50055, 1.50054) = real '3.00109'; -- 3.00109

--- Float8 (double precision): 15 digits of precison
CREATE OR REPLACE FUNCTION returnDoublePython() RETURNS double precision AS $$
return 11.0050000000005
$$ LANGUAGE plpython3u;
SELECT returnDoublePython() = double precision '11.0050000000005';

CREATE OR REPLACE FUNCTION sumDoublePython(a double precision, b double precision) RETURNS double precision AS $$
return a + b
$$ LANGUAGE plpython3u;
SELECT sumDoublePython(10.5000000000055, 10.5000000000054) = double precision  '21.0000000000109'; -- 21.0000000000109
