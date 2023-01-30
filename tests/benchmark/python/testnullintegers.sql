/**************** Null return test functions ***************************/
CREATE OR REPLACE FUNCTION returnNullIntPython() RETURNS integer AS $$
return None
$$ LANGUAGE plpython3u;
SELECT returnNullIntPython() is NULL;

CREATE OR REPLACE FUNCTION returnNullSmallIntPython() RETURNS smallint AS $$
return None
$$ LANGUAGE plpython3u;
SELECT returnNullSmallIntPython() is NULL;

CREATE OR REPLACE FUNCTION returnNullBigIntPython() RETURNS bigint AS $$
return None
$$ LANGUAGE plpython3u;
SELECT returnNullBigIntPython() is NULL;

/**************** Null operations test functions ***************************/
CREATE OR REPLACE FUNCTION sumNullArgIntPython(a integer, b integer) RETURNS integer AS $$
return sum(filter(None, [a,b]))
$$ LANGUAGE plpython3u;
SELECT sumNullArgIntPython(null,null) = integer '0';
SELECT sumNullArgIntPython(null,3) = integer '3';
SELECT sumNullArgIntPython(3,null) = integer '3';
SELECT sumNullArgIntPython(3,3) = integer '6';

CREATE OR REPLACE FUNCTION sumNullArgSmallIntPython(a smallint, b smallint) RETURNS smallint AS $$
return sum(filter(None, [a,b]))
$$ LANGUAGE plpython3u;
SELECT sumNullArgSmallIntPython(null,null) = integer '0';
SELECT sumNullArgSmallIntPython(null,CAST(101 AS smallint)) = integer '101';
SELECT sumNullArgSmallIntPython(CAST(101 AS smallint),null) = integer '101';
SELECT sumNullArgSmallIntPython(CAST(101 AS smallint),CAST(101 AS smallint)) = smallint '202';

CREATE OR REPLACE FUNCTION sumNullArgBigIntPython(a bigint, b bigint) RETURNS bigint AS $$
return sum(filter(None, [a,b]))
$$ LANGUAGE plpython3u;
SELECT sumNullArgBigIntPython(null,null) = integer '0';
SELECT sumNullArgBigIntPython(null,100) = integer '100';
SELECT sumNullArgBigIntPython(9223372036854775707,null) = bigint '9223372036854775707';
SELECT sumNullArgBigIntPython(9223372036854775707,100) = bigint '9223372036854775807';

/**************** Conditional return test functions ***************************/
CREATE OR REPLACE FUNCTION checkedSumNullArgIntPython(a integer, b integer) RETURNS integer AS $$
if (not a or not b):
    return None
else:
    return a + b
$$ LANGUAGE plpython3u;
SELECT checkedSumNullArgIntPython(null,null) is NULL;
SELECT checkedSumNullArgIntPython(null,3) is NULL;
SELECT checkedSumNullArgIntPython(3,null) is NULL;
SELECT checkedSumNullArgIntPython(3,3) = integer '6';

CREATE OR REPLACE FUNCTION checkedSumNullArgSmallIntPython(a smallint, b smallint) RETURNS smallint AS $$
if (not a or not b):
    return None
else:
    return a + b
$$ LANGUAGE plpython3u;
SELECT checkedSumNullArgSmallIntPython(null,null) is NULL;
SELECT checkedSumNullArgSmallIntPython(null,CAST(133 AS smallint)) is NULL;
SELECT checkedSumNullArgSmallIntPython(CAST(133 AS smallint),null) is NULL;
SELECT checkedSumNullArgSmallIntPython(CAST(133 AS smallint),CAST(133 AS smallint)) = smallint '266';

CREATE OR REPLACE FUNCTION checkedSumNullArgBigIntPython(a bigint, b bigint) RETURNS bigint AS $$
if (not a or not b):
    return None
else:
    return a + b
$$ LANGUAGE plpython3u;
SELECT checkedSumNullArgBigIntPython(null,null) is NULL;
SELECT checkedSumNullArgBigIntPython(null,100) is NULL;
SELECT checkedSumNullArgBigIntPython(9223372036854775707,null) is NULL;
SELECT checkedSumNullArgBigIntPython(9223372036854775707,100) = bigint '9223372036854775807';

/**************** Conditional return test functions (Mixed Args)  ***************************/
CREATE OR REPLACE FUNCTION checkedSumNullArgMixedPython(a integer, b smallint, c bigint) RETURNS bigint AS $$
if (not a or not b or not c):
    return None
else:
    return a + b + c
$$ LANGUAGE plpython3u;
SELECT checkedSumNullArgMixedPython(null,null,null) is NULL;
SELECT checkedSumNullArgMixedPython(null,CAST(1313 as smallint),null) is NULL;
SELECT checkedSumNullArgMixedPython(1313,null,null) is NULL;
SELECT checkedSumNullArgMixedPython(null,null,3) is NULL;
SELECT checkedSumNullArgMixedPython(1313,CAST(1313 as smallint), 1313) = smallint '3939';
