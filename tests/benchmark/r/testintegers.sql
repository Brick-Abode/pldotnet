CREATE OR REPLACE FUNCTION maxSmallIntR() RETURNS smallint AS $$
return(32767)
$$ LANGUAGE plr;
SELECT maxSmallIntR() = integer '32767';

CREATE OR REPLACE FUNCTION sum2SmallIntR(a smallint, b smallint) RETURNS smallint AS $$
return(a+b)
$$ LANGUAGE plr;
SELECT sum2SmallIntR(CAST(100 AS smallint), CAST(101 AS smallint)) = smallint '201';

CREATE OR REPLACE FUNCTION maxIntegerR() RETURNS integer AS $$
return(2147483647)
$$ LANGUAGE plr;
SELECT maxIntegerR() = integer '2147483647';

CREATE OR REPLACE FUNCTION returnIntR() RETURNS integer AS $$
return(10)
$$ LANGUAGE plr;
SELECT returnIntR() = integer '10';

CREATE OR REPLACE FUNCTION inc2ToIntR(val integer) RETURNS integer AS $$
return(val + 2)
$$
LANGUAGE plr;
SELECT inc2ToIntR(8) = integer '10';

CREATE OR REPLACE FUNCTION sum3IntegerR(aaa integer, bbb integer, ccc integer) RETURNS integer AS $$
return(aaa + bbb + ccc)
$$
LANGUAGE plr;
SELECT sum3IntegerR(3,2,1) = integer '6';

CREATE OR REPLACE FUNCTION sum4IntegerR(a integer, b integer, c integer, d integer) RETURNS integer AS $$
return(a + b + c + d)
$$
LANGUAGE plr;
SELECT sum4IntegerR(4,3,2,1) = integer '10';

CREATE OR REPLACE FUNCTION sum2IntegerR(a integer, b integer) RETURNS integer AS $$
return(a+b)
$$ LANGUAGE plr;
SELECT sum2IntegerR(32770, 100) = bigint '32870';

CREATE OR REPLACE FUNCTION maxBigIntR() RETURNS bigint AS $$
return('9223372036854775807')
$$ LANGUAGE plr;
SELECT maxBigIntR() = bigint '9223372036854775807';

/* Not Working */
-- CREATE OR REPLACE FUNCTION sum2BigIntR(a bigint, b bigint) RETURNS bigint AS $$
-- return(a+b)
-- $$ LANGUAGE plr;
-- SELECT sum2BigIntR(9223372036854775707, 100) = bigint '9223372036854775807';

/* Not Working */
-- CREATE OR REPLACE FUNCTION mixedBigIntR(a integer, b integer, c bigint) RETURNS bigint AS $$
-- return(a + b)
-- $$ LANGUAGE plr;
-- SELECT mixedBigIntR(32767,  2147483647, CAST(100 as bigint)) = bigint '2147516514';

CREATE OR REPLACE FUNCTION mixedIntR(a smallint, b smallint, c integer) RETURNS integer AS $$
return(a+b+c)
$$ LANGUAGE plr;
SELECT mixedInt(CAST(32767 AS smallint),  CAST(32767 AS smallint), 100) = integer '65634';
