CREATE OR REPLACE FUNCTION returnBoolPg() RETURNS boolean AS $$
DECLARE
BEGIN
    RETURN false;
END
$$ LANGUAGE plpgsql;
SELECT returnBoolPg() is false;

CREATE OR REPLACE FUNCTION BooleanAndPg(a boolean, b boolean) RETURNS boolean AS $$
DECLARE
BEGIN
    RETURN a AND b;
END
$$ LANGUAGE plpgsql;
SELECT BooleanAndPg(true, true) is true;

CREATE OR REPLACE FUNCTION BooleanOrPg(a boolean, b boolean) RETURNS boolean AS $$
DECLARE
BEGIN
    RETURN a OR b;
END
$$ LANGUAGE plpgsql;
SELECT BooleanOrPg(false, false) is false;

CREATE OR REPLACE FUNCTION BooleanXorPg(a boolean, b boolean) RETURNS boolean AS $$
DECLARE
BEGIN
    RETURN (a AND NOT b) OR (b AND NOT a);
END
$$ LANGUAGE plpgsql;
SELECT BooleanXorPg(false, false) is false;
