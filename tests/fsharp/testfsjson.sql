-- JSON

CREATE OR REPLACE FUNCTION returnJsonFSharp(a JSON) RETURNS JSON AS $$
    if System.Object.ReferenceEquals(a, null) then "{\"NULL\": \"NULL_NULL_NULL\"}" else a
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-json', 'returnJsonFSharp1', returnJsonFSharp('{"a":"Sunday", "b":"Monday", "c":"Tuesday"}'::JSON)::TEXT = '{"a":"Sunday", "b":"Monday", "c":"Tuesday"}'::JSON::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-json', 'returnJsonFSharp2', returnJsonFSharp('{"a":"Sunday", "c":"Tuesday", "b":"Monday"}'::JSON)::TEXT = '{"a":"Sunday", "c":"Tuesday", "b":"Monday"}'::JSON::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-json', 'returnJsonFSharp3', returnJsonFSharp('{"Sunday":"2022-11-06", "Monday":"2022-11-07", "Tuesday":"2022-11-08"}'::JSON)::TEXT = '{"Sunday":"2022-11-06", "Monday":"2022-11-07", "Tuesday":"2022-11-08"}'::JSON::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-json-null', 'returnJsonFSharp4', returnJsonFSharp(null::JSON)::TEXT = '{"NULL": "NULL_NULL_NULL"}'::JSON::TEXT;

CREATE OR REPLACE FUNCTION modifyJsonFSharp(a JSON, b TEXT, c TEXT) RETURNS JSON AS $$
    let new_value = String.Concat(", \"", b, "\":\"", c, "\"}")
    a.Replace("}", new_value)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-json', 'modifyJsonFSharp1', modifyJsonFSharp('{"a":"Sunday", "b":"Monday"}'::JSON, 'c'::TEXT, 'Tuesday'::TEXT)::TEXT = '{"a":"Sunday", "b":"Monday", "c":"Tuesday"}'::JSON::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-json-null', 'modifyJsonFSharp2', modifyJsonFSharp('{"Sunday":"2022-11-06", "Monday":"2022-11-07"}'::JSON, null::TEXT, null::TEXT)::TEXT = '{"Sunday":"2022-11-06", "Monday":"2022-11-07", "":""}'::JSON::TEXT;

--- JSON Arrays
CREATE OR REPLACE FUNCTION updateJsonArrayIndexFSharp(a JSON[], b JSON) RETURNS JSON[] AS $$
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
SELECT 'f#-json-null-1array', 'updateJsonArrayIndexFSharp1', updateJsonArrayIndexFSharp(ARRAY['{"age": 20, "name": "Mikael"}'::JSON, '{"age": 25, "name": "Rosicley"}'::JSON, null::JSON, '{"age": 30, "name": "Todd"}'::JSON], '{"age": 40, "name": "John Doe"}'::JSON)::TEXT = ARRAY['{"age": 40, "name": "John Doe"}'::JSON, '{"age": 25, "name": "Rosicley"}'::JSON, null::JSON, '{"age": 30, "name": "Todd"}'::JSON]::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-json-null-2array', 'updateJsonArrayIndexFSharp2', updateJsonArrayIndexFSharp(ARRAY[['{"age": 20, "name": "Mikael"}'::JSON], ['{"age": 25, "name": "Rosicley"}'::JSON], [null::JSON], ['{"age": 30, "name": "Todd"}'::JSON]], '{"age": 40, "name": "John Doe"}'::JSON)::TEXT = ARRAY[['{"age": 40, "name": "John Doe"}'::JSON], ['{"age": 25, "name": "Rosicley"}'::JSON], [null::JSON], ['{"age": 30, "name": "Todd"}'::JSON]]::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-json-null-3array', 'updateJsonArrayIndexFSharp3', updateJsonArrayIndexFSharp(ARRAY[[['{"age": 20, "name": "Mikael"}'::JSON], ['{"age": 25, "name": "Rosicley"}'::JSON]], [[null::JSON], ['{"age": 30, "name": "Todd"}'::JSON]]], '{"age": 40, "name": "John Doe"}'::JSON)::TEXT = ARRAY[[['{"age": 40, "name": "John Doe"}'::JSON], ['{"age": 25, "name": "Rosicley"}'::JSON]], [[null::JSON], ['{"age": 30, "name": "Todd"}'::JSON]]]::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-json-null-1array-arraynull', 'updateJsonArrayIndexFSharp4', updateJsonArrayIndexFSharp(ARRAY[null::JSON, null::JSON, null::JSON, '{"age": 30, "name": "Todd"}'::JSON], '{"age": 40, "name": "John Doe"}'::JSON)::TEXT = ARRAY['{"age": 40, "name": "John Doe"}'::JSON, null::JSON, null::JSON, '{"age": 30, "name": "Todd"}'::JSON]::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-json-null-2array-arraynull', 'updateJsonArrayIndexFSharp5', updateJsonArrayIndexFSharp(ARRAY[[null::JSON, null::JSON], [null::JSON, '{"age": 30, "name": "Todd"}'::JSON]], '{"age": 40, "name": "John Doe"}'::JSON)::TEXT = ARRAY[['{"age": 40, "name": "John Doe"}'::JSON, null::JSON], [null::JSON, '{"age": 30, "name": "Todd"}'::JSON]]::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-json-null-3array-arraynull', 'updateJsonArrayIndexFSharp6', updateJsonArrayIndexFSharp(ARRAY[[[null::JSON, null::JSON], [null::JSON, '{"age": 30, "name": "Todd"}'::JSON]]], '{"age": 40, "name": "John Doe"}'::JSON)::TEXT = ARRAY[[['{"age": 40, "name": "John Doe"}'::JSON, null::JSON], [null::JSON, '{"age": 30, "name": "Todd"}'::JSON]]]::TEXT;

CREATE OR REPLACE FUNCTION GetJsonMultidimensionArrayFSharp() RETURNS JSON[] AS $$
let objects_value = "{\"type\": \"json\", \"action\": \"multidimensional test\"}"
let arr = Array.CreateInstance(typeof<string>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-json-null-3array-arraynull', 'GetJsonMultidimensionArrayFSharp1', GetJsonMultidimensionArrayFSharp()::TEXT = ARRAY[[['{"type": "json", "action": "multidimensional test"}'::JSON]]]::TEXT;
