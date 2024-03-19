--------- POINT
CREATE OR REPLACE FUNCTION middlePointFSharp(pointa point, pointb point) RETURNS point AS $$
let pointa = if pointa.HasValue then pointa.Value else  NpgsqlPoint(0.0, 0.0)
let pointb = if pointb.HasValue then pointb.Value else  NpgsqlPoint(0.0, 0.0)
let x = (pointa.X + pointb.X) * 0.5
let y = (pointa.Y + pointb.Y) * 0.5
let new_point = NpgsqlPoint(x, y)
new_point
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-point', 'middlePointFSharp1',  middlePointFSharp(POINT(10.0,20.0),POINT(20.0,40.0)) ~= POINT(15.0,30.0);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-point-null', 'middlePointFSharp2',  middlePointFSharp(NULL::POINT,POINT(20.0,40.0)) ~= POINT(10.0,20.0);

CREATE OR REPLACE FUNCTION distanceBetweenPointsFSharp(pointa point, pointb point) RETURNS float8 AS $$
let pointa = if pointa.HasValue then pointa.Value else  NpgsqlPoint(0.0, 0.0)
let pointb = if pointb.HasValue then pointb.Value else  NpgsqlPoint(0.0, 0.0)
let dif_x = pointa.X - pointb.X
let dif_y = pointa.Y - pointb.Y
let distance = Math.Sqrt(dif_x * dif_x + dif_y * dif_y)
distance
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-point', 'distanceBetweenPointsFSharp',  distanceBetweenPointsFSharp(POINT(1.5,2.75), POINT(3.0,4.75)) = float8 '2.5';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-point-null', 'distanceBetweenPointsFSharp',  distanceBetweenPointsFSharp(POINT(3.0,4.0), NULL::POINT) = float8 '5';

CREATE OR REPLACE FUNCTION checkPointsFSharp(pointa point, pointb point) RETURNS boolean AS $$
    pointa.X = pointb.X && pointa.Y = pointb.Y
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-point', 'checkPointsFSharp1',  checkPointsFSharp(POINT(2.555701574,8.7552345789),POINT(2.555701574,8.7552345789)) is true;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-point', 'checkPointsFSharp2',  checkPointsFSharp(POINT(2.555701574,8.7552345789),POINT(2.555701574,8.7552345785)) is false;

-- --------- LINE
CREATE OR REPLACE FUNCTION createLineFSharp(a float8, b float8, c float8) RETURNS LINE AS $$
    NpgsqlLine(a, b, c)
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-line', 'createLineFSharp', createLineFSharp(1.50,-2.750,3.25) = LINE '{1.50,-2.750,3.25}';

CREATE OR REPLACE FUNCTION modifyCoefficientsFSharp(original_line LINE) RETURNS LINE AS $$
    if original_line.HasValue then
        let a = original_line.Value.A * -1.0
        let b = original_line.Value.B * -1.0
        let c = original_line.Value.C * -1.0
        NpgsqlLine(a, b, c)
    else
        NpgsqlLine(0,0,0)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-line', 'modifyCoefficientsFSharp1', modifyCoefficientsFSharp(LINE '{-1.5,2.75,-3.25}') = LINE '{1.50,-2.75,3.25}';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-line-null', 'modifyCoefficientsFSharp2', modifyCoefficientsFSharp(NULL::LINE) = LINE '{2.4, 8.2, -32.43}';

CREATE OR REPLACE FUNCTION getMinimumDistanceFSharp(orig_line LINE, orig_point POINT) RETURNS float8 AS $$
    let a = orig_line.A
    let b = orig_line.B
    let c = orig_line.C
    Math.Abs((a*orig_point.X + b*orig_point.Y + c)/Math.Sqrt(a*a+b*b))
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-line', 'getMinimumDistanceFSharp', getMinimumDistanceFSharp(LINE '{4.0, 6.0, 2.0}', POINT(3.0,-6.0)) = float8 '3.05085107923876';

