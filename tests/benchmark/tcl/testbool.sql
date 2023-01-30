CREATE OR REPLACE FUNCTION returnBoolTcl() RETURNS boolean AS $$
return "f";
$$ LANGUAGE pltcl;
SELECT returnBoolTcl() is false;

CREATE OR REPLACE FUNCTION BooleanAndTcl(a boolean, b boolean) RETURNS boolean AS $$
return [expr $1 && $2];
$$ LANGUAGE pltcl;
SELECT BooleanAndTcl(true, true) is true;

CREATE OR REPLACE FUNCTION BooleanOrTcl(a boolean, b boolean) RETURNS boolean AS $$
return [expr $1 || $2];
$$ LANGUAGE pltcl;
SELECT BooleanOrTcl(false, false) is false;

CREATE OR REPLACE FUNCTION BooleanXorTcl(a boolean, b boolean) RETURNS boolean AS $$
return [expr (!$1 && $2) || ($1 && !$2) ];
$$ LANGUAGE pltcl;
SELECT BooleanXorTcl(false, false) is false;
