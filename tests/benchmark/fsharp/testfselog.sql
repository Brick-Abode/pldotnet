CREATE OR REPLACE FUNCTION elogWarningTestFSharp(a text) RETURNS BOOL AS $$
match a with
| Some _a ->
    let greeting = sprintf "Hello %s!" _a
    pldotnet_Warning(greeting) |> ignore
    Some true
| _ -> Some false
$$ LANGUAGE plfsharp;
SELECT elogWarningTestFSharp('World') = true;

CREATE OR REPLACE FUNCTION elogInfoTestFSharp(a text) RETURNS BOOL AS $$
match a with
| Some _a ->
    let greeting = sprintf "Hello %s!" _a
    pldotnet_Info(greeting) |> ignore
    Some true
| _ -> Some false
$$ LANGUAGE plfsharp;
SELECT elogInfoTestFSharp('World') = true;