--------- LSEG
CREATE OR REPLACE FUNCTION createLineSegmentFSharp(start_point POINT, end_point POINT) RETURNS LSEG AS $$
NpgsqlLSeg(start_point, end_point)
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-lseg', 'createLineSegmentFSharp', createLineSegmentFSharp(POINT(0.088997,1.258456),POINT(5.456102,3.04561)) = LSEG '[(0.088997,1.258456),(5.456102,3.04561)]';

CREATE OR REPLACE FUNCTION getReverseLineSegmentFSharp(my_line LSEG) RETURNS LSEG AS $$
    let my_line = if my_line.HasValue then my_line.Value else NpgsqlLSeg(NpgsqlPoint(0, 0), NpgsqlPoint(100, 100))

    let firstPoint = my_line.Start
    let secondPoint = my_line.End
    let newLine = new NpgsqlLSeg(secondPoint, firstPoint)
    newLine
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-lseg', 'getReverseLineSegmentFSharp1', getReverseLineSegmentFSharp(LSEG(POINT(0.0,1.0),POINT(5.0,3.0))) = LSEG '[(5.0,3.0),(0.0,1.0)]';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-lseg-null', 'getReverseLineSegmentFSharp2', getReverseLineSegmentFSharp(NULL::LSEG) = LSEG '[(100.0,100.0),(0.0,0.0)]';

-- --------- BOX
CREATE OR REPLACE FUNCTION testBoxFSharp(my_box BOX) RETURNS BOX AS $$
my_box
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-box', 'testBoxFSharp', testBoxFSharp(BOX '(0.025988, 1.021653), (2.052787, 3.005716)') = BOX '(0.025988, 1.021653), (2.052787, 3.005716)';

CREATE OR REPLACE FUNCTION createBoxFSharp(high POINT, low POINT) RETURNS BOX AS $$
NpgsqlBox(high, low)
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-box', 'createBoxFSharp', createBoxFSharp(POINT '(2.052787, 3.005716)', POINT '(0.025988, 1.021653)') = BOX '(2.052787, 3.005716), (0.025988, 1.021653)';

CREATE OR REPLACE FUNCTION returnWidthFSharp(high POINT, low POINT) RETURNS float8 AS $$
    let new_box = NpgsqlBox(high, low)
    Math.Abs(new_box.Width)
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-box', 'returnWidthFSharp', returnWidthFSharp(POINT '(0.025988, 1.021653)', POINT '(2.052787, 3.005716)') = float8 '2.026799';

CREATE OR REPLACE FUNCTION increaseBoxFSharp(orig_value BOX) RETURNS BOX AS $$
    if orig_value.HasValue then
        NpgsqlBox(
            NpgsqlPoint((orig_value.Value).UpperRight.X + 1.0, (orig_value.Value).UpperRight.Y + 1.0),
            NpgsqlPoint((orig_value.Value).LowerLeft.X + 1.0, (orig_value.Value).LowerLeft.Y + 1.0))
    else
        NpgsqlBox(NpgsqlPoint(0, 0), NpgsqlPoint(100, 100))
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-box', 'increaseBoxFSharp1', increaseBoxFSharp(BOX(POINT(100,100),POINT(1,1))) = BOX(POINT(101,101),POINT(2,2));
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-box-null', 'increaseBoxFSharp1', increaseBoxFSharp(NULL::BOX) = BOX(POINT(101,101),POINT(1,1));

--------- PATH
CREATE OR REPLACE FUNCTION returnPathFSharp(orig_path PATH) RETURNS PATH AS $$
orig_path
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-path', 'returnPathFSharp - open', returnPathFSharp(PATH '[(1.5,2.75),(3.0,4.75),(5.0,5.0)]') <= PATH '[(1.5,2.75),(3.0,4.75),(5.0,5.0)]';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-path', 'returnPathFSharp - close', returnPathFSharp(PATH '((1.5,2.75),(3.0,4.75),(5.0,5.0))') <= PATH '((1.5,2.75),(3.0,4.75),(5.0,5.0))';

