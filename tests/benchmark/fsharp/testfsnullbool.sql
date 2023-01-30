CREATE OR REPLACE FUNCTION returnNullBoolFSharp() RETURNS boolean AS $$
System.Nullable()
$$ LANGUAGE plfsharp;
SELECT returnNullBoolFSharp() is NULL;

CREATE OR REPLACE FUNCTION BooleanNullAndFSharp(a boolean, b boolean) RETURNS boolean AS $$
let checkValue value =
    if value then
        System.Nullable()
    else
        Nullable(false)
match (a.HasValue, b.HasValue) with
| true, true -> Nullable(a.Value && b.Value)
| true, false -> checkValue a.Value
| false, true -> checkValue b.Value
| false, false -> System.Nullable()
$$ LANGUAGE plfsharp;
SELECT BooleanNullAndFSharp(true, null) is NULL;
SELECT BooleanNullAndFSharp(null, true) is NULL;
SELECT BooleanNullAndFSharp(false, null) is false;
SELECT BooleanNullAndFSharp(null, false) is false;
SELECT BooleanNullAndFSharp(null, null) is NULL;

CREATE OR REPLACE FUNCTION BooleanNullOrFSharp(a boolean, b boolean) RETURNS boolean AS $$
let checkValue value =
    if value then
        Nullable(true)
    else
        System.Nullable()
match (a.HasValue, b.HasValue) with
| true, true -> Nullable(a.Value && b.Value)
| true, false -> checkValue a.Value
| false, true -> checkValue b.Value
| false, false -> System.Nullable()
$$ LANGUAGE plfsharp;
SELECT BooleanNullOrFSharp(true, null) is true;
SELECT BooleanNullOrFSharp(null, true) is true;
SELECT BooleanNullOrFSharp(false, null) is NULL;
SELECT BooleanNullOrFSharp(null, false) is NULL;
SELECT BooleanNullOrFSharp(null, null) is NULL;

CREATE OR REPLACE FUNCTION BooleanNullXorFSharp(a boolean, b boolean) RETURNS boolean AS $$
match (a.HasValue, b.HasValue) with
| true, true -> Nullable(a.Value <> b.Value)
| _ -> System.Nullable()
$$ LANGUAGE plfsharp;
SELECT BooleanNullXorFSharp(true, null) is NULL;
SELECT BooleanNullXorFSharp(null, true) is NULL;
SELECT BooleanNullXorFSharp(false, null) is NULL;
SELECT BooleanNullXorFSharp(null, false) is NULL;
SELECT BooleanNullXorFSharp(null, null) is NULL;
