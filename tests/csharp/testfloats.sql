-- Float4 (real): 6 digits of precison
CREATE OR REPLACE FUNCTION returnReal() RETURNS real AS $$
return 1.50055f;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float4', 'returnReal', returnReal() = real '1.50055';

CREATE OR REPLACE FUNCTION sumReal(a real, b real) RETURNS real AS $$
if (a == null)
    a = 0;

if (b == null)
    b = 0;

return a+b;
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float4', 'sumReal1', sumReal(1.50055, 1.50054) = real '3.00109';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float4-null', 'sumReal2', sumReal(NULL, 1.50054) = real '1.50054';

--- Float8 (double precision): 15 digits of precison
CREATE OR REPLACE FUNCTION returnDouble() RETURNS double precision AS $$
return 11.0050000000005;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float8', 'returnDouble', returnDouble() = double precision '11.0050000000005';

CREATE OR REPLACE FUNCTION sumDouble(a double precision, b double precision) RETURNS double precision AS $$
if (a == null)
    a = 0;

if (b == null)
    b = 0;

return a+b;
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float8', 'sumDouble1', sumDouble(10.5000000000055, 10.5000000000054) = double precision  '21.0000000000109';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float8-null', 'sumDouble2', sumDouble(NULL, NULL) = double precision '0';

CREATE OR REPLACE FUNCTION make_pi_n(n int) RETURNS double precision AS $$
double sum = 0.0;
for(int i=0;i<n;i++){ sum += ((i%2)==0?1.0:-1.0)/(2*i+1); }
return 4.0 * sum;
$$
LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float8-make-pi', 'make_pi_lt', make_pi_n(1000) < double precision '3.15';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float8-make-pi', 'make_pi_gt', make_pi_n(1000) > double precision '3.13';