CREATE OR REPLACE FUNCTION increasePathFSharp(orig_value PATH) RETURNS PATH AS $$
let orig_value = if orig_value.HasValue then orig_value.Value else NpgsqlPath(NpgsqlPoint(0, 0), NpgsqlPoint(100, 100), NpgsqlPoint(200, 200))
let new_value = NpgsqlPath(orig_value.Count)
for polygon_point in orig_value do
    new_value.Add(NpgsqlPoint(polygon_point.X + 1.0, polygon_point.Y + 1.0));
new_value
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-path', 'increasePathFSharp1', increasePathFSharp('((1,1),(101,101),(201,201))'::PATH) = '((2,2),(102,102),(202,202))'::PATH;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-path-null', 'increasePathFSharp2', increasePathFSharp(NULL::PATH) = '((1,1),(101,101),(201,201))'::PATH;

-- --------- POLYGON
CREATE OR REPLACE FUNCTION addPointToPolygonFSharp(orig_polygon POLYGON, new_point POINT) RETURNS POLYGON AS $$

    let mutable polygon = if orig_polygon.HasValue then orig_polygon.Value else NpgsqlPolygon([| NpgsqlPoint(0.0, 0.0); NpgsqlPoint(100.0, 100.0); NpgsqlPoint(200.0, 200.0) |])
    let mutable point = if new_point.HasValue then new_point.Value else NpgsqlPoint(0.0, 0.0)
    let npts = polygon.Count
    let mutable new_polygon = NpgsqlPolygon(npts+1)
    for i in 0 .. npts-1 do
        new_polygon.Add(polygon.[i])
    new_polygon.Add(point)
    new_polygon
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-polygon', 'addPointToPolygonFSharp1', addPointToPolygonFSharp(POLYGON '((1.5,2.75),(3.0,4.75),(5.0,5.0))', POINT '(6.5,8.8)') ~= POLYGON '((1.5,2.75),(3.0,4.75),(5.0,5.0),(6.5,8.8))';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-polygon-null', 'addPointToPolygonFSharp2', addPointToPolygonFSharp(NULL::POLYGON, NULL::POINT) ~= POLYGON '((0, 0),(100,100),(200,200),(0,0))';

--------- CIRCLE
CREATE OR REPLACE FUNCTION returnCircleFSharp(orig_circle CIRCLE) RETURNS CIRCLE AS $$
orig_circle
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-circle', 'returnCircleFSharp', returnCircleFSharp(CIRCLE '2.5, 3.5, 12.78') ~= CIRCLE '<(2.5, 3.5), 12.78>';

CREATE OR REPLACE FUNCTION increaseCircleFSharp(orig_value CIRCLE) RETURNS CIRCLE AS $$
let orig_value = if orig_value.HasValue then orig_value.Value else NpgsqlCircle(NpgsqlPoint(0, 0), 3)
NpgsqlCircle(orig_value.Center, (orig_value.Radius + 1.0))
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-circle-null', 'increaseCircleFSharp1', increaseCircleFSharp(NULL::CIRCLE) = CIRCLE '<(0, 0), 4>';

