CREATE OR REPLACE FUNCTION returnNullBool() RETURNS boolean AS $$
return null;
$$ LANGUAGE plcsharp;
SELECT returnNullBool() is NULL;

CREATE OR REPLACE FUNCTION BooleanNullAnd(a boolean, b boolean) RETURNS boolean AS $$
return a&b;
$$ LANGUAGE plcsharp;
SELECT BooleanNullAnd(true, null) is NULL;
SELECT BooleanNullAnd(null, true) is NULL;
SELECT BooleanNullAnd(false, null) is false;
SELECT BooleanNullAnd(null, false) is false;
SELECT BooleanNullAnd(null, null) is NULL;

CREATE OR REPLACE FUNCTION BooleanNullOr(a boolean, b boolean) RETURNS boolean AS $$
return a|b;
$$ LANGUAGE plcsharp;
SELECT BooleanNullOr(true, null) is true;
SELECT BooleanNullOr(null, true) is true;
SELECT BooleanNullOr(false, null) is NULL;
SELECT BooleanNullOr(null, false) is NULL;
SELECT BooleanNullOr(null, null) is NULL;

CREATE OR REPLACE FUNCTION BooleanNullXor(a boolean, b boolean) RETURNS boolean AS $$
return a^b;
$$ LANGUAGE plcsharp;
SELECT BooleanNullXor(true, null) is NULL;
SELECT BooleanNullXor(null, true) is NULL;
SELECT BooleanNullXor(false, null) is NULL;
SELECT BooleanNullXor(null, false) is NULL;
SELECT BooleanNullXor(null, null) is NULL;
