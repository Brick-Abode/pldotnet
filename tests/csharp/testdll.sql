-- BIT

CREATE OR REPLACE FUNCTION modifyBitDLL(a BIT(10)) RETURNS BIT(10) AS '/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!modifybit' LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-bit-dll', 'modifyBitDLL1', modifyBitDLL('10101'::BIT(10)) = '0010100001'::BIT(10);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-bit-null-dll', 'modifyBitDLL2', modifyBitDLL(NULL::BIT(10)) IS NULL;

-- VARBIT

CREATE OR REPLACE FUNCTION modifyVarbitDLL(a BIT VARYING) RETURNS BIT VARYING AS '/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!modifyvarbit' LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-varbit-dll', 'modifyVarbitDLL1', modifyVarbitDLL('1001110001000'::BIT VARYING) = '0001110001001'::BIT VARYING;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-varbit-null-dll', 'modifyVarbitDLL2', modifyVarbitDLL(NULL::BIT VARYING) IS NULL;

-- BOOLEAN

CREATE OR REPLACE FUNCTION BooleanAndDLL(a boolean, b boolean) RETURNS boolean AS '/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!booleanand' LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-bool-dll', 'BooleanAndDLL1', BooleanAndDLL(true, true) is true;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-bool-null-dll', 'BooleanAndDLL2', BooleanAndDLL(NULL::BOOLEAN, true) is false;

-- BYTEA

CREATE OR REPLACE FUNCTION byteaConversionsDLL(a BYTEA, b BYTEA) RETURNS BYTEA AS '/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!byteaconversions' LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-bytea-dll', 'byteaConversionsDLL1', byteaConversionsDLL('Brick Abode is nice!'::BYTEA, 'Thank you very much...'::BYTEA) = '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-bytea-null-dll', 'byteaConversionsDLL2', byteaConversionsDLL(NULL::BYTEA, 'Thank you very much...'::BYTEA) = 'Thank you very much...'::BYTEA;

--- TIMESTAMP

CREATE OR REPLACE FUNCTION setNewDateDLL(orig_timestamp TIMESTAMP, new_date DATE) RETURNS TIMESTAMP AS '/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!setnewdate' LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-timestamp-dll', 'setNewDateDLL1', setNewDateDLL(TIMESTAMP '2004-10-19 10:23:54 PM', DATE '2022-10-17') = TIMESTAMP '2022-10-17 10:23:54 PM';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-timestamp-null-dll', 'setNewDateDLL2', setNewDateDLL(NULL::TIMESTAMP, NULL::DATE) = TIMESTAMP '2023-12-25 08:30:20';

--- FLOAT8
CREATE OR REPLACE FUNCTION sumDoubleArrayDLL(doubles double precision[]) RETURNS double precision AS '/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!sumdoublearray' LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float8-null-1array-dll', 'sumDoubleArrayDLL1', sumDoubleArrayDLL(ARRAY[21.0000000000109::double precision, null::double precision, 4.521234313421::double precision, 7.412344328978::double precision]) = '32.9335786424099';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float8-null-2array-dll', 'sumDoubleArrayDLL2', sumDoubleArrayDLL(ARRAY[[21.0000000000109::double precision, null::double precision], [4.521234313421::double precision, 7.412344328978::double precision]]) = '32.9335786424099';


--- SMALLINT

CREATE OR REPLACE FUNCTION sum2SmallIntDLL(a smallint, b smallint) RETURNS smallint AS '/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!sum2smallint' LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int2-dll', 'sum2SmallIntDLL1', sum2SmallIntDLL(CAST(100 AS smallint), CAST(101 AS smallint)) = smallint '201';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int2-null-dll', 'sum2SmallIntDLL2', sum2SmallIntDLL(NULL::SMALLINT, 30::SMALLINT) = smallint '30';

--- INTEGER

CREATE OR REPLACE FUNCTION sum2IntegerDLL(a integer, b integer) RETURNS integer
AS '/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!sum2integer' LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int4-dll', 'sum2IntegerDLL1', sum2IntegerDLL(32770, 100) = INTEGER '32870';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int4-null-dll', 'sum2IntegerDLL2', sum2IntegerDLL(NULL::INTEGER, 100::INTEGER) = INTEGER '100';

