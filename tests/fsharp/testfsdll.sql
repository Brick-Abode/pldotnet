-- SMALLINT
CREATE OR REPLACE FUNCTION sum2SmallIntFSharpDLL(a INT2, b INT2) RETURNS INT2 AS
'/app/pldotnet/tests/fsharp/DotNetTestProject/bin/Release/net6.0/FSharpTest.dll:TestFSharpDLLFunctions.TestFSharpClass!sum2SmallIntFSharp'
LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int2-dll', 'sum2SmallIntFSharpDLL1', sum2SmallIntFSharpDLL('25'::INT2, '250'::INT2) = '275'::INT2;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int2-dll', 'sum2SmallIntFSharpDLL2', sum2SmallIntFSharpDLL('1997'::INT2, '25'::INT2) = '2022'::INT2;

-- INTEGER
CREATE OR REPLACE FUNCTION mult2IntFSharpDLL(a INT4, b INT4) RETURNS INT4 AS
'/app/pldotnet/tests/fsharp/DotNetTestProject/bin/Release/net6.0/FSharpTest.dll:TestFSharpDLLFunctions.TestFSharpClass!mult2IntFSharp'
LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int4-dll', 'mult2IntFSharpDLL1', mult2IntFSharpDLL('25'::INT2, '30'::INT2) = '750'::INT4;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int4-null-dll', 'mult2IntFSharpDLL2', mult2IntFSharpDLL('25'::INT2, NULL::INT2) = '25'::INT4;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int4-null-dll', 'mult2IntFSharpDLL3', mult2IntFSharpDLL(NULL::INT2, '30'::INT2) = '30'::INT4;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int4-null-dll', 'mult2IntFSharpDLL4', mult2IntFSharpDLL(NULL::INT2, NULL::INT2) IS NULL;

