CREATE OR REPLACE FUNCTION returnNullBoolLua() RETURNS boolean AS $$
return nil
$$ LANGUAGE pllua;
SELECT returnNullBoolLua() is NULL;

CREATE OR REPLACE FUNCTION BooleanNullAndLua(a boolean, b boolean) RETURNS boolean AS $$
return a and b
$$ LANGUAGE pllua;
SELECT BooleanNullAndLua(true, null) is NULL;
SELECT BooleanNullAndLua(null, true) is NULL;
SELECT BooleanNullAndLua(false, null) is NULL;
SELECT BooleanNullAndLua(null, false) is NULL;
SELECT BooleanNullAndLua(null, null) is NULL;

CREATE OR REPLACE FUNCTION BooleanNullOrLua(a boolean, b boolean) RETURNS boolean AS $$
return a or b
$$ LANGUAGE pllua;
SELECT BooleanNullOrLua(true, null) is true;
SELECT BooleanNullOrLua(null, true) is true;
SELECT BooleanNullOrLua(false, null) is false;
SELECT BooleanNullOrLua(null, false) is false;
SELECT BooleanNullOrLua(null, null) is NULL;

CREATE OR REPLACE FUNCTION BooleanNullXorLua(a boolean, b boolean) RETURNS boolean AS $$
return  a ~= b
$$ LANGUAGE pllua;
SELECT BooleanNullXorLua(true, null) is true;
SELECT BooleanNullXorLua(null, true) is true;
SELECT BooleanNullXorLua(false, null) is true;
SELECT BooleanNullXorLua(null, false) is true;
SELECT BooleanNullXorLua(null, null) is false;
