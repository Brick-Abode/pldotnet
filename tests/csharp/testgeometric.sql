--------- POINT
CREATE OR REPLACE FUNCTION middlePoint(pointa point, pointb point) RETURNS point AS $$
if (pointa == null)
    pointa = new NpgsqlPoint(0, 0);

if (pointb == null)
    pointb = new NpgsqlPoint(0, 0);

double x = (((NpgsqlPoint)pointa).X + ((NpgsqlPoint)pointb).X)*0.5;
double y = (((NpgsqlPoint)pointa).Y + ((NpgsqlPoint)pointb).Y)*0.5;
var new_point = new NpgsqlPoint(x,y);
return new_point;
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-point', 'middlePoint1',  middlePoint(POINT(10.0,20.0),POINT(20.0,40.0)) ~= POINT(15.0,30.0);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-point-null', 'middlePoint2',  middlePoint(NULL::POINT,POINT(20.0,40.0)) ~= POINT(10.0,20.0);

CREATE OR REPLACE FUNCTION distanceBetweenPoints(pointa point, pointb point) RETURNS double precision AS $$
double dif_x = (pointa.X - pointb.X);
double dif_y = (pointa.Y - pointb.Y);
double distance = Math.Sqrt(dif_x*dif_x+dif_y*dif_y);
return distance;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-point', 'distanceBetweenPoints',  distanceBetweenPoints(POINT(1.5,2.75), POINT(3.0,4.75)) = double precision '2.5';

CREATE OR REPLACE FUNCTION checkPoints(pointa point, pointb point) RETURNS boolean AS $$
if(pointa.X == pointb.X && pointa.Y == pointb.Y)
{
    return true;
}
return false;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-point', 'checkPoints1',  checkPoints(POINT(2.555701574,8.7552345789),POINT(2.555701574,8.7552345789)) is true;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-point', 'checkPoints2',  checkPoints(POINT(2.555701574,8.7552345789),POINT(2.555701574,8.7552345785)) is false;

--------- LINE
CREATE OR REPLACE FUNCTION createLine(a double precision, b double precision, c double precision) RETURNS LINE AS $$
NpgsqlLine my_line = new NpgsqlLine(a,b,c);
return my_line;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-line', 'createLine', createLine(1.50,-2.750,3.25) = LINE '{1.50,-2.750,3.25}';

CREATE OR REPLACE FUNCTION modifyCoefficients(original_line LINE) RETURNS LINE AS $$
if (original_line == null)
    original_line = new NpgsqlLine(2.4, 8.2, -32.43);

double a = ((NpgsqlLine)original_line).A * -1.0;
double b = ((NpgsqlLine)original_line).B * -1.0;
double c = ((NpgsqlLine)original_line).C * -1.0;
NpgsqlLine my_line = new NpgsqlLine(a,b,c);
return my_line;
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-line', 'modifyCoefficients1', modifyCoefficients(LINE '{-1.5,2.75,-3.25}') = LINE '{1.50,-2.75,3.25}';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-line-null', 'modifyCoefficients2', modifyCoefficients(NULL::LINE) = LINE '{2.4, 8.2, -32.43}';

CREATE OR REPLACE FUNCTION getMinimumDistance(orig_line LINE, orig_point POINT) RETURNS double precision AS $$
double a = orig_line.A;
double b = orig_line.B;
double c = orig_line.C;
return Math.Abs((a*orig_point.X + b*orig_point.Y + c)/Math.Sqrt(a*a+b*b));
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-line', 'getMinimumDistance', getMinimumDistance(LINE '{4.0, 6.0, 2.0}', POINT(3.0,-6.0)) = double precision '3.05085107923876';

--------- LSEG
CREATE OR REPLACE FUNCTION createLineSegment(start_point POINT, end_point POINT) RETURNS LSEG AS $$
NpgsqlLSeg newLine = new NpgsqlLSeg(start_point, end_point);
return newLine;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-lseg', 'createLineSegment', createLineSegment(POINT(0.088997,1.258456),POINT(5.456102,3.04561)) = LSEG '[(0.088997,1.258456),(5.456102,3.04561)]';

