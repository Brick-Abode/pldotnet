/**************** Null return test functions ***************************/
CREATE OR REPLACE FUNCTION returnNullIntFSharp() RETURNS integer AS $$
System.Nullable()
$$ LANGUAGE plfsharp;
SELECT returnNullIntFSharp() is NULL;

CREATE OR REPLACE FUNCTION returnNullSmallIntFSharp() RETURNS smallint AS $$
System.Nullable()
$$ LANGUAGE plfsharp;
SELECT returnNullSmallIntFSharp() is NULL;

CREATE OR REPLACE FUNCTION returnNullBigIntFSharp() RETURNS bigint AS $$
System.Nullable()
$$ LANGUAGE plfsharp;
SELECT returnNullBigIntFSharp() is NULL;

/**************** Null operations test functions ***************************/
CREATE OR REPLACE FUNCTION sumNullArgIntFSharp(a integer, b integer) RETURNS integer AS $$
Nullable(a.GetValueOrDefault() + b.GetValueOrDefault())
$$ LANGUAGE plfsharp;
SELECT sumNullArgIntFSharp(null,null) = integer '0';
SELECT sumNullArgIntFSharp(null,3) = integer '3';
SELECT sumNullArgIntFSharp(3,null) = integer '3';
SELECT sumNullArgIntFSharp(3,3) = integer '6';

CREATE OR REPLACE FUNCTION sumNullArgSmallIntFSharp(a smallint, b smallint) RETURNS smallint AS $$
Nullable(a.GetValueOrDefault() + b.GetValueOrDefault())
$$ LANGUAGE plfsharp;
SELECT sumNullArgSmallIntFSharp(null,null) = integer '0';
SELECT sumNullArgSmallIntFSharp(null,CAST(101 AS smallint)) = integer '101';
SELECT sumNullArgSmallIntFSharp(CAST(101 AS smallint),null) = integer '101';
SELECT sumNullArgSmallIntFSharp(CAST(101 AS smallint),CAST(101 AS smallint)) = smallint '202';

CREATE OR REPLACE FUNCTION sumNullArgBigIntFSharp(a bigint, b bigint) RETURNS bigint AS $$
Nullable(a.GetValueOrDefault() + b.GetValueOrDefault())
$$ LANGUAGE plfsharp;
SELECT sumNullArgBigIntFSharp(null,null) = integer '0';
SELECT sumNullArgBigIntFSharp(null,100) = integer '100';
SELECT sumNullArgBigIntFSharp(9223372036854775707,null) = bigint '9223372036854775707';
SELECT sumNullArgBigIntFSharp(9223372036854775707,100) = bigint '9223372036854775807';

/**************** Conditional return test functions ***************************/
CREATE OR REPLACE FUNCTION checkedSumNullArgIntFSharp(a integer, b integer) RETURNS integer AS $$
match (a.HasValue, b.HasValue) with
| true, true -> Nullable (a.Value + b.Value)
| _ -> System.Nullable()
$$ LANGUAGE plfsharp;
SELECT checkedSumNullArgIntFSharp(null,null) is NULL;
SELECT checkedSumNullArgIntFSharp(null,3) is NULL;
SELECT checkedSumNullArgIntFSharp(3,null) is NULL;
SELECT checkedSumNullArgIntFSharp(3,3) = integer '6';

CREATE OR REPLACE FUNCTION checkedSumNullArgSmallIntFSharp(a smallint, b smallint) RETURNS smallint AS $$
match (a.HasValue, b.HasValue) with
| true, true -> Nullable (a.Value + b.Value)
| _ -> System.Nullable()
$$ LANGUAGE plfsharp;
SELECT checkedSumNullArgSmallIntFSharp(null,null) is NULL;
SELECT checkedSumNullArgSmallIntFSharp(null,CAST(133 AS smallint)) is NULL;
SELECT checkedSumNullArgSmallIntFSharp(CAST(133 AS smallint),null) is NULL;
SELECT checkedSumNullArgSmallIntFSharp(CAST(133 AS smallint),CAST(133 AS smallint)) = smallint '266';

CREATE OR REPLACE FUNCTION checkedSumNullArgBigIntFSharp(a bigint, b bigint) RETURNS bigint AS $$
match (a.HasValue, b.HasValue) with
| true, true -> Nullable (a.Value + b.Value)
| _ -> System.Nullable()
$$ LANGUAGE plfsharp;
SELECT checkedSumNullArgBigIntFSharp(null,null) is NULL;
SELECT checkedSumNullArgBigIntFSharp(null,100) is NULL;
SELECT checkedSumNullArgBigIntFSharp(9223372036854775707,null) is NULL;
SELECT checkedSumNullArgBigIntFSharp(9223372036854775707,100) = bigint '9223372036854775807';

/**************** Conditional return test functions (Mixed Args)  ***************************/
CREATE OR REPLACE FUNCTION checkedSumNullArgMixedFSharp(a integer, b smallint, c bigint) RETURNS bigint AS $$
match (a.HasValue, b.HasValue, c.HasValue) with
| true, true, true -> Nullable ((int64 a.Value) + (int64 b.Value) + c.Value)
| _ -> System.Nullable()
$$ LANGUAGE plfsharp;
SELECT checkedSumNullArgMixedFSharp(null,null,null) is NULL;
SELECT checkedSumNullArgMixedFSharp(null,CAST(1313 as smallint),null) is NULL;
SELECT checkedSumNullArgMixedFSharp(1313,null,null) is NULL;
SELECT checkedSumNullArgMixedFSharp(null,null,3) is NULL;
SELECT checkedSumNullArgMixedFSharp(1313,CAST(1313 as smallint), 1313) = smallint '3939';

