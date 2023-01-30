--- INT4RANGE
CREATE OR REPLACE FUNCTION IncreaseInt4RangeFSharp(a INT4RANGE, b INTEGER) RETURNS INT4RANGE AS $$
let a = if a.HasValue then a.Value else NpgsqlRange<int>(0, true, false, 100, false, false)
let b = if b.HasValue then b.Value else 1

NpgsqlRange<int>(a.LowerBound + int b, a.LowerBoundIsInclusive, a.LowerBoundInfinite, a.UpperBound + int b, a.UpperBoundIsInclusive, a.UpperBoundInfinite)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int4range', 'IncreaseInt4RangeFSharp1', IncreaseInt4RangeFSharp('[2,6)'::INT4RANGE, 1) = '[3,7)'::INT4RANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int4range', 'IncreaseInt4RangeFSharp2', IncreaseInt4RangeFSharp('[,87)'::INT4RANGE, 3) = '(,90)'::INT4RANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int4range', 'IncreaseInt4RangeFSharp3', IncreaseInt4RangeFSharp('[,)'::INT4RANGE, 3) = '(,)'::INT4RANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int4range', 'IncreaseInt4RangeFSharp4', IncreaseInt4RangeFSharp('(-2147483648,2147483644)'::INT4RANGE, 3) = '[-2147483644,2147483647)'::INT4RANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int4range', 'IncreaseInt4RangeFSharp5', IncreaseInt4RangeFSharp('(-456,-123]'::INT4RANGE, 1) = '[-454,-121)'::INT4RANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int4range-null', 'IncreaseInt4RangeFSharp6', IncreaseInt4RangeFSharp(NULL::INT4RANGE, 1) = '[1,101)'::INT4RANGE;

-- -- - INT8RANGE
CREATE OR REPLACE FUNCTION IncreaseInt8RangeFSharp(a INT8RANGE, b INT8) RETURNS INT8RANGE AS $$
let a = if a.HasValue then a.Value else NpgsqlRange<int64>(-429496729, true, false, 429496729, false, false)
let b = if b.HasValue then b.Value else int64 1
NpgsqlRange<int64>(a.LowerBound + int64 b, a.LowerBoundIsInclusive, a.LowerBoundInfinite, a.UpperBound + int64 b, a.UpperBoundIsInclusive, a.UpperBoundInfinite)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int8range', 'IncreaseInt8RangeFSharp1', IncreaseInt8RangeFSharp('[2,6)'::INT8RANGE, 1) = '[3,7)'::INT8RANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int8range', 'IncreaseInt8RangeFSharp2', IncreaseInt8RangeFSharp('[,87)'::INT8RANGE, 3) = '(,90)'::INT8RANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int8range', 'IncreaseInt8RangeFSharp3', IncreaseInt8RangeFSharp('[,)'::INT8RANGE, 3) = '(,)'::INT8RANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int8range', 'IncreaseInt8RangeFSharp8', IncreaseInt8RangeFSharp('(-9223372036854775808,9223372036854775804)'::INT8RANGE, 3) = '[-9223372036854775804,9223372036854775807)'::INT8RANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int8range', 'IncreaseInt8RangeFSharp5', IncreaseInt8RangeFSharp('(-456,-123]'::INT8RANGE, 1) = '[-454,-121)'::INT8RANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int8range-null', 'IncreaseInt8RangeFSharp6', IncreaseInt8RangeFSharp(NULL::INT8RANGE, 1) = '[-429496728,429496730)'::INT8RANGE;