CREATE OR REPLACE FUNCTION getReverseLineSegment(my_line LSEG) RETURNS LSEG AS $$
if (my_line == null)
    my_line = new NpgsqlLSeg(new NpgsqlPoint(0, 0), new NpgsqlPoint(100, 100));

NpgsqlPoint firstPoint = ((NpgsqlLSeg)my_line).Start;
NpgsqlPoint secondPoint = ((NpgsqlLSeg)my_line).End;
NpgsqlLSeg newLine = new NpgsqlLSeg(secondPoint, firstPoint);
return newLine;
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-lseg', 'getReverseLineSegment1', getReverseLineSegment(LSEG(POINT(0.0,1.0),POINT(5.0,3.0))) = LSEG '[(5.0,3.0),(0.0,1.0)]';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-lseg-null', 'getReverseLineSegment2', getReverseLineSegment(NULL::LSEG) = LSEG '[(100.0,100.0),(0.0,0.0)]';

--------- BOX
CREATE OR REPLACE FUNCTION testBox(my_box BOX) RETURNS BOX AS $$
return my_box;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-box', 'testBox', testBox(BOX '(0.025988, 1.021653), (2.052787, 3.005716)') = BOX '(0.025988, 1.021653), (2.052787, 3.005716)';

CREATE OR REPLACE FUNCTION createBox(high POINT, low POINT) RETURNS BOX AS $$
return new NpgsqlBox(high, low);
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-box', 'createBox', createBox(POINT '(2.052787, 3.005716)', POINT '(0.025988, 1.021653)') = BOX '(2.052787, 3.005716), (0.025988, 1.021653)';

CREATE OR REPLACE FUNCTION returnWidth(high POINT, low POINT) RETURNS double precision AS $$
NpgsqlBox new_box = new NpgsqlBox(high, low);
return (double)Math.Abs(new_box.Width);
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-box', 'returnWidth', returnWidth(POINT '(0.025988, 1.021653)', POINT '(2.052787, 3.005716)') = double precision '2.026799';

CREATE OR REPLACE FUNCTION increaseBox(orig_value BOX) RETURNS BOX AS $$
if (orig_value == null)
    orig_value = new NpgsqlBox(new NpgsqlPoint(0, 0), new NpgsqlPoint(100, 100));

NpgsqlBox new_value = new NpgsqlBox(new NpgsqlPoint(((NpgsqlBox)orig_value).UpperRight.X + 1, ((NpgsqlBox)orig_value).UpperRight.Y + 1), new NpgsqlPoint(((NpgsqlBox)orig_value).LowerLeft.X + 1, ((NpgsqlBox)orig_value).LowerLeft.Y + 1));

return new_value;
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-box-null', 'increaseBox1', increaseBox(NULL::BOX) = BOX(POINT(101,101),POINT(1,1));

--------- PATH
CREATE OR REPLACE FUNCTION returnPath(orig_path PATH) RETURNS PATH AS $$
return orig_path;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-path', 'returnPath - open', returnPath(PATH '[(1.5,2.75),(3.0,4.75),(5.0,5.0)]') <= PATH '[(1.5,2.75),(3.0,4.75),(5.0,5.0)]';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-path', 'returnPath - close', returnPath(PATH '((1.5,2.75),(3.0,4.75),(5.0,5.0))') <= PATH '((1.5,2.75),(3.0,4.75),(5.0,5.0))';

CREATE OR REPLACE FUNCTION increasePath(orig_value PATH) RETURNS PATH AS $$
if (orig_value == null)
    orig_value = new NpgsqlPath(new NpgsqlPoint(0, 0), new NpgsqlPoint(100, 100), new NpgsqlPoint(200, 200));

NpgsqlPath new_value = new NpgsqlPath(((NpgsqlPath)orig_value).Count);
foreach (NpgsqlPoint polygon_point in ((NpgsqlPath)orig_value)) {
    new_value.Add(new NpgsqlPoint(((NpgsqlPoint)polygon_point).X + 1, ((NpgsqlPoint)polygon_point).Y + 1));
}

