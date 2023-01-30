/**************** Null return test functions ***************************/
CREATE OR REPLACE FUNCTION returnNullIntPerl() RETURNS integer AS $$
return undef;
$$ LANGUAGE plperl;
SELECT returnNullIntPerl() is NULL;

CREATE OR REPLACE FUNCTION returnNullSmallIntPerl() RETURNS smallint AS $$
return undef;
$$ LANGUAGE plperl;
SELECT returnNullSmallIntPerl() is NULL;

CREATE OR REPLACE FUNCTION returnNullBigIntPerl() RETURNS bigint AS $$
return undef;
$$ LANGUAGE plperl;
SELECT returnNullBigIntPerl() is NULL;

/**************** Null operations test functions ***************************/
CREATE OR REPLACE FUNCTION sumNullArgIntPerl(a integer, b integer) RETURNS integer AS $$
return $_[0] + $_[1];
$$ LANGUAGE plperl;
SELECT sumNullArgIntPerl(null,null) = integer '0';
SELECT sumNullArgIntPerl(null,3) = integer '3';
SELECT sumNullArgIntPerl(3,null) = integer '3';
SELECT sumNullArgIntPerl(3,3) = integer '6';

CREATE OR REPLACE FUNCTION sumNullArgSmallIntPerl(a smallint, b smallint) RETURNS smallint AS $$
return $_[0] + $_[1];
$$ LANGUAGE plperl;
SELECT sumNullArgSmallIntPerl(null,null) = integer '0';
SELECT sumNullArgSmallIntPerl(null,CAST(101 AS smallint)) = integer '101';
SELECT sumNullArgSmallIntPerl(CAST(101 AS smallint),null) = integer '101';
SELECT sumNullArgSmallIntPerl(CAST(101 AS smallint),CAST(101 AS smallint)) = smallint '202';

CREATE OR REPLACE FUNCTION sumNullArgBigIntPerl(a bigint, b bigint) RETURNS bigint AS $$
return $_[0] + $_[1];
$$ LANGUAGE plperl;
SELECT sumNullArgBigIntPerl(null,null) = integer '0';
SELECT sumNullArgBigIntPerl(null,100) = integer '100';
/* Failing due to invalid input syntax for integer: "9.22337203685478e+18" */
-- SELECT sumNullArgBigIntPerl(9223372036854775707, null) = bigint '9223372036854775707';
SELECT sumNullArgBigIntPerl(9223372036854775707,100) = bigint '9223372036854775807';

/**************** Conditional return test functions ***************************/
CREATE OR REPLACE FUNCTION checkedSumNullArgIntPerl(a integer, b integer) RETURNS integer AS $$
if(not $_[0] or not $_[1]){
    return undef;
} else {
    return $_[0] + $_[1];
}
$$ LANGUAGE plperl;
SELECT checkedSumNullArgIntPerl(null,null) is NULL;
SELECT checkedSumNullArgIntPerl(null,3) is NULL;
SELECT checkedSumNullArgIntPerl(3,null) is NULL;
SELECT checkedSumNullArgIntPerl(3,3) = integer '6';

CREATE OR REPLACE FUNCTION checkedSumNullArgSmallIntPerl(a smallint, b smallint) RETURNS smallint AS $$
if(not $_[0] or not $_[1]){
    return undef;
} else {
    return $_[0] + $_[1] }
$$ LANGUAGE plperl;
SELECT checkedSumNullArgSmallIntPerl(null,null) is NULL;
SELECT checkedSumNullArgSmallIntPerl(null,CAST(133 AS smallint)) is NULL;
SELECT checkedSumNullArgSmallIntPerl(CAST(133 AS smallint),null) is NULL;
SELECT checkedSumNullArgSmallIntPerl(CAST(133 AS smallint),CAST(133 AS smallint)) = smallint '266';

CREATE OR REPLACE FUNCTION checkedSumNullArgBigIntPerl(a bigint, b bigint) RETURNS bigint AS $$
if(not $_[0] or not $_[1]) {
    return undef;
} else {
    return $_[0] + $_[1]; }
$$ LANGUAGE plperl;
SELECT checkedSumNullArgBigIntPerl(null,null) is NULL;
SELECT checkedSumNullArgBigIntPerl(null,100) is NULL;
SELECT checkedSumNullArgBigIntPerl(9223372036854775707,null) is NULL;
SELECT checkedSumNullArgBigIntPerl(9223372036854775707,100) = bigint '9223372036854775807';

/**************** Conditional return test functions (Mixed Args)  ***************************/
CREATE OR REPLACE FUNCTION checkedSumNullArgMixedPerl(a integer, b smallint, c bigint) RETURNS bigint AS $$
if(not $_[0] or not $_[1] or not $_[2]) {
    return undef;
} else {
    return $_[0] + $_[1] + $_[2]; }
$$ LANGUAGE plperl;
SELECT checkedSumNullArgMixedPerl(null,null,null) is NULL;
SELECT checkedSumNullArgMixedPerl(null,CAST(1313 as smallint),null) is NULL;
SELECT checkedSumNullArgMixedPerl(1313,null,null) is NULL;
SELECT checkedSumNullArgMixedPerl(null,null,3) is NULL;
SELECT checkedSumNullArgMixedPerl(1313,CAST(1313 as smallint), 1313) = smallint '3939';
