CREATE OR REPLACE FUNCTION maxSmallIntFSharp() RETURNS smallint AS $$
Nullable(32767s)
$$ LANGUAGE plfsharp;
SELECT maxSmallIntFSharp() = integer '32767';

CREATE OR REPLACE FUNCTION sum2SmallIntFSharp(a smallint, b smallint) RETURNS smallint AS $$
Nullable(a.Value + b.Value)
$$ LANGUAGE plfsharp;
SELECT sum2SmallIntFSharp(CAST(100 AS smallint), CAST(101 AS smallint)) = smallint '201';

CREATE OR REPLACE FUNCTION maxIntegerFSharp() RETURNS integer AS $$
Nullable(2147483647)
$$ LANGUAGE plfsharp;
SELECT maxIntegerFSharp() = integer '2147483647';

CREATE OR REPLACE FUNCTION returnIntFSharp() RETURNS integer AS $$
Nullable(10)
$$ LANGUAGE plfsharp;
SELECT returnIntFSHarp() = integer '10';

CREATE OR REPLACE FUNCTION inc2ToIntFSharp(v integer) RETURNS integer AS $$
Nullable(v.Value + 2)
$$ LANGUAGE plfsharp;
SELECT inc2ToIntFSHarp(8) = integer '10';

CREATE OR REPLACE FUNCTION sum3IntegerFSharp(a integer, b integer, c integer) RETURNS integer AS $$
Nullable (a.Value + b.Value + c.Value)
$$ LANGUAGE plfsharp;
SELECT sum3IntegerFSHarp(3,2,1) = integer '6';

CREATE OR REPLACE FUNCTION sum4IntegerFSharp(a integer, b integer, c integer, d integer) RETURNS integer AS $$
Nullable (a.Value + b.Value + c.Value + d.Value)
$$ LANGUAGE plfsharp;
SELECT sum4IntegerFSHarp(4,3,2,1) = integer '10';

CREATE OR REPLACE FUNCTION sum2IntegerFSharp(a integer, b integer) RETURNS integer AS $$
Nullable(a.Value + b.Value)
$$ LANGUAGE plfsharp;
SELECT sum2IntegerFSharp(32770, 100) = int '32870';

CREATE OR REPLACE FUNCTION maxBigIntFSharp() RETURNS bigint AS $$
Nullable(9223372036854775807L)
$$ LANGUAGE plfsharp;
SELECT maxBigIntFSharp() = bigint '9223372036854775807';

CREATE OR REPLACE FUNCTION sum2BigIntFSharp(a bigint, b bigint) RETURNS bigint AS $$
Nullable(a.Value + b.Value)
$$ LANGUAGE plfsharp;
SELECT sum2BigIntFSharp(9223372036854775707, 100) = bigint '9223372036854775807';

CREATE OR REPLACE FUNCTION mixedBigIntFSharp(a integer, b integer, c bigint) RETURNS bigint AS $$
Nullable((int64 a.Value + int64 b.Value + c.Value))
$$ LANGUAGE plfsharp;
SELECT mixedBigIntFSharp(32767,  2147483647, 100) = bigint '2147516514';

CREATE OR REPLACE FUNCTION mixedIntFSharp(a smallint, b smallint, c integer) RETURNS integer AS $$
Nullable((int a.Value + int b.Value + c.Value))
$$ LANGUAGE plfsharp;
SELECT mixedIntFSharp(CAST(32767 AS smallint),  CAST(32767 AS smallint), 100) = integer '65634';

CREATE OR REPLACE FUNCTION mixedBigInt16FSharp(b smallint, c bigint) RETURNS bigint AS $$
Nullable((int64 b.Value + c.Value))
$$ LANGUAGE plfsharp;
SELECT mixedBigInt16FSharp(CAST(32 AS SMALLINT), CAST(100 AS BIGINT)) = bigint '132';

CREATE OR REPLACE FUNCTION mixedBigInt16SmallFSharp(b smallint, c bigint) RETURNS smallint AS $$
Nullable((b.Value + int16 c.Value))
$$ LANGUAGE plfsharp;
SELECT mixedBigInt16SmallFSharp(CAST(32 AS SMALLINT), CAST(100 AS BIGINT)) = smallint '132';
