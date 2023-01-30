CREATE OR REPLACE FUNCTION returnNullBoolV8() RETURNS boolean AS $$
return null;
$$ LANGUAGE plv8;
SELECT returnNullBoolV8() is NULL;

CREATE OR REPLACE FUNCTION BooleanNullAndV8(a boolean, b boolean) RETURNS boolean AS $$
return a&&b;
$$ LANGUAGE plv8;
SELECT BooleanNullAndV8(true, null) is NULL;
SELECT BooleanNullAndV8(null, true) is NULL;
SELECT BooleanNullAndV8(false, null) is false;
SELECT BooleanNullAndV8(null, false) is NULL;
SELECT BooleanNullAndV8(null, null) is NULL;

CREATE OR REPLACE FUNCTION BooleanNullOrV8(a boolean, b boolean) RETURNS boolean AS $$
return a||b;
$$ LANGUAGE plv8;
SELECT BooleanNullOrV8(true, null) is true;
SELECT BooleanNullOrV8(null, true) is true;
SELECT BooleanNullOrV8(false, null) is NULL;
SELECT BooleanNullOrV8(null, false) is false;
SELECT BooleanNullOrV8(null, null) is NULL;

CREATE OR REPLACE FUNCTION BooleanNullXorV8(a boolean, b boolean) RETURNS boolean AS $$
return a^b;
$$ LANGUAGE plv8;
SELECT BooleanNullXorV8(true, null) is true;
SELECT BooleanNullXorV8(null, true) is true;
SELECT BooleanNullXorV8(false, null) is false;
SELECT BooleanNullXorV8(null, false) is false;
SELECT BooleanNullXorV8(null, null) is false;
