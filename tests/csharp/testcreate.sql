CREATE OR REPLACE FUNCTION fibonacci(n integer) RETURNS BIGINT AS $$
    if (n <= 0)
    {
        Elog.Info("Fibonacci number must be greater than 0.");
        return null;
    }
    else if (n <= 2)
    {
        return 1;
    }
    return fibonacci(n-1) + fibonacci(n-2);
$$ LANGUAGE plcsharp;

CREATE OR REPLACE FUNCTION updateMoneyArray(values_array MONEY[], desired MONEY, index integer[]) RETURNS MONEY[] AS $$
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
$$ LANGUAGE plcsharp STRICT;
