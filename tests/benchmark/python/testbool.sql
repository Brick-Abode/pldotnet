CREATE OR REPLACE FUNCTION returnBoolPython() RETURNS boolean AS $$
return False
$$ LANGUAGE plpython3u;
SELECT returnBoolPython() is false;

CREATE OR REPLACE FUNCTION BooleanAndPython(a boolean, b boolean) RETURNS boolean AS $$
return a and b
$$ LANGUAGE plpython3u;
SELECT BooleanAndPython(True, True) is True;

CREATE OR REPLACE FUNCTION BooleanOrPython(a boolean, b boolean) RETURNS boolean AS $$
return a or b
$$ LANGUAGE plpython3u;
SELECT BooleanOrPython(False, False) is False;

CREATE OR REPLACE FUNCTION BooleanXorPython(a boolean, b boolean) RETURNS boolean AS $$
return bool(a) ^ bool(b)
$$ LANGUAGE plpython3u;
SELECT BooleanXorPython(False, False) is False;
