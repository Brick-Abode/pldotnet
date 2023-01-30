CREATE OR REPLACE FUNCTION maxSmallIntTcl() RETURNS smallint AS $$
return 32767;
$$ LANGUAGE pltcl;
SELECT maxSmallIntTcl() = integer '32767';

CREATE OR REPLACE FUNCTION sum2SmallIntTcl(a smallint, b smallint) RETURNS smallint AS $$
return [expr $1 + $2];
$$ LANGUAGE pltcl;
SELECT sum2SmallIntTcl(CAST(100 AS smallint), CAST(101 AS smallint)) = smallint '201';

CREATE OR REPLACE FUNCTION maxIntegerTcl() RETURNS integer AS $$
return 2147483647;
$$ LANGUAGE pltcl;
SELECT maxIntegerTcl() = integer '2147483647';

CREATE OR REPLACE FUNCTION returnIntTcl() RETURNS integer AS $$
return 10;
$$ LANGUAGE pltcl;
SELECT returnIntTcl() = integer '10';

CREATE OR REPLACE FUNCTION inc2ToIntTcl(val integer) RETURNS integer AS $$
return [expr $1 + 2];
$$
LANGUAGE pltcl;
SELECT inc2ToIntTcl(8) = integer '10';

CREATE OR REPLACE FUNCTION sum3IntegerTcl(aaa integer, bbb integer, ccc integer) RETURNS integer AS $$
return [expr $1 + $2 + $3];
$$
LANGUAGE pltcl;
SELECT sum3IntegerTcl(3,2,1) = integer '6';

CREATE OR REPLACE FUNCTION sum4IntegerTcl(a integer, b integer, c integer, d integer) RETURNS integer AS $$
return [expr $1 + $2 + $3 + $4];
$$
LANGUAGE pltcl;
SELECT sum4IntegerTcl(4,3,2,1) = integer '10';

CREATE OR REPLACE FUNCTION sum2IntegerTcl(a integer, b integer) RETURNS integer AS $$
return [expr $1 + $2];
$$ LANGUAGE pltcl;
SELECT sum2IntegerTcl(32770, 100) = bigint '32870';

CREATE OR REPLACE FUNCTION maxBigIntTcl() RETURNS bigint AS $$
return 9223372036854775807;
$$ LANGUAGE pltcl;
SELECT maxBigIntTcl() = bigint '9223372036854775807';

CREATE OR REPLACE FUNCTION sum2BigIntTcl(a bigint, b bigint) RETURNS bigint AS $$
return [expr $1 + $2];
$$ LANGUAGE pltcl;
SELECT sum2BigIntTcl(9223372036854775707, 100) = bigint '9223372036854775807';

CREATE OR REPLACE FUNCTION mixedBigIntTcl(a integer, b integer, c bigint) RETURNS bigint AS $$
return [expr $1 + $2 + $3];
$$ LANGUAGE pltcl;
SELECT mixedBigIntTcl(32767,  2147483647, 100) = bigint '2147516514';

CREATE OR REPLACE FUNCTION mixedIntTcl(a smallint, b smallint, c integer) RETURNS integer AS $$
return [expr $1 + $2 + $3];
$$ LANGUAGE pltcl;
SELECT mixedIntTcl(CAST(32767 AS smallint),  CAST(32767 AS smallint), 100) = integer '65634';
