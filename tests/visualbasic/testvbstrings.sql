CREATE OR REPLACE FUNCTION identityStr(a text) RETURNS text AS
$$
    Console.WriteLine("Got string: {0}", a)
    Return a
$$ LANGUAGE plvisualbasic STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'vb-text', 'identityStr', identityStr('dog') = 'dog';

CREATE OR REPLACE FUNCTION concatenateText(a text, b text) RETURNS text AS
$$
    If a Is Nothing Then
        a = ""
    End If

    If b Is Nothing Then
        b = ""
    End If

    Dim c As String = a & " " & b
    Return c
$$ LANGUAGE plvisualbasic;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'vb-text', 'concatenateText1', concatenateText('red', 'blue') = 'red blue';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'vb-text-null', 'concatenateText2', concatenateText(NULL::TEXT, 'blue') = ' blue';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'vb-text', 'concatenateText3', concatenateText('КРАСНЫЙ', 'СИНИЙ') = 'КРАСНЫЙ СИНИЙ'::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'vb-text', 'concatenateText4', concatenateText('赤', '青い') = '赤 青い'::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'vb-text', 'concatenateText5', concatenateText('紅色的', '藍色的') = '紅色的 藍色的'::TEXT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'vb-text', 'concatenateText6', concatenateText('🐂', '🥰') = '🐂 🥰'::TEXT;

CREATE OR REPLACE FUNCTION multiplyText(a text, b int) RETURNS text AS
$$
    Dim c As String = ""
    For i As Integer = 0 To b - 1
        c = c & a
    Next
    Return c
$$ LANGUAGE plvisualbasic STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'vb-text', 'multiplyText', multiplyText('dog ', 3) = 'dog dog dog ';
