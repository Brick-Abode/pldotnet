CREATE OR REPLACE FUNCTION returnNullBoolPython() RETURNS boolean AS $$
return None
$$ LANGUAGE plpython3u;
SELECT returnNullBoolPython() is NULL;

CREATE OR REPLACE FUNCTION BooleanNullAndPython(a boolean, b boolean) RETURNS boolean AS $$
return a and b
$$ LANGUAGE plpython3u;
SELECT BooleanNullAndPython(true, null) is NULL;
SELECT BooleanNullAndPython(null, true) is NULL;
SELECT BooleanNullAndPython(false, null) is false;
SELECT BooleanNullAndPython(null, false) is NULL;
SELECT BooleanNullAndPython(null, null) is NULL;

CREATE OR REPLACE FUNCTION BooleanNullOrPython(a boolean, b boolean) RETURNS boolean AS $$
return a or b
$$ LANGUAGE plpython3u;
SELECT BooleanNullOrPython(true, null) is true;
SELECT BooleanNullOrPython(null, true) is true;
SELECT BooleanNullOrPython(false, null) is NULL;
SELECT BooleanNullOrPython(null, false) is false;
SELECT BooleanNullOrPython(null, null) is NULL;

CREATE OR REPLACE FUNCTION BooleanNullXorPython(a boolean, b boolean) RETURNS boolean AS $$
return bool(a) ^ bool(b)
$$ LANGUAGE plpython3u;
SELECT BooleanNullXorPython(true, null) is true;
SELECT BooleanNullXorPython(null, true) is true;
SELECT BooleanNullXorPython(false, null) is false;
SELECT BooleanNullXorPython(null, false) is false;
SELECT BooleanNullXorPython(null, null) is false;