-- FLOAT4[]
CREATE OR REPLACE FUNCTION modifyFloat4ArrayFSharpDLL(a FLOAT4[], b FLOAT4) RETURNS FLOAT4[] AS
'/app/pldotnet/tests/fsharp/DotNetTestProject/bin/Release/net6.0/FSharpTest.dll:TestFSharpDLLFunctions.TestFSharpClass!modifyFloat4ArrayFSharp'
LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float4-1darray-null-dll', 'modifyFloat4ArrayFSharpDLL1', modifyFloat4ArrayFSharpDLL(ARRAY[1.50055::FLOAT4, NULL::FLOAT4, 4.52123::FLOAT4, 7.41234::FLOAT4], 12.121212::FLOAT4) = ARRAY[12.121212::FLOAT4, null::FLOAT4, 4.52123::FLOAT4, 7.41234::FLOAT4];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float4-2darray-null-dll', 'modifyFloat4ArrayFSharpDLL2', modifyFloat4ArrayFSharpDLL(ARRAY[[1.50055::FLOAT4, NULL::FLOAT4], [4.52123::FLOAT4, 7.41234::FLOAT4]], 3.141516::FLOAT4) = ARRAY[[3.141516::FLOAT4, NULL::FLOAT4], [4.52123::FLOAT4, 7.41234::FLOAT4]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float4-2darray-null-dll', 'modifyFloat4ArrayFSharpDLL3', modifyFloat4ArrayFSharpDLL(ARRAY[[1.50055::FLOAT4, NULL::FLOAT4], [NULL::FLOAT4, NULL::FLOAT4]], 12.121212::FLOAT4) = ARRAY[[12.121212::FLOAT4, NULL::FLOAT4], [NULL::FLOAT4, NULL::FLOAT4]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float4-3darray-null-dll', 'modifyFloat4ArrayFSharpDLL4', modifyFloat4ArrayFSharpDLL(ARRAY[[[1.50055::FLOAT4, NULL::FLOAT4], [NULL::FLOAT4, NULL::FLOAT4]], [[1.50055::FLOAT4, NULL::FLOAT4], [4.52123::FLOAT4, 7.41234::FLOAT4]]], 3.141516::FLOAT4) = ARRAY[[[3.141516::FLOAT4, NULL::FLOAT4], [NULL::FLOAT4, NULL::FLOAT4]], [[1.50055::FLOAT4, NULL::FLOAT4], [4.52123::FLOAT4, 7.41234::FLOAT4]]];

-- FLOAT8[]
CREATE OR REPLACE FUNCTION modifyFloat8ArrayFSharpDLL(a FLOAT8[], b FLOAT8) RETURNS FLOAT8[] AS
'/app/pldotnet/tests/fsharp/DotNetTestProject/bin/Release/net6.0/FSharpTest.dll:TestFSharpDLLFunctions.TestFSharpClass!modifyFloat8ArrayFSharp'
LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float8-1darray-null-dll', 'modifyFloat8ArrayFSharpDLL1', modifyFloat8ArrayFSharpDLL(ARRAY[21.0000000000109::FLOAT8, NULL::FLOAT8, 4.521234313421::FLOAT8, 7.412344328978::FLOAT8], 11.0050000000005::FLOAT8) = ARRAY[11.0050000000005::FLOAT8, NULL::FLOAT8, 4.521234313421::FLOAT8, 7.412344328978::FLOAT8];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float8-2darray-null-dll', 'modifyFloat8ArrayFSharpDLL2', modifyFloat8ArrayFSharpDLL(ARRAY[[21.0000000000109::FLOAT8, NULL::FLOAT8], [4.521234313421::FLOAT8, 7.412344328978::FLOAT8]], 11.0050000000005::FLOAT8) = ARRAY[[11.0050000000005::FLOAT8, NULL::FLOAT8], [4.521234313421::FLOAT8, 7.412344328978::FLOAT8]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float8-2darray-null-dll', 'modifyFloat8ArrayFSharpDLL3', modifyFloat8ArrayFSharpDLL(ARRAY[[NULL::FLOAT8, NULL::FLOAT8], [NULL::FLOAT8, NULL::FLOAT8]], 11.0050000000005::FLOAT8) = ARRAY[[11.0050000000005::FLOAT8, NULL::FLOAT8], [NULL::FLOAT8, NULL::FLOAT8]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float8-2darray-null-dll', 'modifyFloat8ArrayFSharpDLL4', modifyFloat8ArrayFSharpDLL(ARRAY[[NULL::FLOAT8, NULL::FLOAT8], [NULL::FLOAT8, NULL::FLOAT8]], NULL::FLOAT8) = ARRAY[[0.0::FLOAT8, NULL::FLOAT8], [NULL::FLOAT8, NULL::FLOAT8]];

-- DATE
CREATE OR REPLACE FUNCTION createDateFSharpDLL(a INT4, b INT4, c INT4) RETURNS DATE AS
'/app/pldotnet/tests/fsharp/DotNetTestProject/bin/Release/net6.0/FSharpTest.dll:TestFSharpDLLFunctions.TestFSharpClass!createDateFSharp'
LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-date-dll', 'createDateFSharpDLL1', createDateFSharpDLL(1997, 4, 30) = 'Apr-30-1997'::DATE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-date-dll', 'createDateFSharpDLL2', createDateFSharpDLL(2023, 1, 1) = 'Jan-01-2023'::DATE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-date-null-dll', 'createDateFSharpDLL3', createDateFSharpDLL(2023, 1, NULL) IS NULL;

-- TIME
CREATE OR REPLACE FUNCTION addMinutesFSharpDLL(a TIME, b INT4) RETURNS TIME AS
'/app/pldotnet/tests/fsharp/DotNetTestProject/bin/Release/net6.0/FSharpTest.dll:TestFSharpDLLFunctions.TestFSharpClass!addMinutes'
LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-time-dll', 'addMinutesFSharpDLL1', addMinutesFSharpDLL('05:30 PM'::TIME, 75) = '06:45 PM'::TIME;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-time-dll', 'addMinutesFSharpDLL2', addMinutesFSharpDLL('04:20 PM', NULL::INT4) = '04:20 PM'::TIME;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-time-null-dll', 'addMinutesFSharpDLL3', addMinutesFSharpDLL(NULL::TIME, 75) = '01:45:20'::TIME;

-- TIMESTAMP[]
CREATE OR REPLACE FUNCTION modifyTimestampArrayFSharpDLL(a TIMESTAMP[], b TIMESTAMP) RETURNS TIMESTAMP[] AS
'/app/pldotnet/tests/fsharp/DotNetTestProject/bin/Release/net6.0/FSharpTest.dll:TestFSharpDLLFunctions.TestFSharpClass!modifyTimestampArray'
LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamp-1array-null-dll', 'modifyTimestampArrayFSharpDLL1', modifyTimestampArrayFSharpDLL(ARRAY['2004-12-19 10:23:54 PM'::TIMESTAMP, '2020-10-19 10:23:54 PM'::TIMESTAMP, NULL::TIMESTAMP, '2022-12-25 10:23:54 PM'::TIMESTAMP], NULL::TIMESTAMP) = ARRAY['2022-11-15 13:23:45'::TIMESTAMP, '2020-10-19 10:23:54 PM'::TIMESTAMP, NULL::TIMESTAMP, '2022-12-25 10:23:54 PM'::TIMESTAMP];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamp-2array-null-dll', 'modifyTimestampArrayFSharpDLL2', modifyTimestampArrayFSharpDLL(ARRAY[['2004-12-19 10:23:54 PM'::TIMESTAMP, '2020-10-19 10:23:54 PM'::TIMESTAMP], [NULL::TIMESTAMP, '2022-12-25 10:23:54 PM'::TIMESTAMP]], '2023-01-01 12:12:12 PM'::TIMESTAMP) = ARRAY[['2023-01-01 12:12:12 PM'::TIMESTAMP, '2020-10-19 10:23:54 PM'::TIMESTAMP], [NULL::TIMESTAMP, '2022-12-25 10:23:54 PM'::TIMESTAMP]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamp-2array-null-dll', 'modifyTimestampArrayFSharpDLL3', modifyTimestampArrayFSharpDLL(ARRAY[[NULL::TIMESTAMP, NULL::TIMESTAMP], [NULL::TIMESTAMP, NULL::TIMESTAMP]], '2023-01-01 12:12:12 PM'::TIMESTAMP) = ARRAY[['2023-01-01 12:12:12 PM'::TIMESTAMP, NULL::TIMESTAMP], [NULL::TIMESTAMP, NULL::TIMESTAMP]];

-- TEXT
CREATE OR REPLACE FUNCTION concatenateStringFSharpDLL(a TEXT, b TEXT) RETURNS TEXT AS
'/app/pldotnet/tests/fsharp/DotNetTestProject/bin/Release/net6.0/FSharpTest.dll:TestFSharpDLLFunctions.TestFSharpClass!concatenateString'
LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-text-dll', 'concatenateStringFSharpDLL1', concatenateStringFSharpDLL('Neymar'::TEXT, 'Jr.'::TEXT) = 'Neymar Jr.'::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-text-null-dll', 'concatenateStringFSharpDLL2', concatenateStringFSharpDLL('Brasil'::TEXT, NULL::TEXT) = 'Brasil'::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-text-null-dll', 'concatenateStringFSharpDLL3', concatenateStringFSharpDLL(NULL::TEXT, 'Hello World!'::TEXT) = 'Hello World!'::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-text-null-dll', 'concatenateStringFSharpDLL4', concatenateStringFSharpDLL(NULL::TEXT, NULL::TEXT) IS NULL;

-- VARCHAR[]
CREATE OR REPLACE FUNCTION modifyStringArrayFSharpDLL(a VARCHAR[], b TEXT) RETURNS VARCHAR[] AS
'/app/pldotnet/tests/fsharp/DotNetTestProject/bin/Release/net6.0/FSharpTest.dll:TestFSharpDLLFunctions.TestFSharpClass!modifyStringArray'
LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-varchar-1array-dll', 'modifyStringArrayFSharpDLL1', modifyStringArrayFSharpDLL(ARRAY['Alemanha'::VARCHAR, 'Inglaterra'::VARCHAR, 'Espanha'::VARCHAR], 'Portugal'::TEXT) = ARRAY['Portugal'::VARCHAR, 'Inglaterra'::VARCHAR, 'Espanha'::VARCHAR];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-varchar-2array-null-dll', 'modifyStringArrayFSharpDLL2', modifyStringArrayFSharpDLL(ARRAY[['Alemanha'::VARCHAR, 'Inglaterra'::VARCHAR], ['Espanha'::VARCHAR, NULL::VARCHAR]], 'Portugal'::TEXT) = ARRAY[['Portugal'::VARCHAR, 'Inglaterra'::VARCHAR], ['Espanha'::VARCHAR, NULL::VARCHAR]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-varchar-2array-null-dll', 'modifyStringArrayFSharpDLL3', modifyStringArrayFSharpDLL(ARRAY[[NULL::VARCHAR, 'Inglaterra'::VARCHAR], ['Espanha'::VARCHAR, NULL::VARCHAR]], 'Portugal'::TEXT) = ARRAY[['Portugal'::VARCHAR, 'Inglaterra'::VARCHAR], ['Espanha'::VARCHAR, NULL::VARCHAR]];
