 CREATE OR REPLACE FUNCTION returnBoolLua() RETURNS boolean AS $$
return false
$$ LANGUAGE pllua;
SELECT returnBoolLua() is false;

CREATE OR REPLACE FUNCTION BooleanAndLua(a boolean, b boolean) RETURNS boolean AS $$
return a and b
$$ LANGUAGE pllua;
SELECT BooleanAndLua(True, True) is True;

CREATE OR REPLACE FUNCTION BooleanOrLua(a boolean, b boolean) RETURNS boolean AS $$
return a or b
$$ LANGUAGE pllua;
SELECT BooleanOrLua(False, False) is False;

CREATE OR REPLACE FUNCTION BooleanXorLua(a boolean, b boolean) RETURNS boolean AS $$
return a ~= b
$$ LANGUAGE pllua;
SELECT BooleanXorLua(False, False) is False;
