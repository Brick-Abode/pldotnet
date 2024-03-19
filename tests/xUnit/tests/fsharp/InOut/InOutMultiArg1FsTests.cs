
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "InOut")]
public class InOutMultiArg1FsTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
    if a0.HasValue && a0.Value <> 0 then
        raise <| SystemException(""Failed assertion: a0"")
    if a1.HasValue && a1.Value <> 1 then
        raise <| SystemException(""Failed assertion: a1"")
    if a2.HasValue && a2.Value <> 2 then
        raise <| SystemException(""Failed assertion: a2"")
    if a5.HasValue then
        raise <| SystemException(""Failed assertion: a5"")
    if a6.HasValue && a6.Value <> 6 then
        raise <| SystemException(""Failed assertion: a6"")

    (Nullable(2), Nullable(4), Nullable(5), Nullable(6), Nullable())
    ";

    public InOutMultiArg1FsTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "InOutMultiArg1Fs",
            Arguments = new List<FunctionArgument> { new FunctionArgument("IN a0", "INT"), new FunctionArgument("INOUT a1", "INT"), new FunctionArgument("IN a2", "INT"), new FunctionArgument("OUT a3", "INT"), new FunctionArgument("OUT a4", "INT"), new FunctionArgument("INOUT a5", "INT"), new FunctionArgument("IN a6", "INT"), new FunctionArgument("OUT a7", "INT") },
            ReturnType = "",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-inout-multiarg-1", "inout_multiarg_1_fs", "0, 1, 2, NULL, 6", "= ROW(2, 4, 5, 6, NULL::INT)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutMultiArg1Fs(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
