----------------------------------------
-- Table creation
DROP TABLE IF EXISTS trigger_test_table;
CREATE TABLE trigger_test_table (
    id int,
    message text
);

----------------------------------------
-- Helpful note:
--
-- Use this to delete all triggers:
--
-- echo "SELECT
--     event_object_table as table,
--     trigger_name,
--     event_manipulation as event,
--     action_timing as timing,
--     action_statement
-- FROM
--     information_schema.triggers;
-- "|psql|grep EXEC| awk '{print "drop trigger " $3 " on " $1 ";";}'
----------------------------------------


----------------------------------------
-- Function creation
CREATE OR REPLACE FUNCTION trigger_test_function_modify() RETURNS TRIGGER AS $$
    // SPEC: Modify text where id == 2
    // This lets us confirm that MODIFY works

    // We check the argument just to make sure the right argument is being called in the right place,
    // but also to make sure that argument passing works.
    if (tg.Arguments[1] != "1"){ throw new SystemException($"Assertion failed: wrong trigger argument, '{tg.Arguments[1]}' != '1'"); }

    // This assertion ensures that the correct WHEN clause is on the trigger
    if ((int)tg.NewRow[0] != 2) { throw new SystemException($"Assertion failed: wrong row value, {tg.NewRow[1]} != 2"); }

    tg.NewRow[1] = "MODIFIED Text!!!";
    return ReturnMode.TriggerModify;
$$ LANGUAGE plcsharp;

CREATE OR REPLACE FUNCTION trigger_test_skip() RETURNS TRIGGER AS $$
    // SPEC: Skip insert where id == 5
    // This lets us confirm that SKIP works

    // We check the argument just to make sure the right argument is being called in the right place,
    // and also to make sure that argument passing works.
    if (tg.Arguments[1] != "2"){ throw new SystemException($"Assertion failed: wrong trigger argument, '{tg.Arguments[1]}' != '2'"); }
    if((int)tg.NewRow[0] == 5) { return ReturnMode.TriggerSkip; }
    return ReturnMode.Normal;
$$ LANGUAGE plcsharp;

CREATE OR REPLACE FUNCTION trigger_test_tg_vals() RETURNS TRIGGER AS $$
    // SPEC: Check tg values where id == 6; MODIFY if valid
    // This lets us confirm that tg values are correct by checking
    // the text value.
    if((int)tg.NewRow[0] == 6) {
        if(
            (tg.TriggerName == "test_trigger_bir_3") &&
            (tg.TriggerWhen == "BEFORE") &&
            (tg.TriggerLevel == "ROW") &&
            (tg.TriggerEvent == "INSERT") &&
            (tg.RelationId > 0) &&
            (tg.TableName == "trigger_test_table") &&
            (tg.TableSchema == "public") &&
            (tg.NewRow.Length == 2) &&
            ((int)tg.NewRow[0] == 6) &&
            (tg.Arguments[0] == "BEFORE/INSERT/ROW") &&
            (tg.Arguments[1] == "3")
        )
        {
            tg.NewRow[1] = "TG value assertions passed";
            return ReturnMode.TriggerModify;
        }
    }
    Elog.Warning($"failed test, tg values didn't check out: {tg}");
    return ReturnMode.Normal;
$$ LANGUAGE plcsharp;

-- This test isn't automatically effective, but it can be manually checked.
CREATE OR REPLACE FUNCTION trigger_test_exception() RETURNS TRIGGER AS $$
    throw new SystemException("This is a test of exception handling");
    return ReturnMode.Normal; // unreached
$$ LANGUAGE plcsharp;

CREATE OR REPLACE FUNCTION trigger_test_update_type() RETURNS TRIGGER AS $$
    tg.NewRow[1] = 1; // this is an error; the correct type is string, not int
    return ReturnMode.TriggerModify;
$$ LANGUAGE plcsharp;

----------------------------------------
-- Trigger creation

-- encoding for test case:
-- B/A = BEFORE/AFTER
-- I/U/D/T = INSERT/UPDATE/DELETE/TRUNCATE
-- R/S = ROW/STATEMENT

