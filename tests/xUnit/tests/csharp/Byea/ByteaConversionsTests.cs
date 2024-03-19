
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Byea")]
public class ByTeaConversionsTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
UTF8Encoding utf8_e = new UTF8Encoding();
    if (a == null && b == null)
        return null;
    if (a == null)
        return b;
    if (b == null)
        return a;

    string s1 = utf8_e.GetString(a, 0, a.Length);
    string s2 = utf8_e.GetString(b, 0, b.Length);
    string result = s1 + "" "" + s2;
    return utf8_e.GetBytes(result);
    ";

    public ByTeaConversionsTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ByTeaConversions",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "BYTEA"), new FunctionArgument("b", "BYTEA") },
            ReturnType = "BYTEA",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-bytea", "byteaConversions1", "'Brick Abode is nice!'::BYTEA, 'Thank you very much...'::BYTEA", "= '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA" },
        new object[] { "c#-bytea-null", "byteaConversions2", "NULL::BYTEA, 'Thank you very much...'::BYTEA", "= 'Thank you very much...'::BYTEA" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestByTeaConversions(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
