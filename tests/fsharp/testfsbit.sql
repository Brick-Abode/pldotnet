-- BIT

CREATE OR REPLACE FUNCTION modifybitfsharp(a BIT(10)) RETURNS BIT(10) AS $$
#line 1
    if System.Object.ReferenceEquals(a, null) then
        null
    else
        let first = a.Get(0)
        let last = a.Get(a.Length - 1)
        let result = a
        for i in 0 .. a.Length - 1 do
            result.[i] <- a.Get(i)
        result.[0] <- not first
        result.[a.Length - 1] <- not last
        new BitArray(result)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bit', 'modifybitfsharp1', modifybitfsharp('10101'::BIT(10)) = '0010100001'::BIT(10);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bit-null', 'modifybitfsharp2', modifybitfsharp(NULL::BIT(10)) IS NULL;

-- VARBIT

CREATE OR REPLACE FUNCTION modifyvarbitfsharp(a BIT VARYING) RETURNS BIT VARYING AS $$
    if System.Object.ReferenceEquals(a, null) then
        null
    else
        let result = a
        result.[0] <- if a.[0] = false then true else false
        result.[a.Length - 1] <- if a.[a.Length - 1] = false then true else false
        result
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-varbit', 'modifyvarbitfsharp1', modifyvarbitfsharp('1001110001000'::BIT VARYING) = '0001110001001'::BIT VARYING;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-varbit-null', 'modifyvarbitfsharp2', modifyvarbitfsharp(NULL::BIT VARYING) IS NULL;

CREATE OR REPLACE FUNCTION concatenatevarbitfsharp (a BIT VARYING, b BIT VARYING) RETURNS BIT VARYING AS $$
    let result = new BitArray(a.Length + b.Length)
    for i in 0 .. a.Length - 1 do
        result.[i] <- a.[i]
    for i in a.Length .. result.Length - 1 do
        result.[i] <- b.[i - a.Length]
    result
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-varbit', 'concatenatevarbitfsharp1', concatenatevarbitfsharp('1001110001000'::BIT VARYING, '111010111101111000'::BIT VARYING) = '1001110001000111010111101111000'::BIT VARYING;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-varbit', 'concatenatevarbitfsharp2', concatenatevarbitfsharp('1001110001000'::BIT(10), '111010111101111000'::BIT VARYING) = '1001110001111010111101111000'::BIT VARYING;

-- --- BIT Arrays

