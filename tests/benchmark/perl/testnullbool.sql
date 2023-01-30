CREATE OR REPLACE FUNCTION returnNullBoolPerl() RETURNS boolean AS $$
return undef;
$$ LANGUAGE plperl;
SELECT returnNullBoolPerl() is NULL;

CREATE OR REPLACE FUNCTION BooleanNullAndPerl(a boolean, b boolean) RETURNS boolean AS $$
return($_[0] and $_[1]);
$$ LANGUAGE plperl;
SELECT BooleanNullAndPerl(true, null) is NULL;
SELECT BooleanNullAndPerl(null, true) is NULL;
SELECT BooleanNullAndPerl(false, null) is NULL;
SELECT BooleanNullAndPerl(null, false) is NULL;
SELECT BooleanNullAndPerl(null, null) is NULL;

CREATE OR REPLACE FUNCTION BooleanNullOrPerl(a boolean, b boolean) RETURNS boolean AS $$
return ($_[0] or $_[1]);
$$ LANGUAGE plperl;
SELECT BooleanNullOrPerl(true, null) is true;
SELECT BooleanNullOrPerl(null, true) is true;
SELECT BooleanNullOrPerl(false, null) is false;
SELECT BooleanNullOrPerl(null, false) is false;
SELECT BooleanNullOrPerl(null, null) is NULL;

CREATE OR REPLACE FUNCTION BooleanNullXorPerl(a boolean, b boolean) RETURNS boolean AS $$
return(!$_[0] ^ !$_[1]);
$$ LANGUAGE plperl;
SELECT BooleanNullXorPerl(true, null) is true;
SELECT BooleanNullXorPerl(null, true) is true;
SELECT BooleanNullXorPerl(false, null) is true;
SELECT BooleanNullXorPerl(null, false) is true;
SELECT BooleanNullXorPerl(null, null) is false;