--- TSRANGEOID
CREATE OR REPLACE FUNCTION IncreaseTimestampRangeFSharp(a TSRANGE, b INTEGER) RETURNS TSRANGE AS $$
let a = if a.HasValue then a.Value else NpgsqlRange<DateTime>(DateTime(2022, 1, 1, 12, 30, 30), true, false, DateTime(2022, 12, 25, 17, 30, 30), false, false)
let b = if b.HasValue then b.Value else 1
NpgsqlRange<DateTime>(a.LowerBound.AddDays(int b), a.LowerBoundIsInclusive, a.LowerBoundInfinite, a.UpperBound.AddDays(int b), a.UpperBoundIsInclusive, a.UpperBoundInfinite)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tsrange', 'IncreaseTimestampRangeFSharp1', IncreaseTimestampRangeFSharp('[2021-01-01 14:30, 2021-01-01 15:30)'::TSRANGE, 1) = '[2021-01-02 14:30, 2021-01-02 15:30)'::TSRANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tsrange', 'IncreaseTimestampRangeFSharp2', IncreaseTimestampRangeFSharp('[, 2021-01-01 15:30)'::TSRANGE, 3) = '[, 2021-01-04 15:30)'::TSRANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tsrange', 'IncreaseTimestampRangeFSharp3', IncreaseTimestampRangeFSharp('[,)'::TSRANGE, 3) = '(,)'::TSRANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tsrange', 'IncreaseTimestampRangeFSharp4', IncreaseTimestampRangeFSharp('(2021-01-01 14:30, 2021-01-01 15:30]'::TSRANGE, 3) = '(2021-01-04 14:30, 2021-01-04 15:30]'::TSRANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tsrange-null', 'IncreaseTimestampRangeFSharp5', IncreaseTimestampRangeFSharp(NULL::TSRANGE, 3) = '["2022-01-04 12:30:30","2022-12-28 17:30:30")'::TSRANGE;

--- TSTZRANGEOID
CREATE OR REPLACE FUNCTION IncreaseTimestampTzRangeFSharp(a TSTZRANGE, b INTEGER) RETURNS TSTZRANGE AS $$
let a = if a.HasValue then a.Value else NpgsqlRange<DateTime>(DateTime(2022, 1, 1, 12, 30, 30, DateTimeKind.Utc), true, false, DateTime(2022, 12, 25, 17, 30, 30, DateTimeKind.Utc), false, false)
let b = if b.HasValue then b.Value else 1
NpgsqlRange<DateTime>(a.LowerBound.AddDays(int b), a.LowerBoundIsInclusive, a.LowerBoundInfinite, a.UpperBound.AddDays(int b), a.UpperBoundIsInclusive, a.UpperBoundInfinite)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tstzrange', 'IncreaseTimestampTzRangeFSharp1', IncreaseTimestampTzRangeFSharp('[2021-01-01 14:30 -03, 2021-01-04 15:30 +05)'::TSTZRANGE, 1) = '[2021-01-02 14:30 -03, 2021-01-05 15:30 +05)'::TSTZRANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tstzrange', 'IncreaseTimestampTzRangeFSharp2', IncreaseTimestampTzRangeFSharp('[, 2021-01-01 15:30 -03)'::TSTZRANGE, 3) = '[, 2021-01-04 15:30 -03)'::TSTZRANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tstzrange', 'IncreaseTimestampTzRangeFSharp3', IncreaseTimestampTzRangeFSharp('[,)'::TSTZRANGE, 3) = '(,)'::TSTZRANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tstzrange', 'IncreaseTimestampTzRangeFSharp4', IncreaseTimestampTzRangeFSharp('(2021-01-01 14:30 -03, 2021-01-04 15:30 +05]'::TSTZRANGE, 3) = '(2021-01-04 14:30 -03, 2021-01-07 15:30 +05]'::TSTZRANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tstzrange-null', 'IncreaseTimestampTzRangeFSharp5', IncreaseTimestampTzRangeFSharp(NULL::TSTZRANGE, 3) = '["2022-01-04 12:30:30+00","2022-12-28 17:30:30+00")'::TSTZRANGE;

