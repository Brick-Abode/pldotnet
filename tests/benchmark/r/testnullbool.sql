CREATE OR REPLACE FUNCTION returnNullBoolR() RETURNS boolean AS $$
return(NULL)
$$ LANGUAGE plr;
SELECT returnNullBoolR() is NULL;

CREATE OR REPLACE FUNCTION BooleanNullAndR(a boolean, b boolean) RETURNS boolean AS $$
return(a&b)
$$ LANGUAGE plr;
SELECT BooleanNullAndR(true, null) is NULL;
SELECT BooleanNullAndR(null, true) is NULL;
SELECT BooleanNullAndR(false, null) is NULL;
SELECT BooleanNullAndR(null, false) is NULL;
SELECT BooleanNullAndR(null, null) is NULL;

CREATE OR REPLACE FUNCTION BooleanNullOrR(a boolean, b boolean) RETURNS boolean AS $$
return(a|b)
$$ LANGUAGE plr;
SELECT BooleanNullOrR(true, null) is NULL;
SELECT BooleanNullOrR(null, true) is NULL;
SELECT BooleanNullOrR(false, null) is NULL;
SELECT BooleanNullOrR(null, false) is NULL;
SELECT BooleanNullOrR(null, null) is NULL;

CREATE OR REPLACE FUNCTION BooleanNullXorR(a boolean, b boolean) RETURNS boolean AS $$
return(a^b)
$$ LANGUAGE plr;
SELECT BooleanNullXorR(true, null) is NULL;
SELECT BooleanNullXorR(null, true) is NULL;
SELECT BooleanNullXorR(false, null) is NULL;
SELECT BooleanNullXorR(null, false) is NULL;
SELECT BooleanNullXorR(null, null) is NULL;
