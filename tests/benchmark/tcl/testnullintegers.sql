/**************** Null return test functions ***************************/
CREATE OR REPLACE FUNCTION returnNullIntTcl() RETURNS integer AS $$
return_null;
$$ LANGUAGE pltcl;
SELECT returnNullIntTcl() is NULL;

CREATE OR REPLACE FUNCTION returnNullSmallIntTcl() RETURNS smallint AS $$
return_null;
$$ LANGUAGE pltcl;
SELECT returnNullSmallIntTcl() is NULL;

CREATE OR REPLACE FUNCTION returnNullBigIntTcl() RETURNS bigint AS $$
return_null;
$$ LANGUAGE pltcl;
SELECT returnNullBigIntTcl() is NULL;

/**************** Null operations test functions ***************************/
CREATE OR REPLACE FUNCTION sumNullArgIntTcl(a integer, b integer) RETURNS integer AS $$
if {[argisnull 1] || [argisnull 2]} return_null;
return [expr $1 + $2];
$$
LANGUAGE pltcl;
SELECT sumNullArgIntTcl(null,null) is NULL;
SELECT sumNullArgIntTcl(null,3) is NULL;
SELECT sumNullArgIntTcl(3,null) is NULL;
SELECT sumNullArgIntTcl(3,3) = integer '6';

CREATE OR REPLACE FUNCTION sumNullArgSmallIntTcl(a smallint, b smallint) RETURNS smallint AS $$
if {[argisnull 1] || [argisnull 2]} return_null;
return [expr $1 + $2];
$$
LANGUAGE pltcl;
SELECT sumNullArgSmallIntTcl(null,null) is NULL;
SELECT sumNullArgSmallIntTcl(null,CAST(101 AS smallint)) is NULL;
SELECT sumNullArgSmallIntTcl(CAST(101 AS smallint),null) is NULL;
SELECT sumNullArgSmallIntTcl(CAST(101 AS smallint),CAST(101 AS smallint)) = smallint '202';

CREATE OR REPLACE FUNCTION sumNullArgBigIntTcl(a bigint, b bigint) RETURNS bigint AS $$
if {[argisnull 1] || [argisnull 2]} return_null;
return [expr $1 + $2];
$$
LANGUAGE pltcl;
SELECT sumNullArgBigIntTcl(null,null) is NULL;
SELECT sumNullArgBigIntTcl(null,100) is NULL;
SELECT sumNullArgBigIntTcl(9223372036854775707,null) is NULL;
SELECT sumNullArgBigIntTcl(9223372036854775707,100) = bigint '9223372036854775807';

/**************** Conditional return test functions ***************************/
CREATE OR REPLACE FUNCTION checkedsumNullArgIntTcl(a integer, b integer) RETURNS integer AS $$
if {[argisnull 1] || [argisnull 2]} return_null;
return [expr $1 + $2];
$$
LANGUAGE pltcl;
SELECT checkedsumNullArgIntTcl(null,null) is NULL;
SELECT checkedsumNullArgIntTcl(null,3) is NULL;
SELECT checkedsumNullArgIntTcl(3,null) is NULL;
SELECT checkedsumNullArgIntTcl(3,3) = integer '6';

CREATE OR REPLACE FUNCTION checkedsumNullArgSmallIntTcl(a smallint, b smallint) RETURNS smallint AS $$
if {[argisnull 1] || [argisnull 2]} return_null;
return [expr $1 + $2];
$$
LANGUAGE pltcl;
SELECT checkedsumNullArgSmallIntTcl(null,null) is NULL;
SELECT checkedsumNullArgSmallIntTcl(null,CAST(133 AS smallint)) is NULL;
SELECT checkedsumNullArgSmallIntTcl(CAST(133 AS smallint),null) is NULL;
SELECT checkedsumNullArgSmallIntTcl(CAST(133 AS smallint),CAST(133 AS smallint)) = smallint '266';

CREATE OR REPLACE FUNCTION checkedsumNullArgBigIntTcl(a bigint, b bigint) RETURNS bigint AS $$
if {[argisnull 1] || [argisnull 2]} return_null;
return [expr $1 + $2];
$$
LANGUAGE pltcl;
SELECT checkedsumNullArgBigIntTcl(null,null) is NULL;
SELECT checkedsumNullArgBigIntTcl(null,100) is NULL;
SELECT checkedsumNullArgBigIntTcl(9223372036854775707,null) is NULL;
SELECT checkedsumNullArgBigIntTcl(9223372036854775707,100) = bigint '9223372036854775807';

/**************** Conditional return test functions (Mixed Args)  ***************************/
CREATE OR REPLACE FUNCTION checkedSumNullArgMixedTcl(a integer, b smallint, c bigint) RETURNS bigint AS $$
if {[argisnull 1] || [argisnull 2] || [argisnull 3]} return_null;
return [expr $1 + $2 + $3];
$$
LANGUAGE pltcl;
SELECT checkedSumNullArgMixedTcl(null,null,null) is NULL;
SELECT checkedSumNullArgMixedTcl(null,CAST(1313 as smallint),null) is NULL;
SELECT checkedSumNullArgMixedTcl(1313,null,null) is NULL;
SELECT checkedSumNullArgMixedTcl(null,null,3) is NULL;
SELECT checkedSumNullArgMixedTcl(1313,CAST(1313 as smallint), 1313) = smallint '3939';