--- DATERANGEOID
CREATE OR REPLACE FUNCTION IncreaseDateonlyRangeFSharp(a DATERANGE, b INTEGER) RETURNS DATERANGE AS $$
let a = if a.HasValue then a.Value else NpgsqlRange<DateOnly>(DateOnly(2022, 1, 1), true, false, DateOnly(2022, 12, 25), false, false)
let b = if b.HasValue then b.Value else 1
NpgsqlRange<DateOnly>(a.LowerBound.AddDays(int b), a.LowerBoundIsInclusive, a.LowerBoundInfinite, a.UpperBound.AddDays(int b), a.UpperBoundIsInclusive, a.UpperBoundInfinite)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-daterange', 'IncreaseDateonlyRangeFSharp1', IncreaseDateonlyRangeFSharp('[2021-01-01, 2021-01-04)'::DATERANGE, 1) = '[2021-01-02, 2021-01-05)'::DATERANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-daterange', 'IncreaseDateonlyRangeFSharp2', IncreaseDateonlyRangeFSharp('[, 2021-01-01)'::DATERANGE, 3) = '[, 2021-01-04)'::DATERANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-daterange', 'IncreaseDateonlyRangeFSharp3', IncreaseDateonlyRangeFSharp('[,)'::DATERANGE, 3) = '(,)'::DATERANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-daterange', 'IncreaseDateonlyRangeFSharp4', IncreaseDateonlyRangeFSharp('(2021-01-01, 2021-01-04]'::DATERANGE, 3) = '(2021-01-04, 2021-01-07]'::DATERANGE;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-daterange-null', 'IncreaseDateonlyRangeFSharp5', IncreaseDateonlyRangeFSharp(NULL::DATERANGE, 3) = '[2022-01-04,2022-12-28)'::DATERANGE;