return new_value;
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-path-null', 'increasePath1', increasePath(NULL::PATH) = '((1,1),(101,101),(201,201))'::PATH;

--------- POLYGON
CREATE OR REPLACE FUNCTION addPointToPolygon(orig_polygon POLYGON, new_point POINT) RETURNS POLYGON AS $$
if (orig_polygon == null)
    orig_polygon = new NpgsqlPolygon(new NpgsqlPoint(0, 0), new NpgsqlPoint(100, 100), new NpgsqlPoint(200, 200));

if (new_point == null)
    new_point = new NpgsqlPoint(0, 0);

int npts = ((NpgsqlPolygon)orig_polygon).Count;
NpgsqlPolygon new_polygon = new NpgsqlPolygon(npts+1);
for(int i = 0; i < npts; i++)
{
    new_polygon.Add(((NpgsqlPolygon)orig_polygon)[i]);
}
new_polygon.Add(((NpgsqlPoint)new_point));
return new_polygon;
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-polygon', 'addPointToPolygon1', addPointToPolygon(POLYGON '((1.5,2.75),(3.0,4.75),(5.0,5.0))', POINT '(6.5,8.8)') ~= POLYGON '((1.5,2.75),(3.0,4.75),(5.0,5.0),(6.5,8.8))';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-polygon-null', 'addPointToPolygon2', addPointToPolygon(NULL::POLYGON, NULL::POINT) ~= POLYGON '((0, 0),(100,100),(200,200),(0,0))';

--------- CIRCLE
CREATE OR REPLACE FUNCTION returnCircle(orig_circle CIRCLE) RETURNS CIRCLE AS $$
return orig_circle;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-circle', 'returnCircle', returnCircle(CIRCLE '2.5, 3.5, 12.78') ~= CIRCLE '<(2.5, 3.5), 12.78>';

CREATE OR REPLACE FUNCTION increaseCircle(orig_value CIRCLE) RETURNS CIRCLE AS $$
if (orig_value == null)
    orig_value = new NpgsqlCircle(new NpgsqlPoint(0, 0), 3);

NpgsqlCircle new_value = new NpgsqlCircle(((NpgsqlCircle)orig_value).Center, ((NpgsqlCircle)orig_value).Radius + 1);

return new_value;
$$ LANGUAGE plcsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-circle-null', 'increaseCircle1', increaseCircle(NULL::CIRCLE) = CIRCLE '<(0, 0), 4>';

--- POINT Arrays
CREATE OR REPLACE FUNCTION updateArrayPointIndex(values_array point[], desired point, index integer[]) RETURNS point[] AS $$
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-point-null-1array', 'updateArrayPointIndex1', CAST(updateArrayPointIndex(ARRAY[POINT(10.0,20.0), POINT(30.0,55.0), null::point, POINT(40.5,21.3)], POINT(31.43, 32.44), ARRAY[2]) AS TEXT) = CAST(ARRAY[POINT(10.0,20.0), POINT(30.0,55.0), POINT(31.43, 32.44), POINT(40.5,21.3)] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-point-null-2array-arraynull', 'updateArrayPointIndex2', CAST(updateArrayPointIndex(ARRAY[[null::point, null::point], [null::point, POINT(40.5,21.3)]], POINT(31.43, 32.44), ARRAY[1,0]) AS TEXT) = CAST(ARRAY[[null::point, null::point], [POINT(31.43, 32.44), POINT(40.5,21.3)]] AS TEXT);

CREATE OR REPLACE FUNCTION IncreasePoints(values_array point[]) RETURNS point[] AS $$
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    NpgsqlPoint orig_value = (NpgsqlPoint)flatten_values.GetValue(i);
    NpgsqlPoint new_value = new NpgsqlPoint(orig_value.X + 1, orig_value.Y + 1);

    flatten_values.SetValue((NpgsqlPoint)new_value, i);
}
return flatten_values;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-point-null-1array', 'IncreasePoints1', CAST(IncreasePoints(ARRAY[POINT(10.0,20.0), POINT(30.0,55.0), null::point, POINT(40.5,21.3)]) AS TEXT) = CAST(ARRAY[POINT(11.0,21.0), POINT(31.0,56.0), null::point, POINT(41.5,22.3)] AS TEXT);

