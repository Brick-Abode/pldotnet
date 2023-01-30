CREATE OR REPLACE FUNCTION returnNullBoolPg() RETURNS boolean AS $$
DECLARE
BEGIN
    RETURN NULL;
END
$$ LANGUAGE plpgsql;
SELECT returnNullBoolPg() is NULL;

CREATE OR REPLACE FUNCTION BooleanNullAndPg(a boolean, b boolean) RETURNS boolean AS $$
DECLARE
BEGIN
    RETURN a AND b;
END
$$ LANGUAGE plpgsql;
SELECT BooleanNullAndPg(true, null) is NULL;
SELECT BooleanNullAndPg(null, true) is NULL;
SELECT BooleanNullAndPg(false, null) is false;
SELECT BooleanNullAndPg(null, false) is false;
SELECT BooleanNullAndPg(null, null) is NULL;

CREATE OR REPLACE FUNCTION BooleanNullOrPg(a boolean, b boolean) RETURNS boolean AS $$
DECLARE
BEGIN
    RETURN a OR b;
END
$$ LANGUAGE plpgsql;
SELECT BooleanNullOrPg(true, null) is true;
SELECT BooleanNullOrPg(null, true) is true;
SELECT BooleanNullOrPg(false, null) is NULL;
SELECT BooleanNullOrPg(null, false) is NULL;
SELECT BooleanNullOrPg(null, null) is NULL;

CREATE OR REPLACE FUNCTION BooleanNullXorPg(a boolean, b boolean) RETURNS boolean AS $$
DECLARE
BEGIN
    RETURN (a AND NOT b) OR (b AND NOT a);
END
$$ LANGUAGE plpgsql;
SELECT BooleanNullXorPg(true, null) is NULL;
SELECT BooleanNullXorPg(null, true) is NULL;
SELECT BooleanNullXorPg(false, null) is NULL;
SELECT BooleanNullXorPg(null, false) is NULL;
SELECT BooleanNullXorPg(null, null) is NULL;
