/**************** Null return test functions ***************************/
CREATE OR REPLACE FUNCTION returnNullInt() RETURNS integer AS $$
return null;
$$ LANGUAGE plcsharp;
SELECT returnNullInt() is NULL;

CREATE OR REPLACE FUNCTION returnNullSmallInt() RETURNS smallint AS $$
return null;
$$ LANGUAGE plcsharp;
SELECT returnNullSmallInt() is NULL;

CREATE OR REPLACE FUNCTION returnNullBigInt() RETURNS bigint AS $$
return null;
$$ LANGUAGE plcsharp;
SELECT returnNullBigInt() is NULL;

/**************** Null operations test functions ***************************/
CREATE OR REPLACE FUNCTION sumNullArgInt(a integer, b integer) RETURNS integer AS $$
return a + b;
$$
LANGUAGE plcsharp;
SELECT sumNullArgInt(null,null) is NULL;
SELECT sumNullArgInt(null,3) is NULL;
SELECT sumNullArgInt(3,null) is NULL;
SELECT sumNullArgInt(3,3) = integer '6';

CREATE OR REPLACE FUNCTION sumNullArgSmallInt(a smallint, b smallint) RETURNS smallint AS $$
return (short?)(a + b);
$$
LANGUAGE plcsharp;
SELECT sumNullArgSmallInt(null,null) is NULL;
SELECT sumNullArgSmallInt(null,CAST(101 AS smallint)) is NULL;
SELECT sumNullArgSmallInt(CAST(101 AS smallint),null) is NULL;
SELECT sumNullArgSmallInt(CAST(101 AS smallint),CAST(101 AS smallint)) = smallint '202';

CREATE OR REPLACE FUNCTION sumNullArgBigInt(a bigint, b bigint) RETURNS bigint AS $$
return a + b;
$$
LANGUAGE plcsharp;
SELECT sumNullArgBigInt(null,null) is NULL;
SELECT sumNullArgBigInt(null,100) is NULL;
SELECT sumNullArgBigInt(9223372036854775707,null) is NULL;
SELECT sumNullArgBigInt(9223372036854775707,100) = bigint '9223372036854775807';

/**************** Conditional return test functions ***************************/
CREATE OR REPLACE FUNCTION checkedSumNullArgInt(a integer, b integer) RETURNS integer AS $$
if(!a.HasValue || !b.HasValue)
    return null;
else
    return a + b;
$$
LANGUAGE plcsharp;
SELECT checkedSumNullArgInt(null,null) is NULL;
SELECT checkedSumNullArgInt(null,3) is NULL;
SELECT checkedSumNullArgInt(3,null) is NULL;
SELECT checkedSumNullArgInt(3,3) = integer '6';

CREATE OR REPLACE FUNCTION checkedSumNullArgSmallInt(a smallint, b smallint) RETURNS smallint AS $$
if(!a.HasValue || !b.HasValue)
    return null;
else
    return (short?)(a + b);
$$
LANGUAGE plcsharp;
SELECT checkedSumNullArgSmallInt(null,null) is NULL;
SELECT checkedSumNullArgSmallInt(null,CAST(133 AS smallint)) is NULL;
SELECT checkedSumNullArgSmallInt(CAST(133 AS smallint),null) is NULL;
SELECT checkedSumNullArgSmallInt(CAST(133 AS smallint),CAST(133 AS smallint)) = smallint '266';

CREATE OR REPLACE FUNCTION checkedSumNullArgBigInt(a bigint, b bigint) RETURNS bigint AS $$
if(!a.HasValue || !b.HasValue)
    return null;
else
    return a + b;
$$
LANGUAGE plcsharp;
SELECT checkedSumNullArgBigInt(null,null) is NULL;
SELECT checkedSumNullArgBigInt(null,100) is NULL;
SELECT checkedSumNullArgBigInt(9223372036854775707,null) is NULL;
SELECT checkedSumNullArgBigInt(9223372036854775707,100) = bigint '9223372036854775807';

/**************** Conditional return test functions (Mixed Args)  ***************************/
CREATE OR REPLACE FUNCTION checkedSumNullArgMixed(a integer, b smallint, c bigint) RETURNS bigint AS $$
if(!a.HasValue || !b.HasValue || !c.HasValue)
    return null;
else
    return (long)a + (long)b + c;
$$
LANGUAGE plcsharp;
SELECT checkedSumNullArgMixed(null,null,null) is NULL;
SELECT checkedSumNullArgMixed(null,CAST(1313 as smallint),null) is NULL;
SELECT checkedSumNullArgMixed(1313,null,null) is NULL;
SELECT checkedSumNullArgMixed(null,null,3) is NULL;
SELECT checkedSumNullArgMixed(1313,CAST(1313 as smallint), 1313) = smallint '3939';

