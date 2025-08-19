using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseInOutArray11FsSTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseInOutArray11FsSTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "InOutArray11FsS", Arguments = new List<FunctionArgument> { new FunctionArgument("OUT values_array", "MACADDR[]"), new FunctionArgument("IN address", "MACADDR"), new FunctionArgument("IN count", "INT") }, ReturnType = "", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-inout-array-S-11", "inout_array_11_fsS", "MACADDR '08-00-2b-01-02-03', 3", @"= ARRAY[
                    MACADDR '08-00-2b-01-02-03',
                    MACADDR '08-00-2b-01-02-03',
                    MACADDR '08-00-2b-01-02-03'
                ]" }, new object[] { "f#-inout-array-12", "inout_array_11_fsS", "MACADDR '08-00-2b-01-02-03', 5", @"= ARRAY[
                    MACADDR '08-00-2b-01-02-03',
                    MACADDR '08-00-2b-01-02-03',
                    MACADDR '08-00-2b-01-02-03',
                    NULL,
                    MACADDR '08-00-2b-01-02-03'
                ]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutArray11FsS(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "InOut")]
public class InOutArray11FsSTestsFSharp : BaseInOutArray11FsSTests
{
    protected override string FunctionBody => @"
let output = Array.CreateInstance(typeof<obj>, count)
for i = 0 to count - 1 do
    output.SetValue(address :> obj, i)
if count > 3 then
    output.SetValue(Nullable() :> obj, 3)
output
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}