CREATE OR REPLACE FUNCTION updateBitArrayIndexFSharp(a BIT(8)[], b BIT(8)) RETURNS BIT(8)[] AS $$
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
$$ LANGUAGE plfsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bit-null-1array', 'updateBitArrayIndexFSharp1', updateBitArrayIndexFSharp(ARRAY['10101001'::BIT(8), '10101101'::BIT(8), null::BIT(8), '11101001'::BIT(8)], '11111111'::BIT(8)) = ARRAY['11111111'::BIT(8), '10101101'::BIT(8), null::BIT(8), '11101001'::BIT(8)];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bit-null-2array', 'updateBitArrayIndexFSharp2', updateBitArrayIndexFSharp(ARRAY[['10101001'::BIT(8), '10101101'::BIT(8)], [null::BIT(8), '11101001'::BIT(8)]], '11111111'::BIT(8)) = ARRAY[['11111111'::BIT(8), '10101101'::BIT(8)], [null::BIT(8), '11101001'::BIT(8)]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bit-null-3array', 'updateBitArrayIndexFSharp3', updateBitArrayIndexFSharp(ARRAY[[['10101001'::BIT(8), '10101101'::BIT(8)], [null::BIT(8), '11101001'::BIT(8)]]], '11111111'::BIT(8)) = ARRAY[[['11111111'::BIT(8), '10101101'::BIT(8)], [null::BIT(8), '11101001'::BIT(8)]]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bit-null-2array-arraynull', 'updateBitArrayIndexFSharp4', updateBitArrayIndexFSharp(ARRAY[[null::BIT(8), null::BIT(8)], [null::BIT(8), '11101001'::BIT(8)]], '11111111'::BIT(8)) = ARRAY[['11111111'::BIT(8), null::BIT(8)], [null::BIT(8), '11101001'::BIT(8)]];

CREATE OR REPLACE FUNCTION ToggleFirstBitsFSharp(values_array BIT(8)[]) RETURNS BIT(8)[] AS $$
    for i in 0 .. values_array.Length - 1 do
        match values_array.GetValue(i) with
        | :? BitArray as orig_value ->
            let new_value = new BitArray(orig_value)
            new_value.[0] <- not new_value.[0]
            values_array.SetValue(new_value, i)
        | _ -> ()
    values_array
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bit-null-1array', 'ToggleFirstBitsFSharp1', ToggleFirstBitsFSharp(ARRAY['10101001'::BIT(8), '10101101'::BIT(8), null::BIT(8), '01101001'::BIT(8)]) = ARRAY['00101001'::BIT(8), '00101101'::BIT(8), null::BIT(8), '11101001'::BIT(8)];

-- --- VARBIT Arrays

CREATE OR REPLACE FUNCTION updateVarbitArrayIndexFSharp(a BIT VARYING[], b BIT VARYING) RETURNS BIT VARYING[] AS $$
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
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-varbit-null-1array', 'updateVarbitArrayIndexFSharp1', updateVarbitArrayIndexFSharp(ARRAY['1010101101101'::BIT VARYING, '101011101'::BIT VARYING, null::BIT VARYING, '101001'::BIT VARYING], '1111111001111'::BIT VARYING) = ARRAY['1111111001111'::BIT VARYING, '101011101'::BIT VARYING, null::BIT VARYING, '101001'::BIT VARYING];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-varbit-null-2array', 'updateVarbitArrayIndexFSharp2', updateVarbitArrayIndexFSharp(ARRAY[['1010101101101'::BIT VARYING, '101011101'::BIT VARYING], [null::BIT VARYING, '101001'::BIT VARYING]], '1111111001111'::BIT VARYING) = ARRAY[['1111111001111'::BIT VARYING, '101011101'::BIT VARYING], [null::BIT VARYING, '101001'::BIT VARYING]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-varbit-null-3array', 'updateVarbitArrayIndexFSharp3', updateVarbitArrayIndexFSharp(ARRAY[[['1010101101101'::BIT VARYING, '101011101'::BIT VARYING], [null::BIT VARYING, '101001'::BIT VARYING]]], '1111111001111'::BIT VARYING) = ARRAY[[['1111111001111'::BIT VARYING, '101011101'::BIT VARYING], [null::BIT VARYING, '101001'::BIT VARYING]]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-varbit-null-2array-arraynull', 'updateVarbitArrayIndexFSharp4', updateVarbitArrayIndexFSharp(ARRAY[[null::BIT VARYING, null::BIT VARYING], [null::BIT VARYING, '101001'::BIT VARYING]], '1111111001111'::BIT VARYING) = ARRAY[['1111111001111'::BIT VARYING, null::BIT VARYING], [null::BIT VARYING, '101001'::BIT VARYING]];

CREATE OR REPLACE FUNCTION ToggleFirstVarbitsFSharp(values_array BIT VARYING[]) RETURNS BIT VARYING[] AS $$
    for i in 0 .. values_array.Length - 1 do
        match values_array.GetValue(i) with
        | :? BitArray as orig_value ->
            let new_value = new BitArray(orig_value)
            new_value.[0] <- not new_value.[0]
            values_array.SetValue(new_value, i)
        | _ -> ()
    values_array
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-varbit-null-1array', 'ToggleFirstVarbitsFSharp1', ToggleFirstVarbitsFSharp(ARRAY['1010101101101'::BIT VARYING, '101011101'::BIT VARYING, null::BIT VARYING, '001001'::BIT VARYING]) = ARRAY['0010101101101'::BIT VARYING, '001011101'::BIT VARYING, null::BIT VARYING, '101001'::BIT VARYING];

