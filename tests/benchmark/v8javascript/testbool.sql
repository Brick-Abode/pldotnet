CREATE OR REPLACE FUNCTION returnBoolV8() RETURNS boolean AS $$
return false;
$$ LANGUAGE plv8;
SELECT returnBoolV8() is false;

CREATE OR REPLACE FUNCTION BooleanAndV8(a boolean, b boolean) RETURNS boolean AS $$
return a&b;
$$ LANGUAGE plv8;
SELECT BooleanAndV8(true, true) is true;

CREATE OR REPLACE FUNCTION BooleanOrV8(a boolean, b boolean) RETURNS boolean AS $$
return a||b;
$$ LANGUAGE plv8;
SELECT BooleanOrV8(false, false) is false;

CREATE OR REPLACE FUNCTION BooleanXorV8(a boolean, b boolean) RETURNS boolean AS $$
return a^b;
$$ LANGUAGE plv8;
SELECT BooleanXorV8(false, false) is false;
