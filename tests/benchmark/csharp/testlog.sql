CREATE OR REPLACE FUNCTION elogWarningTest(a text) RETURNS BOOL AS $$
Elog.Info($"Hello {a}!");
return true;
$$ LANGUAGE plcsharp;
SELECT elogWarningTest('World') = true;

CREATE OR REPLACE FUNCTION elogInfoTest(a text) RETURNS BOOL AS $$
Elog.Info($"Hello {a}!");
return true;
$$ LANGUAGE plcsharp;
SELECT elogInfoTest('World') = true;
