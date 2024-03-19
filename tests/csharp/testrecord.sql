----------------------------------------
-- Tests for dynamic records, including SRF
----------------------------------------

----------------------------------------
-- First, a test of varying record functions
----------------------------------------

-- Function

CREATE OR REPLACE FUNCTION dynamic_record_generator(scenario INT4)
RETURNS record
LANGUAGE plcsharp
AS $$

    switch(scenario)
    {
        case 1:
            return new object[]{1, "Alice"};
        case 2:
            var barbara = new NpgsqlParameter
                    {
                        ParameterName = "_",
                        NpgsqlDbType = NpgsqlDbType.Varchar,
                        Value = "Barbara"
                    };
            return new object[]{2, barbara};
        case 3:
           return new object[]{1.5, 2.5, true};
        default:
            throw new SystemException($"Unrecognized scenario: {scenario}");
    }
$$;

-- Tests

WITH cte AS (
    SELECT * FROM dynamic_record_generator(1)
        AS (a int4, b text)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-drec', 'drec-comparison-1a', a = 1 from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator(1)
        AS (a int4, b text)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-drec', 'drec-comparison-1b', b = 'Alice' from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator(2)
        AS (a int4, b varchar)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-drec', 'drec-comparison-2a', a = 2 from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator(2)
        AS (a int4, b varchar)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-drec', 'drec-comparison-2b', b = 'Barbara'::VARCHAR from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator(3)
        AS (a float, b float, c bool)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-drec', 'drec-comparison-3', c = true from cte;

----------------------------------------
-- Second, an SRF that returns records
----------------------------------------

-- Function

CREATE OR REPLACE FUNCTION dynamic_record_generator_srf(lim INT8)
RETURNS SETOF record
LANGUAGE plcsharp
AS $$
    if (!(lim > 0)){ yield break; }
    for(long i=0;i<lim;i++){ yield return new object?[] { (long)i, $"Number is {i}" }; }
$$;

-- Tests

WITH cte AS (
    SELECT * FROM dynamic_record_generator_srf(10)
        AS t(a int8, b text)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-drec-srf-sum', 'drec-srf-1a', SUM(a) = 45 from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_srf(10)
        AS t(a int8, b text)
        WHERE a = 5
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-drec-srf-sum', 'drec-srf-2b', b = 'Number is 5' from cte;

----------------------------------------
-- Third, various type tests
----------------------------------------

-- Function

CREATE OR REPLACE FUNCTION dynamic_record_generator_types(scenario INT4)
RETURNS record
LANGUAGE plcsharp
AS $$
    switch(scenario)
    {
        case 1: // short
            return new object[]{101, (short)1};
        case 2: // int
            return new object[]{102, (int)2};
        case 3: // long
            return new object[]{103, (long)3};
        case 4: // string
            return new object[]{104, "4"};
        case 5: // varchar
            var five = new NpgsqlParameter
                    {
                        ParameterName = "_",
                        NpgsqlDbType = NpgsqlDbType.Varchar,
                        Value = "five"
                    };
            return new object[]{105, five};
        case 6: // float
            return new object[]{101, (float)6.0};
        case 7: // double
            return new object[]{101, (double)7.0};
        case 8:
            var nullInt = new NpgsqlParameter
            {
                ParameterName = "_",
                NpgsqlDbType = NpgsqlDbType.Integer
            };
            var nullFloat = new NpgsqlParameter
            {
                ParameterName = "_",
                NpgsqlDbType = NpgsqlDbType.Real
            };
            return new object[]{nullInt, nullFloat};
        default:
            throw new SystemException($"Unrecognized scenario: {scenario}");
    }
$$;

-- Tests

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types(1)
        AS (a int4, b int2)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-drec-types', 'drec-type-1', b = 1 from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types(2)
        AS (a int4, b int4)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-drec-types', 'drec-type-2', b = 2 from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types(3)
        AS (a int4, b int8)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-drec-types', 'drec-type-3', b = 3 from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types(4)
        AS (a int4, b text)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-drec-types', 'drec-type-4', b = '4' from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types(5)
        AS (a int4, b varchar)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-drec-types', 'drec-type-5', b = 'five'::varchar from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types(6)
        AS (a int4, b float4)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-drec-types', 'drec-type-6a', b > 5.999 from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types(6)
        AS (a int4, b float4)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-drec-types', 'drec-type-6b', b < 6.001 from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types(7)
        AS (a int4, b float8)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-drec-types', 'drec-type-7a', b > 6.999 from cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types(7)
        AS (a int4, b float8)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-drec-types', 'drec-type-7b', b < 7.001 from cte;

-- test for null and not-null

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types(8)
        AS (a int4, b float4)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-drec-types', 'null-is-present', (a IS NULL AND b IS NULL)
FROM cte;

WITH cte AS (
    SELECT * FROM dynamic_record_generator_types(6)
        AS (a int4, b float4)
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-drec-types', 'null-is-not-present', (a IS NOT NULL AND b IS NOT NULL)
FROM cte;