--- BIGINT

CREATE OR REPLACE FUNCTION sum2BigIntDLL(a bigint, b bigint) RETURNS bigint AS '/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!sum2bigint'
LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int8-dll', 'sum2BigIntDLL1', sum2BigIntDLL(9223372036854775707, 100) = bigint '9223372036854775807';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int8-null-dll', 'sum2BigIntDLL2', sum2BigIntDLL(9223372036854775707::BIGINT, NULL::BIGINT) = bigint '9223372036854775707';

--- POLYGON Arrays

CREATE OR REPLACE FUNCTION updateArrayPolygonIndexDLL(values_array POLYGON[], desired POLYGON, index integer[]) RETURNS POLYGON[] AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!updatearraypolygonindex'
LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-polygon-null-1array-dll', 'updateArrayPolygonIndexDLL1', CAST(updateArrayPolygonIndexDLL(ARRAY['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, null::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON], '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, ARRAY[2]) AS TEXT) = CAST(ARRAY['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-polygon-null-2array-arraynull-dll', 'updateArrayPolygonIndexDLL2', CAST(updateArrayPolygonIndexDLL(ARRAY[[null::POLYGON, null::POLYGON], [null::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON]], '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, ARRAY[1,0]) AS TEXT) = CAST(ARRAY[[null::POLYGON, null::POLYGON], ['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON]] AS TEXT);

--- JSON Arrays

CREATE OR REPLACE FUNCTION updateJsonArrayIndexDLL(values_array JSON[], desired JSON, index integer[]) RETURNS JSON[] AS '/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!updatejsonarrayindex' LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-json-null-1array-dll', 'updateJsonArrayIndexDLL1', updateJsonArrayIndexDLL(ARRAY['{"age": 20, "name": "Mikael"}'::JSON, '{"age": 25, "name": "Rosicley"}'::JSON, null::JSON, '{"age": 30, "name": "Todd"}'::JSON], '{"age": 40, "name": "John Doe"}'::JSON, ARRAY[2])::TEXT = ARRAY['{"age": 20, "name": "Mikael"}'::JSON, '{"age": 25, "name": "Rosicley"}'::JSON, '{"age": 40, "name": "John Doe"}'::JSON, '{"age": 30, "name": "Todd"}'::JSON]::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-json-null-2array-arraynull-dll', 'updateJsonArrayIndexDLL2', updateJsonArrayIndexDLL(ARRAY[[null::JSON, null::JSON], [null::JSON, '{"age": 30, "name": "Todd"}'::JSON]], '{"age": 40, "name": "John Doe"}'::JSON, ARRAY[1,0])::TEXT = ARRAY[[null::JSON, null::JSON], ['{"age": 40, "name": "John Doe"}'::JSON, '{"age": 30, "name": "Todd"}'::JSON]]::TEXT;

--- MONEY Arrays
CREATE OR REPLACE FUNCTION IncreaseMoneyDLL(values_array MONEY[]) RETURNS MONEY[] AS '/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!increasemoney' LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-money-null-1array-dll', 'IncreaseMoneyDLL1', IncreaseMoneyDLL(ARRAY['32500.0'::MONEY, '-500.4'::MONEY, null::MONEY, '900540.2'::MONEY]) = ARRAY['32501.0'::MONEY, '-499.4'::MONEY, null::MONEY, '900541.2'::MONEY];

--- MACADDR8OID Arrays

CREATE OR REPLACE FUNCTION IncreaseMacAddress8DLL(values_array MACADDR8[]) RETURNS MACADDR8[] AS '/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!increasemacaddress8' LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-macaddr8-1array-dll', 'IncreaseMacAddress8DLL1', IncreaseMacAddress8DLL(ARRAY[MACADDR8 '08-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac', null::macaddr, MACADDR8 'a8-00-2b-01-02-03-ab-ac']) = ARRAY[MACADDR8 '09-00-2b-01-02-03-ab-ac', MACADDR8 '0a-00-2b-01-02-03-ab-ac', null::macaddr, MACADDR8 'a9-00-2b-01-02-03-ab-ac'];

--- CIDROID Arrays

CREATE OR REPLACE FUNCTION IncreaseCIDRAddressDLL(values_array CIDR[]) RETURNS CIDR[] AS '/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!increasecidraddress' LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-cidr-1array-dll', 'IncreaseCIDRAddressDLL1', IncreaseCIDRAddressDLL(ARRAY[CIDR '192.168.231.0/24', CIDR '175.170.14.0/24', null::cidr, CIDR '167.168.41.0/24']) = ARRAY[CIDR '193.168.231.0/24', CIDR '176.170.14.0/24', null::cidr, CIDR '168.168.41.0/24'];

--- INT8RANGE Arrays

CREATE OR REPLACE FUNCTION IncreaseInt8RangesDLL(values_array INT8RANGE[]) RETURNS INT8RANGE[] AS '/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!increaseint8ranges' LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int8range-null-1array-dll', 'IncreaseInt8RangesDLL1', IncreaseInt8RangesDLL(ARRAY['[2,6)'::INT8RANGE, '(,6)'::INT8RANGE, null::INT8RANGE, '[,)'::INT8RANGE]) = ARRAY['[3,7)'::INT8RANGE, '(,7)'::INT8RANGE, null::INT8RANGE, '[,)'::INT8RANGE];

--- DATERANGE Arrays

CREATE OR REPLACE FUNCTION IncreaseDateonlyRangesDLL(values_array DATERANGE[]) RETURNS DATERANGE[] AS '/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!increasedateonlyranges' LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-daterange-null-1array-dll', 'IncreaseDateonlyRangesDLL1', IncreaseDateonlyRangesDLL(ARRAY['[2021-01-01, 2021-01-01)'::DATERANGE, '(, 2021-04-04)'::DATERANGE, null::DATERANGE, '[,)'::DATERANGE]) = ARRAY['[2021-01-02, 2021-01-02)'::DATERANGE, '(, 2021-04-05)'::DATERANGE, null::DATERANGE, '[,)'::DATERANGE];

--- VARCHAR

CREATE OR REPLACE FUNCTION concatenateVarCharsDLL(a VARCHAR, b VARCHAR, c BPCHAR) RETURNS VARCHAR AS '/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!concatenatevarchars' LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-varchar-dll', 'concatenateVarCharsDLL1', concatenateVarCharsDLL('hello'::VARCHAR, 'beautiful'::VARCHAR, 'world!'::BPCHAR) = 'HELLO BEAUTIFUL WORLD!'::VARCHAR;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-varchar-null-dll', 'concatenateVarCharsDLL2', concatenateVarCharsDLL(NULL::VARCHAR, 'beautiful'::VARCHAR, NULL::BPCHAR) = ' BEAUTIFUL '::VARCHAR;

--- UUID

CREATE OR REPLACE FUNCTION combineUUIDsDLL(a UUID, b UUID) RETURNS UUID AS '/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!combineuuids' LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-uuid-dll', 'combineUUIDsDLL1', combineUUIDsDLL('a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID) = 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-uuid-dll', 'combineUUIDsDLL2', combineUUIDsDLL('123e4567-e89b-12d3-a456-426614174000'::UUID, '024be913-3bf8-4499-9694-12769239b763'::UUID) = '123e4567-e89b-12d3-9694-12769239b763'::UUID;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-uuid-null-dll', 'combineUUIDsDLL3', combineUUIDsDLL(NULL::UUID, '024be913-3bf8-4499-9694-12769239b763'::UUID) = 'a0eebc99-9c0b-4ef8-9694-12769239b763'::UUID;

--- TESTING VALID FUNCTIONS

--- STRICT FUNCTION CALLS C# FUNCTION WITH T
CREATE OR REPLACE FUNCTION middlePointStrictCallStrict(pointa point, pointb point) RETURNS point AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.TestClass!middlePointStrict'
LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-point-dll', 'middlePointStrictCallStrict', middlePointStrictCallStrict(POINT(10.0,20.0), POINT(20.0,40.0)) ~= POINT(15.0,30.0);

-- STRICT FUNCTION CALLS C# FUNCTION WITH T?
CREATE OR REPLACE FUNCTION middlePointStrictCallDefault(pointa point, pointb point) RETURNS point AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.TestClass!middlePointDefault'
LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-point-dll', 'middlePointStrictCallDefault', middlePointStrictCallDefault(POINT(10.0,20.0), POINT(20.0,40.0)) ~= POINT(15.0,30.0);

--- DEFAULT FUNCTION CALLS C# FUNCTION WITH T?
CREATE OR REPLACE FUNCTION middlePointDefaultCallDefault(pointa point, pointb point) RETURNS point AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.TestClass!middlePointDefault'
LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-point-null-dll', 'middlePointDefaultCallDefault', middlePointDefaultCallDefault(NULL::POINT, POINT(20.0,40.0)) ~= POINT(10.0,20.0);


--- INOUT TESTS
CREATE OR REPLACE FUNCTION inoutBasic0DLL(INOUT argument_0 INT) AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.InoutTests!inoutBasic0'
LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-basic-0-dll', 'inoutBasic0DLL', inoutBasic0DLL(0) = 1;

CREATE OR REPLACE FUNCTION inoutBasic1DLL(OUT argument_0 INT) AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.InoutTests!inoutBasic1'
LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-basic-1-dll', 'inoutBasic1DLL', inoutBasic1DLL() = 1;

CREATE OR REPLACE FUNCTION inputNull1DLL(INOUT argument_0 INT) AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.InoutTests!inputNull1'
LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-null-1-dll', 'inputNull1DLL', inputNull1DLL(11) IS NULL;

CREATE OR REPLACE FUNCTION inputNull2DLL(INOUT argument_0 INT) AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.InoutTests!inputNull2'
LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-null-2-dll', 'inputNull2DLL', inputNull2DLL(NULL) = 3;

CREATE OR REPLACE FUNCTION inputNull3DLL(OUT argument_0 INT) AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.InoutTests!inputNull3'
LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-null-3-dll', 'inputNull3DLL', inputNull3DLL() IS NULL;

CREATE OR REPLACE FUNCTION inputNull4DLL(INOUT argument_0 INT, in argument_1 INT) AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.InoutTests!inputNull4'
LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-null-4-dll', 'inputNull4DLL', inputNull4DLL(11, 3) IS NULL;

CREATE OR REPLACE FUNCTION inputNull5DLL(INOUT argument_0 INT, IN argument_1 INT) AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.InoutTests!inputNull5'
LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-null-5-dll', 'inputNull5DLL', inputNull5DLL(NULL, 3) = 3;

CREATE OR REPLACE FUNCTION inputNull6DLL(IN argument_0 INT, OUT argument_1 INT) AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.InoutTests!inputNull6'
LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-null-6-dll', 'inputNull6DLL', inputNull6DLL(1) IS NULL;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-null-7', 'inputNull6DLL', inputNull6DLL(2) = 2;

CREATE OR REPLACE FUNCTION inoutMultiarg1DLL(IN argument_0 INT, INOUT argument_1 INT, IN argument_2 INT, OUT argument_3 INT, OUT argument_4 INT, INOUT argument_5 INT, IN argument_6 INT, OUT argument_7 INT) AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.InoutTests!inoutMultiarg1'
LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-multiarg-1-dll', 'inoutMultiarg1DLL', inoutMultiarg1DLL(0, 1, 2, NULL, 6) = ROW(2, 4, 5, 6, NULL::INT);

CREATE OR REPLACE FUNCTION inoutMultiarg2DLL(INOUT argument_0 INT, OUT argument_1 INT, INOUT argument_2 INT, INOUT argument_3 INT, IN argument_4 INT, OUT argument_5 INT, IN argument_6 INT, OUT argument_7 INT) AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.InoutTests!inoutMultiarg2'
LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-multiarg-2-dll', 'inoutMultiarg2DLL', inoutMultiarg2DLL(NULL, 2, 3, 4, 6) = ROW(1, 2, 3, NULL::INT, 6, 8);

CREATE OR REPLACE FUNCTION inoutMultiarg3DLL(OUT argument_0 INT, IN argument_1 INT, IN argument_2 INT, OUT argument_3 INT, INOUT argument_4 INT, OUT argument_5 INT, IN argument_6 INT, INOUT argument_7 INT) AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.InoutTests!inoutMultiarg3'
LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-multiarg-3-dll', 'inoutMultiarg3DLL', inoutMultiarg3DLL(1, 2, 4, 6, 7) = ROW(NULL::INT, 4, 5, 6, 8);

CREATE OR REPLACE FUNCTION inoutMultiarg4DLL(IN argument_0 INT, INOUT argument_1 INT, IN argument_2 INT, OUT argument_3 INT, OUT argument_4 INT, INOUT argument_5 INT, INOUT argument_6 INT, IN argument_7 INT) AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.InoutTests!inoutMultiarg4'
LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-multiarg-4-dll', 'inoutMultiarg4DLL', inoutMultiarg4DLL(0, 1, 2, 5, 6, 7) = ROW(2, 4, 5, NULL::INT, 7);

CREATE OR REPLACE FUNCTION inoutArray10DLL(INOUT values_array MACADDR[], OUT nulls INT) AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.InoutTests!inoutArray10'
LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-array-10-dll', 'inoutArray10DLL', inoutArray10DLL(
    ARRAY[
        MACADDR '08-00-2b-01-02-03',
        MACADDR '09-00-2b-01-02-03',
        null::macaddr,
        MACADDR 'a8-00-2b-01-02-03',
        null::macaddr
    ]) = ROW(
        ARRAY[
            MACADDR '09-00-2b-01-02-03',
            MACADDR '0a-00-2b-01-02-03',
            null::macaddr,
            MACADDR 'a9-00-2b-01-02-03',
            null::macaddr
        ], 2
    );

CREATE OR REPLACE FUNCTION inoutArray11DLL(OUT values_array MACADDR[], IN address MACADDR, IN count INT) AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.InoutTests!inoutArray11'
LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-array-11-dll', 'inoutArray11DLL', inoutArray11DLL(MACADDR '08-00-2b-01-02-03', 3) = 
    ARRAY[
            MACADDR '08-00-2b-01-02-03',
            MACADDR '08-00-2b-01-02-03',
            MACADDR '08-00-2b-01-02-03'
        ];

CREATE OR REPLACE FUNCTION inoutSimple10DLL(OUT checksum INT, IN address INET) AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.InoutTests!inoutSimple10'
LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-simple-10-dll', 'inoutSimple10DLL', inoutSimple10DLL(CIDR '192.168/24') = 360;

CREATE OR REPLACE FUNCTION inoutSimple20DLL(INOUT address INET, IN pos INT, IN delta INT) AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.InoutTests!inoutSimple20'
LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-simple-20-dll', 'inoutSimple20DLL', inoutSimple20DLL(CIDR '192.168/24', 2, 3) = INET '192.168.3.0/24';

CREATE OR REPLACE FUNCTION inoutSimple30DLL(OUT checksum INT, INOUT address INET, IN pos INT, IN delta INT) AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.InoutTests!inoutSimple30'
LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-simple-30-dll', 'inoutSimple30DLL', inoutSimple30DLL(CIDR '192.168/24', 2, 3) = ROW(363, INET '192.168.3.0/24');

CREATE OR REPLACE FUNCTION inoutObject10DLL(IN a text, INOUT b text) AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.InoutTests!inoutObject10'
LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-object-10-dll', 'inoutObject10DLL', inoutObject10DLL('red', 'blue') = 'red blue';

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-object-11-dll', 'inoutObject10DLL', inoutObject10DLL('red', NULL) = 'red ';

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-object-12-dll', 'inoutObject10DLL', inoutObject10DLL(NULL, 'blue') = ' blue';


CREATE OR REPLACE FUNCTION inoutObject20DLL(IN a text, b text, OUT c text) AS
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.OtherTests.InoutTests!inoutObject20'
LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-object-20-dll', 'inoutObject20DLL', inoutObject20DLL('red', 'blue') = 'red blue';

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-object-21', 'inoutObject20DLL', inoutObject20DLL('red', NULL) = 'red ';

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-object-22', 'inoutObject20DLL', inoutObject20DLL(NULL, 'blue') = ' blue';

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-object-23', 'inoutObject20DLL', inoutObject20DLL('🐂', '🥰') = '🐂 🥰'::TEXT;