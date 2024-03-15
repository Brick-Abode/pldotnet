BEGIN;

----------------------------------------------------------------------
-- First, we test simple use cases for INOUT and OUT
----------------------------------------------------------------------

CREATE OR REPLACE FUNCTION fs_inout_allthreeS(IN a INT, INOUT b INT, OUT c INT) AS $$
    Nullable(b+1), Nullable(a+b)
$$ LANGUAGE plfsharp STRICT;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inout-allthree-S', 'fs_inout_allthreeS', fs_inout_allthreeS(3, 8)
= ROW(9, 11);

----------------------------------------------------------------------
-- Second, we test NULL for INOUT and OUT
----------------------------------------------------------------------

CREATE OR REPLACE FUNCTION fs_inout_allthree(IN a INT, INOUT b INT, OUT c INT) AS $$
    if a.HasValue && b.HasValue then
        Nullable(a.Value + 1), Nullable(a.Value + b.Value)
    else
        Nullable(), Nullable()
$$ LANGUAGE plfsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inout-allthree-1', 'fs_inout_allthree', fs_inout_allthree(11, 8)
= ROW(12,19);

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inout-allthree-2', 'fs_inout_allthree', fs_inout_allthree(NULL::int, 8)
= ROW(NULL::int, NULL::int);

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inout-allthree-3', 'fs_inout_allthree', fs_inout_allthree(8, NULL::int)
= ROW(NULL::int, NULL::int);

CREATE OR REPLACE FUNCTION fs_inout_allthreearray(IN a INT, INOUT b INT, OUT c int2[]) AS $$
    let c = b + 1
    let arr = Array.CreateInstance(typeof<int16>, 3, 3)
    arr.SetValue((int16)a, 0, 0)
    arr.SetValue((int16)a, 1, 1)
    arr.SetValue((int16)a, 2, 2)
    Nullable(c), arr
$$ LANGUAGE plfsharp STRICT;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inout-allthreearray', 'fs_inout_allthreearray', fs_inout_allthreearray(3, 8)
= ROW(9, ARRAY[[3::int2,0::int2,0::int2], [0::int2, 3::int2, 0::int2], [0::int2, 0::int2, 3::int2]]);

----------------------------------------------------------------------
-- Third, we test large argument sets, with first arguments being
-- each of IN/INOUT/OUT
----------------------------------------------------------------------

CREATE OR REPLACE FUNCTION inout_multiarg_1_fsS(IN a0 INT, INOUT a1 INT, IN a2 INT, OUT a3 INT, OUT a4 INT, INOUT a5 INT, IN a6 INT, OUT a7 INT) AS $$
    if a0 <> 0 then
        raise <| SystemException("Failed assertion: a0")
    if  a1 <> 1 then
        raise <| SystemException("Failed assertion: a1")
    if  a2 <> 2 then
        raise <| SystemException("Failed assertion: a2")
    if a5 <> 5 then
        raise <| SystemException("Failed assertion: a5")
    if  a6 <> 6 then
        raise <| SystemException("Failed assertion: a6")

    (Nullable(2), Nullable(4), Nullable(5), Nullable(6), Nullable(7))
$$ LANGUAGE plfsharp STRICT;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inout-multiarg-1-S', 'inout_multiarg_1_fsS', inout_multiarg_1_fsS(0, 1, 2, 5, 6) = ROW(2, 4, 5, 6, 7);

CREATE OR REPLACE FUNCTION inout_multiarg_1_fs(IN a0 INT, INOUT a1 INT, IN a2 INT, OUT a3 INT, OUT a4 INT, INOUT a5 INT, IN a6 INT, OUT a7 INT) AS $$
    if a0.HasValue && a0.Value <> 0 then
        raise <| SystemException("Failed assertion: a0")
    if a1.HasValue && a1.Value <> 1 then
        raise <| SystemException("Failed assertion: a1")
    if a2.HasValue && a2.Value <> 2 then
        raise <| SystemException("Failed assertion: a2")
    if a5.HasValue then
        raise <| SystemException("Failed assertion: a5")
    if a6.HasValue && a6.Value <> 6 then
        raise <| SystemException("Failed assertion: a6")

    (Nullable(2), Nullable(4), Nullable(5), Nullable(6), Nullable())
