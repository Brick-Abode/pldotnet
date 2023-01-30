--- DATEOID
CREATE OR REPLACE FUNCTION modifyInputDateFSharp(orig_date DATE) RETURNS DATE AS $$
let orig_date = if System.Object.ReferenceEquals(orig_date, null) then DateOnly(2022, 1, 1) else orig_date.Value
let day = orig_date.Day
let month = orig_date.Month
let year = orig_date.Year
DateOnly(year + 3, month + 1, day + 6)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-date', 'modifyInputDateFSharp1', modifyInputDateFSharp(DATE 'Oct-14-2022') = DATE 'Nov-20-2025';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-date-null', 'modifyInputDateFSharp2', modifyInputDateFSharp(NULL::DATE) = DATE 'Feb-07-2025';

--- TIMEOID
CREATE OR REPLACE FUNCTION addMinutesFSharp(orig_time TIME, min_to_add INT) RETURNS TIME AS $$
    match (orig_time.HasValue, min_to_add.HasValue) with
        | (true, true) ->
            Nullable((orig_time.Value).AddMinutes(double min_to_add.Value))
        | (true, false) -> Nullable(orig_time.Value)
        | (false, true) -> Nullable((TimeOnly(0, 0, 0)).AddMinutes(double min_to_add.Value))
        | (false, false) -> Nullable(TimeOnly(0, 0, 0))
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-time', 'addMinutesFSharp1', addMinutesFSharp(TIME '05:30 PM', 75) = TIME '06:45 PM';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-time-null', 'addMinutesFSharp2', addMinutesFSharp(NULL::TIME, 75) = TIME '01:15:00';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-time-null', 'addMinutesFSharp1', addMinutesFSharp(TIME '05:30 PM', NULL) = TIME '17:30';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-time-null', 'addMinutesFSharp2', addMinutesFSharp(NULL::TIME, NULL) = TIME '00:00:00';

--- TIMETZOID
CREATE OR REPLACE FUNCTION addHoursFSharp(orig_time TIMETZ, hours_to_add FLOAT) RETURNS TIMETZ AS $$
let orig_time = if orig_time.HasValue then orig_time.Value else DateTimeOffset(2022, 1, 1, 8, 30, 20, TimeSpan(2, 0, 0))
orig_time.AddHours(hours_to_add.Value)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timetz', 'addHoursFSharp1', addHoursFSharp(TIMETZ '04:05:06-08:00',1.5) = TIMETZ '05:35:06-08:00';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timetz-null', 'addHoursFSharp2', addHoursFSharp(NULL::TIMETZ,1.5) = TIMETZ '10:00:20+02:00';

--- TIMESTAMP
CREATE OR REPLACE FUNCTION setNewDateFSharp(orig_timestamp TIMESTAMP, new_date DATE) RETURNS TIMESTAMP AS $$
let orig_timestamp = if orig_timestamp.HasValue then orig_timestamp.Value else DateTime(2022, 1, 1, 8, 30, 20)
let new_date = if new_date.HasValue then new_date.Value else DateOnly(2023, 12, 25)
let new_day = new_date.Day
let new_month = new_date.Month
let new_year = new_date.Year
DateTime(new_year, new_month, new_day, orig_timestamp.Hour, orig_timestamp.Minute, orig_timestamp.Second)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamp', 'setNewDateFSharp1', setNewDateFSharp(TIMESTAMP '2004-10-19 10:23:54 PM', DATE '2022-10-17') = TIMESTAMP '2022-10-17 10:23:54 PM';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamp-null', 'setNewDateFSharp2', setNewDateFSharp(NULL::TIMESTAMP, NULL::DATE) = TIMESTAMP '2023-12-25 08:30:20';

