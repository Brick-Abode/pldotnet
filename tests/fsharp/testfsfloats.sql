-- Float4 (real): 6 digits of precison
CREATE OR REPLACE FUNCTION returnRealFSharp() RETURNS real AS $$
1.50055f
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float4', 'returnRealFSharp', returnRealFSharp() = real '1.50055';

CREATE OR REPLACE FUNCTION sumRealFSharp(a float4, b float4) RETURNS float4 AS $$
let a = if a.HasValue then a.Value else float32 0.0
let b = if b.HasValue then b.Value else float32 0.0
System.Nullable(a + b)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float4', 'sumRealFSharp1', sumRealFSharp(1.50055, 1.50054) = float4 '3.00109';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float4-null', 'sumRealFSharp2', sumRealFSharp(NULL, 1.50054) = float4 '1.50054';

--- Float8 (float8): 15 digits of precison
CREATE OR REPLACE FUNCTION returnDoubleFSharp() RETURNS float8 AS $$
11.0050000000005
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float8', 'returnDoubleFSharp', returnDoubleFSharp() = float8 '11.0050000000005';

CREATE OR REPLACE FUNCTION sumDoubleFSharp(a float8, b float8) RETURNS float8 AS $$
let a = if a.HasValue then a.Value else float 0.0
let b = if b.HasValue then b.Value else float 0.0
System.Nullable(a + b)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float8', 'sumDoubleFSharp1', sumDoubleFSharp(10.5000000000055, 10.5000000000054) = float8  '21.0000000000109';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float8-null', 'sumDoubleFSharp2', sumDoubleFSharp(NULL, NULL) = float8 '0';