--- INT4RANGE Arrays
CREATE OR REPLACE FUNCTION updateInt4RangeIndexFSharp(a INT4RANGE[], b INT4RANGE) RETURNS INT4RANGE[] AS $$
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
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int4range-null-1array', 'updateInt4RangeIndexFSharp1', updateInt4RangeIndexFSharp(ARRAY['[2,6)'::INT4RANGE, '(,6)'::INT4RANGE, null::INT4RANGE, '[,)'::INT4RANGE], '[6,)'::INT4RANGE) = ARRAY['[6,)'::INT4RANGE, '(,6)'::INT4RANGE, null::INT4RANGE, '[,)'::INT4RANGE];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int4range-null-2array', 'updateInt4RangeIndexFSharp2', updateInt4RangeIndexFSharp(ARRAY[['[2,6)'::INT4RANGE, '(,6)'::INT4RANGE], [null::INT4RANGE, '[,)'::INT4RANGE]], '[6,)'::INT4RANGE) = ARRAY[['[6,)'::INT4RANGE, '(,6)'::INT4RANGE], [null::INT4RANGE, '[,)'::INT4RANGE]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int4range-null-3array', 'updateInt4RangeIndexFSharp3', updateInt4RangeIndexFSharp(ARRAY[[['[2,6)'::INT4RANGE, '(,6)'::INT4RANGE], [null::INT4RANGE, '[,)'::INT4RANGE]]], '[6,)'::INT4RANGE) = ARRAY[[['[6,)'::INT4RANGE, '(,6)'::INT4RANGE], [null::INT4RANGE, '[,)'::INT4RANGE]]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int4range-null-2array-arraynull', 'updateInt4RangeIndexFSharp4', updateInt4RangeIndexFSharp(ARRAY[[null::INT4RANGE, null::INT4RANGE], [null::INT4RANGE, '[,)'::INT4RANGE]], '[6,)'::INT4RANGE) = ARRAY[['[6,)'::INT4RANGE, null::INT4RANGE], [null::INT4RANGE, '[,)'::INT4RANGE]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int4range-null-3array-arraynull', 'updateInt4RangeIndexFSharp5', updateInt4RangeIndexFSharp(ARRAY[[[null::INT4RANGE, null::INT4RANGE], [null::INT4RANGE, '[,)'::INT4RANGE]]], '[6,)'::INT4RANGE) = ARRAY[[['[6,)'::INT4RANGE, null::INT4RANGE], [null::INT4RANGE, '[,)'::INT4RANGE]]];

CREATE OR REPLACE FUNCTION CreateInt4RangeArrayFSharp() RETURNS INT4RANGE[] AS $$
let arr = Array.CreateInstance(typeof<NpgsqlRange<int>>, 1, 1, 1)
let objects_value = NpgsqlRange<int>(64, true, false, 89, false, false)
arr.SetValue(objects_value, 0, 0, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int4range-null-3array-arraynull', 'CreateInt4RangeArrayFSharp1', CreateInt4RangeArrayFSharp() = ARRAY[[['[64,89)'::INT4RANGE]]];

--- INT8RANGE Arrays
CREATE OR REPLACE FUNCTION updateInt8RangeIndexFSharp(a INT8RANGE[], b INT8RANGE) RETURNS INT8RANGE[] AS $$
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
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int8range-null-1array', 'updateInt8RangeIndexFSharp1', updateInt8RangeIndexFSharp(ARRAY['[2,6)'::INT8RANGE, '(,6)'::INT8RANGE, null::INT8RANGE, '[,)'::INT8RANGE], '[6,)'::INT8RANGE) = ARRAY['[6,)'::INT8RANGE, '(,6)'::INT8RANGE, null::INT8RANGE, '[,)'::INT8RANGE];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int8range-null-2array', 'updateInt8RangeIndexFSharp2', updateInt8RangeIndexFSharp(ARRAY[['[2,6)'::INT8RANGE, '(,6)'::INT8RANGE], [null::INT8RANGE, '[,)'::INT8RANGE]], '[6,)'::INT8RANGE) = ARRAY[['[6,)'::INT8RANGE, '(,6)'::INT8RANGE], [null::INT8RANGE, '[,)'::INT8RANGE]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int8range-null-3array', 'updateInt8RangeIndexFSharp3', updateInt8RangeIndexFSharp(ARRAY[[['[2,6)'::INT8RANGE, '(,6)'::INT8RANGE], [null::INT8RANGE, '[,)'::INT8RANGE]]], '[6,)'::INT8RANGE) = ARRAY[[['[6,)'::INT8RANGE, '(,6)'::INT8RANGE], [null::INT8RANGE, '[,)'::INT8RANGE]]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int8range-null-2array-arraynull', 'updateInt8RangeIndexFSharp4', updateInt8RangeIndexFSharp(ARRAY[[null::INT8RANGE, null::INT8RANGE], [null::INT8RANGE, '[,)'::INT8RANGE]], '[6,)'::INT8RANGE) = ARRAY[['[6,)'::INT8RANGE, null::INT8RANGE], [null::INT8RANGE, '[,)'::INT8RANGE]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int8range-null-3array-arraynull', 'updateInt8RangeIndexFSharp5', updateInt8RangeIndexFSharp(ARRAY[[[null::INT8RANGE, null::INT8RANGE], [null::INT8RANGE, '[,)'::INT8RANGE]]], '[6,)'::INT8RANGE) = ARRAY[[['[6,)'::INT8RANGE, null::INT8RANGE], [null::INT8RANGE, '[,)'::INT8RANGE]]];

CREATE OR REPLACE FUNCTION CreateInt8RangeArrayFSharp() RETURNS INT8RANGE[] AS $$
let arr = Array.CreateInstance(typeof<NpgsqlRange<int64>>, 1, 1, 1)
let objects_value = NpgsqlRange<int64>(64, true, false, 89, false, false)
arr.SetValue(objects_value, 0, 0, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int8range-null-3array-arraynull', 'CreateInt8RangeArrayFSharp1', CreateInt8RangeArrayFSharp() = ARRAY[[['[64,89)'::INT8RANGE]]];

--- TSRANGE Arrays
CREATE OR REPLACE FUNCTION updateTimestampRangeIndexFSharp(a TSRANGE[], b TSRANGE) RETURNS TSRANGE[] AS $$
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
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tsrange-null-1array', 'updateTimestampRangeIndexFSharp1', updateTimestampRangeIndexFSharp(ARRAY['[2021-01-01 14:30, 2021-01-01 15:30)'::TSRANGE, '(, 2021-04-01 15:30)'::TSRANGE, null::TSRANGE, '[,)'::TSRANGE], '[2021-05-25 14:30,)'::TSRANGE) = ARRAY['[2021-05-25 14:30,)'::TSRANGE, '(, 2021-04-01 15:30)'::TSRANGE, null::TSRANGE, '[,)'::TSRANGE];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tsrange-null-2array', 'updateTimestampRangeIndexFSharp2', updateTimestampRangeIndexFSharp(ARRAY[['[2021-01-01 14:30, 2021-01-01 15:30)'::TSRANGE, '(, 2021-04-01 15:30)'::TSRANGE], [null::TSRANGE, '[,)'::TSRANGE]], '[2021-05-25 14:30,)'::TSRANGE) = ARRAY[['[2021-05-25 14:30,)'::TSRANGE, '(, 2021-04-01 15:30)'::TSRANGE], [null::TSRANGE, '[,)'::TSRANGE]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tsrange-null-3array', 'updateTimestampRangeIndexFSharp3', updateTimestampRangeIndexFSharp(ARRAY[[['[2021-01-01 14:30, 2021-01-01 15:30)'::TSRANGE, '(, 2021-04-01 15:30)'::TSRANGE], [null::TSRANGE, '[,)'::TSRANGE]]], '[2021-05-25 14:30,)'::TSRANGE) = ARRAY[[['[2021-05-25 14:30,)'::TSRANGE, '(, 2021-04-01 15:30)'::TSRANGE], [null::TSRANGE, '[,)'::TSRANGE]]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tsrange-null-2array-arraynull', 'updateTimestampRangeIndexFSharp4', updateTimestampRangeIndexFSharp(ARRAY[[null::TSRANGE, null::TSRANGE], [null::TSRANGE, '[,)'::TSRANGE]], '[2021-05-25 14:30,)'::TSRANGE) = ARRAY[['[2021-05-25 14:30,)'::TSRANGE, null::TSRANGE], [null::TSRANGE, '[,)'::TSRANGE]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tsrange-null-3array-arraynull', 'updateTimestampRangeIndexFSharp5', updateTimestampRangeIndexFSharp(ARRAY[[[null::TSRANGE, null::TSRANGE], [null::TSRANGE, '[,)'::TSRANGE]]], '[2021-05-25 14:30,)'::TSRANGE) = ARRAY[[['[2021-05-25 14:30,)'::TSRANGE, null::TSRANGE], [null::TSRANGE, '[,)'::TSRANGE]]];

CREATE OR REPLACE FUNCTION CreateTimestampRangeArrayEmptyFSharp() RETURNS TSRANGE[] AS $$
Array.CreateInstance(typeof<NpgsqlRange<DateTime>>, 1, 1, 1)
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tsrange-null-3array-arraynull', 'CreateTimestampRangeArrayEmptyFSharp1', CAST(CreateTimestampRangeArrayEmptyFSharp() as TEXT) = '{{{empty}}}';

CREATE OR REPLACE FUNCTION CreateTimestampRangeArrayFSharp() RETURNS TSRANGE[] AS $$
let arr = Array.CreateInstance(typeof<NpgsqlRange<DateTime>>, 1, 1, 1)
let objects_value = NpgsqlRange<DateTime>(DateTime(2022, 4, 14, 12, 30, 25), true, false, DateTime(2022, 4, 15, 17, 30, 25), false, false)
arr.SetValue(objects_value, 0, 0, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tsrange-3array', 'CreateTimestampRangeArrayFSharp1', CreateTimestampRangeArrayFSharp() = ARRAY[[['[2022-04-14 12:30:25, 2022-04-15 17:30:25)'::TSRANGE]]];

--- TSTZRANGE Arrays
CREATE OR REPLACE FUNCTION updateTimestampTzRangeIndexFSharp(a TSTZRANGE[], b TSTZRANGE) RETURNS TSTZRANGE[] AS $$
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
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tstzrange-null-1array', 'updateTimestampTzRangeIndexFSharp1', updateTimestampTzRangeIndexFSharp(ARRAY['[2021-01-01 14:30 +02, 2021-01-01 15:30 -05)'::TSTZRANGE, '(, 2021-04-01 15:30 +05)'::TSTZRANGE, null::TSTZRANGE, '[,)'::TSTZRANGE], '[2021-05-25 14:30 +03,)'::TSTZRANGE) = ARRAY['[2021-05-25 14:30 +03,)'::TSTZRANGE, '(, 2021-04-01 15:30 +05)'::TSTZRANGE, null::TSTZRANGE, '[,)'::TSTZRANGE];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tstzrange-null-2array', 'updateTimestampTzRangeIndexFSharp2', updateTimestampTzRangeIndexFSharp(ARRAY[['[2021-01-01 14:30 +02, 2021-01-01 15:30 -05)'::TSTZRANGE, '(, 2021-04-01 15:30 +05)'::TSTZRANGE], [null::TSTZRANGE, '[,)'::TSTZRANGE]], '[2021-05-25 14:30 +03,)'::TSTZRANGE) = ARRAY[['[2021-05-25 14:30 +03,)'::TSTZRANGE, '(, 2021-04-01 15:30 +05)'::TSTZRANGE], [null::TSTZRANGE, '[,)'::TSTZRANGE]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tstzrange-null-3array', 'updateTimestampTzRangeIndexFSharp3', updateTimestampTzRangeIndexFSharp(ARRAY[[['[2021-01-01 14:30 +02, 2021-01-01 15:30 -05)'::TSTZRANGE, '(, 2021-04-01 15:30 +05)'::TSTZRANGE], [null::TSTZRANGE, '[,)'::TSTZRANGE]]], '[2021-05-25 14:30 +03,)'::TSTZRANGE) = ARRAY[[['[2021-05-25 14:30 +03,)'::TSTZRANGE, '(, 2021-04-01 15:30 +05)'::TSTZRANGE], [null::TSTZRANGE, '[,)'::TSTZRANGE]]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tstzrange-null-2array-arraynull', 'updateTimestampTzRangeIndexFSharp4', updateTimestampTzRangeIndexFSharp(ARRAY[[null::TSTZRANGE, null::TSTZRANGE], [null::TSTZRANGE, '[,)'::TSTZRANGE]], '[2021-05-25 14:30 +03,)'::TSTZRANGE) = ARRAY[['[2021-05-25 14:30 +03,)'::TSTZRANGE, null::TSTZRANGE], [null::TSTZRANGE, '[,)'::TSTZRANGE]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tstzrange-null-3array-arraynull', 'updateTimestampTzRangeIndexFSharp5', updateTimestampTzRangeIndexFSharp(ARRAY[[[null::TSTZRANGE, null::TSTZRANGE], [null::TSTZRANGE, '[,)'::TSTZRANGE]]], '[2021-05-25 14:30 +03,)'::TSTZRANGE) = ARRAY[[['[2021-05-25 14:30 +03,)'::TSTZRANGE, null::TSTZRANGE], [null::TSTZRANGE, '[,)'::TSTZRANGE]]];

CREATE OR REPLACE FUNCTION CreateTimestampTzRangeArrayFSharp() RETURNS TSTZRANGE[] AS $$
let arr = Array.CreateInstance(typeof<NpgsqlRange<DateTime>>, 1, 1, 1)
let objects_value = NpgsqlRange<DateTime>(DateTime(2022, 4, 14, 12, 30, 25, DateTimeKind.Utc), true, false, DateTime(2022, 4, 15, 17, 30, 25, DateTimeKind.Utc), false, false)
arr.SetValue(objects_value, 0, 0, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-tstzrange-null-3array-arraynull', 'CreateTimestampTzRangeArrayFSharp1', CreateTimestampTzRangeArrayFSharp() = ARRAY[[['[2022-04-14 12:30:25 +00, 2022-04-15 17:30:25 +00)'::TSTZRANGE]]];

--- DATERANGE Arrays
CREATE OR REPLACE FUNCTION updateDateonlyRangeIndexFSharp(a DATERANGE[], b DATERANGE) RETURNS DATERANGE[] AS $$
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
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-daterange-null-1array', 'updateDateonlyRangeIndexFSharp1', updateDateonlyRangeIndexFSharp(ARRAY['[2021-01-01, 2021-01-01)'::DATERANGE, '(, 2021-04-01)'::DATERANGE, null::DATERANGE, '[,)'::DATERANGE], '[2021-05-25,)'::DATERANGE) = ARRAY['[2021-05-25,)'::DATERANGE, '(, 2021-04-01)'::DATERANGE, null::DATERANGE, '[,)'::DATERANGE];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-daterange-null-2array', 'updateDateonlyRangeIndexFSharp2', updateDateonlyRangeIndexFSharp(ARRAY[['[2021-01-01, 2021-01-01)'::DATERANGE, '(, 2021-04-01)'::DATERANGE], [null::DATERANGE, '[,)'::DATERANGE]], '[2021-05-25,)'::DATERANGE) = ARRAY[['[2021-05-25,)'::DATERANGE, '(, 2021-04-01)'::DATERANGE], [null::DATERANGE, '[,)'::DATERANGE]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-daterange-null-3array', 'updateDateonlyRangeIndexFSharp3', updateDateonlyRangeIndexFSharp(ARRAY[[['[2021-01-01, 2021-01-01)'::DATERANGE, '(, 2021-04-01)'::DATERANGE], [null::DATERANGE, '[,)'::DATERANGE]]], '[2021-05-25,)'::DATERANGE) = ARRAY[[['[2021-05-25,)'::DATERANGE, '(, 2021-04-01)'::DATERANGE], [null::DATERANGE, '[,)'::DATERANGE]]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-daterange-null-2array-arraynull', 'updateDateonlyRangeIndexFSharp4', updateDateonlyRangeIndexFSharp(ARRAY[[null::DATERANGE, null::DATERANGE], [null::DATERANGE, '[,)'::DATERANGE]], '[2021-05-25,)'::DATERANGE) = ARRAY[['[2021-05-25,)'::DATERANGE, null::DATERANGE], [null::DATERANGE, '[,)'::DATERANGE]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-daterange-null-3array-arraynull', 'updateDateonlyRangeIndexFSharp5', updateDateonlyRangeIndexFSharp(ARRAY[[[null::DATERANGE, null::DATERANGE], [null::DATERANGE, '[,)'::DATERANGE]]], '[2021-05-25,)'::DATERANGE) = ARRAY[[['[2021-05-25,)'::DATERANGE, null::DATERANGE], [null::DATERANGE, '[,)'::DATERANGE]]];

CREATE OR REPLACE FUNCTION CreateDateonlyRangeArrayFSharp() RETURNS DATERANGE[] AS $$
let arr = Array.CreateInstance(typeof<NpgsqlRange<DateOnly>>, 1, 1, 1)
let objects_value = NpgsqlRange<DateOnly>(DateOnly(2022, 4, 14), true, false, DateOnly(2022, 4, 15), false, false)
arr.SetValue(objects_value, 0, 0, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-daterange-null-3array-arraynull', 'CreateDateonlyRangeArrayFSharp1', CreateDateonlyRangeArrayFSharp() = ARRAY[[['[2022-04-14, 2022-04-15)'::DATERANGE]]];
