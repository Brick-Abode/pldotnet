CREATE OR REPLACE PROCEDURE printSumProcedureFSharp(a integer, b integer) AS $$
let c = a.Value + b.Value
Elog.Info("[F#] c = " + c.ToString())
$$ LANGUAGE plfsharp;
CALL printSumProcedureFSharp(10, 25);
CALL printSumProcedureFSharp(1450, 275);

CREATE OR REPLACE PROCEDURE sayHelloFSharp(name TEXT) AS $$
let message = "Hello, " + name + "! Welcome to plfsharp."
Elog.Info(message)
$$ LANGUAGE plfsharp;
CALL sayHelloFSharp('Mikael'::TEXT);
CALL sayHelloFSharp('Rosicley'::TEXT);
CALL sayHelloFSharp('Todd'::TEXT);
