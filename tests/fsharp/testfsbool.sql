CREATE OR REPLACE FUNCTION returnBoolFSharp() RETURNS boolean AS $$
false
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool', 'returnBoolFSharp', returnBoolFSharp() is false;

CREATE OR REPLACE FUNCTION BooleanAndFSharp(a boolean, b boolean) RETURNS boolean AS $$
let a = if a.HasValue then a else false
let b = if b.HasValue then b else false
a.Value && b.Value
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool', 'BooleanAndFSharp1', BooleanAndFSharp(true, true) is true;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool-null', 'BooleanAndFSharp2', BooleanAndFSharp(NULL::BOOLEAN, true) is false;

CREATE OR REPLACE FUNCTION BooleanOrFSharp(a boolean, b boolean) RETURNS boolean AS $$
a.Value || b.Value
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool', 'BooleanOrFSharp1', BooleanOrFSharp(false, false) is false;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool-null', 'BooleanOrFSharp2', BooleanOrFSharp(true, NULL::BOOLEAN) is true;

CREATE OR REPLACE FUNCTION BooleanXorFSharp(a boolean, b boolean) RETURNS boolean AS $$
let a = if a.HasValue then a else false
let b = if b.HasValue then b else false
(a.Value && not b.Value) || (not a.Value && b.Value)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool', 'BooleanXorFSharp1', BooleanXorFSharp(false, false) is false;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool-null', 'BooleanXorFSharp2', BooleanXorFSharp(NULL::BOOLEAN, NULL::BOOLEAN) is false;

-- --- Arrays

CREATE OR REPLACE FUNCTION returnBooleanArrayFSharp(booleans boolean[]) RETURNS boolean[] AS $$
booleans
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool-null-1array', 'returnBooleanArrayFSharp1', returnBooleanArrayFSharp(ARRAY[true, null::boolean, false, false]) = ARRAY[true, null::boolean, false, false];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool-null-2array-arraynull', 'returnBooleanArrayFSharp2', returnBooleanArrayFSharp(ARRAY[[true, false], [null::boolean, null::boolean]]) = ARRAY[[true, false], [null::boolean, null::boolean]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool-null-3array-arraynull', 'returnBooleanArrayFSharp3', returnBooleanArrayFSharp(ARRAY[[[true, false], [null::boolean, null::boolean]], [[true, null::boolean], [true, null::boolean]]]) = ARRAY[[[true, false], [null::boolean, null::boolean]], [[true, null::boolean], [true, null::boolean]]];

CREATE OR REPLACE FUNCTION countBoolFSharp(booleans boolean[], desired boolean) RETURNS Integer AS $$
let flatten_booleans = Array.CreateInstance(typeof<Object>, booleans.Length)
ArrayManipulation.FlatArray(booleans, ref flatten_booleans) |> ignore
let mutable count = 0
for i = 0 to flatten_booleans.Length - 1 do
    if System.Object.ReferenceEquals(flatten_booleans.GetValue(i), null) then
        ()
    else if flatten_booleans.GetValue(i).Equals(desired) then
        count <- count + 1
count
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool-null-1array', 'countBoolFSharp1', countBoolFSharp(ARRAY[true, true, false, true, null::boolean], true) = integer '3';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool-null-1array', 'countBoolFSharp2', countBoolFSharp(ARRAY[true, true, false, true, null::boolean], false) = integer '1';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool-null-2array', 'countBoolFSharp3', countBoolFSharp(ARRAY[[true, null::boolean, true], [true, false, null::boolean]], true) = integer '3';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool-null-2array', 'countBoolFSharp4', countBoolFSharp(ARRAY[[true, null::boolean, true], [true, false, null::boolean]], false) = integer '1';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool-null-3array', 'countBoolFSharp5', countBoolFSharp(ARRAY[[[true, true, null::boolean], [true, null::boolean, false]], [[null::boolean, true, false], [true, null::boolean, false]]], true) = integer '5';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool-null-3array', 'countBoolFSharp6', countBoolFSharp(ARRAY[[[true, true, null::boolean], [true, null::boolean, false]], [[null::boolean, true, false], [true, null::boolean, false]]], false) = integer '3';

CREATE OR REPLACE FUNCTION CreateBooleanMultidimensionalArrayFSharp() RETURNS boolean[] AS $$
let arr = Array.CreateInstance(typeof<bool>, 3, 3)
arr.SetValue(true, 0, 0)
arr.SetValue(true, 1, 1)
arr.SetValue(true, 2, 2)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool-null-3array-arraynull', 'CreateBooleanMultidimensionalArrayFSharp', CreateBooleanMultidimensionalArrayFSharp() = ARRAY[[true, false, false], [false, true, false], [false, false, true]];

CREATE OR REPLACE FUNCTION updateArrayBooleanIndexFSharp(a boolean[], b boolean) RETURNS boolean[] AS $$
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
SELECT 'f#-bool-1array', 'updateArrayBooleanIndexFSharp1', updateArrayBooleanIndexFSharp(ARRAY[true, false, true], false) = ARRAY[false, false, true];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool-2array', 'updateArrayBooleanIndexFSharp2', updateArrayBooleanIndexFSharp(ARRAY[[true, false], [true, false]], false) = ARRAY[[false, false], [true, false]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool-2array', 'updateArrayBooleanIndexFSharp3', updateArrayBooleanIndexFSharp(ARRAY[[[true, false], [true, false]]], false) = ARRAY[[[false, false], [true, false]]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool-null-1array', 'updateArrayBooleanIndexFSharp4', updateArrayBooleanIndexFSharp(ARRAY[null::boolean, false, true], true) = ARRAY[true, false, true];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool-null-2array', 'updateArrayBooleanIndexFSharp5', updateArrayBooleanIndexFSharp(ARRAY[[null::boolean, false], [null::boolean, false]], false) = ARRAY[[false, false], [null::boolean, false]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool-null-2array', 'updateArrayBooleanIndexFSharp6', updateArrayBooleanIndexFSharp(ARRAY[[[null::boolean, false], [null::boolean, false]]], false) = ARRAY[[[false, false], [null::boolean, false]]];