CREATE OR REPLACE TRIGGER test_trigger_BIR_1
BEFORE INSERT ON trigger_test_table
FOR EACH ROW
WHEN (new.id = 2)
EXECUTE FUNCTION trigger_test_function_modify('BEFORE/INSERT/ROW', 1);

CREATE OR REPLACE TRIGGER test_trigger_BIR_2
BEFORE INSERT ON trigger_test_table
FOR EACH ROW
WHEN (new.id = 5)
EXECUTE FUNCTION trigger_test_skip('BEFORE/INSERT/ROW', 2);

CREATE OR REPLACE TRIGGER test_trigger_BIR_3
BEFORE INSERT ON trigger_test_table
FOR EACH ROW
WHEN (new.id = 6)
EXECUTE FUNCTION trigger_test_tg_vals('BEFORE/INSERT/ROW', 3);

CREATE OR REPLACE TRIGGER test_trigger_BIR_4
BEFORE INSERT ON trigger_test_table
FOR EACH ROW
WHEN (new.id = 7)
EXECUTE FUNCTION trigger_test_update_type('BEFORE/INSERT/ROW', 4);

CREATE OR REPLACE TRIGGER test_trigger_AUS_4
AFTER UPDATE ON trigger_test_table
FOR EACH STATEMENT
EXECUTE FUNCTION trigger_test_exception ('AFTER/UPDATE/STATEMENT', 4);

----------------------------------------
-- Data manipulation

INSERT INTO trigger_test_table VALUES (1, 'Inserted Text');
INSERT INTO trigger_test_table VALUES (2, 'Inserted Second Text');
INSERT INTO trigger_test_table VALUES (3, 'Inserted Third Text (for updating)');
INSERT INTO trigger_test_table VALUES (4, 'Inserted Fourth Text (for deletion)');
INSERT INTO trigger_test_table VALUES (5, 'Inserted Fifth Text (for simple skip)');
INSERT INTO trigger_test_table VALUES (6, 'Inserted Sixth Text (for values check modify)');
INSERT INTO trigger_test_table VALUES (7, 'This should be a string');
UPDATE trigger_test_table SET message ='Updated Text' WHERE id=1;
UPDATE trigger_test_table SET id=13, message ='Updated Third Text' WHERE id=3;
DELETE FROM trigger_test_table WHERE id=4;

-- Test for zero rows affected, but per-statement trigger fires
UPDATE trigger_test_table SET id=2, message='Updated Text' WHERE id=1234;


-- Test 1: Check if 'id' values are only 1, 2, 6, 7, or 13
-- This ensures that the delete worked, and we have no extra values
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-trigger', 'validRows',
       NOT EXISTS (SELECT 1 FROM trigger_test_table WHERE id NOT IN (1, 2, 6, 7, 13));

-- Test 2: Check 'b' value for 'a' = 1
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-trigger', 'rowUpdated',
       (SELECT message = 'Updated Text' FROM trigger_test_table WHERE id = 1);

-- Test 3: Check 'b' value for 'a' = 2
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-trigger', 'rowModifiedByTrigger',
       (SELECT message = 'MODIFIED Text!!!' FROM trigger_test_table WHERE id = 2);

-- Test 4: Check 'b' value for 'a' = 13
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-trigger', 'rowUpdated-2',
       (SELECT message = 'Updated Third Text' FROM trigger_test_table WHERE id = 13);

-- Test 5: Check that skip works
-- (already checked in Test 1, but this is specific)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-trigger', 'skipWorks',
       NOT EXISTS (SELECT 1 FROM trigger_test_table WHERE id = 5);

-- Test 6: Check that tg values are correct
-- (already checked in Test 1, but this is specific)
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-trigger', 'tgValuesCorrect',
       EXISTS (SELECT 1 FROM trigger_test_table WHERE id = 6 and message = 'TG value assertions passed');

-- Test 7: Ensure that type-mismatched updates are ignored
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-trigger', 'tgTypeMismatchHandling',
       EXISTS (SELECT 1 FROM trigger_test_table WHERE id = 7 and message = 'This should be a string');

