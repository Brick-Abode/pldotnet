INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int8', 'fibonacci2', fibonacci(5) = BIGINT '5';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int8', 'fibonacci3', fibonacci(15) = BIGINT '610';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int8', 'fibonacci1', fibonacci(20) = BIGINT '6765';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-int8', 'fibonacci1', fibonacci(30) = BIGINT '832040';

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-money-null-1array', 'updateMoneyArray1', updateMoneyArray(ARRAY['32500.0'::MONEY, '-500.4'::MONEY, null::MONEY, '900540.2'::MONEY], '1390540.2'::MONEY, ARRAY[2]) = ARRAY['32500.0'::MONEY, '-500.4'::MONEY, '1390540.2'::MONEY, '900540.2'::MONEY];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-money-null-2array-arraynull', 'updateMoneyArray2', updateMoneyArray(ARRAY[['32500.0'::MONEY, '-500.4'::MONEY], [null::MONEY, null::MONEY]], '1390540.2'::MONEY, ARRAY[1,0]) = ARRAY[['32500.0'::MONEY, '-500.4'::MONEY], ['1390540.2'::MONEY, null::MONEY]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-money-null-2array-arraynull', 'updateMoneyArray3', updateMoneyArray(ARRAY[[null::MONEY, null::MONEY], [null::MONEY, null::MONEY]], '1390540.2'::MONEY, ARRAY[1,0]) = ARRAY[[null::MONEY, null::MONEY], ['1390540.2'::MONEY, null::MONEY]];
