----------------------------------------
-- Tests for dynamic records, including SRF
----------------------------------------

----------------------------------------
-- First, a test of varying record functions
----------------------------------------

-- Function

CREATE OR REPLACE FUNCTION dynamic_record_generator_fsharp(scenario INT4)
RETURNS record
LANGUAGE plfsharp
AS $$

let barbara =
    let param = new NpgsqlParameter()
    param.ParameterName <- "_"
    param.NpgsqlDbType <- NpgsqlDbType.Varchar
    param.Value <- "Barbara"
    param
let s = if scenario.HasValue then scenario.Value else int 0
match s with
| _ when 1 = s -> [| 1; "Alice" |]
| _ when 2 = s -> [| 2; barbara |]
| _ when 3 = s -> [| 1.5; 2.5; true |]
| _ -> failwithf "Unrecognized scenario: %d" s
$$;
-- Tests

WITH cte AS (
    SELECT * FROM dynamic_record_generator_fsharp(1)
        AS (a int4, b text)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-drec', 'drec-comparison-1a', a = 1 from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_fsharp(1)
        AS (a int4, b text)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-drec', 'drec-comparison-1b', b = 'Alice' from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_fsharp(2)
        AS (a int4, b varchar)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-drec', 'drec-comparison-2a', a = 2 from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_fsharp(2)
        AS (a int4, b varchar)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-drec', 'drec-comparison-2b', b = 'Barbara'::VARCHAR from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_fsharp(3)
        AS (a float, b float, c bool)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-drec', 'drec-comparison-3', c = true from cte;

----------------------------------------
-- Second, an SRF that returns records
----------------------------------------

-- Function

CREATE OR REPLACE FUNCTION dynamic_record_generator_srf_fsharp(lim INT8)
RETURNS SETOF record
LANGUAGE plfsharp
AS $$
match lim.HasValue with
| false ->
    seq { for i in 0 .. System.Int32.MaxValue do yield [| box i; $"Number is {i}" |] }
| true ->
    if not (lim.Value > 0) then
        seq { () }
    else
        seq { for i in 0L .. lim.Value - 1L do yield [| box i; $"Number is {i}" |] }
$$;

-- -- Tests

WITH cte AS (
    SELECT * FROM dynamic_record_generator_srf_fsharp(10)
        AS t(a int8, b text)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-drec-srf-sum', 'drec-srf-1a', SUM(a) = 45 from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_srf_fsharp(10)
        AS t(a int8, b text)
        WHERE a = 5
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-drec-srf-sum', 'drec-srf-2b', b = 'Number is 5' from cte;

----------------------------------------
-- Third, various type tests
----------------------------------------

-- Function

CREATE OR REPLACE FUNCTION dynamic_record_generator_types_fsharp(scenario INT4)
RETURNS record
LANGUAGE plfsharp
AS $$

let createParam (dbType: NpgsqlDbType) (value: obj) =
    let param = new NpgsqlParameter("_", dbType)
    param.Value <- value
    param

let s = if scenario.HasValue then scenario.Value else int 0

match s with
| _ when 1 = s -> [| 101; (int16 1) |]
| _ when 2 = s -> [| 102; 2 |]
| _ when 3 = s -> [| 103; (int64 3) |]
| _ when 4 = s -> [| 104; "4" |]
| _ when 5 = s -> [| 105; (createParam NpgsqlDbType.Varchar "five") |]
| _ when 6 = s -> [| 106; (float32 6.0) |]
| _ when 7 = s -> [| 107; 7.0 |]
| _ when 8 = s ->
    let nullInt = createParam NpgsqlDbType.Integer (DBNull.Value :> obj)
    let nullFloat = createParam NpgsqlDbType.Real (DBNull.Value :> obj)
    [| nullInt; nullFloat |]
| _ -> failwith "Unrecognized scenario: %d" scenario
$$;


-- Tests

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types_fsharp(1)
        AS (a int4, b int2)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-drec-types', 'drec-type-1', b = 1 from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types_fsharp(2)
        AS (a int4, b int4)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-drec-types', 'drec-type-2', b = 2 from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types_fsharp(3)
        AS (a int4, b int8)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-drec-types', 'drec-type-3', b = 3 from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types_fsharp(4)
        AS (a int4, b text)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-drec-types', 'drec-type-4', b = '4' from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types_fsharp(5)
        AS (a int4, b varchar)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-drec-types', 'drec-type-5', b = 'five'::varchar from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types_fsharp(6)
        AS (a int4, b float4)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-drec-types', 'drec-type-6a', b > 5.999 from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types_fsharp(6)
        AS (a int4, b float4)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-drec-types', 'drec-type-6b', b < 6.001 from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types_fsharp(7)
        AS (a int4, b float8)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-drec-types', 'drec-type-7a', b > 6.999 from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types_fsharp(7)
        AS (a int4, b float8)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-drec-types', 'drec-type-7b', b < 7.001 from cte;

-- test for null and not-null

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types_fsharp(8)
        AS (a int4, b float4)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-drec-types', 'null-is-present', (a IS NULL AND b IS NULL)
FROM cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types_fsharp(6)
        AS (a int4, b float4)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-drec-types', 'null-is-not-present', (a IS NOT NULL AND b IS NOT NULL)
FROM cte;
