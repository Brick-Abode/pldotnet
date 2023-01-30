-- Float4 (real): 6 digits of precison
CREATE OR REPLACE FUNCTION returnRealPg() RETURNS real AS $$
DECLARE
BEGIN
    RETURN 1.50055;
END
$$ LANGUAGE plpgsql;
SELECT returnRealPg() = real '1.50055';

CREATE OR REPLACE FUNCTION sumRealPg(a real, b real) RETURNS real AS $$
DECLARE
BEGIN
    RETURN a+b;
END
$$ LANGUAGE plpgsql;
SELECT sumRealPg(1.50055, 1.50054) = real '3.00109'; -- 3.00109

--- Float8 (double precision): 15 digits of precison
CREATE OR REPLACE FUNCTION returnDoublePg() RETURNS double precision AS $$
DECLARE
BEGIN
    RETURN 11.0050000000005;
END
$$ LANGUAGE plpgsql;
SELECT returnDoublePg() = double precision '11.0050000000005';

CREATE OR REPLACE FUNCTION sumDoublePg(a double precision, b double precision) RETURNS double precision AS $$
DECLARE
BEGIN
    RETURN a+b;
END
$$ LANGUAGE plpgsql;
SELECT sumDoublePg(10.5000000000055, 10.5000000000054) = double precision  '21.0000000000109'; -- 21.0000000000109
