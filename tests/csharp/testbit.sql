-- BIT

CREATE OR REPLACE FUNCTION nulltest() RETURNS int4 AS $$
    return null;
$$ LANGUAGE plcsharp;

CREATE OR REPLACE FUNCTION modifybit(a BIT(10)) RETURNS BIT(10) AS $$
    if (a == null)
        return null;

    a[0] = a[0] ? false : true;
    a[a.Length-1] = a[a.Length-1] ? false : true;
    return a;
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-bit', 'modifybit1', modifybit('10101'::BIT(10)) = '0010100001'::BIT(10);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-bit-null', 'modifybit2', modifybit(NULL::BIT(10)) IS NULL;

-- VARBIT

CREATE OR REPLACE FUNCTION modifyvarbit(a BIT VARYING) RETURNS BIT VARYING AS $$
    if (a == null)
        return null;

    a[0] = a[0] ? false : true;
    a[a.Length-1] = a[a.Length-1] ? false : true;
    return a;
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-varbit', 'modifyvarbit1', modifyvarbit('1001110001000'::BIT VARYING) = '0001110001001'::BIT VARYING;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-varbit-null', 'modifyvarbit2', modifyvarbit(NULL::BIT VARYING) IS NULL;

CREATE OR REPLACE FUNCTION concatenatevarbit(a BIT VARYING, b BIT VARYING) RETURNS BIT VARYING AS $$
    BitArray c = new BitArray(a.Length+b.Length);
    for(int i = 0; i < a.Length;i++)
        c[i] = a[i];
    for(int i = 0, cont = a.Length; i < b.Length;i++)
        c[cont++] = b[i];
    return c;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-varbit', 'concatenatevarbit1', concatenatevarbit('1001110001000'::BIT VARYING, '111010111101111000'::BIT VARYING) = '1001110001000111010111101111000'::BIT VARYING;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-varbit', 'concatenatevarbit2', concatenatevarbit('1001110001000'::BIT(10), '111010111101111000'::BIT VARYING) = '1001110001111010111101111000'::BIT VARYING;

--- BIT Arrays

CREATE OR REPLACE FUNCTION updateBitArrayIndex(values_array BIT(8)[], desired BIT(8), index integer[]) RETURNS BIT(8)[] AS $$
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-bit-null-1array', 'updateBitArrayIndex1', updateBitArrayIndex(ARRAY['10101001'::BIT(8), '10101101'::BIT(8), null::BIT(8), '11101001'::BIT(8)], '11111111'::BIT(8), ARRAY[2]) = ARRAY['10101001'::BIT(8), '10101101'::BIT(8), '11111111'::BIT(8), '11101001'::BIT(8)];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-bit-null-2array', 'updateBitArrayIndex2', updateBitArrayIndex(ARRAY[['10101001'::BIT(8), '10101101'::BIT(8)], [null::BIT(8), '11101001'::BIT(8)]], '11111111'::BIT(8), ARRAY[1,0]) = ARRAY[['10101001'::BIT(8), '10101101'::BIT(8)], ['11111111'::BIT(8), '11101001'::BIT(8)]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-bit-null-2array-arraynull', 'updateBitArrayIndex3', updateBitArrayIndex(ARRAY[[null::BIT(8), null::BIT(8)], [null::BIT(8), '11101001'::BIT(8)]], '11111111'::BIT(8), ARRAY[1,0]) = ARRAY[[null::BIT(8), null::BIT(8)], ['11111111'::BIT(8), '11101001'::BIT(8)]];

CREATE OR REPLACE FUNCTION ToggleFirstBits(values_array BIT(8)[]) RETURNS BIT(8)[] AS $$
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    BitArray orig_value = (BitArray)flatten_values.GetValue(i);
    BitArray new_value = orig_value;
    new_value[0] = new_value[0] ? false : true;

    flatten_values.SetValue((BitArray)new_value, i);
}
return flatten_values;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-bit-null-1array', 'ToggleFirstBits1', ToggleFirstBits(ARRAY['10101001'::BIT(8), '10101101'::BIT(8), null::BIT(8), '01101001'::BIT(8)]) = ARRAY['00101001'::BIT(8), '00101101'::BIT(8), null::BIT(8), '11101001'::BIT(8)];


