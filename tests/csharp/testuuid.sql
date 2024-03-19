CREATE OR REPLACE FUNCTION returnUUID(a UUID) RETURNS UUID AS $$
    return a;
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-uuid', 'returnUUID1', returnUUID('a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID) = 'a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-uuid', 'returnUUID2', returnUUID('123e4567-e89b-12d3-a456-426614174000'::UUID) = '123e4567-e89b-12d3-a456-426614174000'::UUID;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-uuid', 'returnUUID3', returnUUID('87e3006a-604e-11ed-9b6a-0242ac120002'::UUID) = '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-uuid', 'returnUUID4', returnUUID('024be913-3bf8-4499-9694-12769239b763'::UUID) = '024be913-3bf8-4499-9694-12769239b763'::UUID;

CREATE OR REPLACE FUNCTION createUUID(a TEXT) RETURNS UUID AS $$
    return new Guid(a);
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-uuid', 'createUUID1', createUUID('a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::TEXT) = 'a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-uuid', 'createUUID2', createUUID('123e4567-e89b-12d3-a456-426614174000'::TEXT) = '123e4567-e89b-12d3-a456-426614174000'::UUID;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-uuid', 'createUUID3', createUUID('87e3006a-604e-11ed-9b6a-0242ac120002'::TEXT) = '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-uuid', 'createUUID4', createUUID('024be913-3bf8-4499-9694-12769239b763'::TEXT) = '024be913-3bf8-4499-9694-12769239b763'::UUID;

CREATE OR REPLACE FUNCTION combineUUIDs(a UUID, b UUID) RETURNS UUID AS $$
    if (a == null)
        a = new Guid("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11");

    if (b == null)
        b = new Guid("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11");

    string aStr = a.ToString();
    string bStr = b.ToString();
    var aList = aStr.Split('-');
    var bList = bStr.Split('-');
    string newUuuidStr = aList[0] + aList[1] + aList[2] + bList[3] + bList[4];
    return new Guid(newUuuidStr);
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-uuid', 'combineUUIDs1', combineUUIDs('a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID) = 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-uuid', 'combineUUIDs2', combineUUIDs('123e4567-e89b-12d3-a456-426614174000'::UUID, '024be913-3bf8-4499-9694-12769239b763'::UUID) = '123e4567-e89b-12d3-9694-12769239b763'::UUID;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-uuid-null', 'combineUUIDs3', combineUUIDs(NULL::UUID, '024be913-3bf8-4499-9694-12769239b763'::UUID) = 'a0eebc99-9c0b-4ef8-9694-12769239b763'::UUID;

--- UUID Arrays

CREATE OR REPLACE FUNCTION updateUUIDArrayIndex(values_array UUID[], desired UUID, index integer[]) RETURNS UUID[] AS $$
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-uuid-null-1array', 'updateUUIDArrayIndex1', updateUUIDArrayIndex(ARRAY['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID, null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID], 'fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, ARRAY[2]) = ARRAY['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID, 'fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-uuid-null-2array', 'updateUUIDArrayIndex2', updateUUIDArrayIndex(ARRAY[['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID], [null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]], 'fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, ARRAY[1,0]) = ARRAY[['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID], ['fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-uuid-null-2array-arraynull', 'updateUUIDArrayIndex3', updateUUIDArrayIndex(ARRAY[[null::UUID, null::UUID], [null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]], 'fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, ARRAY[1,0]) = ARRAY[[null::UUID, null::UUID], ['fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]];

CREATE OR REPLACE FUNCTION updateUUIDArray(values_array UUID[]) RETURNS UUID[] AS $$
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    string orig_value = (string)flatten_values.GetValue(i).ToString();
    Guid new_value = new Guid("aaaaaaaa" + orig_value.Substring(8));

    flatten_values.SetValue((Guid)new_value, i);
}
return flatten_values;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-uuid-null-1array', 'updateUUIDArray1', updateUUIDArray(ARRAY['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID, null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]) = ARRAY['aaaaaaaa-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, 'aaaaaaaa-604e-11ed-9b6a-0242ac120002'::UUID, null::UUID, 'aaaaaaaa-9c0b-4ef8-9b6a-0242ac120002'::UUID];


CREATE OR REPLACE FUNCTION CreateUUIDMultidimensionalArray() RETURNS UUID[] AS $$
Guid objects_value = new Guid("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11");
Guid?[, ,] three_dimensional_array = new Guid?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-uuid-null-3array-arraynull', 'CreateUUIDMultidimensionalArray1', CreateUUIDMultidimensionalArray() = ARRAY[[['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, 'a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID], [null::UUID, null::UUID]], [['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, null::UUID], ['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, 'a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID]]];
