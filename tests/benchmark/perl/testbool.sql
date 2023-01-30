CREATE OR REPLACE FUNCTION returnBoolPerl() RETURNS boolean AS $$
return 0
$$ LANGUAGE plperl;
SELECT returnBoolPerl() is false;

CREATE OR REPLACE FUNCTION BooleanAndPerl(a boolean, b boolean) RETURNS boolean AS $$
return($_[0] and $_[1]);
$$ LANGUAGE plperl;
SELECT BooleanAndPerl(True, True) is True;

CREATE OR REPLACE FUNCTION BooleanOrPerl(a boolean, b boolean) RETURNS boolean AS $$
return($_[0] or $_[1]);
$$ LANGUAGE plperl;
SELECT BooleanOrPerl(False, False) is False;

CREATE OR REPLACE FUNCTION BooleanXorPerl(a boolean, b boolean) RETURNS boolean AS $$
return(!$_[0] ^ !$_[1]);
$$ LANGUAGE plperl;
SELECT BooleanXorPerl(False, False) is False;
