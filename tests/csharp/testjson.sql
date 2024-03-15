-- JSON

CREATE OR REPLACE FUNCTION returnJson(a JSON) RETURNS JSON AS $$
    if(a==null)
        return "{\"NULL\": \"NULL_NULL_NULL\"}";
    return a;
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-json', 'returnJson1', returnJson('{"a":"Sunday", "b":"Monday", "c":"Tuesday"}'::JSON)::TEXT = '{"a":"Sunday", "b":"Monday", "c":"Tuesday"}'::JSON::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-json', 'returnJson2', returnJson('{"a":"Sunday", "c":"Tuesday", "b":"Monday"}'::JSON)::TEXT = '{"a":"Sunday", "c":"Tuesday", "b":"Monday"}'::JSON::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-json', 'returnJson3', returnJson('{"Sunday":"2022-11-06", "Monday":"2022-11-07", "Tuesday":"2022-11-08"}'::JSON)::TEXT = '{"Sunday":"2022-11-06", "Monday":"2022-11-07", "Tuesday":"2022-11-08"}'::JSON::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-json-null', 'returnJson4', returnJson(null::JSON)::TEXT = '{"NULL": "NULL_NULL_NULL"}'::JSON::TEXT;

CREATE OR REPLACE FUNCTION modifyJson(a JSON, b TEXT, c TEXT) RETURNS JSON AS $$
    string new_value = $", \"{b}\":\"{c}\""+"}";
    return a.Replace("}", new_value);
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-json', 'modifyJson1', modifyJson('{"a":"Sunday", "b":"Monday"}'::JSON, 'c'::TEXT, 'Tuesday'::TEXT)::TEXT = '{"a":"Sunday", "b":"Monday", "c":"Tuesday"}'::JSON::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-json-null', 'modifyJson2', modifyJson('{"Sunday":"2022-11-06", "Monday":"2022-11-07"}'::JSON, null::TEXT, null::TEXT)::TEXT = '{"Sunday":"2022-11-06", "Monday":"2022-11-07", "":""}'::JSON::TEXT;

--- JSON Arrays
CREATE OR REPLACE FUNCTION updateJsonArrayIndex(values_array JSON[], desired JSON, index integer[]) RETURNS JSON[] AS $$
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-json-null-1array', 'updateJsonArrayIndex1', updateJsonArrayIndex(ARRAY['{"age": 20, "name": "Mikael"}'::JSON, '{"age": 25, "name": "Rosicley"}'::JSON, null::JSON, '{"age": 30, "name": "Todd"}'::JSON], '{"age": 40, "name": "John Doe"}'::JSON, ARRAY[2])::TEXT = ARRAY['{"age": 20, "name": "Mikael"}'::JSON, '{"age": 25, "name": "Rosicley"}'::JSON, '{"age": 40, "name": "John Doe"}'::JSON, '{"age": 30, "name": "Todd"}'::JSON]::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-json-null-2array-arraynull', 'updateJsonArrayIndex2', updateJsonArrayIndex(ARRAY[[null::JSON, null::JSON], [null::JSON, '{"age": 30, "name": "Todd"}'::JSON]], '{"age": 40, "name": "John Doe"}'::JSON, ARRAY[1,0])::TEXT = ARRAY[[null::JSON, null::JSON], ['{"age": 40, "name": "John Doe"}'::JSON, '{"age": 30, "name": "Todd"}'::JSON]]::TEXT;

CREATE OR REPLACE FUNCTION ReplaceJsonsKey(values_array JSON[]) RETURNS JSON[] AS $$
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    string orig_value = (string)flatten_values.GetValue(i);
    string new_value = orig_value.Replace("name", "first_name");

    flatten_values.SetValue((string)new_value, i);
}
return flatten_values;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-json-null-1array', 'ReplaceJsonsKey1', ReplaceJsonsKey(ARRAY['{"age": 20, "name": "Mikael"}'::JSON, '{"age": 25, "name": "Rosicley"}'::JSON, null::JSON, '{"age": 30, "name": "Todd"}'::JSON])::TEXT = ARRAY['{"age": 20, "first_name": "Mikael"}'::JSON, '{"age": 25, "first_name": "Rosicley"}'::JSON, null::JSON, '{"age": 30, "first_name": "Todd"}'::JSON]::TEXT;

CREATE OR REPLACE FUNCTION GetJsonMultidimensionArray() RETURNS JSON[] AS $$
string objects_value = "{\"type\": \"json\", \"action\": \"multidimensional test\"}";
string?[, ,] three_dimensional_array = new string?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-json-null-3array-arraynull', 'GetJsonMultidimensionArray1', GetJsonMultidimensionArray()::TEXT = ARRAY[[['{"type": "json", "action": "multidimensional test"}'::JSON, '{"type": "json", "action": "multidimensional test"}'::JSON], [null::JSON, null::JSON]], [['{"type": "json", "action": "multidimensional test"}'::JSON, null::JSON], ['{"type": "json", "action": "multidimensional test"}'::JSON, '{"type": "json", "action": "multidimensional test"}'::JSON]]]::TEXT;
