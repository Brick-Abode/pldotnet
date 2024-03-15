----------------------------------------
-- First, simple integer tests
CREATE OR REPLACE FUNCTION numbers_fsharp(count INT4)
RETURNS SETOF INT4 AS
$$
let c = if count.HasValue then count.Value else int 0

match count.HasValue with
| false ->
    seq { for i in 0 .. System.Int32.MaxValue do yield i }
| true ->
    seq { for i in 0 .. count.Value - 1 do yield i }

$$
LANGUAGE plfsharp;

CREATE OR REPLACE FUNCTION numbers_fsharp()
RETURNS SETOF int4 AS
$$
    seq {
        for i in 0 .. System.Int32.MaxValue do
            yield i
    }
$$
LANGUAGE plfsharp;

WITH data AS (SELECT numbers_fsharp() AS num LIMIT 100)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-srf-sum', 'numbers-1', SUM(num) = 4950 FROM data;

WITH data AS (SELECT numbers_fsharp(100) AS num)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-srf-sum', 'numbers-2', SUM(num) = 4950 FROM data;

----------------------------------------
-- Second, float tests

CREATE OR REPLACE FUNCTION make_pi_fsharp()
RETURNS SETOF float8 AS
$$
seq {
    let mutable sum : float = 0.0
    for i = 0 to System.Int32.MaxValue do
        yield double(4.0 * sum)
        sum <- sum + ((if i % 2 = 0 then 1.0 else -1.0)/ float(2.0 * float(i) + 1.0))
}
$$
LANGUAGE plfsharp;
select make_pi() LIMIT 10;

WITH data AS (SELECT numbers() AS num, make_pi_fsharp() AS pi_value)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-srf-pi', 'make_pi-1', pi_value < 3.143 FROM data WHERE num = 2000 LIMIT 1;

WITH data AS (SELECT numbers() AS num, make_pi_fsharp() AS pi_value)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-srf-pi', 'make_pi-2', pi_value > 3.141 FROM data WHERE num = 2000 LIMIT 1;

--------------------------------------

-- Third, string array input tests

-- C# is often more expressive than pl/pgsql
CREATE OR REPLACE FUNCTION string_to_integer_array_plsql(strings text[])
RETURNS SETOF integer AS
$$
    DECLARE
      value text;
    BEGIN
      FOREACH value IN ARRAY strings
      LOOP
        IF value IS NULL THEN
          RETURN NEXT 0;
        ELSE
          RETURN NEXT value::integer;
        END IF;
      END LOOP;
      RETURN;
    END;
$$
LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION string_to_integer_array_fsharp(args text[])
RETURNS SETOF integer AS
$$

seq {
    for str in args do
        match str with
        | null -> Nullable(0)
        | _ -> match System.Int32.TryParse(str.ToString()) with
                | true, parsedValue -> Nullable(parsedValue)
                | false, _ -> Nullable(0)
}

$$
LANGUAGE plfsharp;

WITH data1 AS (
    WITH data2 AS (
        SELECT ARRAY ['1', '0 ', '-3', NULL::text, '99'] as input
    )
    SELECT
        string_to_integer_array_fsharp(input) AS col1,
        string_to_integer_array_plsql(input) AS col2
    FROM data2
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-srf-comparison', 'comparison-1', bool_and(COALESCE(col1 = col2, false)) AS all_equal
FROM data1;

--------------------------------------

-- Fourth, string input/output tests
-- (string is an object type, not a struct type)

CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE OR REPLACE FUNCTION ten_items_fsharp(arg text)
RETURNS SETOF text AS
$$
seq {
        for i in 1..10 do
            yield $"{i} {arg}"
    }
$$
LANGUAGE plfsharp STRICT;

SELECT 'c#-srf-comparison', 'comparison-1', ten_items_fsharp('apples');

WITH aggregated AS (
    SELECT string_agg(ten_items_fsharp, '') AS concatenated_items
    FROM (SELECT ten_items_fsharp('apples')) AS t
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT
    'f#-srf-strings',
    'string-checksum-1',
    encode(digest(concatenated_items, 'sha256'), 'hex') = '94091910bae126a50dfb041cd9e9a44efd716c77185628b3bce7a5965a207555'
FROM aggregated;