-- - Float Arrays
CREATE OR REPLACE FUNCTION returnRealArrayFSharp(floats real[]) RETURNS real[] AS $$
floats
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float4-null-1array', 'returnRealArrayFSharp1', returnRealArrayFSharp(ARRAY[1.50055::real, null::real, 4.52123::real, 7.41234::real]) = ARRAY[1.50055::real, null::real, 4.52123::real, 7.41234::real];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float4-null-2array-arraynull', 'returnRealArrayFSharp2', returnRealArrayFSharp(ARRAY[[null::real, null::real], [4.52123::real, 7.41234::real]]) = ARRAY[[null::real, null::real], [4.52123::real, 7.41234::real]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float4-null-3array-arraynull', 'returnRealArrayFSharp3', returnRealArrayFSharp(ARRAY[[[null::real, null::real], [null::real, null::real]], [[7.50055::real, 8.30300::real], [9.52123::real, 11.41234::real]]]) = ARRAY[[[null::real, null::real], [null::real, null::real]], [[7.50055::real, 8.30300::real], [9.52123::real, 11.41234::real]]];

CREATE OR REPLACE FUNCTION CreateRealMultidimensionalArrayFSharp() RETURNS real[] AS $$
let arr = Array.CreateInstance(typeof<float32>, 3, 3)
arr.SetValue(float32 1.24323, 0, 0)
arr.SetValue(float32 8.11134, 1, 1)
arr.SetValue(float32 16.14256, 2, 2)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float4-null-3array-arraynull', 'CreateRealMultidimensionalArrayFSharp', CreateRealMultidimensionalArrayFSharp() = ARRAY[[1.24323::real, 0::real, 0::real], [0::real, 8.11134::real, 0::real], [0::real, 0::real, 16.14256::real]];

CREATE OR REPLACE FUNCTION updateArrayRealIndexFSharp(a real[], b real) RETURNS real[] AS $$
let dim = a.Rank
match dim with
| 1 ->
    a.SetValue(b, 0) |> ignore
    a
| 2 ->
    a.SetValue(b, 0, 0) |> ignore
    a
| 3 ->
    a.SetValue(b, 0, 0, 0) |> ignore
    a
| _ -> a
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float4-null-1array', 'updateArrayRealIndexFSharp1', updateArrayRealIndexFSharp(ARRAY[4.55555::real, 10.11324::real, null::real], 9.83212) = ARRAY[9.83212::real, 10.11324::real, null::real];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float4-null-2array', 'updateArrayRealIndexFSharp2', updateArrayRealIndexFSharp(ARRAY[[4.55555::real, 10.11324::real], [null::real, 16.12464::real]], 9.83212) = ARRAY[[9.83212::real, 10.11324::real], [null::real, 16.12464::real]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float4-null-3array', 'updateArrayRealIndexFSharp3', updateArrayRealIndexFSharp(ARRAY[[[4.55555::real, 10.11324::real], [null::real, 16.12464::real]]], 9.83212) = ARRAY[[[9.83212::real, 10.11324::real], [null::real, 16.12464::real]]];

-- --- Double Arrays
CREATE OR REPLACE FUNCTION returnDoubleArrayFSharp(doubles float8[]) RETURNS float8[] AS $$
doubles
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float8-null-1array', 'returnDoubleArrayFSharp1', returnDoubleArrayFSharp(ARRAY[21.0000000000109::float8, null::float8, 4.521234313421::float8, 7.412344328978::float8]) = ARRAY[21.0000000000109::float8, null::float8, 4.521234313421::float8, 7.412344328978::float8];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float8-null-2array-arraynull', 'returnDoubleArrayFSharp2', returnDoubleArrayFSharp(ARRAY[[null::float8, null::float8], [4.521234313421::float8, 7.412344328978::float8]]) = ARRAY[[null::float8, null::float8], [4.521234313421::float8, 7.412344328978::float8]];

CREATE OR REPLACE FUNCTION CreateDoubleMultidimensionalArrayFSharp() RETURNS float8[] AS $$
let arr = Array.CreateInstance(typeof<float>, 3, 3)
arr.SetValue(float 1.24323, 0, 0)
arr.SetValue(float 8.11134, 1, 1)
arr.SetValue(float 16.14256, 2, 2)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float4-null-3array-arraynull', 'CreateDoubleMultidimensionalArrayFSharp', CreateDoubleMultidimensionalArrayFSharp() = ARRAY[[1.24323::float8, 0::float8, 0::float8], [0::float8, 8.11134::float8, 0::float8], [0::float8, 0::float8, 16.14256::float8]];

CREATE OR REPLACE FUNCTION make_pi_n_fs(a int) RETURNS float8 AS $$
    let mutable sum : float = 0.0
    for i = 0 to a do
        sum <- sum + ((if i % 2 = 0 then 1.0 else -1.0)/ float(2.0 * float(i) + 1.0))
    Nullable<float>(sum * 4.0);
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float8-make-pi', 'make_pi_lt', make_pi_n_fs(1000) < double precision '3.15';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float8-make-pi', 'make_pi_gt', make_pi_n_fs(1000) > double precision '3.13';

CREATE OR REPLACE FUNCTION updateArrayDoubleIndexFSharp(a float8[], b float8) RETURNS float8[] AS $$
let dim = a.Rank
match dim with
| 1 ->
    a.SetValue(b, 0) |> ignore
    a
| 2 ->
    a.SetValue(b, 0, 0) |> ignore
    a
| 3 ->
    a.SetValue(b, 0, 0, 0) |> ignore
    a
| _ -> a
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float8-null-1array', 'updateArrayDoubleIndexFSharp1', updateArrayDoubleIndexFSharp(ARRAY[4.55535544213::float8, 10.1133254154::float8, null::float8], 9.8321432132) = ARRAY[9.8321432132::float8, 10.1133254154::float8, null::float8];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float8-null-2array', 'updateArrayDoubleIndexFSharp2', updateArrayDoubleIndexFSharp(ARRAY[[4.55535544213::float8, 10.1133254154::float8], [null::float8, 16.16155::float8]], 9.8321432132) = ARRAY[[9.8321432132::float8, 10.1133254154::float8], [null::float8, 16.16155::float8]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-float8-null-2array', 'updateArrayDoubleIndexFSharp2', updateArrayDoubleIndexFSharp(ARRAY[[[4.55535544213::float8, 10.1133254154::float8], [null::float8, 16.16155::float8]]], 9.8321432132) = ARRAY[[[9.8321432132::float8, 10.1133254154::float8], [null::float8, 16.16155::float8]]];
