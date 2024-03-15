-- BYTEA

CREATE OR REPLACE FUNCTION byteaConversions(a BYTEA, b BYTEA) RETURNS BYTEA AS $$
    UTF8Encoding utf8_e = new UTF8Encoding();
    if (a == null && b == null)
        return null;
    if (a == null)
        return b;
    if (b == null)
        return a;

    string s1 = utf8_e.GetString(a, 0, a.Length);
    string s2 = utf8_e.GetString(b, 0, b.Length);
    string result = s1 + " " + s2;
    return utf8_e.GetBytes(result);
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-bytea', 'byteaConversions1', byteaConversions('Brick Abode is nice!'::BYTEA, 'Thank you very much...'::BYTEA) = '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-bytea-null', 'byteaConversions2', byteaConversions(NULL::BYTEA, 'Thank you very much...'::BYTEA) = 'Thank you very much...'::BYTEA;

CREATE OR REPLACE FUNCTION concatenateBytea(a BYTEA, b TEXT) RETURNS BYTEA AS $$
    UTF8Encoding utf8_e = new UTF8Encoding();
    byte[] b_bytes = new byte[b.Length];
    utf8_e.GetBytes(b, 0, b.Length, b_bytes, 0);
    byte[] c = new byte[a.Length + b_bytes.Length];
    a.CopyTo(c, 0);
    b_bytes.CopyTo(c, a.Length);
    return c;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-bytea', 'concatenateBytea', concatenateBytea('\x427269636b2041626f6465206973206e69636521'::BYTEA, ' Thank you very much...'::TEXT) = '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA;

--- BYTEA Arrays

CREATE OR REPLACE FUNCTION updateByteaArrayIndex(values_array BYTEA[], desired BYTEA, index integer[]) RETURNS BYTEA[] AS $$
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-bytea-null-1array', 'updateByteaArrayIndex1', updateByteaArrayIndex(ARRAY['Brick Abode is nice!'::BYTEA, 'Test 1!'::BYTEA, null::BYTEA, '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA], 'Inserted BYTEA'::BYTEA, ARRAY[2]) = ARRAY['Brick Abode is nice!'::BYTEA, 'Test 1!'::BYTEA, 'Inserted BYTEA'::BYTEA, '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-bytea-null-2array', 'updateByteaArrayIndex2', updateByteaArrayIndex(ARRAY[['Brick Abode is nice!'::BYTEA, 'Test 1!'::BYTEA], [null::BYTEA, '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]], 'Inserted BYTEA'::BYTEA, ARRAY[1,0]) = ARRAY[['Brick Abode is nice!'::BYTEA, 'Test 1!'::BYTEA], ['Inserted BYTEA'::BYTEA, '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-bytea-null-2array-arraynull', 'updateByteaArrayIndex3', updateByteaArrayIndex(ARRAY[[null::BYTEA, null::BYTEA], [null::BYTEA, '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]], 'Inserted BYTEA'::BYTEA, ARRAY[1,0]) = ARRAY[[null::BYTEA, null::BYTEA], ['Inserted BYTEA'::BYTEA, '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]];

CREATE OR REPLACE FUNCTION ConvertByteaArray(values_array BYTEA[]) RETURNS BYTEA[] AS $$
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
UTF8Encoding utf8_e = new UTF8Encoding();
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    byte[] orig_value = (byte[])flatten_values.GetValue(i);
    string s1 = utf8_e.GetString(orig_value, 0, orig_value.Length);
    byte[] new_value = utf8_e.GetBytes(s1);

    flatten_values.SetValue((byte[])new_value, i);
}
return flatten_values;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-bytea-null-1array', 'ConvertByteaArray1', ConvertByteaArray(ARRAY['Brick Abode is nice!'::BYTEA, 'Test 1!'::BYTEA, null::BYTEA, '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]) = ARRAY['\x427269636b2041626f6465206973206e69636521'::BYTEA, '\x54657374203121'::BYTEA, null::BYTEA, '\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA];

CREATE OR REPLACE FUNCTION CreateByteaMultidimensionalArray() RETURNS BYTEA[] AS $$
byte[] objects_value = new byte[] { 0x92, 0x83, 0x74, 0x65, 0x56, 0x47, 0x38 };
byte[]?[, ,] three_dimensional_array = new byte[]?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-bytea-null-3array-arraynull', 'CreateByteaMultidimensionalArray1', CreateByteaMultidimensionalArray() = ARRAY[[['\x92837465564738'::BYTEA, '\x92837465564738'::BYTEA], [null::BYTEA, null::BYTEA]], [['\x92837465564738'::BYTEA, null::BYTEA], ['\x92837465564738'::BYTEA, '\x92837465564738'::BYTEA]]];
