using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseInOutArray10FsSTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseInOutArray10FsSTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "InOutArray10FsS", Arguments = new List<FunctionArgument> { new FunctionArgument("OUT output_array", "MACADDR[]"), new FunctionArgument("IN input_array", "MACADDR[]") }, ReturnType = "", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-inout-array-10", "inout_array_10_fsS", "ARRAY[ MACADDR '08-00-2b-01-02-03', NULL, MACADDR '08-00-2b-01-02-03' ]", @"= ARRAY[
                    MACADDR '08-00-2b-01-02-03',
                    NULL,
                    MACADDR '08-00-2b-01-02-03'
                ]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutArray10FsS(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "InOut")]
public class InOutArray10FsSTestsFSharp : BaseInOutArray10FsSTests
{
    protected override string FunctionBody => @"
let count = input_array.Length
let output = Array.CreateInstance(typeof<obj>, count)
for i = 0 to count - 1 do
    output.SetValue(input_array.GetValue(i), i)
if count > 3 then
    output.SetValue(Nullable() :> obj, 3)
output
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}