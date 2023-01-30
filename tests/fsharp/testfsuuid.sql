CREATE OR REPLACE FUNCTION returnUUIDFSharp(a UUID) RETURNS UUID AS $$
a
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-uuid', 'returnUUIDFSharp1', returnUUIDFSharp('a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID) = 'a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-uuid', 'returnUUIDFSharp2', returnUUIDFSharp('123e4567-e89b-12d3-a456-426614174000'::UUID) = '123e4567-e89b-12d3-a456-426614174000'::UUID;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-uuid', 'returnUUIDFSharp3', returnUUIDFSharp('87e3006a-604e-11ed-9b6a-0242ac120002'::UUID) = '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-uuid', 'returnUUIDFSharp4', returnUUIDFSharp('024be913-3bf8-4499-9694-12769239b763'::UUID) = '024be913-3bf8-4499-9694-12769239b763'::UUID;

CREATE OR REPLACE FUNCTION createUUIDFSharp(a TEXT) RETURNS UUID AS $$
Guid(a)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-uuid', 'createUUIDFSharp1', createUUIDFSharp('a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::TEXT) = 'a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-uuid', 'createUUIDFSharp2', createUUIDFSharp('123e4567-e89b-12d3-a456-426614174000'::TEXT) = '123e4567-e89b-12d3-a456-426614174000'::UUID;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-uuid', 'createUUIDFSharp3', createUUIDFSharp('87e3006a-604e-11ed-9b6a-0242ac120002'::TEXT) = '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-uuid', 'createUUIDFSharp4', createUUIDFSharp('024be913-3bf8-4499-9694-12769239b763'::TEXT) = '024be913-3bf8-4499-9694-12769239b763'::UUID;

CREATE OR REPLACE FUNCTION combineUUIDsFSharp(a UUID, b UUID) RETURNS UUID AS $$
let a = if a.HasValue then a.Value else Guid("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11")
let b = if b.HasValue then b.Value else Guid("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11")
let aStr = a.ToString()
let bStr = b.ToString()
let aList = aStr.Split('-')
let bList = bStr.Split('-')
let newUuuidStr = aList[0] + aList[1] + aList[2] + bList[3] + bList[4]
Guid(newUuuidStr)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-uuid', 'combineUUIDsFSharp1', combineUUIDsFSharp('a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID) = 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-uuid', 'combineUUIDsFSharp2', combineUUIDsFSharp('123e4567-e89b-12d3-a456-426614174000'::UUID, '024be913-3bf8-4499-9694-12769239b763'::UUID) = '123e4567-e89b-12d3-9694-12769239b763'::UUID;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-uuid-null', 'combineUUIDsFSharp3', combineUUIDsFSharp(NULL::UUID, '024be913-3bf8-4499-9694-12769239b763'::UUID) = 'a0eebc99-9c0b-4ef8-9694-12769239b763'::UUID;

--- UUID Arrays

CREATE OR REPLACE FUNCTION updateUUIDArrayIndexFSharp(a UUID[], b UUID) RETURNS UUID[] AS $$
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
SELECT 'f#-uuid-null-1array', 'updateUUIDArrayIndexFSharp1', updateUUIDArrayIndexFSharp(ARRAY['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID, null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID], 'fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID) = ARRAY['fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID, null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-uuid-null-2array', 'updateUUIDArrayIndexFSharp2', updateUUIDArrayIndexFSharp(ARRAY[['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID], [null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]], 'fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID) = ARRAY[['fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID], [null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-uuid-null-3array', 'updateUUIDArrayIndexFSharp3', updateUUIDArrayIndexFSharp(ARRAY[[['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID], [null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]]], 'fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID) = ARRAY[[['fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID], [null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-uuid-null-1array-arraynull', 'updateUUIDArrayIndexFSharp4', updateUUIDArrayIndexFSharp(ARRAY[null::UUID, null::UUID, null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID], 'fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID) = ARRAY['fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, null::UUID, null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-uuid-null-2array-arraynull', 'updateUUIDArrayIndexFSharp5', updateUUIDArrayIndexFSharp(ARRAY[[null::UUID, null::UUID], [null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]], 'fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID) = ARRAY[['fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, null::UUID], [null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-uuid-null-3array-arraynull', 'updateUUIDArrayIndexFSharp6', updateUUIDArrayIndexFSharp(ARRAY[[[null::UUID, null::UUID], [null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]]], 'fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID) = ARRAY[[['fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, null::UUID], [null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]]];

CREATE OR REPLACE FUNCTION CreateUUIDMultidimensionalArrayFSharp() RETURNS UUID[] AS $$
let objects_value = Guid("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11")
let arr = Array.CreateInstance(typeof<Guid>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-uuid-null-3array-arraynull', 'CreateUUIDMultidimensionalArrayFSharp1', CreateUUIDMultidimensionalArrayFSharp() = ARRAY[[['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID]]];
