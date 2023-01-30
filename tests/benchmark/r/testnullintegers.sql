/**************** Null return test functions ***************************/
CREATE OR REPLACE FUNCTION returnNullIntR() RETURNS integer AS $$
return(NULL)
$$ LANGUAGE plr;
SELECT returnNullIntR() is NULL;

CREATE OR REPLACE FUNCTION returnNullSmallIntR() RETURNS smallint AS $$
return(NULL)
$$ LANGUAGE plr;
SELECT returnNullSmallIntR() is NULL;

CREATE OR REPLACE FUNCTION returnNullBigIntR() RETURNS bigint AS $$
return(NULL)
$$ LANGUAGE plr;
SELECT returnNullBigIntR() is NULL;

/**************** Null operations test functions ***************************/
CREATE OR REPLACE FUNCTION sumNullArgIntR(a integer, b integer) RETURNS integer AS $$
return(a + b)
$$
LANGUAGE plr;
SELECT sumNullArgIntR(null,null) is NULL;
SELECT sumNullArgIntR(null,3) is NULL;
SELECT sumNullArgIntR(3,null) is NULL;
SELECT sumNullArgIntR(3,3) = integer '6';

CREATE OR REPLACE FUNCTION sumNullArgSmallIntR(a smallint, b smallint) RETURNS smallint AS $$
return(a + b)
$$
LANGUAGE plr;
SELECT sumNullArgSmallIntR(null,null) is NULL;
SELECT sumNullArgSmallIntR(null,CAST(101 AS smallint)) is NULL;
SELECT sumNullArgSmallIntR(CAST(101 AS smallint),null) is NULL;
SELECT sumNullArgSmallIntR(CAST(101 AS smallint),CAST(101 AS smallint)) = smallint '202';

CREATE OR REPLACE FUNCTION sumNullArgBigIntR(a bigint, b bigint) RETURNS bigint AS $$
return(a + b)
$$
LANGUAGE plr;
SELECT sumNullArgBigIntR(null,null) is NULL;
SELECT sumNullArgBigIntR(null,100) is NULL;
SELECT sumNullArgBigIntR(9223372036854775707,null) is NULL;
/* Not working */
-- SELECT sumNullArgBigIntR(9223372036854775707,100) = bigint '9223372036854775807';

/**************** Conditional return test functions ***************************/
CREATE OR REPLACE FUNCTION checkedSumNullArgIntR(a integer, b integer) RETURNS integer AS $$
if(is.null(a) | is.null(b))
    return(NULL)
else
    return(a + b)
$$
LANGUAGE plr;
SELECT checkedSumNullArgIntR(null,null) is NULL;
SELECT checkedSumNullArgIntR(null,3) is NULL;
SELECT checkedSumNullArgIntR(3,null) is NULL;
SELECT checkedSumNullArgIntR(3,3) = integer '6';

CREATE OR REPLACE FUNCTION checkedSumNullArgSmallIntR(a smallint, b smallint) RETURNS smallint AS $$
if(is.null(a) | is.null(b))
    return(NULL)
else
    return(a + b)
$$
LANGUAGE plr;
SELECT checkedSumNullArgSmallIntR(null,null) is NULL;
SELECT checkedSumNullArgSmallIntR(null,CAST(133 AS smallint)) is NULL;
SELECT checkedSumNullArgSmallIntR(CAST(133 AS smallint),null) is NULL;
SELECT checkedSumNullArgSmallIntR(CAST(133 AS smallint),CAST(133 AS smallint)) = smallint '266';

CREATE OR REPLACE FUNCTION checkedSumNullArgBigIntR(a bigint, b bigint) RETURNS bigint AS $$
if(is.null(a) | is.null(b))
    return(NULL)
else
    return(a + b)
$$
LANGUAGE plr;
SELECT checkedSumNullArgBigIntR(null,null) is NULL;
SELECT checkedSumNullArgBigIntR(null,100) is NULL;
SELECT checkedSumNullArgBigIntR(9223372036854775707,null) is NULL;
/* Not Working */
-- SELECT checkedSumNullArgBigIntR(9223372036854775707,100) = bigint '9223372036854775807';

/**************** Conditional return test functions (Mixed Args)  ***************************/
CREATE OR REPLACE FUNCTION checkedSumNullArgMixedR(a integer, b smallint, c bigint) RETURNS bigint AS $$
if(is.null(a) | is.null(b) | is.null(c))
    return(NULL)
else
    return(a + b + c)
$$
LANGUAGE plr;
SELECT checkedSumNullArgMixedR(null,null,null) is NULL;
SELECT checkedSumNullArgMixedR(null,CAST(1313 as smallint),null) is NULL;
SELECT checkedSumNullArgMixedR(1313,null,null) is NULL;
SELECT checkedSumNullArgMixedR(null,null,3) is NULL;
SELECT checkedSumNullArgMixedR(1313,CAST(1313 as smallint), 1313) = smallint '3939';
