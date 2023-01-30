CREATE OR REPLACE FUNCTION returnBoolFSharp() RETURNS boolean AS $$
Nullable(false)
$$ LANGUAGE plfsharp;
SELECT returnBoolFSharp() is false;

CREATE OR REPLACE FUNCTION BooleanAndFSharp(a boolean, b boolean) RETURNS boolean AS $$
Nullable(a.Value && b.Value)
$$ LANGUAGE plfsharp;
SELECT BooleanAndFSharp(true, true) is true;

CREATE OR REPLACE FUNCTION BooleanOrFSharp(a boolean, b boolean) RETURNS boolean AS $$
Nullable(a.Value || b.Value)
$$ LANGUAGE plfsharp;
SELECT BooleanOrFSharp(false, false) is false;

CREATE OR REPLACE FUNCTION BooleanXorFSharp(a boolean, b boolean) RETURNS boolean AS $$
Nullable(a.Value <> b.Value)
$$ LANGUAGE plfsharp;
SELECT BooleanXorFSharp(false, false) is false;

