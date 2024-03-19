
----------------------------------------
-- New Function Creation
CREATE OR REPLACE FUNCTION table_type_test()
RETURNS TABLE(id integer, name text) AS
$$
    yield return (1, "Alice");
    yield return (2, "Bob");
$$
LANGUAGE plcsharp;

-- Test Invocation
WITH data AS (SELECT * FROM table_type_test())
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-table-function', 'type-integrity',
        BOOL_AND(pg_typeof(id) = 'integer'::regtype AND pg_typeof(name) = 'text'::regtype) FROM data;

----------------------------------------
-- Looping test

CREATE OR REPLACE FUNCTION table_arg_test(lim int4)
RETURNS TABLE(id integer, name text) AS
$$
    return lim.HasValue
        ? Enumerable.Range(0, lim.Value).Select(i => ((int?)i, $"The number is {i}"))
        : Enumerable.Empty<(int? id, string? name)>();
$$
LANGUAGE plcsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-table-function', 'data-integrity-1', SUM(id) = 45 
    FROM table_arg_test(10);

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-table-function', 'data-integrity-2', SUM(id) IS NULL
    FROM table_arg_test(NULL::int);

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-table-function', 'data-integrity-3', COUNT(*) = 0
    FROM table_arg_test(NULL::int);

----------------------------------------
-- array test

CREATE OR REPLACE FUNCTION table_array_test(lim int4)
RETURNS TABLE(id integer[], name text) AS
$$
            return lim.HasValue
                ? Enumerable.Range(0, lim.Value).Select(i => ((Array)new int?[] { i, i*i*i, null }, $"The number is {i}"))
                : Enumerable.Empty<(Array? id, string? name)>();
$$
LANGUAGE plcsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-table-function', 'array-output-1', COUNT(*) = 5
    FROM table_array_test(5);

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-table-function', 'array-output-2', SUM(val) = 110
FROM table_array_test(5), UNNEST(id) AS val;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-table-function', 'array-output-3', SUM(val) = 110
FROM table_array_test(5), UNNEST(id) AS val
WHERE ARRAY_POSITION(id, NULL) = 3;

-- this is a disgusting way to confirm that none of the arrays
-- has NULL in the second column.
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-table-function', 'array-output-4', COALESCE(SUM(val) = 110, true)
FROM table_array_test(5), UNNEST(id) AS val
WHERE ARRAY_POSITION(id, NULL) = 2;

----------------------------------------
-- empty table

CREATE OR REPLACE FUNCTION table_empty_test(lim int4)
RETURNS TABLE(id integer[], name text) AS
$$
    yield break; // can't have an empty body, but this will do
$$
LANGUAGE plcsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-table-function', 'empty-result-1', COUNT(*) = 0
FROM table_empty_test(5);