--- TIMESTAMPTZ
CREATE OR REPLACE FUNCTION addDaysFSharp(my_timestamp TIMESTAMP WITH TIME ZONE, days_to_add INT) RETURNS TIMESTAMP WITH TIME ZONE AS $$
let my_timestamp = if my_timestamp.HasValue then my_timestamp.Value else DateTime(2022, 1, 1, 8, 30, 20, DateTimeKind.Utc)
my_timestamp.AddDays(int32 days_to_add)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamptz', 'addDaysFSharp1', addDaysFSharp(TIMESTAMP WITH TIME ZONE '2004-10-19 10:23:54 PM +02', 2) = TIMESTAMP WITH TIME ZONE '2004-10-21 22:23:54 +02';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamptz-null', 'addDaysFSharp2', addDaysFSharp(NULL::TIMESTAMP WITH TIME ZONE, 2) = TIMESTAMP WITH TIME ZONE '2022-01-03 08:30:20 +00';

--- INTERVAL
CREATE OR REPLACE FUNCTION modifyIntervalFSharp(orig_interval INTERVAL, days_to_add INT, months_to_add INT) RETURNS INTERVAL AS $$
let orig_interval = if orig_interval.HasValue then orig_interval.Value else NpgsqlInterval(4, 25, 900000000)
NpgsqlInterval(orig_interval.Months + months_to_add.Value, orig_interval.Days + days_to_add.Value, orig_interval.Time)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-interval', 'modifyIntervalFSharp1', modifyIntervalFSharp(INTERVAL '4 hours 5 minutes 6 seconds', 15, 20) = INTERVAL '1 YEAR 8 MONTHS 15 DAYS 4 HOURS 5 MINUTES 6 SECONDS';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-interval-null', 'modifyIntervalFSharp2', modifyIntervalFSharp(NULL::INTERVAL, 15, 20) = INTERVAL '2 YEAR 40 DAYS 15 MINUTES';

