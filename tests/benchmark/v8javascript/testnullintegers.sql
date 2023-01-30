/**************** Null return test functions ***************************/
CREATE OR REPLACE FUNCTION returnNullIntV8() RETURNS integer AS $$
return null;
$$ LANGUAGE plv8;
SELECT returnNullIntV8() is NULL;

CREATE OR REPLACE FUNCTION returnNullSmallIntV8() RETURNS smallint AS $$
return null;
$$ LANGUAGE plv8;
SELECT returnNullSmallIntV8() is NULL;

CREATE OR REPLACE FUNCTION returnNullBigIntV8() RETURNS bigint AS $$
return null;
$$ LANGUAGE plv8;
SELECT returnNullBigIntV8() is NULL;

/**************** Null operations test functions ***************************/
CREATE OR REPLACE FUNCTION sumNullArgIntV8(a integer, b integer) RETURNS integer AS $$
return a + b;
$$
LANGUAGE plv8;
SELECT sumNullArgIntV8(null,null) = integer '0';
SELECT sumNullArgIntV8(null,3) = integer '3';
SELECT sumNullArgIntV8(3,null) = integer '3';
SELECT sumNullArgIntV8(3,3) = integer '6';

CREATE OR REPLACE FUNCTION sumNullArgSmallIntV8(a smallint, b smallint) RETURNS smallint AS $$
return a + b;
$$
LANGUAGE plv8;
SELECT sumNullArgSmallIntV8(null,null) = integer '0';
SELECT sumNullArgSmallIntV8(null,CAST(101 AS smallint)) = integer '101';
SELECT sumNullArgSmallIntV8(CAST(101 AS smallint),null) = integer '101';
SELECT sumNullArgSmallIntV8(CAST(101 AS smallint),CAST(101 AS smallint)) = smallint '202';

CREATE OR REPLACE FUNCTION sumNullArgBigIntV8(a bigint, b bigint) RETURNS bigint AS $$
if (a == null) {
    a = 0n;
}
if (b == null ) {
    b = 0n;
}
return a + b;
$$
LANGUAGE plv8;
SELECT sumNullArgBigIntV8(null,null) = integer '0';
SELECT sumNullArgBigIntV8(null,100) = integer '100';
/* Failing test case */
SELECT sumNullArgBigIntV8(9223372036854775707,null) = bigint '9223372036854775707';
SELECT sumNullArgBigIntV8(9223372036854775707,100) = bigint '9223372036854775807';

/**************** Conditional return test functions ***************************/
CREATE OR REPLACE FUNCTION checkedSumNullArgIntV8(a integer, b integer) RETURNS integer AS $$
if(!a || !b)
    return null;
else
    return a + b;
$$
LANGUAGE plv8;
SELECT checkedSumNullArgIntV8(null,null) is NULL;
SELECT checkedSumNullArgIntV8(null,3) is NULL;
SELECT checkedSumNullArgIntV8(3,null) is NULL;
SELECT checkedSumNullArgIntV8(3,3) = integer '6';

CREATE OR REPLACE FUNCTION checkedSumNullArgSmallIntV8(a smallint, b smallint) RETURNS smallint AS $$
if(!a || !b)
    return null;
else
    return a + b;
$$
LANGUAGE plv8;
SELECT checkedSumNullArgSmallIntV8(null,null) is NULL;
SELECT checkedSumNullArgSmallIntV8(null,CAST(133 AS smallint)) is NULL;
SELECT checkedSumNullArgSmallIntV8(CAST(133 AS smallint),null) is NULL;
SELECT checkedSumNullArgSmallIntV8(CAST(133 AS smallint),CAST(133 AS smallint)) = smallint '266';

CREATE OR REPLACE FUNCTION checkedSumNullArgBigIntV8(a bigint, b bigint) RETURNS bigint AS $$
if(!a || !b)
    return null;
else
    return a + b;
$$
LANGUAGE plv8;
SELECT checkedSumNullArgBigIntV8(null,null) is NULL;
SELECT checkedSumNullArgBigIntV8(null,100) is NULL;
SELECT checkedSumNullArgBigIntV8(9223372036854775707,null) is NULL;
/* Failing test case*/
SELECT checkedSumNullArgBigIntV8(9223372036854775707,100) = bigint '9223372036854775807';

/**************** Conditional return test functions (Mixed Args)  ***************************/
CREATE OR REPLACE FUNCTION checkedSumNullArgMixedV8(a integer, b smallint, c bigint) RETURNS bigint AS $$
if(!a || !b || !c)
    return null;
else
    return BigInt(a) + BigInt(b) + c;
$$
LANGUAGE plv8;
SELECT checkedSumNullArgMixedV8(null,null,null) is NULL;
SELECT checkedSumNullArgMixedV8(null,CAST(1313 as smallint),null) is NULL;
SELECT checkedSumNullArgMixedV8(1313,null,null) is NULL;
SELECT checkedSumNullArgMixedV8(null,null,3) is NULL;
SELECT checkedSumNullArgMixedV8(1313,CAST(1313 as smallint), 1313) = smallint '3939';