--- POINT Arrays
CREATE OR REPLACE FUNCTION updateArrayPointIndexFSharp(a point[], b point) RETURNS point[] AS $$
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
SELECT 'f#-point-null-1array', 'updateArrayPointIndexFSharp1', CAST(updateArrayPointIndexFSharp(ARRAY[POINT(10.0,20.0), POINT(30.0,55.0), null::point, POINT(40.5,21.3)], POINT(31.43, 32.44)) AS TEXT) = CAST(ARRAY[POINT(31.43, 32.44), POINT(30.0,55.0), null::point, POINT(40.5,21.3)] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-point-null-2array-arraynull', 'updateArrayPointIndexFSharp2', CAST(updateArrayPointIndexFSharp(ARRAY[[null::point, null::point], [null::point, POINT(40.5,21.3)]], POINT(31.43, 32.44)) AS TEXT) = CAST(ARRAY[[POINT(31.43, 32.44), null::point], [null::point, POINT(40.5,21.3)]] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-point-null-3array-arraynull', 'updateArrayPointIndexFSharp3', CAST(updateArrayPointIndexFSharp(ARRAY[[[null::point, null::point], [null::point, POINT(40.5,21.3)]]], POINT(31.43, 32.44)) AS TEXT) = CAST(ARRAY[[[POINT(31.43, 32.44), null::point], [null::point, POINT(40.5,21.3)]]] AS TEXT);

CREATE OR REPLACE FUNCTION CreatePointMultidimensionalArrayFSharp() RETURNS point[] AS $$
let arr = Array.CreateInstance(typeof<NpgsqlPoint>, 3, 3, 1)
let objects_value = NpgsqlPoint(2.4, 8.2)
arr.SetValue(objects_value, 0, 0, 0)
arr.SetValue(objects_value, 1, 1, 0)
arr.SetValue(objects_value, 2, 2, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-point-null-3array-arraynull', 'CreatePointMultidimensionalArrayFSharp1', CAST(CreatePointMultidimensionalArrayFSharp() AS TEXT) = CAST(ARRAY[[[POINT(2.4,8.2)], [POINT(0,0)], [POINT(0,0)]], [[POINT(0,0)], [POINT(2.4,8.2)], [POINT(0,0)]], [[POINT(0,0)], [POINT(0,0)], [POINT(2.4,8.2)]]] AS TEXT);

-- --- LINE Arrays
CREATE OR REPLACE FUNCTION updateArrayLineIndexFSharp(A LINE[], b LINE) RETURNS LINE[] AS $$
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
SELECT 'f#-line-1array', 'updateArrayLineIndexFSharp1', CAST(updateArrayLineIndexFSharp(ARRAY[LINE '{-1.5,2.75,-3.25}', LINE '{-1.5,2.75,-3.25}', null::LINE, LINE '{-1.5,2.75,-3.25}'], LINE '{-1.5,2.75,-3.25}') AS TEXT) = CAST(ARRAY[LINE '{-1.5,2.75,-3.25}', LINE '{-1.5,2.75,-3.25}', null::LINE, LINE '{-1.5,2.75,-3.25}'] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-line-null-2array-arraynull', 'updateArrayLineIndexFSharp2', CAST(updateArrayLineIndexFSharp(ARRAY[[null::LINE, null::LINE], [null::LINE, LINE '{-1.5,2.75,-3.25}']], LINE '{-1.5,2.75,-3.25}') AS TEXT) = CAST(ARRAY[[LINE '{-1.5,2.75,-3.25}', null::LINE], [null::LINE, LINE '{-1.5,2.75,-3.25}']] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-line-null-3array-arraynull', 'updateArrayLineIndexFSharp3', CAST(updateArrayLineIndexFSharp(ARRAY[[[null::LINE, null::LINE], [null::LINE, LINE '{-1.5,2.75,-3.25}']]], LINE '{-1.5,2.75,-3.25}') AS TEXT) = CAST(ARRAY[[[LINE '{-1.5,2.75,-3.25}', null::LINE], [null::LINE, LINE '{-1.5,2.75,-3.25}']]] AS TEXT);

CREATE OR REPLACE FUNCTION IncreaseLinesFSharp(values_array LINE[]) RETURNS LINE[] AS $$
    let flatten_values = Array.CreateInstance(typeof<NpgsqlLine>, values_array.Length)
    ArrayManipulation.FlatArray(values_array, ref flatten_values) |> ignore
    for i in 0 .. flatten_values.Length - 1 do
        if System.Object.ReferenceEquals(flatten_values.GetValue(i), null) then
            ()
        else
            let orig_value = flatten_values.GetValue(i) :?> NpgsqlLine
            let new_value = NpgsqlLine(orig_value.A + 1., orig_value.B + 1., orig_value.C + 1.)
            flatten_values.SetValue(new_value, i)
    flatten_values
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-line-1array', 'IncreaseLinesFSharp1', CAST(IncreaseLinesFSharp(ARRAY[LINE '{-4.5,5.75,-7.25}', LINE '{-46.5,32.75,-54.5}', null::LINE, LINE '{-1.5,2.75,-3.25}']) AS TEXT) = CAST(ARRAY[LINE '{-3.5,6.75,-6.25}', LINE '{-45.5,33.75,-53.5}', LINE '{1,1,1}', LINE '{-0.5,3.75,-2.25}'] AS TEXT);

CREATE OR REPLACE FUNCTION CreateLineMultidimensionalArrayFSharp() RETURNS LINE[] AS $$
let arr = Array.CreateInstance(typeof<NpgsqlLine>, 3, 3, 1)
let objects_value = NpgsqlLine(2.4,8.2,-32.43)
arr.SetValue(objects_value, 0, 0, 0)
arr.SetValue(objects_value, 1, 1, 0)
arr.SetValue(objects_value, 2, 2, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-line-3array', 'CreateLineMultidimensionalArrayFSharp1', CAST(CreateLineMultidimensionalArrayFSharp() AS TEXT) = '{{{"{2.4,8.2,-32.43}"},{"{0,0,0}"},{"{0,0,0}"}},{{"{0,0,0}"},{"{2.4,8.2,-32.43}"},{"{0,0,0}"}},{{"{0,0,0}"},{"{0,0,0}"},{"{2.4,8.2,-32.43}"}}}';

--- LSEG Arrays

CREATE OR REPLACE FUNCTION updateArrayLSEGIndexFSharp(a LSEG[], b LSEG) RETURNS LSEG[] AS $$
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
SELECT 'f#-lseg-null-1array', 'updateArrayLSEGIndexFSharp1', CAST(updateArrayLSEGIndexFSharp(ARRAY[LSEG(POINT(0.0,1.0),POINT(5.0,3.0)), LSEG(POINT(-5.0,4.5),POINT(6.7,12.3)), null::LSEG, LSEG(POINT(0.0,1.0),POINT(4.7,9.2))], LSEG(POINT(0.0,1.0),POINT(4.7,9.2))) AS TEXT) = CAST(ARRAY[LSEG(POINT(0.0,1.0),POINT(4.7,9.2)), LSEG(POINT(-5.0,4.5),POINT(6.7,12.3)), null::LSEG, LSEG(POINT(0.0,1.0),POINT(4.7,9.2))] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-lseg-null-2array-arraynull', 'updateArrayLSEGIndexFSharp2', CAST(updateArrayLSEGIndexFSharp(ARRAY[[null::LSEG, null::LSEG], [null::LSEG, LSEG(POINT(0.0,1.0),POINT(4.7,9.2))]], LSEG(POINT(0.0,1.0),POINT(4.7,9.2))) AS TEXT) = CAST(ARRAY[[LSEG(POINT(0.0,1.0),POINT(4.7,9.2)), null::LSEG], [null::LSEG, LSEG(POINT(0.0,1.0),POINT(4.7,9.2))]] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-lseg-null-3array-arraynull', 'updateArrayLSEGIndexFSharp3', CAST(updateArrayLSEGIndexFSharp(ARRAY[[[null::LSEG, null::LSEG], [null::LSEG, LSEG(POINT(0.0,1.0),POINT(4.7,9.2))]]], LSEG(POINT(0.0,1.0),POINT(4.7,9.2))) AS TEXT) = CAST(ARRAY[[[LSEG(POINT(0.0,1.0),POINT(4.7,9.2)), null::LSEG], [null::LSEG, LSEG(POINT(0.0,1.0),POINT(4.7,9.2))]]] AS TEXT);

CREATE OR REPLACE FUNCTION IncreaseLSEGsFSharp(values_array LSEG[]) RETURNS LSEG[] AS $$
let flatten_values = Array.CreateInstance(typeof<NpgsqlLSeg>, values_array.Length)
ArrayManipulation.FlatArray(values_array, ref flatten_values) |> ignore
for i in 0 .. flatten_values.Length - 1 do
    if System.Object.ReferenceEquals(flatten_values.GetValue(i), null) then
        ()
    else
        let orig_value = flatten_values.GetValue(i) :?> NpgsqlLSeg
        let new_value = NpgsqlLSeg(NpgsqlPoint(orig_value.Start.X + 1., orig_value.Start.Y + 1.), NpgsqlPoint(orig_value.End.X + 1., orig_value.End.Y + 1.))
        flatten_values.SetValue(new_value, i)
flatten_values
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-lseg-null-1array', 'IncreaseLSEGsFSharp1', CAST(IncreaseLSEGsFSharp(ARRAY[LSEG(POINT(0.0,1.0),POINT(5.0,3.0)), LSEG(POINT(-5.0,4.5),POINT(6.7,12.3)), null::LSEG, LSEG(POINT(0.0,1.0),POINT(4.7,9.2))]) AS TEXT) = CAST(ARRAY[LSEG(POINT(1.0,2.0),POINT(6.0,4.0)), LSEG(POINT(-4.0,5.5),POINT(7.7,13.3)), LSEG(POINT(1,1),POINT(1,1)), LSEG(POINT(1.0,2.0),POINT(5.7,10.2))] AS TEXT);

CREATE OR REPLACE FUNCTION CreateLSEGMultidimensionalArrayFSharp() RETURNS LSEG[] AS $$
let arr = Array.CreateInstance(typeof<NpgsqlLSeg>, 3, 3, 1)
let objects_value = NpgsqlLSeg(NpgsqlPoint(25.4, -54.2), NpgsqlPoint(78.3, 122.31))
arr.SetValue(objects_value, 0, 0, 0)
arr.SetValue(objects_value, 1, 1, 0)
arr.SetValue(objects_value, 2, 2, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-lseg-null-3array-arraynull', 'CreateLSEGMultidimensionalArrayFSharp1', CAST(CreateLSEGMultidimensionalArrayFSharp() AS TEXT) = CAST(ARRAY[[[LSEG(POINT(25.4,-54.2),POINT(78.3,122.31))],[LSEG(POINT(0,0),POINT(0,0))],[LSEG(POINT(0,0),POINT(0,0))]],[[LSEG(POINT(0,0),POINT(0,0))],[LSEG(POINT(25.4,-54.2),POINT(78.3,122.31))],[LSEG(POINT(0,0),POINT(0,0))]],[[LSEG(POINT(0,0),POINT(0,0))],[LSEG(POINT(0,0),POINT(0,0))],[LSEG(POINT(25.4,-54.2),POINT(78.3,122.31))]]] AS TEXT);

-- --- BOX Arrays

CREATE OR REPLACE FUNCTION updateArrayBoxIndexFSharp(a BOX[], b BOX) RETURNS BOX[] AS $$
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
SELECT 'f#-box-null-1array', 'updateArrayBoxIndexFSharp1', CAST(updateArrayBoxIndexFSharp(ARRAY[BOX(POINT(0.0,1.0),POINT(5.0,3.0)), BOX(POINT(-5.0,4.5),POINT(6.7,12.3)), null::BOX, BOX(POINT(0.0,1.0),POINT(4.7,9.2))], BOX(POINT(0.0,1.0),POINT(4.7,9.2))) AS TEXT) = CAST(ARRAY[BOX(POINT(0.0,1.0),POINT(4.7,9.2)), BOX(POINT(-5.0,4.5),POINT(6.7,12.3)), null::BOX, BOX(POINT(0.0,1.0),POINT(4.7,9.2))] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-box-null-2array-arraynull', 'updateArrayBoxIndexFSharp2', CAST(updateArrayBoxIndexFSharp(ARRAY[[null::BOX, null::BOX], [null::BOX, BOX(POINT(0.0,1.0),POINT(4.7,9.2))]], BOX(POINT(0.0,1.0),POINT(4.7,9.2))) AS TEXT) = CAST(ARRAY[[BOX(POINT(0.0,1.0),POINT(4.7,9.2)), null::BOX], [null::BOX, BOX(POINT(0.0,1.0),POINT(4.7,9.2))]] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-box-null-3array-arraynull', 'updateArrayBoxIndexFSharp3', CAST(updateArrayBoxIndexFSharp(ARRAY[[[null::BOX, null::BOX], [null::BOX, BOX(POINT(0.0,1.0),POINT(4.7,9.2))]]], BOX(POINT(0.0,1.0),POINT(4.7,9.2))) AS TEXT) = CAST(ARRAY[[[BOX(POINT(0.0,1.0),POINT(4.7,9.2)), null::BOX], [null::BOX, BOX(POINT(0.0,1.0),POINT(4.7,9.2))]]] AS TEXT);

CREATE OR REPLACE FUNCTION IncreaseBoxsFSharp(values_array BOX[]) RETURNS BOX[] AS $$
let flatten_values = Array.CreateInstance(typeof<NpgsqlBox>, values_array.Length)
ArrayManipulation.FlatArray(values_array, ref flatten_values) |> ignore
for i in 0 .. flatten_values.Length - 1 do
    if System.Object.ReferenceEquals(flatten_values.GetValue(i), null) then
        ()
    else
        let orig_value = flatten_values.GetValue(i) :?> NpgsqlBox
        let new_value = NpgsqlBox(NpgsqlPoint(orig_value.UpperRight.X + 1., orig_value.UpperRight.Y + 1.), NpgsqlPoint(orig_value.LowerLeft.X + 1., orig_value.LowerLeft.Y + 1.))
        flatten_values.SetValue(new_value, i)
flatten_values
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-box-null-1array', 'IncreaseBoxsFSharp1', CAST(IncreaseBoxsFSharp(ARRAY[BOX(POINT(0.0,1.0),POINT(5.0,3.0)), BOX(POINT(-5.0,4.5),POINT(6.7,12.3)), null::BOX, BOX(POINT(0.0,1.0),POINT(4.7,9.2))]) AS TEXT) = CAST(ARRAY[BOX(POINT(1.0,2.0),POINT(6.0,4.0)), BOX(POINT(-4.0,5.5),POINT(7.7,13.3)), BOX(POINT(1,1),POINT(1,1)), BOX(POINT(1.0,2.0),POINT(5.7,10.2))] AS TEXT);

CREATE OR REPLACE FUNCTION CreateBoxMultidimensionalArrayFSharp() RETURNS BOX[] AS $$
let arr = Array.CreateInstance(typeof<NpgsqlBox>, 3, 3, 1)
let objects_value = NpgsqlBox(NpgsqlPoint(1.5, 2.75), NpgsqlPoint(3.0, 4.75))
arr.SetValue(objects_value, 0, 0, 0)
arr.SetValue(objects_value, 1, 1, 0)
arr.SetValue(objects_value, 2, 2, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-box-null-3array-arraynull', 'CreateBoxMultidimensionalArrayFSharp1', CAST(CreateBoxMultidimensionalArrayFSharp() AS TEXT) = '{{{(3,4.75),(1.5,2.75)};{(0,0),(0,0)};{(0,0),(0,0)}};{{(0,0),(0,0)};{(3,4.75),(1.5,2.75)};{(0,0),(0,0)}};{{(0,0),(0,0)};{(0,0),(0,0)};{(3,4.75),(1.5,2.75)}}}';

-- --- PATH Arrays
CREATE OR REPLACE FUNCTION updateArrayPathIndexFSharp(a PATH[], b PATH) RETURNS PATH[] AS $$
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
SELECT 'f#-path-null-1array', 'updateArrayPathIndexFSharp1', CAST(updateArrayPathIndexFSharp(ARRAY['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, null::path, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH], '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH) AS TEXT) = CAST(ARRAY['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, null::path, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-path-null-2array-arraynull', 'updateArrayPathIndexFSharp2', CAST(updateArrayPathIndexFSharp(ARRAY[[null::PATH, null::PATH], [null::path, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH]], '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH) AS TEXT) = CAST(ARRAY[['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, null::PATH], [null::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH]] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-path-null-3array-arraynull', 'updateArrayPathIndexFSharp3', CAST(updateArrayPathIndexFSharp(ARRAY[[[null::PATH, null::PATH], [null::path, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH]]], '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH) AS TEXT) = CAST(ARRAY[[['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, null::PATH], [null::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH]]] AS TEXT);

-- --- POLYGON Arrays

CREATE OR REPLACE FUNCTION updateArrayPolygonIndexFSharp(a POLYGON[], b POLYGON) RETURNS POLYGON[] AS $$
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
SELECT 'f#-polygon-null-1array', 'updateArrayPolygonIndexFSharp1', CAST(updateArrayPolygonIndexFSharp(ARRAY['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, null::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON], '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON) AS TEXT) = CAST(ARRAY['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, null::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-polygon-null-2array-arraynull', 'updateArrayPolygonIndexFSharp2', CAST(updateArrayPolygonIndexFSharp(ARRAY[[null::POLYGON, null::POLYGON], [null::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON]], '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON) AS TEXT) = CAST(ARRAY[['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, null::POLYGON], [null::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON]] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-polygon-null-3array-arraynull', 'updateArrayPolygonIndexFSharp3', CAST(updateArrayPolygonIndexFSharp(ARRAY[[[null::POLYGON, null::POLYGON], [null::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON]]], '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON) AS TEXT) = CAST(ARRAY[[['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, null::POLYGON], [null::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON]]] AS TEXT);

-- --- CIRCLE Arrays

CREATE OR REPLACE FUNCTION updateArrayCircleIndexFSharp(a CIRCLE[], b CIRCLE) RETURNS CIRCLE[] AS $$
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
SELECT 'f#-circle-null-1array', 'updateArrayCircleIndexFSharp1', CAST(updateArrayCircleIndexFSharp(ARRAY[CIRCLE(POINT(0.0,1.0), 2.5), CIRCLE(POINT(-5.0,4.5), 4), null::CIRCLE, CIRCLE(POINT(0.0,1.0),4.5)], CIRCLE(POINT(0.0,1.0), 2)) AS TEXT) = CAST(ARRAY[CIRCLE(POINT(0.0,1.0), 2), CIRCLE(POINT(-5.0,4.5), 4), null::CIRCLE, CIRCLE(POINT(0.0,1.0),4.5)] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-circle-null-2array-arraynull', 'updateArrayCircleIndexFSharp2', CAST(updateArrayCircleIndexFSharp(ARRAY[[null::CIRCLE, null::CIRCLE], [null::CIRCLE, CIRCLE(POINT(0.0,1.0),4.5)]], CIRCLE(POINT(0.0,1.0), 2)) AS TEXT) = CAST(ARRAY[[CIRCLE(POINT(0.0,1.0), 2), null::CIRCLE], [null::CIRCLE, CIRCLE(POINT(0.0,1.0),4.5)]] AS TEXT);
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-circle-null-3array-arraynull', 'updateArrayCircleIndexFSharp3', CAST(updateArrayCircleIndexFSharp(ARRAY[[[null::CIRCLE, null::CIRCLE], [null::CIRCLE, CIRCLE(POINT(0.0,1.0),4.5)]]], CIRCLE(POINT(0.0,1.0), 2)) AS TEXT) = CAST(ARRAY[[[CIRCLE(POINT(0.0,1.0), 2), null::CIRCLE], [null::CIRCLE, CIRCLE(POINT(0.0,1.0),4.5)]]] AS TEXT);
