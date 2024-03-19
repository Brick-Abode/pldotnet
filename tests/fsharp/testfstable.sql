
----------------------------------------
-- New Function Creation
CREATE OR REPLACE FUNCTION table_type_test_fsharp()
RETURNS TABLE(id integer, name text) AS
$$
    seq {
        yield struct(1, "Alice");
        yield struct(1, "Bob");
    }
$$
LANGUAGE plfsharp;

-- Test Invocation
WITH data AS (SELECT * FROM table_type_test_fsharp())
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-table-function', 'type-integrity',
        BOOL_AND(pg_typeof(id) = 'integer'::regtype AND pg_typeof(name) = 'text'::regtype) FROM data;

-- ----------------------------------------
-- Looping test

CREATE OR REPLACE FUNCTION table_arg_test_fsharp(lim int4)
RETURNS TABLE(id integer, name text) AS
$$
match lim.HasValue with
| true ->
    seq {
        for i = 0 to lim.Value-1 do
            yield struct(i, "The number is "+i.ToString());
        }
| false ->
    Seq.empty
$$
LANGUAGE plfsharp;

SELECT * FROM table_arg_test_fsharp(10);

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-table-function', 'data-integrity-1', SUM(id) = 45 
    FROM table_arg_test_fsharp(10);

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-table-function', 'data-integrity-2', SUM(id) IS NULL
    FROM table_arg_test_fsharp(NULL::int);

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-table-function', 'data-integrity-3', COUNT(*) = 0
    FROM table_arg_test_fsharp(NULL::int);

----------------------------------------
-- array test

CREATE OR REPLACE FUNCTION table_array_test_fsharp(lim int4)
RETURNS TABLE(id integer[], name text) AS
$$
match lim.HasValue with
| true ->
    seq {
        for i = 0 to lim.Value-1 do
            yield struct([| Nullable(i); Nullable(i*i*i); System.Nullable() |], "The number is "+i.ToString());
        }
| false ->
    Seq.empty
$$
LANGUAGE plfsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-table-function', 'array-output-1', COUNT(*) = 5
    FROM table_array_test_fsharp(5);

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-table-function', 'array-output-2', SUM(val) = 110
FROM table_array_test_fsharp(5), UNNEST(id) AS val;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-table-function', 'array-output-3', SUM(val) = 110
FROM table_array_test_fsharp(5), UNNEST(id) AS val
WHERE ARRAY_POSITION(id, NULL) = 3;

-- this is a disgusting way to confirm that none of the arrays
-- has NULL in the second column.
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-table-function', 'array-output-4', COALESCE(SUM(val) = 110, true)
FROM table_array_test_fsharp(5), UNNEST(id) AS val
WHERE ARRAY_POSITION(id, NULL) = 2;

----------------------------------------
-- empty table

CREATE OR REPLACE FUNCTION table_empty_test_fsharp(lim int4)
RETURNS TABLE(id integer[], name text) AS
$$
Seq.empty // cant have an empty body, but this will do
$$
LANGUAGE plfsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-table-function', 'empty-result-1', COUNT(*) = 0
FROM table_empty_test_fsharp(5);

