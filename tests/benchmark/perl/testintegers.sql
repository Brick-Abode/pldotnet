CREATE OR REPLACE FUNCTION maxSmallIntPerl() RETURNS smallint AS $$
return 32767;
$$ LANGUAGE plperl;
SELECT maxSmallIntPerl() = integer '32767';

CREATE OR REPLACE FUNCTION sum2SmallIntPerl(a smallint, b smallint) RETURNS smallint AS $$
return $_[0] + $_[1];
$$ LANGUAGE plperl;
SELECT sum2SmallIntPerl(CAST(100 AS smallint), CAST(101 AS smallint)) = smallint '201';

CREATE OR REPLACE FUNCTION maxIntegerPerl() RETURNS integer AS $$
return 2147483647;
$$ LANGUAGE plperl;
SELECT maxIntegerPerl() = integer '2147483647';

CREATE OR REPLACE FUNCTION returnIntPerl() RETURNS integer AS $$
return 10;
$$ LANGUAGE plperl;
SELECT returnIntPerl() = integer '10';

CREATE OR REPLACE FUNCTION inc2ToIntPerl(val integer) RETURNS integer AS $$
return $_[0] + 2;
$$ LANGUAGE plperl;
SELECT inc2ToIntPerl(8) = integer '10';

CREATE OR REPLACE FUNCTION sum3IntegerPerl(aaa integer, bbb integer, ccc integer) RETURNS integer AS $$
return $_[0] + $_[1] + $_[2];
$$
LANGUAGE plperl;
SELECT sum3IntegerPerl(3,2,1) = integer '6';

CREATE OR REPLACE FUNCTION sum4IntegerPerl(a integer, b integer, c integer, d integer) RETURNS integer AS $$
return $_[0] + $_[1] + $_[2] + $_[3];
$$ LANGUAGE plperl;
SELECT sum4IntegerPerl(4,3,2,1) = integer '10';

CREATE OR REPLACE FUNCTION sum2IntegerPerl(a integer, b integer) RETURNS integer AS $$
return $_[0] + $_[1];
$$ LANGUAGE plperl;
SELECT sum2IntegerPerl(32770, 100) = bigint '32870';

CREATE OR REPLACE FUNCTION maxBigIntPerl() RETURNS bigint AS $$
return 9223372036854775807
$$ LANGUAGE plperl;
SELECT maxBigIntPerl() = bigint '9223372036854775807';

CREATE OR REPLACE FUNCTION sum2BigIntPerl(a bigint, b bigint) RETURNS bigint AS $$
return $_[0] + $_[1];
$$ LANGUAGE plperl;
SELECT sum2BigIntPerl(9223372036854775707, 100) = bigint '9223372036854775807';

CREATE OR REPLACE FUNCTION mixedBigIntPerl(a integer, b integer, c bigint) RETURNS bigint AS $$
return $_[0] + $_[1] + $_[2];
$$ LANGUAGE plperl;
SELECT mixedBigIntPerl(32767,  2147483647, 100) = bigint '2147516514';

CREATE OR REPLACE FUNCTION mixedIntPerl(a smallint, b smallint, c integer) RETURNS integer AS $$
return $_[0] + $_[1] + $_[2];
$$ LANGUAGE plperl;
SELECT mixedIntPerl(CAST(32767 AS smallint),  CAST(32767 AS smallint), 100) = integer '65634';
