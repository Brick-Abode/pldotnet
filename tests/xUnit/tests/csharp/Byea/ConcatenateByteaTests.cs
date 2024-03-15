
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Byea")]
public class ConcatenateByTeaTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
UTF8Encoding utf8_e = new UTF8Encoding();
    byte[] b_bytes = new byte[b.Length];
    utf8_e.GetBytes(b, 0, b.Length, b_bytes, 0);
    byte[] c = new byte[a.Length + b_bytes.Length];
    a.CopyTo(c, 0);
    b_bytes.CopyTo(c, a.Length);
    return c;
    ";

    public ConcatenateByTeaTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ConcatenateByTea",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "BYTEA"), new FunctionArgument("b", "TEXT") },
            ReturnType = "BYTEA",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-bytea", "concatenateBytea", "'\\x427269636b2041626f6465206973206e69636521'::BYTEA, ' Thank you very much...'::TEXT", "= '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestConcatenateByTea(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