CREATE OR REPLACE FUNCTION CreateBitMultidimensionalArray() RETURNS BIT(8)[] AS $$
BitArray objects_value = new BitArray(new bool[8]{true, false, true, false, true, true, false, false});
BitArray?[, ,] three_dimensional_array = new BitArray?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-bit-null-3array-arraynull', 'CreateBitMultidimensionalArray1', CreateBitMultidimensionalArray() = ARRAY[[['10101100'::BIT(8), '10101100'::BIT(8)], [null::BIT(8), null::BIT(8)]], [['10101100'::BIT(8), null::BIT(8)], ['10101100'::BIT(8), '10101100'::BIT(8)]]];

--- VARBIT Arrays

CREATE OR REPLACE FUNCTION updateVarbitArrayIndex(values_array BIT VARYING[], desired BIT VARYING, index integer[]) RETURNS BIT VARYING[] AS $$
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-varbit-null-1array', 'updateVarbitArrayIndex1', updateVarbitArrayIndex(ARRAY['1010101101101'::BIT VARYING, '101011101'::BIT VARYING, null::BIT VARYING, '101001'::BIT VARYING], '1111111001111'::BIT VARYING, ARRAY[2]) = ARRAY['1010101101101'::BIT VARYING, '101011101'::BIT VARYING, '1111111001111'::BIT VARYING, '101001'::BIT VARYING];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-varbit-null-2array', 'updateVarbitArrayIndex2', updateVarbitArrayIndex(ARRAY[['1010101101101'::BIT VARYING, '101011101'::BIT VARYING], [null::BIT VARYING, '101001'::BIT VARYING]], '1111111001111'::BIT VARYING, ARRAY[1, 0]) = ARRAY[['1010101101101'::BIT VARYING, '101011101'::BIT VARYING], ['1111111001111'::BIT VARYING, '101001'::BIT VARYING]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-varbit-null-2array-arraynull', 'updateVarbitArrayIndex3', updateVarbitArrayIndex(ARRAY[[null::BIT VARYING, null::BIT VARYING], [null::BIT VARYING, '101001'::BIT VARYING]], '1111111001111'::BIT VARYING, ARRAY[1, 0]) = ARRAY[[null::BIT VARYING, null::BIT VARYING], ['1111111001111'::BIT VARYING, '101001'::BIT VARYING]];

CREATE OR REPLACE FUNCTION ToggleFirstVarbits(values_array BIT VARYING[]) RETURNS BIT VARYING[] AS $$
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    BitArray orig_value = (BitArray)flatten_values.GetValue(i);
    BitArray new_value = orig_value;
    new_value[0] = new_value[0] ? false : true;

    flatten_values.SetValue((BitArray)new_value, i);
}
return flatten_values;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-varbit-null-1array', 'ToggleFirstVarbits1', ToggleFirstVarbits(ARRAY['1010101101101'::BIT VARYING, '101011101'::BIT VARYING, null::BIT VARYING, '001001'::BIT VARYING]) = ARRAY['0010101101101'::BIT VARYING, '001011101'::BIT VARYING, null::BIT VARYING, '101001'::BIT VARYING];


CREATE OR REPLACE FUNCTION CreateVarbitMultidimensionalArray() RETURNS BIT VARYING[] AS $$
BitArray objects_value = new BitArray(new bool[8]{true, false, true, false, true, true, false, false});
BitArray?[, ,] three_dimensional_array = new BitArray?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-varbit-null-3array-arraynull', 'CreateVarbitMultidimensionalArray1', CreateVarbitMultidimensionalArray() = ARRAY[[['10101100'::BIT VARYING, '10101100'::BIT VARYING], [null::BIT VARYING, null::BIT VARYING]], [['10101100'::BIT VARYING, null::BIT VARYING], ['10101100'::BIT VARYING, '10101100'::BIT VARYING]]];
