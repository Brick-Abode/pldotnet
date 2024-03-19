----------------------------------------
-- First, simple integer tests
CREATE OR REPLACE FUNCTION numbers(count int4)
RETURNS SETOF int4 AS
$$
	if(count == null){ for(int i=0;;i++) { yield return i; } }
	else { for(int i=0;i<count;i++) { yield return i; } }
$$
LANGUAGE plcsharp;

CREATE OR REPLACE FUNCTION numbers(count int8)
RETURNS SETOF int8 AS
$$
	if(count == null){ for(long i=0;;i++) { yield return i; } }
	else { for(long i=0;i<count;i++) { yield return i; } }
$$
LANGUAGE plcsharp;

CREATE OR REPLACE FUNCTION numbers()
RETURNS SETOF int8 AS
$$
	for(long i=0;;i++){ yield return i;}
$$
LANGUAGE plcsharp;

WITH data AS (SELECT numbers() AS num LIMIT 100)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-srf-sum', 'numbers-1', SUM(num) = 4950 FROM data;

WITH data AS (SELECT numbers(100) AS num)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-srf-sum', 'numbers-2', SUM(num) = 4950 FROM data;

----------------------------------------
-- Second, float tests

CREATE OR REPLACE FUNCTION make_pi()
RETURNS SETOF float8 AS
$$
        double sum = 0.0;
        for(int i=0;;i++){yield return 4*(sum+=((i%2)==0?1.0:-1.0)/(2*i+1));}
$$
LANGUAGE plcsharp;
select make_pi() LIMIT 10;

WITH data AS (SELECT numbers() AS num, make_pi() AS pi_value)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-srf-pi', 'make_pi-1', pi_value < 3.143 FROM data WHERE num = 1000 LIMIT 1;

WITH data AS (SELECT numbers() AS num, make_pi() AS pi_value)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-srf-pi', 'make_pi-2', pi_value > 3.141 FROM data WHERE num = 1000 LIMIT 1;

----------------------------------------
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

CREATE OR REPLACE FUNCTION string_to_integer_array_orig(args text[])
RETURNS SETOF integer AS
$$
        foreach (string arg in args) {
            yield return ( (arg == null) ? 0 : int.Parse(arg) );
        }
$$
LANGUAGE plcsharp;

CREATE OR REPLACE FUNCTION string_to_integer_array(args text[])
RETURNS SETOF integer AS
$$
    return args.Cast<string>().Select(arg => (int?)(arg == null ? 0 : int.Parse(arg)));
$$
LANGUAGE plcsharp;

WITH data1 AS (
    WITH data2 AS (
        SELECT ARRAY ['1', '0 ', '-3', NULL::text, '99'] as input
    )
    SELECT
        string_to_integer_array(input) AS col1,
        string_to_integer_array_plsql(input) AS col2
    FROM data2
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-srf-comparison', 'comparison-1', bool_and(COALESCE(col1 = col2, false)) AS all_equal
FROM data1;

----------------------------------------
-- Fourth, string input/output tests
-- (string is an object type, not a struct type)
CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE OR REPLACE FUNCTION ten_items(arg text)
RETURNS SETOF text AS
$$
        for(int i=1; i<=10; i++){ yield return $"{i} {arg}"; }
$$
LANGUAGE plcsharp STRICT;

SELECT 'c#-srf-comparison', 'comparison-1', ten_items('apples');

WITH aggregated AS (
    SELECT string_agg(ten_items, '') AS concatenated_items
    FROM (SELECT ten_items('apples')) AS t
)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT
    'c#-srf-strings',
    'string-checksum-1',
    encode(digest(concatenated_items, 'sha256'), 'hex') = '94091910bae126a50dfb041cd9e9a44efd716c77185628b3bce7a5965a207555'
FROM aggregated;