CREATE OR REPLACE FUNCTION CreatePointMultidimensionalArray() RETURNS point[] AS $$
NpgsqlPoint objects_value = new NpgsqlPoint(2.4, 8.2);
NpgsqlPoint?[, ,] three_dimensional_array = new NpgsqlPoint?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-point-null-3array-arraynull', 'CreatePointMultidimensionalArray1', CAST(CreatePointMultidimensionalArray() AS TEXT) = CAST(ARRAY[[[POINT(2.4,8.2), POINT(2.4,8.2)], [null::point, null::point]], [[POINT(2.4,8.2), null::point], [POINT(2.4,8.2), POINT(2.4,8.2)]]] AS TEXT);

--- LINE Arrays
CREATE OR REPLACE FUNCTION updateArrayLineIndex(values_array LINE[], desired LINE, index integer[]) RETURNS LINE[] AS $$
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-line-1array', 'updateArrayLineIndex1', CAST(updateArrayLineIndex(ARRAY[LINE '{-1.5,2.75,-3.25}', LINE '{-1.5,2.75,-3.25}', null::LINE, LINE '{-1.5,2.75,-3.25}'], LINE '{-1.5,2.75,-3.25}', ARRAY[2]) AS TEXT) = CAST(ARRAY[LINE '{-1.5,2.75,-3.25}', LINE '{-1.5,2.75,-3.25}', LINE '{-1.5,2.75,-3.25}', LINE '{-1.5,2.75,-3.25}'] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-line-null-2array-arraynull', 'updateArrayLineIndex2', CAST(updateArrayLineIndex(ARRAY[[null::LINE, null::LINE], [null::LINE, LINE '{-1.5,2.75,-3.25}']], LINE '{-1.5,2.75,-3.25}', ARRAY[1,0]) AS TEXT) = CAST(ARRAY[[null::LINE, null::LINE], [LINE '{-1.5,2.75,-3.25}', LINE '{-1.5,2.75,-3.25}']] AS TEXT);

CREATE OR REPLACE FUNCTION IncreaseLines(values_array LINE[]) RETURNS LINE[] AS $$
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    NpgsqlLine orig_value = (NpgsqlLine)flatten_values.GetValue(i);
    NpgsqlLine new_value = new NpgsqlLine(orig_value.A + 1, orig_value.B + 1, orig_value.C + 1);

    flatten_values.SetValue((NpgsqlLine)new_value, i);
}
return flatten_values;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-line-1array', 'IncreaseLines1', CAST(IncreaseLines(ARRAY[LINE '{-4.5,5.75,-7.25}', LINE '{-46.5,32.75,-54.5}', null::LINE, LINE '{-1.5,2.75,-3.25}']) AS TEXT) = CAST(ARRAY[LINE '{-3.5,6.75,-6.25}', LINE '{-45.5,33.75,-53.5}', null::LINE, LINE '{-0.5,3.75,-2.25}'] AS TEXT);


CREATE OR REPLACE FUNCTION CreateLineMultidimensionalArray() RETURNS LINE[] AS $$
NpgsqlLine objects_value = new NpgsqlLine(2.4, 8.2, -32.43);
NpgsqlLine?[, ,] three_dimensional_array = new NpgsqlLine?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-line-3array', 'CreateLineMultidimensionalArray1', CAST(CreateLineMultidimensionalArray() AS TEXT) = CAST(ARRAY[[[LINE '{2.4,8.2,-32.43}', LINE '{2.4,8.2,-32.43}'], [null::LINE, null::LINE]], [[LINE '{2.4,8.2,-32.43}', null::LINE], [LINE '{2.4,8.2,-32.43}', LINE '{2.4,8.2,-32.43}']]] AS TEXT);

--- LSEG Arrays

