CREATE OR REPLACE FUNCTION returnBoolR() RETURNS boolean AS $$
return(FALSE)
$$ LANGUAGE plr;
SELECT returnBoolR() is false;

CREATE OR REPLACE FUNCTION BooleanAndR(a boolean, b boolean) RETURNS boolean AS $$
return(a&b)
$$ LANGUAGE plr;
SELECT BooleanAndR(true, true) is true;

CREATE OR REPLACE FUNCTION BooleanOrR(a boolean, b boolean) RETURNS boolean AS $$
return(a|b)
$$ LANGUAGE plr;
SELECT BooleanOrR(false, false) is false;

CREATE OR REPLACE FUNCTION BooleanXorR(a boolean, b boolean) RETURNS boolean AS $$
return((a & !b)| (b & !a))
$$ LANGUAGE plr;
SELECT BooleanXorR(false, false) is false;
