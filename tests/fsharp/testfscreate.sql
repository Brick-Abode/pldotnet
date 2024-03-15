CREATE OR REPLACE FUNCTION testMoneyFSharp(a MONEY) RETURNS MONEY AS $$
let a = if a.HasValue then a.Value else 0
a
$$ LANGUAGE plfsharp;

CREATE OR REPLACE FUNCTION updateMoneyArrayFSharp(values_array MONEY[], desired MONEY, index int) RETURNS MONEY[] AS $$
    let flatten_floats = Array.CreateInstance(typeof<decimal>, values_array.Length)
    ArrayManipulation.FlatArray(values_array, ref flatten_floats) |> ignore
    flatten_floats.SetValue(desired, index)
    flatten_floats
$$ LANGUAGE plfsharp STRICT;