CREATE OR REPLACE FUNCTION updateArrayLSEGIndex(values_array LSEG[], desired LSEG, index integer[]) RETURNS LSEG[] AS $$
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-lseg-null-1array', 'updateArrayLSEGIndex1', CAST(updateArrayLSEGIndex(ARRAY[LSEG(POINT(0.0,1.0),POINT(5.0,3.0)), LSEG(POINT(-5.0,4.5),POINT(6.7,12.3)), null::LSEG, LSEG(POINT(0.0,1.0),POINT(4.7,9.2))], LSEG(POINT(0.0,1.0),POINT(4.7,9.2)), ARRAY[2]) AS TEXT) = CAST(ARRAY[LSEG(POINT(0.0,1.0),POINT(5.0,3.0)), LSEG(POINT(-5.0,4.5),POINT(6.7,12.3)), LSEG(POINT(0.0,1.0),POINT(4.7,9.2)), LSEG(POINT(0.0,1.0),POINT(4.7,9.2))] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-lseg-null-2array-arraynull', 'updateArrayLSEGIndex2', CAST(updateArrayLSEGIndex(ARRAY[[null::LSEG, null::LSEG], [null::LSEG, LSEG(POINT(0.0,1.0),POINT(4.7,9.2))]], LSEG(POINT(0.0,1.0),POINT(4.7,9.2)), ARRAY[1,0]) AS TEXT) = CAST(ARRAY[[null::LSEG, null::LSEG], [LSEG(POINT(0.0,1.0),POINT(4.7,9.2)), LSEG(POINT(0.0,1.0),POINT(4.7,9.2))]] AS TEXT);

CREATE OR REPLACE FUNCTION IncreaseLSEGs(values_array LSEG[]) RETURNS LSEG[] AS $$
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    NpgsqlLSeg orig_value = (NpgsqlLSeg)flatten_values.GetValue(i);
    NpgsqlLSeg new_value = new NpgsqlLSeg(new NpgsqlPoint(orig_value.Start.X + 1, orig_value.Start.Y + 1), new NpgsqlPoint(orig_value.End.X + 1, orig_value.End.Y + 1));

    flatten_values.SetValue((NpgsqlLSeg)new_value, i);
}
return flatten_values;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-lseg-null-1array', 'IncreaseLSEGs1', CAST(IncreaseLSEGs(ARRAY[LSEG(POINT(0.0,1.0),POINT(5.0,3.0)), LSEG(POINT(-5.0,4.5),POINT(6.7,12.3)), null::LSEG, LSEG(POINT(0.0,1.0),POINT(4.7,9.2))]) AS TEXT) = CAST(ARRAY[LSEG(POINT(1.0,2.0),POINT(6.0,4.0)), LSEG(POINT(-4.0,5.5),POINT(7.7,13.3)), null::LSEG, LSEG(POINT(1.0,2.0),POINT(5.7,10.2))] AS TEXT);


CREATE OR REPLACE FUNCTION CreateLSEGMultidimensionalArray() RETURNS LSEG[] AS $$
NpgsqlLSeg objects_value = new NpgsqlLSeg(new NpgsqlPoint(25.4, -54.2), new NpgsqlPoint(78.3, 122.31));
NpgsqlLSeg?[, ,] three_dimensional_array = new NpgsqlLSeg?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-lseg-null-3array-arraynull', 'CreateLSEGMultidimensionalArray1', CAST(CreateLSEGMultidimensionalArray() AS TEXT) = CAST(ARRAY[[[LSEG(POINT(25.4,-54.2),POINT(78.3,122.31)), LSEG(POINT(25.4,-54.2),POINT(78.3,122.31))], [null::LSEG, null::LSEG]], [[LSEG(POINT(25.4,-54.2),POINT(78.3,122.31)), null::LSEG], [LSEG(POINT(25.4,-54.2),POINT(78.3,122.31)), LSEG(POINT(25.4,-54.2),POINT(78.3,122.31))]]] AS TEXT);

--- BOX Arrays

