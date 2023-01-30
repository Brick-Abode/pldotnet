--- DATEOID
CREATE OR REPLACE FUNCTION createDateFSharp(year int, month int, day int) RETURNS DATE AS $$
match (year.HasValue, month.HasValue, day.HasValue) with
| (true, true, true) -> Nullable (new DateOnly(year.Value, month.Value, day.Value))
| _ -> System.Nullable()
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-date', 'createDateFSharp', createDateFSharp(CAST(2022 AS int), CAST(10 AS int), CAST(14 AS int)) = DATE 'Oct-14-2022';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-date-null', 'createDateFSharp2', createDateFSharp(NULL::int, NULL::int, NULL::int) is NULL;

CREATE OR REPLACE FUNCTION addMinutesTimeFSharp(orig_time TIME, minutes int) RETURNS TIME AS $$
    match (orig_time.HasValue, minutes.HasValue) with
        | (true, true) ->
            Nullable((orig_time.Value).AddMinutes(double minutes.Value))
        | (true, false) -> Nullable(orig_time.Value)
        | (false, true) -> Nullable((TimeOnly(0, 0, 0)).AddMinutes(double minutes.Value))
        | (false, false) -> Nullable(TimeOnly(0, 0, 0))
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-date', 'addMinutesTimeFSharp1', addMinutesTimeFSharp(TIME '05:30 PM', CAST(75 AS int)) = TIME '06:45 PM';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-date-null', 'addMinutesTimeFSharp2', addMinutesTimeFSharp(TIME '05:30 PM', NULL::int) = TIME '05:30 PM';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-date-null', 'addMinutesTimeFSharp3', addMinutesTimeFSharp(NULL::TIME, CAST(75 AS int)) = TIME '01:15:00';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-date-null', 'addMinutesTimeFSharp4', addMinutesTimeFSharp(NULL::TIME, NULL::int) = TIME '00:00:00';

CREATE OR REPLACE FUNCTION modifyTimestampArrayFSharp(a TIMESTAMP[], b TIMESTAMP) RETURNS TIMESTAMP[] AS $$
#line 1
    let dim = a.Rank
    let newValue =
        match b.HasValue with
        | true -> b.Value
        | false -> new DateTime(2022, 11, 15, 13, 23, 45)
    match dim with
    | 1 ->
        a.SetValue(newValue, 0) |> ignore
        a
    | 2 ->
        a.SetValue(newValue, 0, 0) |> ignore
        a
    | 3 ->
        a.SetValue(newValue, 0, 0, 0) |> ignore
        a
    | _ -> a
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamp-1array-null', 'modifyTimestampArrayFSharp1', modifyTimestampArrayFSharp(ARRAY['2004-12-19 10:23:54 PM'::TIMESTAMP, '2020-10-19 10:23:54 PM'::TIMESTAMP, NULL::TIMESTAMP, '2022-12-25 10:23:54 PM'::TIMESTAMP], NULL::TIMESTAMP) = ARRAY['2022-11-15 13:23:45'::TIMESTAMP, '2020-10-19 10:23:54 PM'::TIMESTAMP, NULL::TIMESTAMP, '2022-12-25 10:23:54 PM'::TIMESTAMP];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamp-1array-null', 'modifyTimestampArrayFSharp2', modifyTimestampArrayFSharp(ARRAY['2004-12-19 10:23:54 PM'::TIMESTAMP, '2020-10-19 10:23:54 PM'::TIMESTAMP, NULL::TIMESTAMP, '2022-12-25 10:23:54 PM'::TIMESTAMP], '2023-01-01 12:12:12 PM'::TIMESTAMP) = ARRAY['2023-01-01 12:12:12 PM'::TIMESTAMP, '2020-10-19 10:23:54 PM'::TIMESTAMP, NULL::TIMESTAMP, '2022-12-25 10:23:54 PM'::TIMESTAMP];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamp-2array-null', 'modifyTimestampArrayFSharp3', modifyTimestampArrayFSharp(ARRAY[['2004-12-19 10:23:54 PM'::TIMESTAMP, '2020-10-19 10:23:54 PM'::TIMESTAMP], [NULL::TIMESTAMP, '2022-12-25 10:23:54 PM'::TIMESTAMP]], '2023-01-01 12:12:12 PM'::TIMESTAMP) = ARRAY[['2023-01-01 12:12:12 PM'::TIMESTAMP, '2020-10-19 10:23:54 PM'::TIMESTAMP], [NULL::TIMESTAMP, '2022-12-25 10:23:54 PM'::TIMESTAMP]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamp-2array-null', 'modifyTimestampArrayFSharp4', modifyTimestampArrayFSharp(ARRAY[[NULL::TIMESTAMP, NULL::TIMESTAMP], [NULL::TIMESTAMP, NULL::TIMESTAMP]], '2023-01-01 12:12:12 PM'::TIMESTAMP) = ARRAY[['2023-01-01 12:12:12 PM'::TIMESTAMP, NULL::TIMESTAMP], [NULL::TIMESTAMP, NULL::TIMESTAMP]];