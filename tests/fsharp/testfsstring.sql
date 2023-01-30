-- TEXT
CREATE OR REPLACE FUNCTION identityStrFSharp(a text) RETURNS text AS $$
    a
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-text', 'identityStrFSharp', identityStrFSharp('dog') = 'dog';

CREATE OR REPLACE FUNCTION concatenateTextFSharp(a text, b text) RETURNS text AS $$
    a + " " + b
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-text', 'concatenateTextFSharp1', concatenateTextFSharp('red', 'blue') = 'red blue';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-text-null', 'concatenateTextFSharp2', concatenateTextFSharp(NULL::TEXT, 'blue') = ' blue';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-text-null', 'concatenateTextFSharp3', concatenateTextFSharp(NULL::TEXT, NULL::TEXT) = ' ';

CREATE OR REPLACE FUNCTION multiplyTextFSharp(a text, b int) RETURNS text AS $$
    let i = 0
    let mutable c: string = ""
    for i in 1 .. b do
        c <- c + a
    c
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-text', 'multiplyTextFSharp', multiplyTextFSharp('dog ', 3) = 'dog dog dog ';

-- CHAR
CREATE OR REPLACE FUNCTION addGoodbyeFSharp(a BPCHAR) RETURNS BPCHAR AS $$
    a + " Goodbye ^.^"
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bpchar', 'testingBpCharFSharp', addGoodbyeFSharp('HELLO!') = 'HELLO! Goodbye ^.^'::BPCHAR;

CREATE OR REPLACE FUNCTION concatenateCharsFSharp(a BPCHAR, b BPCHAR, c BPCHAR) RETURNS BPCHAR AS $$
    (a + " " + b + " " + c).ToUpper()
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bpchar', 'concatenateCharsFSharp1', concatenateCharsFSharp('hello'::BPCHAR, 'beautiful'::BPCHAR, 'world!'::BPCHAR) = 'HELLO BEAUTIFUL WORLD!'::BPCHAR;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bpchar-null', 'concatenateCharsFSharp2', concatenateCharsFSharp(NULL::BPCHAR, 'beautiful'::BPCHAR, NULL::BPCHAR) = ' BEAUTIFUL '::BPCHAR;

-- VARCHAR
CREATE OR REPLACE FUNCTION concatenateVarCharsFSharp(a VARCHAR, b VARCHAR, c BPCHAR) RETURNS VARCHAR AS $$
    (a + " " + b + " " + c).ToUpper()
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-varchar', 'concatenateVarCharsFSharp1', concatenateVarCharsFSharp('hello'::VARCHAR, 'beautiful'::VARCHAR, 'world!'::BPCHAR) = 'HELLO BEAUTIFUL WORLD!'::VARCHAR;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-varchar-null', 'concatenateVarCharsFSharp2', concatenateVarCharsFSharp(NULL::VARCHAR, 'beautiful'::VARCHAR, NULL::BPCHAR) = ' BEAUTIFUL '::VARCHAR;

CREATE OR REPLACE FUNCTION multiplyVarCharFSharp(a VARCHAR, b int) RETURNS VARCHAR AS $$
    let mutable  c:string = ""
    let i = 0
    for i in 1..b do
        c <- c + a
    c.ToUpper()
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-varchar', 'multiplyVarCharFSharp', multiplyVarCharFSharp('hello '::VARCHAR, 5) = 'HELLO HELLO HELLO HELLO HELLO '::VARCHAR;

-- XML
CREATE OR REPLACE FUNCTION modifyXmlFSharp(a XML) RETURNS XML AS $$
    let mutable new_xml: string = ""
    if System.Object.ReferenceEquals(a, null) ||a.Equals("") then
        new_xml <- "<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Hello, World, it was null!</title>"
    else
        new_xml <- a
    new_xml <- (new_xml.Replace("Hello", "Goodbye")).Replace("World", "beautiful World")

    new_xml
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-xml', 'modifyXmlFSharp1', modifyXmlFSharp('<?xml version="1.0" encoding="utf-8"?><title>Hello, World!</title>'::XML)::text = '<?xml version="1.0" encoding="utf-8"?><title>Goodbye, beautiful World!</title>'::XML::text;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-xml', 'modifyXmlFSharp2', modifyXmlFSharp(''::XML)::text = '<?xml version="1.0" encoding="utf-8"?><title>Goodbye, beautiful World, it was null!</title>'::XML::text;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-xml-null', 'modifyXmlFSharp2', modifyXmlFSharp(NULL::XML)::text = '<?xml version="1.0" encoding="utf-8"?><title>Goodbye, beautiful World, it was null!</title>'::XML::text;

