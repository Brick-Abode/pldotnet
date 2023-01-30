CREATE OR REPLACE FUNCTION maxSmallInt() RETURNS smallint AS $$
return (short)32767;
$$ LANGUAGE plcsharp;
SELECT maxSmallInt() = integer '32767';

CREATE OR REPLACE FUNCTION sum2SmallInt(a smallint, b smallint) RETURNS smallint AS $$
return (short)(a+b); //C# requires short cast
$$ LANGUAGE plcsharp;
SELECT sum2SmallInt(CAST(100 AS smallint), CAST(101 AS smallint)) = smallint '201';

CREATE OR REPLACE FUNCTION maxInteger() RETURNS integer AS $$
return 2147483647;
$$ LANGUAGE plcsharp;
SELECT maxInteger() = integer '2147483647';

CREATE OR REPLACE FUNCTION returnInt() RETURNS integer AS $$
return 10;
$$ LANGUAGE plcsharp;
SELECT returnInt() = integer '10';

CREATE OR REPLACE FUNCTION inc2ToInt(val integer) RETURNS integer AS $$
return val + 2;
$$
LANGUAGE plcsharp;
SELECT inc2ToInt(8) = integer '10';

CREATE OR REPLACE FUNCTION sum3Integer(aaa integer, bbb integer, ccc integer) RETURNS integer AS $$
return aaa + bbb + ccc;
$$
LANGUAGE plcsharp;
SELECT sum3Integer(3,2,1) = integer '6';

CREATE OR REPLACE FUNCTION sum4Integer(a integer, b integer, c integer, d integer) RETURNS integer AS $$
return a + b + c + d;
$$
LANGUAGE plcsharp;
SELECT sum4Integer(4,3,2,1) = integer '10';

CREATE OR REPLACE FUNCTION sum2Integer(a integer, b integer) RETURNS integer AS $$
return a+b;
$$ LANGUAGE plcsharp;
SELECT sum2Integer(32770, 100) = bigint '32870';

CREATE OR REPLACE FUNCTION maxBigInt() RETURNS bigint AS $$
return 9223372036854775807;
$$ LANGUAGE plcsharp;
SELECT maxBigInt() = bigint '9223372036854775807';

CREATE OR REPLACE FUNCTION sum2BigInt(a bigint, b bigint) RETURNS bigint AS $$
return a+b;
$$ LANGUAGE plcsharp;
SELECT sum2BigInt(9223372036854775707, 100) = bigint '9223372036854775807';

CREATE OR REPLACE FUNCTION mixedBigInt(a integer, b integer, c bigint) RETURNS bigint AS $$
return (long)a+(long)b+c;
$$ LANGUAGE plcsharp;
SELECT mixedBigInt(32767,  2147483647, 100) = bigint '2147516514';

CREATE OR REPLACE FUNCTION mixedInt(a smallint, b smallint, c integer) RETURNS integer AS $$
return (int)a+(int)b+c;
$$ LANGUAGE plcsharp;
SELECT mixedInt(CAST(32767 AS smallint),  CAST(32767 AS smallint), 100) = integer '65634';

CREATE OR REPLACE FUNCTION mixedBigInt8(b smallint, c bigint) RETURNS smallint AS $$
return (short)(b+c);
$$ LANGUAGE plcsharp;
SELECT mixedBigInt8(CAST(32 AS SMALLINT), CAST(100 AS BIGINT)) = smallint '132';
