CREATE OR REPLACE FUNCTION maxSmallInt() RETURNS smallint AS $$
return (short)32767;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int2', 'maxSmallInt', maxSmallInt() = smallint '32767';

CREATE OR REPLACE FUNCTION sum2SmallInt(a smallint, b smallint) RETURNS smallint AS $$
if (a == null)
    a = 0;

if (b == null)
    b = 0;

return (short)(a+b); //C# requires short cast
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int2', 'sum2SmallInt', sum2SmallInt(CAST(100 AS smallint), CAST(101 AS smallint)) = smallint '201';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int2-null', 'sum2SmallInt2', sum2SmallInt(NULL::SMALLINT, 30::SMALLINT) = smallint '30';

CREATE OR REPLACE FUNCTION maxInteger() RETURNS integer AS $$
return 2147483647;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int4', 'maxInteger', maxInteger() = integer '2147483647';

CREATE OR REPLACE FUNCTION sum2Integer(a integer, b integer) RETURNS integer AS $$
if (a == null)
    a = 0;

if (b == null)
    b = 0;

return a+b;
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int4', 'sum2Integer1', sum2Integer(32770, 100) = INTEGER '32870';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int4-null', 'sum2Integer2', sum2Integer(NULL::INTEGER, 100::INTEGER) = INTEGER '100';

CREATE OR REPLACE FUNCTION maxBigInt() RETURNS bigint AS $$
return 9223372036854775807;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int8', 'maxBigInt', maxBigInt() = bigint '9223372036854775807';

CREATE OR REPLACE FUNCTION sum2BigInt(a bigint, b bigint) RETURNS bigint AS $$
if (a == null)
    a = 0;

if (b == null)
    b = 0;

return a+b;
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int8', 'sum2BigInt1', sum2BigInt(9223372036854775707, 100) = bigint '9223372036854775807';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int8-null', 'sum2BigInt2', sum2BigInt(9223372036854775707::BIGINT, NULL::BIGINT) = bigint '9223372036854775707';

CREATE OR REPLACE FUNCTION mixedBigInt(a integer, b integer, c bigint) RETURNS bigint AS $$
return (long)a+(long)b+c;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int8', 'mixedBigInt', mixedBigInt(32767,  2147483647, 100) = bigint '2147516514';

CREATE OR REPLACE FUNCTION mixedInt(a smallint, b smallint, c integer) RETURNS integer AS $$
return (int)a+(int)b+c;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int4', 'mixedInt', mixedInt(CAST(32767 AS smallint),  CAST(32767 AS smallint), 100) = integer '65634';

CREATE OR REPLACE FUNCTION mixedBigInt8(b smallint, c bigint) RETURNS smallint AS $$
return (short)(b+c);
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int8', 'mixedBigInt8', mixedBigInt8(CAST(32 AS SMALLINT), CAST(100 AS BIGINT)) = smallint '132';