$$ LANGUAGE plfsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inout-multiarg-1', 'inout_multiarg_1_fs', inout_multiarg_1_fs(0, 1, 2, NULL, 6) = ROW(2, 4, 5, 6, NULL::INT);

----------------------------------------------------------------------
-- Fourth, we test ARRAY (with NULL) for INOUT, OUT
----------------------------------------------------------------------

CREATE OR REPLACE FUNCTION inout_array_10_fsS(OUT output_array MACADDR[], IN input_array MACADDR[]) AS $$
let count = input_array.Length
let output = Array.CreateInstance(typeof<obj>, count)
for i = 0 to count - 1 do
    output.SetValue(input_array.GetValue(i), i)
if count > 3 then
    output.SetValue(Nullable() :> obj, 3)
output
$$ LANGUAGE plfsharp STRICT;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inout-array-10', 'inout_array_10_fsS', inout_array_10_fsS( ARRAY[ MACADDR '08-00-2b-01-02-03', NULL, MACADDR '08-00-2b-01-02-03' ])
        = ARRAY[
            MACADDR '08-00-2b-01-02-03',
            NULL,
            MACADDR '08-00-2b-01-02-03'
        ];

CREATE OR REPLACE FUNCTION inout_array_11_fsS(OUT values_array MACADDR[], IN address MACADDR, IN count INT) AS $$
let output = Array.CreateInstance(typeof<obj>, count)
for i = 0 to count - 1 do
    output.SetValue(address :> obj, i)
if count > 3 then
    output.SetValue(Nullable() :> obj, 3)
output
$$ LANGUAGE plfsharp STRICT;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inout-array-S-11', 'inout_array_11_fsS', inout_array_11_fsS(MACADDR '08-00-2b-01-02-03', 3)
        = ARRAY[
            MACADDR '08-00-2b-01-02-03',
            MACADDR '08-00-2b-01-02-03',
            MACADDR '08-00-2b-01-02-03'
        ];

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inout-array-12', 'inout_array_11_fsS', inout_array_11_fsS(MACADDR '08-00-2b-01-02-03', 5)
        = ARRAY[
            MACADDR '08-00-2b-01-02-03',
            MACADDR '08-00-2b-01-02-03',
            MACADDR '08-00-2b-01-02-03',
            NULL,
            MACADDR '08-00-2b-01-02-03'
        ];

----------------------------------------------------------------------
-- Fifth, we test non-trivial ObjectTypeHandler, here StringHandler
----------------------------------------------------------------------

CREATE OR REPLACE FUNCTION inout_object_10_fs(IN a text, INOUT b text) AS $$
    a + " " + b;
$$ LANGUAGE plfsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inout-object-10', 'inout_object_10_fs', inout_object_10_fs('red', 'blue') = 'red blue';

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inout-object-11', 'inout_object_10_fs', inout_object_10_fs('red', NULL) = 'red ';

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inout-object-12', 'inout_object_10_fs', inout_object_10_fs(NULL, 'blue') = ' blue';

CREATE OR REPLACE FUNCTION inout_object_20_fs(IN a text, b text, OUT c text) AS $$
    a + " " + b;
$$ LANGUAGE plfsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inout-object-20', 'inout_object_20_fs', inout_object_20_fs('red', 'blue') = 'red blue';

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inout-object-21', 'inout_object_20_fs', inout_object_20_fs('red', NULL) = 'red ';

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inout-object-22', 'inout_object_20_fs', inout_object_20_fs(NULL, 'blue') = ' blue';

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inout-object-23', 'inout_object_20_fs', inout_object_20_fs('🐂', '🥰') = '🐂 🥰'::TEXT;

COMMIT;