CREATE OR REPLACE FUNCTION createXmlFSharp(title TEXT, p1 TEXT, p2 TEXT) RETURNS XML AS $$
    "<?xml version=\"1.0\" encoding=\"utf-8\"?><title>" + title.ToUpper() + "</title><body><p>" + p1 + "</p><p>" + p2 + "</p></body>"
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-xml', 'createXmlFSharp', createXmlFSharp('hello world'::TEXT, 'First paragraph'::TEXT, 'Second paragraph'::TEXT)::TEXT = '<?xml version="1.0" encoding="utf-8"?><title>HELLO WORLD</title><body><p>First paragraph</p><p>Second paragraph</p></body>'::XML::TEXT;

--- Text Arrays
CREATE OR REPLACE FUNCTION returnTextArrayFSharp(texts text[]) RETURNS text[] AS $$
    texts
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-text-null-1array', 'returnTextArrayFSharp1', returnTextArrayFSharp(ARRAY['test1'::text, null::text, 'test string 2'::text, null::text]) = ARRAY['test1'::text, null::text, 'test string 2'::text, null::text];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-text-null-2array-arraynull', 'returnTextArrayFSharp2', returnTextArrayFSharp(ARRAY[[null::text, null::text], ['test1'::text, 'test string 2'::text]]) = ARRAY[[null::text, null::text], ['test1'::text, 'test string 2'::text]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-text-null-3array-arraynull', 'returnTextArrayFSharp3', returnTextArrayFSharp(ARRAY[[[null::text, null::text], [null::text, null::text]], [['test1'::text, 'test 2'::text], ['test 3  abc'::text, 'test4'::text]]]) = ARRAY[[[null::text, null::text], [null::text, null::text]], [['test1'::text, 'test 2'::text], ['test 3  abc'::text, 'test4'::text]]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-text-arraynull', 'returnTextArrayFSharp4', returnTextArrayFSharp(NULL::TEXT[]) is NULL;

CREATE OR REPLACE FUNCTION updateArrayTextIndexFSharp(texts text[], desired text, index integer[]) RETURNS text[] AS $$
let arrayInteger: int[] = index.Cast<int>().ToArray()
texts.SetValue(desired, arrayInteger)
texts
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-text-null-1array', 'updateArrayTextIndexFSharp1', updateArrayTextIndexFSharp(ARRAY['test1'::text, null::text, ' test string 2'::text, null::text], 'test updated', ARRAY[1]) = ARRAY['test1'::text, 'test updated'::text, ' test string 2'::text, null::text];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-text-null-3array', 'updateArrayTextIndexFSharp2', updateArrayTextIndexFSharp(ARRAY[[['test1'::text, null::text, ' appended'::text], [' to'::text, ' another'::text, ' text:'::text]], [[' test string 2,'::text, null::text, ' is'::text], [' this'::text, ' text'::text, ' good?'::text]]], 'test updated', ARRAY[1, 0, 2]) = ARRAY[[['test1'::text, null::text, ' appended'::text], [' to'::text, ' another'::text, ' text:'::text]], [[' test string 2,'::text, null::text, 'test updated'::text], [' this'::text, ' text'::text, ' good?'::text]]];

--- BPCHAR Arrays
CREATE OR REPLACE FUNCTION updateCharArrayIndexFSharp(values_array BPCHAR[], desired BPCHAR, index integer[]) RETURNS BPCHAR[] AS $$
let arrayInteger: int[]  = index.Cast<int>().ToArray()
values_array.SetValue(desired, arrayInteger)
values_array
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bpchar-null-1array', 'updateCharArrayIndexFSharp1', updateCharArrayIndexFSharp(ARRAY['hello'::BPCHAR, 'hi'::BPCHAR, null::BPCHAR, 'bye'::BPCHAR], 'goodbye'::BPCHAR, ARRAY[2]) = ARRAY['hello'::BPCHAR, 'hi'::BPCHAR, 'goodbye'::BPCHAR, 'bye'::BPCHAR];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bpchar-null-2array-arraynull', 'updateCharArrayIndexFSharp2', updateCharArrayIndexFSharp(ARRAY[[null::BPCHAR, null::BPCHAR], [null::BPCHAR, 'bye'::BPCHAR]], 'goodbye'::BPCHAR, ARRAY[1,0]) = ARRAY[[null::BPCHAR, null::BPCHAR], ['goodbye'::BPCHAR, 'bye'::BPCHAR]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bpchar-null-1array', 'updateCharArrayIndexFSharp3', updateCharArrayIndexFSharp(ARRAY['goodbye'::BPCHAR, 'bye'::BPCHAR, null::BPCHAR, 'bye'::BPCHAR], 'goodbye'::BPCHAR, ARRAY[2]) = ARRAY['goodbye'::BPCHAR, 'bye'::BPCHAR, 'goodbye'::BPCHAR, 'bye'::BPCHAR];

--- VARCHAR Arrays
CREATE OR REPLACE FUNCTION updateVarcharArrayIndexFSharp(values_array VARCHAR[], desired VARCHAR, index integer[]) RETURNS VARCHAR[] AS $$
let arrayInteger: int[] = index.Cast<int>().ToArray()
values_array.SetValue(desired, arrayInteger)
values_array
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-varchar-null-1array', 'updateVarcharArrayIndexFSharp1', updateVarcharArrayIndexFSharp(ARRAY['hello'::VARCHAR, 'hi'::VARCHAR, null::VARCHAR, 'bye'::VARCHAR], 'goodbye'::VARCHAR, ARRAY[2]) = ARRAY['hello'::VARCHAR, 'hi'::VARCHAR, 'goodbye'::VARCHAR, 'bye'::VARCHAR];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-varchar-null-2array-arraynull', 'updateVarcharArrayIndexFSharp2', updateVarcharArrayIndexFSharp(ARRAY[[null::VARCHAR, null::VARCHAR], [null::VARCHAR, 'bye'::VARCHAR]], 'goodbye'::VARCHAR, ARRAY[1,0]) = ARRAY[[null::VARCHAR, null::VARCHAR], ['goodbye'::VARCHAR, 'bye'::VARCHAR]];

--- XML Arrays
CREATE OR REPLACE FUNCTION updateXMLArrayIndexFSharp(values_array XML[], desired XML, index integer[]) RETURNS XML[] AS $$
let arrayInteger: int[] = index.Cast<int>().ToArray()
values_array.SetValue(desired, arrayInteger)
values_array
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-xml-null-1array', 'updateXMLArrayIndexFSharp1', updateXMLArrayIndexFSharp(ARRAY['<?xml version="1.0" encoding="utf-8"?><title>Hello, World!</title>'::XML, '<?xml version="1.0" encoding="utf-8"?><title>Test 1!</title>'::XML, null::XML, '<?xml version="1.0" encoding="utf-8"?><title>Goodbye, World!</title>'::XML], '<?xml version="1.0" encoding="utf-8"?><title>Writing tests!</title>'::XML, ARRAY[2])::TEXT = ARRAY['<?xml version="1.0" encoding="utf-8"?><title>Hello, World!</title>'::XML, '<?xml version="1.0" encoding="utf-8"?><title>Test 1!</title>'::XML, '<?xml version="1.0" encoding="utf-8"?><title>Writing tests!</title>'::XML, '<?xml version="1.0" encoding="utf-8"?><title>Goodbye, World!</title>'::XML]::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-xml-null-2array-arraynull', 'updateXMLArrayIndexFSharp2', updateXMLArrayIndexFSharp(ARRAY[[null::XML, null::XML], [null::XML, '<?xml version="1.0" encoding="utf-8"?><title>Goodbye, World!</title>'::XML]], '<?xml version="1.0" encoding="utf-8"?><title>Writing tests!</title>'::XML, ARRAY[1,0])::TEXT = ARRAY[[null::XML, null::XML], ['<?xml version="1.0" encoding="utf-8"?><title>Writing tests!</title>'::XML, '<?xml version="1.0" encoding="utf-8"?><title>Goodbye, World!</title>'::XML]]::TEXT;
