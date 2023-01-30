INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-money', 'testMoneyFSharp1', testMoneyFSharp('32500.0'::MONEY) = '32500.0'::MONEY;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-money-null', 'testMoneyFSharp1', testMoneyFSharp(NULL::MONEY) = 0::MONEY;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-money-null-1array', 'updateMoneyArrayFSharp1', updateMoneyArrayFSharp(ARRAY['32500.0'::MONEY, '-500.4'::MONEY, null::MONEY, '900540.2'::MONEY], '1390540.2'::MONEY, 2::int) = ARRAY['32500.0'::MONEY, '-500.4'::MONEY, '1390540.2'::MONEY, '900540.2'::MONEY];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-money-null-2array-arraynull', 'updateMoneyArrayFSharp2', updateMoneyArrayFSharp(ARRAY[['32500.0'::MONEY, '-500.4'::MONEY], [null::MONEY, null::MONEY]], '1390540.2'::MONEY, 1::int) = ARRAY['32500.0'::MONEY, '1390540.2'::MONEY, 0::MONEY, 0::MONEY];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-money-null-2array-arraynull', 'updateMoneyArrayFSharp3', updateMoneyArrayFSharp(ARRAY[[null::MONEY, null::MONEY], [null::MONEY, null::MONEY]], '1390540.2'::MONEY, 1::int) = ARRAY[0::MONEY, '1390540.2'::MONEY, 0::MONEY, 0::MONEY];