--- DATEOID Arrays
CREATE OR REPLACE FUNCTION updateArrayDateIndexFSharp(a DATE[], b DATE) RETURNS DATE[] AS $$
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
SELECT 'f#-date-1array', 'updateArrayDateIndexFSharp1', updateArrayDateIndexFSharp(ARRAY[DATE 'Oct-14-2022', DATE 'Oct-15-2022', null::date, DATE 'Oct-16-2022'], DATE 'Nov-18-2022') = ARRAY[DATE 'Nov-18-2022', DATE 'Oct-15-2022', null::date, DATE 'Oct-16-2022'];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-date-2array', 'updateArrayDateIndexFSharp2', updateArrayDateIndexFSharp(ARRAY[[DATE 'Oct-14-2022', DATE 'Oct-15-2022'], [null::date, DATE 'Oct-16-2022']], DATE 'Nov-18-2022') = ARRAY[[DATE 'Nov-18-2022', DATE 'Oct-15-2022'], [null::date, DATE 'Oct-16-2022']];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-date-3array', 'updateArrayDateIndexFSharp3', updateArrayDateIndexFSharp(ARRAY[[[DATE 'Oct-14-2022', DATE 'Oct-15-2022'], [null::date, DATE 'Oct-16-2022']]], DATE 'Nov-18-2022') = ARRAY[[[DATE 'Nov-18-2022', DATE 'Oct-15-2022'], [null::date, DATE 'Oct-16-2022']]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-date-null-1array-arraynull', 'updateArrayDateIndexFSharp4', updateArrayDateIndexFSharp(ARRAY[null::date, null::date, null::date, DATE 'Oct-16-2022'], DATE 'Nov-18-2022') = ARRAY[DATE 'Nov-18-2022', null::date, null::date, DATE 'Oct-16-2022'];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-date-null-2array-arraynull', 'updateArrayDateIndexFSharp5', updateArrayDateIndexFSharp(ARRAY[[null::date, null::date], [null::date, DATE 'Oct-16-2022']], DATE 'Nov-18-2022') = ARRAY[[DATE 'Nov-18-2022', null::date], [null::date, DATE 'Oct-16-2022']];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-date-null-3array-arraynull', 'updateArrayDateIndexFSharp6', updateArrayDateIndexFSharp(ARRAY[[[null::date, null::date], [null::date, DATE 'Oct-16-2022']]], DATE 'Nov-18-2022') = ARRAY[[[DATE 'Nov-18-2022', null::date], [null::date, DATE 'Oct-16-2022']]];

CREATE OR REPLACE FUNCTION CreateDateMultidimensionalArrayFSharp() RETURNS DATE[] AS $$
let objects_value = DateOnly(2022,12,25)
let arr = Array.CreateInstance(typeof<DateOnly>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-date-null-3array-arraynull', 'CreateDateMultidimensionalArrayFSharp', CreateDateMultidimensionalArrayFSharp() = ARRAY[[[DATE 'Dec-25-2022'::date]]];

--- TIMEOID Arrays
CREATE OR REPLACE FUNCTION updateArrayTimeIndexFSharp(a TIME[], b TIME) RETURNS TIME[] AS $$
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
SELECT 'f#-time-1array', 'updateArrayTimeIndexFSharp1', updateArrayTimeIndexFSharp(ARRAY[TIME '05:30 PM', TIME '06:30 PM', null::time, TIME '09:30 AM'], TIME '5:45 AM') = ARRAY[TIME '5:45 AM', TIME '06:30 PM', null::time, TIME '09:30 AM'];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-time-2array', 'updateArrayTimeIndexFSharp2', updateArrayTimeIndexFSharp(ARRAY[[TIME '05:30 PM', TIME '06:30 PM'], [null::time, TIME '09:30 AM']], TIME '5:45 AM') = ARRAY[[TIME '5:45 AM', TIME '06:30 PM'], [null::time, TIME '09:30 AM']];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-time-3array', 'updateArrayTimeIndexFSharp3', updateArrayTimeIndexFSharp(ARRAY[[[TIME '05:30 PM'], [TIME '06:30 PM']], [[null::time], [TIME '09:30 AM']]], TIME '5:45 AM') = ARRAY[[[TIME '5:45 AM'], [TIME '06:30 PM']], [[null::time], [TIME '09:30 AM']]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-time-null-1array-arraynull', 'updateArrayTimeIndexFSharp4', updateArrayTimeIndexFSharp(ARRAY[null::TIME, null::TIME, null::time, TIME '09:30 AM'], TIME '5:45 AM') = ARRAY[TIME '5:45 AM', null::TIME, null::time, TIME '09:30 AM'];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-time-null-2array-arraynull', 'updateArrayTimeIndexFSharp5', updateArrayTimeIndexFSharp(ARRAY[[null::TIME, null::TIME], [null::time, TIME '09:30 AM']], TIME '5:45 AM') = ARRAY[[TIME '5:45 AM', null::TIME], [null::time, TIME '09:30 AM']];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-time-null-3array-arraynull', 'updateArrayTimeIndexFSharp6', updateArrayTimeIndexFSharp(ARRAY[[[null::TIME], [null::TIME]], [[null::time], [TIME '09:30 AM']]], TIME '5:45 AM') = ARRAY[[[TIME '5:45 AM'], [null::TIME]], [[null::time], [TIME '09:30 AM']]];

CREATE OR REPLACE FUNCTION CreateTimeMultidimensionalArrayFSharp() RETURNS TIME[] AS $$
let objects_value = TimeOnly(10,33,55)
let arr = Array.CreateInstance(typeof<TimeOnly>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-time-3array', 'CreateTimeMultidimensionalArrayFSharp', CreateTimeMultidimensionalArrayFSharp() = ARRAY[[[TIME '10:33:55 AM']]];

--- TIMETZOID Arrays
CREATE OR REPLACE FUNCTION updateArrayTimetzIndexFSharp(a TIMETZ[], b TIMETZ) RETURNS TIMETZ[] AS $$
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
SELECT 'f#-timetz-1array', 'updateArrayTimetzIndexFSharp1', updateArrayTimetzIndexFSharp(ARRAY[TIMETZ '05:30-03:00', TIMETZ '06:30-03:00', null::timetz, TIMETZ '22:30-03:00'], TIMETZ '02:30-05:00') = ARRAY[TIMETZ '02:30-05:00', TIMETZ '06:30-03:00', null::timetz, TIMETZ '22:30-03:00'];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timetz-2array', 'updateArrayTimetzIndexFSharp2', updateArrayTimetzIndexFSharp(ARRAY[[TIMETZ '05:30-03:00', TIMETZ '06:30-03:00'], [null::timetz, TIMETZ '22:30-03:00']], TIMETZ '02:30-05:00') = ARRAY[[TIMETZ '02:30-05:00', TIMETZ '06:30-03:00'], [null::timetz, TIMETZ '22:30-03:00']];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timetz-3array', 'updateArrayTimetzIndexFSharp3', updateArrayTimetzIndexFSharp(ARRAY[[[TIMETZ '05:30-03:00', TIMETZ '06:30-03:00'], [null::timetz, TIMETZ '22:30-03:00']]], TIMETZ '02:30-05:00') = ARRAY[[[TIMETZ '02:30-05:00', TIMETZ '06:30-03:00'], [null::timetz, TIMETZ '22:30-03:00']]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timetz-null-1array-arraynull', 'updateArrayTimetzIndexFSharp4', updateArrayTimetzIndexFSharp(ARRAY[null::TIMETZ, null::TIMETZ, null::timetz, TIMETZ '22:30-03:00'], TIMETZ '02:30-05:00') = ARRAY[TIMETZ '02:30-05:00', null::TIMETZ, null::timetz, TIMETZ '22:30-03:00'];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timetz-null-2array-arraynull', 'updateArrayTimetzIndexFSharp5', updateArrayTimetzIndexFSharp(ARRAY[[null::TIMETZ, null::TIMETZ], [null::timetz, TIMETZ '22:30-03:00']], TIMETZ '02:30-05:00') = ARRAY[[TIMETZ '02:30-05:00', null::TIMETZ], [null::timetz, TIMETZ '22:30-03:00']];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timetz-null-3array-arraynull', 'updateArrayTimetzIndexFSharp6', updateArrayTimetzIndexFSharp(ARRAY[[[null::TIMETZ, null::TIMETZ], [null::timetz, TIMETZ '22:30-03:00']]], TIMETZ '02:30-05:00') = ARRAY[[[TIMETZ '02:30-05:00', null::TIMETZ], [null::timetz, TIMETZ '22:30-03:00']]];

CREATE OR REPLACE FUNCTION CreateTimetzMultidimensionalArrayFSharp() RETURNS TIMETZ[] AS $$
let objects_value = DateTimeOffset(2022, 12, 25, 10, 33, 55, TimeSpan(2, 0, 0));
let arr = Array.CreateInstance(typeof<DateTimeOffset>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timetz-3array', 'CreateTimetzMultidimensionalArrayFSharp', CreateTimetzMultidimensionalArrayFSharp() = ARRAY[[[TIMETZ '10:33:55+02:00']]];

--- TIMESTAMP Arrays
CREATE OR REPLACE FUNCTION updateArrayTimestampIndexFSharp(a TIMESTAMP[], b TIMESTAMP) RETURNS TIMESTAMP[] AS $$
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
SELECT 'f#-timestamp-1array', 'updateArrayTimestampIndexFSharp1', updateArrayTimestampIndexFSharp(ARRAY[TIMESTAMP '2004-10-19 10:23:54 PM', TIMESTAMP '2020-10-19 10:23:54 PM', null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM'], TIMESTAMP '2025-10-19 10:23:54 PM') = ARRAY[TIMESTAMP '2025-10-19 10:23:54 PM', TIMESTAMP '2020-10-19 10:23:54 PM', null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM'];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamp-2array', 'updateArrayTimestampIndexFSharp2', updateArrayTimestampIndexFSharp(ARRAY[[TIMESTAMP '2004-10-19 10:23:54 PM', TIMESTAMP '2020-10-19 10:23:54 PM'], [null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM']], TIMESTAMP '2025-10-19 10:23:54 PM') = ARRAY[[TIMESTAMP '2025-10-19 10:23:54 PM', TIMESTAMP '2020-10-19 10:23:54 PM'], [null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM']];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamp-3array', 'updateArrayTimestampIndexFSharp3', updateArrayTimestampIndexFSharp(ARRAY[[[TIMESTAMP '2004-10-19 10:23:54 PM', TIMESTAMP '2020-10-19 10:23:54 PM'], [null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM']]], TIMESTAMP '2025-10-19 10:23:54 PM') = ARRAY[[[TIMESTAMP '2025-10-19 10:23:54 PM', TIMESTAMP '2020-10-19 10:23:54 PM'], [null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM']]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamp-null-1array-arraynull', 'updateArrayTimestampIndexFSharp4', updateArrayTimestampIndexFSharp(ARRAY[null::timestamp, null::timestamp, null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM'], TIMESTAMP '2025-10-19 10:23:54 PM') = ARRAY[TIMESTAMP '2025-10-19 10:23:54 PM', null::timestamp, null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM'];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamp-null-2array-arraynull', 'updateArrayTimestampIndexFSharp5', updateArrayTimestampIndexFSharp(ARRAY[[null::timestamp, null::timestamp], [null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM']], TIMESTAMP '2025-10-19 10:23:54 PM') = ARRAY[[TIMESTAMP '2025-10-19 10:23:54 PM', null::timestamp], [null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM']];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamp-null-3array-arraynull', 'updateArrayTimestampIndexFSharp6', updateArrayTimestampIndexFSharp(ARRAY[[[null::timestamp, null::timestamp], [null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM']]], TIMESTAMP '2025-10-19 10:23:54 PM') = ARRAY[[[TIMESTAMP '2025-10-19 10:23:54 PM', null::timestamp], [null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM']]];

CREATE OR REPLACE FUNCTION CreateTimestampMultidimensionalArrayFSharp() RETURNS TIMESTAMP[] AS $$
let objects_value = DateTime(2022, 11, 15, 13, 23, 45)
let arr = Array.CreateInstance(typeof<DateTime>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamp-3array', 'CreateTimestampMultidimensionalArrayFSharp', CreateTimestampMultidimensionalArrayFSharp() = ARRAY[[[TIMESTAMP '2022-11-15 13:23:45']]];

--- TIMESTAMPTZ Arrays
CREATE OR REPLACE FUNCTION updateArrayTimestamptzIndexFSharp(a TIMESTAMP WITH TIME ZONE[], b TIMESTAMP WITH TIME ZONE) RETURNS TIMESTAMP WITH TIME ZONE[] AS $$
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
SELECT 'f#-timestamptz-1array', 'updateArrayTimestamptzIndexFSharp1', updateArrayTimestamptzIndexFSharp(ARRAY[TIMESTAMP WITH TIME ZONE '2004-10-19 10:23:54 PM +02', TIMESTAMP WITH TIME ZONE '2020-10-19 10:23:54 PM +03', null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05'], TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03') = ARRAY[TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03', TIMESTAMP WITH TIME ZONE '2020-10-19 10:23:54 PM +03', null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05'];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamptz-2array', 'updateArrayTimestamptzIndexFSharp2', updateArrayTimestamptzIndexFSharp(ARRAY[[TIMESTAMP WITH TIME ZONE '2004-10-19 10:23:54 PM +02', TIMESTAMP WITH TIME ZONE '2020-10-19 10:23:54 PM +03', null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05']], TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03') = ARRAY[[TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03', TIMESTAMP WITH TIME ZONE '2020-10-19 10:23:54 PM +03', null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05']];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamptz-3array', 'updateArrayTimestamptzIndexFSharp3', updateArrayTimestamptzIndexFSharp(ARRAY[[[TIMESTAMP WITH TIME ZONE '2004-10-19 10:23:54 PM +02', TIMESTAMP WITH TIME ZONE '2020-10-19 10:23:54 PM +03'], [null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05']]], TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03') = ARRAY[[[TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03', TIMESTAMP WITH TIME ZONE '2020-10-19 10:23:54 PM +03'], [null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05']]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamptz-null-1array-arraynull', 'updateArrayTimestamptzIndexFSharp4', updateArrayTimestamptzIndexFSharp(ARRAY[null::timestamptz, null::timestamptz, null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05'], TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03') = ARRAY[TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03', null::timestamptz, null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05'];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamptz-null-2array-arraynull', 'updateArrayTimestamptzIndexFSharp5', updateArrayTimestamptzIndexFSharp(ARRAY[[null::timestamptz, null::timestamptz], [null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05']], TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03') = ARRAY[[TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03', null::timestamptz], [null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05']];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamptz-null-3array-arraynull', 'updateArrayTimestamptzIndexFSharp6', updateArrayTimestamptzIndexFSharp(ARRAY[[[null::timestamptz, null::timestamptz], [null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05']]], TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03') = ARRAY[[[TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03', null::timestamptz], [null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05']]];

CREATE OR REPLACE FUNCTION CreateTimestamptzMultidimensionalArrayFSharp() RETURNS TIMESTAMP WITH TIME ZONE[] AS $$
let objects_value = DateTime(2022, 11, 15, 13, 23, 45, DateTimeKind.Utc);
let arr = Array.CreateInstance(typeof<DateTime>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-timestamptz-3array', 'CreateTimestamptzMultidimensionalArrayFSharp', CreateTimestamptzMultidimensionalArrayFSharp() = ARRAY[[[TIMESTAMP WITH TIME ZONE '2022-11-15 13:23:45 +00']]];

--- INTERVAL Arrays
CREATE OR REPLACE FUNCTION updateArrayIntervalIndexFSharp(a INTERVAL[], b INTERVAL) RETURNS INTERVAL[] AS $$
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
SELECT 'f#-interval-1array', 'updateArrayIntervalIndexFSharp1', updateArrayIntervalIndexFSharp(ARRAY[INTERVAL '4 hours 5 minutes 6 seconds', INTERVAL '8 hours 1 minutes 2 seconds', null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds'], INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds') = ARRAY[INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds', INTERVAL '8 hours 1 minutes 2 seconds', null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds'];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-interval-2array', 'updateArrayIntervalIndexFSharp2', updateArrayIntervalIndexFSharp(ARRAY[[INTERVAL '4 hours 5 minutes 6 seconds', INTERVAL '8 hours 1 minutes 2 seconds'], [null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']], INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds') = ARRAY[[INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds', INTERVAL '8 hours 1 minutes 2 seconds'], [null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-interval-3array', 'updateArrayIntervalIndexFSharp3', updateArrayIntervalIndexFSharp(ARRAY[[[INTERVAL '4 hours 5 minutes 6 seconds', INTERVAL '8 hours 1 minutes 2 seconds'], [null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']]], INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds') = ARRAY[[[INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds', INTERVAL '8 hours 1 minutes 2 seconds'], [null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-interval-null-1array-arraynull', 'updateArrayIntervalIndexFSharp4', updateArrayIntervalIndexFSharp(ARRAY[null::interval, null::interval, null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds'], INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds') = ARRAY[INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds', null::interval, null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds'];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-interval-null-2array-arraynull', 'updateArrayIntervalIndexFSharp5', updateArrayIntervalIndexFSharp(ARRAY[[null::interval, null::interval], [null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']], INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds') = ARRAY[[INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds', null::interval], [null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-interval-null-3array-arraynull', 'updateArrayIntervalIndexFSharp6', updateArrayIntervalIndexFSharp(ARRAY[[[null::interval, null::interval], [null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']]], INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds') = ARRAY[[[INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds', null::interval], [null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']]];

CREATE OR REPLACE FUNCTION CreateIntervalMultidimensionalArrayFSharp() RETURNS INTERVAL[] AS $$
let objects_value = NpgsqlInterval(10, 33, 900000000)
let arr = Array.CreateInstance(typeof<NpgsqlInterval>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-interval-3array', 'CreateIntervalMultidimensionalArrayFSharp', CreateIntervalMultidimensionalArrayFSharp() = ARRAY[[[INTERVAL '10 months 33 days 15 minutes']]];
