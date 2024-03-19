-- MONEY
CREATE OR REPLACE FUNCTION computeNewSalary(salary MONEY, rate FLOAT8) RETURNS MONEY AS $$
    decimal aux = (decimal)(1.0+rate);
    return (decimal)salary*aux;
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-money', 'computeNewSalary', computeNewSalary('32500'::MONEY, 0.059875) = '34445.9375'::MONEY;

CREATE OR REPLACE FUNCTION returnMaxMoney() RETURNS MONEY AS $$
    decimal value = 92233720368547758.07M;
    return value;
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-money', 'returnMaxMoney', returnMaxMoney() = '92233720368547758.07'::MONEY;

CREATE OR REPLACE FUNCTION returnMinMoney() RETURNS MONEY AS $$
    decimal value = -92233720368547758.08M;
    return value;
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-money', 'returnMinMoney', returnMinMoney() = '-92233720368547758.08'::MONEY;

-- NULL
CREATE OR REPLACE FUNCTION returnMoney(salary MONEY, bonus MONEY, discounts MONEY) RETURNS MONEY AS $$
    decimal s = salary == null ? 0.0M : (decimal)salary;
    decimal b = bonus == null ? 0.0M : (decimal)bonus;
    decimal d = discounts == null ? 0.0M : (decimal)discounts;
    return s+b-d;
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-money', 'returnMoney1', returnMoney('32500.0'::MONEY, '1556.25'::MONEY, '899.99'::MONEY) = '33156.26'::MONEY;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-money-null', 'returnMoney2', returnMoney('13525.21'::MONEY, null::MONEY, '899.99'::MONEY) = '12625.22'::MONEY;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-money-null', 'returnMoney3', returnMoney(null::MONEY, null::MONEY, null::MONEY) = '0'::MONEY;

--- MONEY Arrays
CREATE OR REPLACE FUNCTION updateMoneyArrayIndex(values_array MONEY[], desired MONEY, index integer[]) RETURNS MONEY[] AS $$
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-money-null-1array', 'updateMoneyArrayIndex1', updateMoneyArrayIndex(ARRAY['32500.0'::MONEY, '-500.4'::MONEY, null::MONEY, '900540.2'::MONEY], '1390540.2'::MONEY, ARRAY[2]) = ARRAY['32500.0'::MONEY, '-500.4'::MONEY, '1390540.2'::MONEY, '900540.2'::MONEY];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-money-null-2array-arraynull', 'updateMoneyArrayIndex2', updateMoneyArrayIndex(ARRAY[['32500.0'::MONEY, '-500.4'::MONEY], [null::MONEY, null::MONEY]], '1390540.2'::MONEY, ARRAY[1,0]) = ARRAY[['32500.0'::MONEY, '-500.4'::MONEY], ['1390540.2'::MONEY, null::MONEY]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-money-null-2array-arraynull', 'updateMoneyArrayIndex3', updateMoneyArrayIndex(ARRAY[[null::MONEY, null::MONEY], [null::MONEY, null::MONEY]], '1390540.2'::MONEY, ARRAY[1,0]) = ARRAY[[null::MONEY, null::MONEY], ['1390540.2'::MONEY, null::MONEY]];

CREATE OR REPLACE FUNCTION IncreaseMoney(values_array MONEY[]) RETURNS MONEY[] AS $$
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    decimal orig_value = (decimal)flatten_values.GetValue(i);
    decimal new_value = orig_value + 1;

    flatten_values.SetValue((decimal)new_value, i);
}
return flatten_values;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-money-null-1array', 'IncreaseMoney1', IncreaseMoney(ARRAY['32500.0'::MONEY, '-500.4'::MONEY, null::MONEY, '900540.2'::MONEY]) = ARRAY['32501.0'::MONEY, '-499.4'::MONEY, null::MONEY, '900541.2'::MONEY];

CREATE OR REPLACE FUNCTION CreateMoneyMultidimensionalArray() RETURNS MONEY[] AS $$
decimal objects_value = 3720368547758.08M;
decimal?[, ,] three_dimensional_array = new decimal?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-money-null-3array-arraynull', 'CreateMoneyMultidimensionalArray1', CreateMoneyMultidimensionalArray() = ARRAY[[[3720368547758.08::MONEY, 3720368547758.08::MONEY], [null::MONEY, null::MONEY]], [[3720368547758.08::MONEY, null::MONEY], [3720368547758.08::MONEY, 3720368547758.08::MONEY]]];
