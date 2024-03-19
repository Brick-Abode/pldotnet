
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "ByTea")]
public class ByTeaConversionsFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
    let utf8_e = new UTF8Encoding()
    match (a, b) with
    | (null, null) -> null
    | (null, _) -> b
    | (_, null) -> a
    | _ ->
        let s1 = utf8_e.GetString(a, 0, a.Length)
        let s2 = utf8_e.GetString(b, 0, b.Length)
        let result = s1 + "" "" + s2
        utf8_e.GetBytes result
    ";

    public ByTeaConversionsFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ByTeaConversionsFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "BYTEA"), new FunctionArgument("b", "BYTEA") },
            ReturnType = "BYTEA",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-bytea", "byteaConversionsFSharp1", "'Brick Abode is nice!'::BYTEA, 'Thank you very much...'::BYTEA", "= '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA" },
        new object[] { "f#-bytea-null", "byteaConversionsFSharp2", "NULL::BYTEA, 'Thank you very much...'::BYTEA", "= 'Thank you very much...'::BYTEA" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestByTeaConversionsFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
