CREATE OR REPLACE PROCEDURE printSumProcedure(a integer, b integer) AS $$
int c = (int)a + (int)b;
Elog.Info($"c = {c}");
$$ LANGUAGE plcsharp;
CALL printSumProcedure(10, 25);
CALL printSumProcedure(1450, 275);

CREATE OR REPLACE PROCEDURE printSmallestValueProcedure(doublevalues double precision[]) AS $$
double min = double.MaxValue;
for(int i = 0; i < doublevalues.Length; i++)
{
    double value = (double)doublevalues.GetValue(i);
    min = min < value ? min : value;
}
Elog.Info($"Minimum value = {min}");
$$ LANGUAGE plcsharp;
CALL printSmallestValueProcedure(ARRAY[2.25698, 2.85956, 2.85456, 0.00128, 0.00127, 2.36875]);
CALL printSmallestValueProcedure(ARRAY[2.25698, -2.85956, 2.85456, -0.00128, 0.00127, 12.36875, -23.2354]);
