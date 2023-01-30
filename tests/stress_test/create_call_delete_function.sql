CREATE OR REPLACE FUNCTION IncreaseInt4Range(orig_value INT4RANGE, increment_value INTEGER) RETURNS INT4RANGE AS $$
return new NpgsqlRange<int>(orig_value.LowerBound + increment_value, orig_value.LowerBoundIsInclusive, orig_value.LowerBoundInfinite, orig_value.UpperBound + increment_value, orig_value.UpperBoundIsInclusive, orig_value.UpperBoundInfinite);
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int4range', 'IncreaseInt4Range1', IncreaseInt4Range('(-2147483648,2147483644)'::INT4RANGE, 3) = '[-2147483644,2147483647)'::INT4RANGE;
DROP FUNCTION IncreaseInt4Range;