CREATE OR REPLACE FUNCTION maxSmallInt() RETURNS smallint AS $$
  Return short.MaxValue
$$ LANGUAGE plvisualbasic STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'vb-int2', 'maxSmallInt', maxSmallInt() = smallint '32767';

CREATE OR REPLACE FUNCTION sum2SmallInt(a smallint, b smallint) RETURNS smallint AS $$
  If Not a.HasValue Then
    a = 0
  End If

  If Not b.HasValue Then
    b = 0
  End If

  Return CType(a, Short) + CType(b, Short)
$$ LANGUAGE plvisualbasic;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'vb-int2', 'sum2SmallInt', sum2SmallInt(CAST(100 AS smallint), CAST(101 AS smallint)) = smallint '201';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'vb-int2-null', 'sum2SmallInt2', sum2SmallInt(NULL::SMALLINT, 30::SMALLINT) = smallint '30';

CREATE OR REPLACE FUNCTION maxInteger() RETURNS integer AS $$
  Return integer.MaxValue
$$ LANGUAGE plvisualbasic STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'vb-int4', 'maxInteger', maxInteger() = integer '2147483647';

CREATE OR REPLACE FUNCTION sum2Integer(a integer, b integer) RETURNS integer AS $$
  If Not a.HasValue Then
    a = 0
  End If

  If Not b.HasValue Then
    b = 0
  End If

  Return a + b
$$ LANGUAGE plvisualbasic;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'vb-int4', 'sum2Integer1', sum2Integer(32770, 100) = INTEGER '32870';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'vb-int4-null', 'sum2Integer2', sum2Integer(NULL::INTEGER, 100::INTEGER) = INTEGER '100';

CREATE OR REPLACE FUNCTION maxBigInt() RETURNS bigint AS $$
  Return long.MaxValue
$$ LANGUAGE plvisualbasic STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'vb-int8', 'maxBigInt', maxBigInt() = bigint '9223372036854775807';

CREATE OR REPLACE FUNCTION sum2BigInt(a bigint, b bigint) RETURNS bigint AS $$
  If Not a.HasValue Then
    a = 0
  End If

  If Not b.HasValue Then
    b = 0
  End If

  Return a + b
$$ LANGUAGE plvisualbasic;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'vb-int8', 'sum2BigInt1', sum2BigInt(9223372036854775707, 100) = bigint '9223372036854775807';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'vb-int8-null', 'sum2BigInt2', sum2BigInt(9223372036854775707::BIGINT, NULL::BIGINT) = bigint '9223372036854775707';

CREATE OR REPLACE FUNCTION mixedBigInt8(b smallint, c bigint) RETURNS smallint AS
$$
Dim result As Short
result = CShort(b + c)
Return result
$$ LANGUAGE plvisualbasic STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'vb-int8', 'mixedBigInt8', mixedBigInt8(CAST(32 AS SMALLINT), CAST(100 AS BIGINT)) = smallint '132';

-- Array is not working!
-- CREATE OR REPLACE FUNCTION returnSmallIntArray(small_integers smallint[]) RETURNS smallint[] AS
-- $$
-- Return small_integers
-- $$ LANGUAGE plvisualbasic STRICT;
-- INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
-- SELECT 'vb-int2-null-1array', 'returnSmallIntArray1', returnSmallIntArray(ARRAY[12345::smallint, null::smallint, 123::smallint, 4356::smallint]) = ARRAY[12345::smallint, null::smallint, 123::smallint, 4356::smallint];
-- INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
-- SELECT 'vb-int2-null-2array-arraynull', 'returnSmallIntArray2', returnSmallIntArray(ARRAY[[null::smallint, null::smallint], [12345::smallint, 654::smallint]]) = ARRAY[[null::smallint, null::smallint], [12345::smallint, 654::smallint]];
-- INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
-- SELECT 'vb-int2-null-3array-arraynull', 'returnSmallIntArray3', returnSmallIntArray(ARRAY[[[null::smallint, null::smallint], [null::smallint, null::smallint]], [[186::smallint, 23823::smallint], [9521::smallint, 934::smallint]]]) = ARRAY[[[null::smallint, null::smallint], [null::smallint, null::smallint]], [[186::smallint, 23823::smallint], [9521::smallint, 934::smallint]]];