--- Small Integer Arrays
CREATE OR REPLACE FUNCTION returnSmallIntArray(small_integers smallint[]) RETURNS smallint[] AS $$
return small_integers;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int2-null-1array', 'returnSmallIntArray1', returnSmallIntArray(ARRAY[12345::smallint, null::smallint, 123::smallint, 4356::smallint]) = ARRAY[12345::smallint, null::smallint, 123::smallint, 4356::smallint];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int2-null-2array-arraynull', 'returnSmallIntArray2', returnSmallIntArray(ARRAY[[null::smallint, null::smallint], [12345::smallint, 654::smallint]]) = ARRAY[[null::smallint, null::smallint], [12345::smallint, 654::smallint]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int2-null-3array-arraynull', 'returnSmallIntArray3', returnSmallIntArray(ARRAY[[[null::smallint, null::smallint], [null::smallint, null::smallint]], [[186::smallint, 23823::smallint], [9521::smallint, 934::smallint]]]) = ARRAY[[[null::smallint, null::smallint], [null::smallint, null::smallint]], [[186::smallint, 23823::smallint], [9521::smallint, 934::smallint]]];

CREATE OR REPLACE FUNCTION sumSmallIntArray(small_integers smallint[]) RETURNS smallint AS $$
Array flatten_small_integers = Array.CreateInstance(typeof(object), small_integers.Length);
ArrayManipulation.FlatArray(small_integers, ref flatten_small_integers);
short small_integers_sum = (short)0;
for(int i = 0; i < flatten_small_integers.Length; i++)
{
    if (flatten_small_integers.GetValue(i) == null)
        continue;
    small_integers_sum = (short)(small_integers_sum + (short)flatten_small_integers.GetValue(i));
}
return small_integers_sum;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int2-null-1array', 'sumSmallIntArray1', sumSmallIntArray(ARRAY[12345::smallint, null::smallint, 123::smallint, 4356::smallint]) = '16824';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int2-null-2array-arraynull', 'sumSmallIntArray2', sumSmallIntArray(ARRAY[[null::smallint, null::smallint], [12345::smallint, 654::smallint]]) = '12999';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int2-null-3array-arraynull', 'sumSmallIntArray3', sumSmallIntArray(ARRAY[[[null::smallint, null::smallint], [null::smallint, null::smallint]], [[186::smallint, 13823::smallint], [9521::smallint, 934::smallint]]]) = '24464';

CREATE OR REPLACE FUNCTION CreateSmallIntMultidimensionalArray() RETURNS smallint[] AS $$
short?[, ,] smallint_three_dimensional = new short?[2, 2, 2] {{{423, 536}, {null, null}}, {{8763, 15}, {943, 1003}}};
return smallint_three_dimensional;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int2-null-3array-arraynull', 'CreateSmallIntMultidimensionalArray', CreateSmallIntMultidimensionalArray() = ARRAY[[[423::smallint, 536::smallint], [null::smallint, null::smallint]], [[8763::smallint, 15::smallint], [943::smallint, 1003::smallint]]];

CREATE OR REPLACE FUNCTION updateArraySmallIntIndex(small_integers smallint[], desired smallint, index integer[]) RETURNS smallint[] AS $$
int[] arrayInteger = index.Cast<int>().ToArray();
small_integers.SetValue(desired, arrayInteger);
return small_integers;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int2-null-1array', 'updateArraySmallIntIndex1', updateArraySmallIntIndex(ARRAY[342::smallint, 10456::smallint, null::smallint], CAST(13212 AS smallint), ARRAY[1]) = ARRAY[342::smallint, 13212::smallint, null::smallint];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int2-null-2array-arraynull', 'updateArraySmallIntIndex2', updateArraySmallIntIndex(ARRAY[[45::smallint, 11324::smallint], [null::smallint, 12464::smallint]], CAST(13212 AS smallint), ARRAY[1, 0]) = ARRAY[[45::smallint, 11324::smallint], [13212::smallint, 12464::smallint]];

--- Integer Arrays
CREATE OR REPLACE FUNCTION returnIntegerArray(integers integer[]) RETURNS integer[] AS $$
return integers;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int4-null-1array', 'returnIntegerArray1', returnIntegerArray(ARRAY[2047483647::integer, null::integer, 304325::integer, 706524::integer]) = ARRAY[2047483647::integer, null::integer, 304325::integer, 706524::integer];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int4-null-2array-arraynull', 'returnIntegerArray2', returnIntegerArray(ARRAY[[null::integer, null::integer], [2047483647::integer, 304325::integer]]) = ARRAY[[null::integer, null::integer], [2047483647::integer, 304325::integer]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int4-null-3array-arraynull', 'returnIntegerArray3', returnIntegerArray(ARRAY[[[null::integer, null::integer], [null::integer, null::integer]], [[2047483647::integer, 304325::integer], [706524::integer, 934::integer]]]) = ARRAY[[[null::integer, null::integer], [null::integer, null::integer]], [[2047483647::integer, 304325::integer], [706524::integer, 934::integer]]];

CREATE OR REPLACE FUNCTION sumIntegerArray(integers integer[]) RETURNS integer AS $$
Array flatten_integers = Array.CreateInstance(typeof(object), integers.Length);
ArrayManipulation.FlatArray(integers, ref flatten_integers);
int integers_sum = 0;
for(int i = 0; i < flatten_integers.Length; i++)
{
    if (flatten_integers.GetValue(i) == null)
        continue;
    integers_sum = integers_sum + (int)flatten_integers.GetValue(i);
}
return integers_sum;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int4-null-1array', 'sumIntegerArray1', sumIntegerArray(ARRAY[2047483647::integer, null::integer, 304325::integer, 4356::integer]) = '2047792328';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int4-null-2array-arraynull', 'sumIntegerArray2', sumIntegerArray(ARRAY[[null::integer, null::integer], [2047483647::integer, 304325::integer]]) = '2047787972';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int4-null-3array-arraynull', 'sumIntegerArray3', sumIntegerArray(ARRAY[[[null::integer, null::integer], [null::integer, null::integer]], [[2047483647::integer, 304325::integer], [706524::integer, 4356::integer]]]) = '2048498852';

CREATE OR REPLACE FUNCTION CreateIntegerMultidimensionalArray() RETURNS integer[] AS $$
int?[, ,] integer_three_dimensional = new int?[2, 2, 2] {{{2047483647, 304325}, {null, null}}, {{706524, 9652345}, {943, 4134677}}};
return integer_three_dimensional;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int4-null-3array-arraynull', 'CreateIntegerMultidimensionalArray', CreateIntegerMultidimensionalArray() = ARRAY[[[2047483647::integer, 304325::integer], [null::integer, null::integer]], [[706524::integer, 9652345::integer], [943::integer, 4134677::integer]]];

CREATE OR REPLACE FUNCTION updateArrayIntegerIndex(integers integer[], desired integer, index integer[]) RETURNS integer[] AS $$
int[] arrayInteger = index.Cast<int>().ToArray();
integers.SetValue(desired, arrayInteger);
return integers;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int4-null-1array', 'updateArrayIntegerIndex1', updateArrayIntegerIndex(ARRAY[2047483647::integer, 304325::integer, null::integer], 65464532, ARRAY[1]) = ARRAY[2047483647::integer, 65464532::integer, null::integer];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int4-null-2array', 'updateArrayIntegerIndex2', updateArrayIntegerIndex(ARRAY[[2047483647::integer, 304325::integer], [null::integer, 12465464::integer]], 65464532, ARRAY[1, 0]) = ARRAY[[2047483647::integer, 304325::integer], [65464532::integer, 12465464::integer]];

--- Big Integer Arrays
CREATE OR REPLACE FUNCTION returnBigIntegerArray(big_integers bigint[]) RETURNS bigint[] AS $$
return big_integers;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int8-null-1array', 'returnBigIntegerArray1', returnBigIntegerArray(ARRAY[9223372036854775707::bigint, null::bigint, 23372036854775707::bigint, 706524::bigint]) = ARRAY[9223372036854775707::bigint, null::bigint, 23372036854775707::bigint, 706524::bigint];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int8-null-2array-arraynull', 'returnBigIntegerArray2', returnBigIntegerArray(ARRAY[[null::bigint, null::bigint], [9223372036854775707::bigint, 23372036854775707::bigint]]) = ARRAY[[null::bigint, null::bigint], [9223372036854775707::bigint, 23372036854775707::bigint]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int8-null-3array-arraynull', 'returnBigIntegerArray3', returnBigIntegerArray(ARRAY[[[null::bigint, null::bigint], [null::bigint, null::bigint]], [[9223372036854775707::bigint, 23372036854775707::bigint], [706524::bigint, 7563452434247987::bigint]]]) = ARRAY[[[null::bigint, null::bigint], [null::bigint, null::bigint]], [[9223372036854775707::bigint, 23372036854775707::bigint], [706524::bigint, 7563452434247987::bigint]]];

CREATE OR REPLACE FUNCTION sumBigIntegerArray(big_integers bigint[]) RETURNS bigint AS $$
Array flatten_big_integers = Array.CreateInstance(typeof(object), big_integers.Length);
ArrayManipulation.FlatArray(big_integers, ref flatten_big_integers);
long big_integers_sum = 0;
for(int i = 0; i < flatten_big_integers.Length; i++)
{
    if (flatten_big_integers.GetValue(i) == null)
        continue;
    big_integers_sum = (long)(big_integers_sum + (long)flatten_big_integers.GetValue(i));
}
return big_integers_sum;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int8-null-1array', 'sumBigIntegerArray1', sumBigIntegerArray(ARRAY[223372036854775707::bigint, null::bigint, 23372036854775707::bigint, 4356::bigint]) = '246744073709555770';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int8-null-2array-arraynull', 'sumBigIntegerArray2', sumBigIntegerArray(ARRAY[[null::bigint, null::bigint], [92332036854775707::bigint, 23372036854775707::bigint]]) = '115704073709551414';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int8-null-3array-arraynull', 'sumBigIntegerArray3', sumBigIntegerArray(ARRAY[[[null::bigint, null::bigint], [null::bigint, null::bigint]], [[92232036854775707::bigint, 2337203684775707::bigint], [706524::bigint, 756452434247987::bigint]]]) = '95325692974505925';

CREATE OR REPLACE FUNCTION CreateBigIntegerMultidimensionalArray() RETURNS bigint[] AS $$
long?[, ,] big_integer_three_dimensional = new long?[2, 2, 2] {{{92232036854775707, 2337203684775707}, {null, null}}, {{706524, 756452434247987}, {943, 4134677}}};
return big_integer_three_dimensional;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int8-null-3array-arraynull', 'CreateBigIntegerMultidimensionalArray', CreateBigIntegerMultidimensionalArray() = ARRAY[[[92232036854775707::bigint, 2337203684775707::bigint], [null::bigint, null::bigint]], [[706524::bigint, 756452434247987::bigint], [943::bigint, 4134677::bigint]]];

CREATE OR REPLACE FUNCTION updateArrayBigIntegerIndex(big_integers bigint[], desired bigint, index integer[]) RETURNS bigint[] AS $$
int[] arrayInteger = index.Cast<int>().ToArray();
big_integers.SetValue(desired, arrayInteger);
return big_integers;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int8-null-1array', 'updateArrayBigIntegerIndex1', updateArrayBigIntegerIndex(ARRAY[92232036854775707::bigint, 2337203684775707::bigint, null::bigint], CAST(67337203684775707 AS BIGINT), ARRAY[1]) = ARRAY[92232036854775707::bigint, 67337203684775707::bigint, null::bigint];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int8-null-2array', 'updateArrayBigIntegerIndex2', updateArrayBigIntegerIndex(ARRAY[[92232036854775707::bigint, 2337203684775707::bigint], [null::bigint, 12465464::bigint]], CAST(67337203684775707 AS BIGINT), ARRAY[1, 0]) = ARRAY[[92232036854775707::bigint, 2337203684775707::bigint], [67337203684775707::bigint, 12465464::bigint]];
