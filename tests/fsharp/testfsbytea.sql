-- BYTEA

CREATE OR REPLACE FUNCTION byteaConversionsFSharp(a BYTEA, b BYTEA) RETURNS BYTEA AS $$
    let utf8_e = new UTF8Encoding()
    match (a, b) with
    | (null, null) -> null
    | (null, _) -> b
    | (_, null) -> a
    | _ ->
        let s1 = utf8_e.GetString(a, 0, a.Length)
        let s2 = utf8_e.GetString(b, 0, b.Length)
        let result = s1 + " " + s2
        utf8_e.GetBytes result
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bytea', 'byteaConversionsFSharp1', byteaConversionsFSharp('Brick Abode is nice!'::BYTEA, 'Thank you very much...'::BYTEA) = '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bytea-null', 'byteaConversionsFSharp2', byteaConversionsFSharp(NULL::BYTEA, 'Thank you very much...'::BYTEA) = 'Thank you very much...'::BYTEA;

-- --- BYTEA Arrays

CREATE OR REPLACE FUNCTION updateByteaArrayIndex(a BYTEA[], b BYTEA) RETURNS BYTEA[] AS $$
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
SELECT 'f#-bytea-null-1array', 'updateByteaArrayIndex1', updateByteaArrayIndex(ARRAY['Brick Abode is nice!'::BYTEA, 'Test 1!'::BYTEA, null::BYTEA, '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA], 'Inserted BYTEA'::BYTEA) = ARRAY['Inserted BYTEA'::BYTEA, 'Test 1!'::BYTEA, null::BYTEA, '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bytea-null-2array', 'updateByteaArrayIndex2', updateByteaArrayIndex(ARRAY[['Brick Abode is nice!'::BYTEA, 'Test 1!'::BYTEA], [null::BYTEA, '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]], 'Inserted BYTEA'::BYTEA) = ARRAY[['Inserted BYTEA'::BYTEA, 'Test 1!'::BYTEA], [null::BYTEA, '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bytea-null-3array', 'updateByteaArrayIndex3', updateByteaArrayIndex(ARRAY[[['Brick Abode is nice!'::BYTEA, 'Test 1!'::BYTEA], [null::BYTEA, '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]]], 'Inserted BYTEA'::BYTEA) = ARRAY[[['Inserted BYTEA'::BYTEA, 'Test 1!'::BYTEA], [null::BYTEA, '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bytea-null-1array-arraynull', 'updateByteaArrayIndex4', updateByteaArrayIndex(ARRAY[null::BYTEA, null::BYTEA, null::BYTEA, '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA], 'Inserted BYTEA'::BYTEA) = ARRAY['Inserted BYTEA'::BYTEA, null::BYTEA, null::BYTEA, '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bytea-null-2array-arraynull', 'updateByteaArrayIndex3', updateByteaArrayIndex(ARRAY[[null::BYTEA, null::BYTEA], [null::BYTEA, '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]], 'Inserted BYTEA'::BYTEA) = ARRAY[['Inserted BYTEA'::BYTEA, null::BYTEA], [null::BYTEA, '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bytea-null-3array-arraynull', 'updateByteaArrayIndex3', updateByteaArrayIndex(ARRAY[[[null::BYTEA, null::BYTEA], [null::BYTEA, '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]]], 'Inserted BYTEA'::BYTEA) = ARRAY[[['Inserted BYTEA'::BYTEA, null::BYTEA], [null::BYTEA, '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]]];

CREATE OR REPLACE FUNCTION CreateByteaMultidimensionalArray() RETURNS BYTEA[] AS $$
let objects_value = [| 0x92uy; 0x83uy; 0x74uy; 0x65uy; 0x56uy; 0x47uy; 0x38uy |]
let arr = Array.CreateInstance(typeof<byte[]>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bytea-null-3array-arraynull', 'CreateByteaMultidimensionalArray1', CreateByteaMultidimensionalArray() = ARRAY[[['\x92837465564738'::BYTEA]]];
