/**************** Null return test functions ***************************/
CREATE OR REPLACE FUNCTION returnNullIntPg() RETURNS integer AS $$
DECLARE
BEGIN
    RETURN NULL;
END
$$ LANGUAGE plpgsql;
SELECT returnNullIntPg() is NULL;

CREATE OR REPLACE FUNCTION returnNullSmallIntPg() RETURNS smallint AS $$
DECLARE
BEGIN
    RETURN NULL;
END
$$ LANGUAGE plpgsql;
SELECT returnNullSmallIntPg() is NULL;

CREATE OR REPLACE FUNCTION returnNullBigIntPg() RETURNS bigint AS $$
DECLARE
BEGIN
    RETURN NULL;
END
$$ LANGUAGE plpgsql;
SELECT returnNullBigIntPg() is NULL;

/**************** Null operations test functions ***************************/
CREATE OR REPLACE FUNCTION sumNullArgIntPg(a integer, b integer) RETURNS integer AS $$
DECLARE
BEGIN
    RETURN a + b;
END
$$
LANGUAGE plpgsql;
SELECT sumNullArgIntPg(null,null) is NULL;
SELECT sumNullArgIntPg(null,3) is NULL;
SELECT sumNullArgIntPg(3,null) is NULL;
SELECT sumNullArgIntPg(3,3) = integer '6';

CREATE OR REPLACE FUNCTION sumNullArgSmallIntPg(a smallint, b smallint) RETURNS smallint AS $$
DECLARE
BEGIN
    RETURN a + b;
END
$$
LANGUAGE plpgsql;
SELECT sumNullArgSmallIntPg(null,null) is NULL;
SELECT sumNullArgSmallIntPg(null,CAST(101 AS smallint)) is NULL;
SELECT sumNullArgSmallIntPg(CAST(101 AS smallint),null) is NULL;
SELECT sumNullArgSmallIntPg(CAST(101 AS smallint),CAST(101 AS smallint)) = smallint '202';

CREATE OR REPLACE FUNCTION sumNullArgBigIntPg(a bigint, b bigint) RETURNS bigint AS $$
DECLARE
BEGIN
    RETURN a + b;
END
$$
LANGUAGE plpgsql;
SELECT sumNullArgBigIntPg(null,null) is NULL;
SELECT sumNullArgBigIntPg(null,100) is NULL;
SELECT sumNullArgBigIntPg(9223372036854775707,null) is NULL;
SELECT sumNullArgBigIntPg(9223372036854775707,100) = bigint '9223372036854775807';

/**************** Conditional return test functions ***************************/
CREATE OR REPLACE FUNCTION checkedSumNullArgIntPg(a integer, b integer) RETURNS integer AS $$
DECLARE
BEGIN
    IF a IS NULL OR b IS NULL THEN
        RETURN NULL;
    ELSE
        RETURN a + b;
    END IF;
END
$$
LANGUAGE plpgsql;
SELECT checkedSumNullArgIntPg(null,null) is NULL;
SELECT checkedSumNullArgIntPg(null,3) is NULL;
SELECT checkedSumNullArgIntPg(3,null) is NULL;
SELECT checkedSumNullArgIntPg(3,3) = integer '6';

CREATE OR REPLACE FUNCTION checkedSumNullArgSmallIntPg(a smallint, b smallint) RETURNS smallint AS $$
DECLARE
BEGIN
    IF a IS NULL OR b IS NULL THEN
        RETURN NULL;
    ELSE
        RETURN a + b;
    END IF;
END
$$
LANGUAGE plpgsql;
SELECT checkedSumNullArgSmallIntPg(null,null) is NULL;
SELECT checkedSumNullArgSmallIntPg(null,CAST(133 AS smallint)) is NULL;
SELECT checkedSumNullArgSmallIntPg(CAST(133 AS smallint),null) is NULL;
SELECT checkedSumNullArgSmallIntPg(CAST(133 AS smallint),CAST(133 AS smallint)) = smallint '266';

CREATE OR REPLACE FUNCTION checkedSumNullArgBigIntPg(a bigint, b bigint) RETURNS bigint AS $$
DECLARE
BEGIN
    IF a IS NULL OR b IS NULL THEN
        RETURN NULL;
    ELSE
        RETURN a + b;
    END IF;
END
$$
LANGUAGE plpgsql;
SELECT checkedSumNullArgBigIntPg(null,null) is NULL;
SELECT checkedSumNullArgBigIntPg(null,100) is NULL;
SELECT checkedSumNullArgBigIntPg(9223372036854775707,null) is NULL;
SELECT checkedSumNullArgBigIntPg(9223372036854775707,100) = bigint '9223372036854775807';

/**************** Conditional return test functions (Mixed Args)  ***************************/
CREATE OR REPLACE FUNCTION checkedSumNullArgMixedPg(a integer, b smallint, c bigint) RETURNS bigint AS $$
DECLARE
BEGIN
    IF a IS NULL OR b IS NULL or c IS NULL THEN
        RETURN NULL;
    ELSE
        RETURN a + b + c;
    END IF;
END
$$
LANGUAGE plpgsql;
SELECT checkedSumNullArgMixedPg(null,null,null) is NULL;
SELECT checkedSumNullArgMixedPg(null,CAST(1313 as smallint),null) is NULL;
SELECT checkedSumNullArgMixedPg(1313,null,null) is NULL;
SELECT checkedSumNullArgMixedPg(null,null,3) is NULL;
SELECT checkedSumNullArgMixedPg(1313,CAST(1313 as smallint), 1313) = smallint '3939';