CREATE OR REPLACE FUNCTION updateArrayBoxIndex(values_array BOX[], desired BOX, index integer[]) RETURNS BOX[] AS $$
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-box-null-1array', 'updateArrayBoxIndex1', CAST(updateArrayBoxIndex(ARRAY[BOX(POINT(0.0,1.0),POINT(5.0,3.0)), BOX(POINT(-5.0,4.5),POINT(6.7,12.3)), null::BOX, BOX(POINT(0.0,1.0),POINT(4.7,9.2))], BOX(POINT(0.0,1.0),POINT(4.7,9.2)), ARRAY[2]) AS TEXT) = CAST(ARRAY[BOX(POINT(0.0,1.0),POINT(5.0,3.0)), BOX(POINT(-5.0,4.5),POINT(6.7,12.3)), BOX(POINT(0.0,1.0),POINT(4.7,9.2)), BOX(POINT(0.0,1.0),POINT(4.7,9.2))] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-box-null-2array-arraynull', 'updateArrayBoxIndex2', CAST(updateArrayBoxIndex(ARRAY[[null::BOX, null::BOX], [null::BOX, BOX(POINT(0.0,1.0),POINT(4.7,9.2))]], BOX(POINT(0.0,1.0),POINT(4.7,9.2)), ARRAY[1,0]) AS TEXT) = CAST(ARRAY[[null::BOX, null::BOX], [BOX(POINT(0.0,1.0),POINT(4.7,9.2)), BOX(POINT(0.0,1.0),POINT(4.7,9.2))]] AS TEXT);

CREATE OR REPLACE FUNCTION IncreaseBoxs(values_array BOX[]) RETURNS BOX[] AS $$
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    NpgsqlBox orig_value = (NpgsqlBox)flatten_values.GetValue(i);
    NpgsqlBox new_value = new NpgsqlBox(new NpgsqlPoint(orig_value.UpperRight.X + 1, orig_value.UpperRight.Y + 1), new NpgsqlPoint(orig_value.LowerLeft.X + 1, orig_value.LowerLeft.Y + 1));

    flatten_values.SetValue((NpgsqlBox)new_value, i);
}
return flatten_values;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-box-null-1array', 'IncreaseBoxs1', CAST(IncreaseBoxs(ARRAY[BOX(POINT(0.0,1.0),POINT(5.0,3.0)), BOX(POINT(-5.0,4.5),POINT(6.7,12.3)), null::BOX, BOX(POINT(0.0,1.0),POINT(4.7,9.2))]) AS TEXT) = CAST(ARRAY[BOX(POINT(1.0,2.0),POINT(6.0,4.0)), BOX(POINT(-4.0,5.5),POINT(7.7,13.3)), null::BOX, BOX(POINT(1.0,2.0),POINT(5.7,10.2))] AS TEXT);


CREATE OR REPLACE FUNCTION CreateBoxMultidimensionalArray() RETURNS BOX[] AS $$
NpgsqlBox objects_value = new NpgsqlBox(new NpgsqlPoint(25.4, -54.2), new NpgsqlPoint(78.3, 122.31));
NpgsqlBox?[, ,] three_dimensional_array = new NpgsqlBox?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-box-null-3array-arraynull', 'CreateBoxMultidimensionalArray1', CAST(CreateBoxMultidimensionalArray() AS TEXT) = CAST(ARRAY[[[BOX(POINT(25.4,-54.2),POINT(78.3,122.31)), BOX(POINT(25.4,-54.2),POINT(78.3,122.31))], [null::BOX, null::BOX]], [[BOX(POINT(25.4,-54.2),POINT(78.3,122.31)), null::BOX], [BOX(POINT(25.4,-54.2),POINT(78.3,122.31)), BOX(POINT(25.4,-54.2),POINT(78.3,122.31))]]] AS TEXT);