--- Float Arrays
CREATE OR REPLACE FUNCTION returnRealArray(floats real[]) RETURNS real[] AS $$
return floats;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float4-null-1array', 'returnRealArray1', returnRealArray(ARRAY[1.50055::real, null::real, 4.52123::real, 7.41234::real]) = ARRAY[1.50055::real, null::real, 4.52123::real, 7.41234::real];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float4-null-2array-arraynull', 'returnRealArray2', returnRealArray(ARRAY[[null::real, null::real], [4.52123::real, 7.41234::real]]) = ARRAY[[null::real, null::real], [4.52123::real, 7.41234::real]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float4-null-3array-arraynull', 'returnRealArray3', returnRealArray(ARRAY[[[null::real, null::real], [null::real, null::real]], [[7.50055::real, 8.30300::real], [9.52123::real, 11.41234::real]]]) = ARRAY[[[null::real, null::real], [null::real, null::real]], [[7.50055::real, 8.30300::real], [9.52123::real, 11.41234::real]]];

CREATE OR REPLACE FUNCTION sumRealArray(floats real[]) RETURNS real AS $$
Array flatten_floats = Array.CreateInstance(typeof(object), floats.Length);
ArrayManipulation.FlatArray(floats, ref flatten_floats);
float float_sum = 0;
for(int i = 0; i < flatten_floats.Length; i++)
{
    if (flatten_floats.GetValue(i) == null)
        continue;
    float_sum = float_sum + (float)flatten_floats.GetValue(i);
}
return float_sum;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float4-null-1array', 'sumRealArray1', sumRealArray(ARRAY[1.50055::real, 2.30300::real, 4.52123::real, 7.41234::real, null::real]) = '15.737121';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float4-null-2array-arraynull', 'sumRealArray2', sumRealArray(ARRAY[[1.50055::real, 2.30300::real], [4.52123::real, 7.41234::real], [null::real, null::real]]) = '15.737121';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float4-null-3array-arraynull', 'sumRealArray3', sumRealArray(ARRAY[[[1.50055::real, 2.30300::real], [4.52123::real, 7.41234::real], [null::real, null::real]], [[7.50055::real, 8.30300::real], [null::real, null::real], [9.52123::real, 11.41234::real]]]) = '52.474243';

CREATE OR REPLACE FUNCTION CreateRealMultidimensionalArray() RETURNS real[] AS $$
float?[, ,] float_three_dimensional = new float?[2, 2, 2] {{{1.24323f, 3.42345f}, {null, null}}, {{9.32425f, 8.11134f}, {10.32145f, 16.14256f}}};
return float_three_dimensional;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float4-null-3array-arraynull', 'CreateRealMultidimensionalArray', CreateRealMultidimensionalArray() = ARRAY[[[1.24323::real, 3.42345::real], [null::real, null::real]], [[9.32425::real, 8.11134::real], [10.32145::real, 16.14256::real]]];

CREATE OR REPLACE FUNCTION updateArrayRealIndex(floats real[], desired real, index integer[]) RETURNS real[] AS $$
int[] arrayInteger = index.Cast<int>().ToArray();
floats.SetValue(desired, arrayInteger);
return floats;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float4-null-1array', 'updateArrayRealIndex1', updateArrayRealIndex(ARRAY[4.55555::real, 10.11324::real, null::real], 9.83212, ARRAY[1]) = ARRAY[4.55555::real, 9.83212::real, null::real];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float4-null-2array', 'updateArrayRealIndex2', updateArrayRealIndex(ARRAY[[4.55555::real, 10.11324::real], [null::real, 16.12464::real]], 9.83212, ARRAY[1, 0]) = ARRAY[[4.55555::real, 10.11324::real], [9.83212::real, 16.12464::real]];

--- Double Arrays
CREATE OR REPLACE FUNCTION returnDoubleArray(doubles double precision[]) RETURNS double precision[] AS $$
return doubles;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float8-null-1array', 'returnDoubleArray1', returnDoubleArray(ARRAY[21.0000000000109::double precision, null::double precision, 4.521234313421::double precision, 7.412344328978::double precision]) = ARRAY[21.0000000000109::double precision, null::double precision, 4.521234313421::double precision, 7.412344328978::double precision];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float8-null-2array-arraynull', 'returnDoubleArray2', returnDoubleArray(ARRAY[[null::double precision, null::double precision], [4.521234313421::double precision, 7.412344328978::double precision]]) = ARRAY[[null::double precision, null::double precision], [4.521234313421::double precision, 7.412344328978::double precision]];

CREATE OR REPLACE FUNCTION sumDoubleArray(doubles double precision[]) RETURNS double precision AS $$
Array flatten_doubles = Array.CreateInstance(typeof(object), doubles.Length);
ArrayManipulation.FlatArray(doubles, ref flatten_doubles);
double double_sum = 0;
for(int i = 0; i < flatten_doubles.Length; i++)
{
    if (flatten_doubles.GetValue(i) == null)
        continue;
    double_sum = double_sum + (double)flatten_doubles.GetValue(i);
}
return double_sum;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float8-null-1array', 'sumDoubleArray1', sumDoubleArray(ARRAY[21.0000000000109::double precision, null::double precision, 4.521234313421::double precision, 7.412344328978::double precision]) = '32.9335786424099';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float8-null-2array', 'sumDoubleArray2', sumDoubleArray(ARRAY[[21.0000000000109::double precision, null::double precision], [4.521234313421::double precision, 7.412344328978::double precision]]) = '32.9335786424099';

CREATE OR REPLACE FUNCTION CreateDoubleMultidimensionalArray() RETURNS double precision[] AS $$
double?[, ,] double_three_dimensional = new double?[2, 2, 2] {{{1.243235421, 3.423454214}, {null, null}}, {{9.3242542134, 8.1113476543}, {10.321451237, 16.142541316}}};
return double_three_dimensional;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float8-null-3array-arraynull', 'CreateDoubleMultidimensionalArray', CreateDoubleMultidimensionalArray() = ARRAY[[[1.243235421::double precision, 3.423454214::double precision], [null::double precision, null::double precision]], [[9.3242542134::double precision, 8.1113476543::double precision], [10.321451237::double precision, 16.142541316::double precision]]];

CREATE OR REPLACE FUNCTION updateArrayDoubleIndex(doubles double precision[], desired double precision, index integer[]) RETURNS double precision[] AS $$
int[] arrayInteger = index.Cast<int>().ToArray();
doubles.SetValue(desired, arrayInteger);
return doubles;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float8-null-1array', 'updateArrayDoubleIndex1', updateArrayDoubleIndex(ARRAY[4.55535544213::double precision, 10.1133254154::double precision, null::double precision], 9.8321432132, ARRAY[1]) = ARRAY[4.55535544213::double precision, 9.8321432132::double precision, null::double precision];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-float8-null-2array', 'updateArrayDoubleIndex2', updateArrayDoubleIndex(ARRAY[[4.55535544213::double precision, 10.1133254154::double precision], [null::double precision, 16.16155::double precision]], 9.8321432132, ARRAY[1, 0]) = ARRAY[[4.55535544213::double precision, 10.1133254154::double precision], [9.8321432132::double precision, 16.16155::double precision]];