--- PATH Arrays
CREATE OR REPLACE FUNCTION updateArrayPathIndex(values_array PATH[], desired PATH, index integer[]) RETURNS PATH[] AS $$
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-path-null-1array', 'updateArrayPathIndex1', CAST(updateArrayPathIndex(ARRAY['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, null::path, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH], '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, ARRAY[2]) AS TEXT) = CAST(ARRAY['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-path-null-2array-arraynull', 'updateArrayPathIndex2', CAST(updateArrayPathIndex(ARRAY[[null::PATH, null::PATH], [null::path, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH]], '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, ARRAY[1,0]) AS TEXT) = CAST(ARRAY[[null::PATH, null::PATH], ['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH]] AS TEXT);

CREATE OR REPLACE FUNCTION IncreasePaths(values_array PATH[]) RETURNS PATH[] AS $$
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    NpgsqlPath orig_value = (NpgsqlPath)flatten_values.GetValue(i);

    NpgsqlPath new_value = new NpgsqlPath(orig_value.Count);
    foreach (NpgsqlPoint polygon_point in orig_value) {
        new_value.Add(new NpgsqlPoint(polygon_point.X + 1, polygon_point.Y + 1));
    }

    flatten_values.SetValue((NpgsqlPath)new_value, i);
}
return flatten_values;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-path-null-1array', 'IncreasePaths1', CAST(IncreasePaths(ARRAY['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, null::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH]) AS TEXT) = CAST(ARRAY['((2.5,3.75),(4.0,5.75),(6.0,6.0))'::PATH, '((2.5,3.75),(4.0,5.75),(6.0,6.0))'::PATH, null::PATH, '((2.5,3.75),(4.0,5.75),(6.0,6.0))'::PATH] AS TEXT);

CREATE OR REPLACE FUNCTION CreatePathMultidimensionalArray() RETURNS PATH[] AS $$
NpgsqlPath objects_value = new NpgsqlPath(new NpgsqlPoint(1.5, 2.75), new NpgsqlPoint(3.0, 4.75), new NpgsqlPoint(5.0, 5.0));
NpgsqlPath?[, ,] three_dimensional_array = new NpgsqlPath?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-path-null-3array-arraynull', 'CreatePathMultidimensionalArray1', CAST(CreatePathMultidimensionalArray() AS TEXT) = CAST(ARRAY[[['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH], [null::PATH, null::PATH]], [['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, null::PATH], ['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH]]] AS TEXT);

--- POLYGON Arrays

CREATE OR REPLACE FUNCTION updateArrayPolygonIndex(values_array POLYGON[], desired POLYGON, index integer[]) RETURNS POLYGON[] AS $$
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-polygon-null-1array', 'updateArrayPolygonIndex1', CAST(updateArrayPolygonIndex(ARRAY['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, null::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON], '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, ARRAY[2]) AS TEXT) = CAST(ARRAY['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-polygon-null-2array-arraynull', 'updateArrayPolygonIndex2', CAST(updateArrayPolygonIndex(ARRAY[[null::POLYGON, null::POLYGON], [null::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON]], '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, ARRAY[1,0]) AS TEXT) = CAST(ARRAY[[null::POLYGON, null::POLYGON], ['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON]] AS TEXT);

CREATE OR REPLACE FUNCTION IncreasePolygons(values_array POLYGON[]) RETURNS POLYGON[] AS $$
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    NpgsqlPolygon orig_value = (NpgsqlPolygon)flatten_values.GetValue(i);

    NpgsqlPolygon new_value = new NpgsqlPolygon(orig_value.Count);
    foreach (NpgsqlPoint polygon_point in orig_value) {
        new_value.Add(new NpgsqlPoint(polygon_point.X + 1, polygon_point.Y + 1));
    }

    flatten_values.SetValue((NpgsqlPolygon)new_value, i);
}
return flatten_values;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-polygon-null-1array', 'IncreasePolygons1', CAST(IncreasePolygons(ARRAY['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, null::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON]) AS TEXT) = CAST(ARRAY['((2.5,3.75),(4.0,5.75),(6.0,6.0))'::POLYGON, '((2.5,3.75),(4.0,5.75),(6.0,6.0))'::POLYGON, null::POLYGON, '((2.5,3.75),(4.0,5.75),(6.0,6.0))'::POLYGON] AS TEXT);


CREATE OR REPLACE FUNCTION CreatePolygonMultidimensionalArray() RETURNS POLYGON[] AS $$
NpgsqlPolygon objects_value = new NpgsqlPolygon(new NpgsqlPoint(1.5, 2.75), new NpgsqlPoint(3.0, 4.75), new NpgsqlPoint(5.0, 5.0));
NpgsqlPolygon?[, ,] three_dimensional_array = new NpgsqlPolygon?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-polygon-null-3array-arraynull', 'CreatePolygonMultidimensionalArray1', CAST(CreatePolygonMultidimensionalArray() AS TEXT) = CAST(ARRAY[[['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON], [null::POLYGON, null::POLYGON]], [['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, null::POLYGON], ['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON]]] AS TEXT);

--- CIRCLE Arrays

CREATE OR REPLACE FUNCTION updateArrayCircleIndex(values_array CIRCLE[], desired CIRCLE, index integer[]) RETURNS CIRCLE[] AS $$
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-circle-null-1array', 'updateArrayCircleIndex1', CAST(updateArrayCircleIndex(ARRAY[CIRCLE(POINT(0.0,1.0), 2.5), CIRCLE(POINT(-5.0,4.5), 4), null::CIRCLE, CIRCLE(POINT(0.0,1.0),4.5)], CIRCLE(POINT(0.0,1.0), 2), ARRAY[2]) AS TEXT) = CAST(ARRAY[CIRCLE(POINT(0.0,1.0), 2.5), CIRCLE(POINT(-5.0,4.5), 4), CIRCLE(POINT(0.0,1.0), 2), CIRCLE(POINT(0.0,1.0),4.5)] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-circle-null-2array-arraynull', 'updateArrayCircleIndex2', CAST(updateArrayCircleIndex(ARRAY[[null::CIRCLE, null::CIRCLE], [null::CIRCLE, CIRCLE(POINT(0.0,1.0),4.5)]], CIRCLE(POINT(0.0,1.0), 2), ARRAY[1,0]) AS TEXT) = CAST(ARRAY[[null::CIRCLE, null::CIRCLE], [CIRCLE(POINT(0.0,1.0), 2), CIRCLE(POINT(0.0,1.0),4.5)]] AS TEXT);

CREATE OR REPLACE FUNCTION IncreaseCircles(values_array CIRCLE[]) RETURNS CIRCLE[] AS $$
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    NpgsqlCircle orig_value = (NpgsqlCircle)flatten_values.GetValue(i);
    NpgsqlCircle new_value = new NpgsqlCircle(orig_value.Center, orig_value.Radius + 1);

    flatten_values.SetValue((NpgsqlCircle)new_value, i);
}
return flatten_values;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-circle-null-1array', 'IncreaseCircles1', CAST(IncreaseCircles(ARRAY[CIRCLE(POINT(0.0,1.0), 2.5), CIRCLE(POINT(-5.0,4.5), 4), null::CIRCLE, CIRCLE(POINT(0.0,1.0),4.5)]) AS TEXT) = CAST(ARRAY[CIRCLE(POINT(0.0,1.0), 3.5), CIRCLE(POINT(-5.0,4.5), 5), null::CIRCLE, CIRCLE(POINT(0.0,1.0),5.5)] AS TEXT);


CREATE OR REPLACE FUNCTION CreateCircleMultidimensionalArray() RETURNS CIRCLE[] AS $$
NpgsqlCircle objects_value = new NpgsqlCircle(new NpgsqlPoint(25.4, -54.2), 3);
NpgsqlCircle?[, ,] three_dimensional_array = new NpgsqlCircle?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-circle-null-3array-arraynull', 'CreateCircleMultidimensionalArray1', CAST(CreateCircleMultidimensionalArray() AS TEXT) = CAST(ARRAY[[[CIRCLE(POINT(25.4,-54.2),3), CIRCLE(POINT(25.4,-54.2),3)], [null::CIRCLE, null::CIRCLE]], [[CIRCLE(POINT(25.4,-54.2),3), null::CIRCLE], [CIRCLE(POINT(25.4,-54.2),3), CIRCLE(POINT(25.4,-54.2),3)]]] AS TEXT);
