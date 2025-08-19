using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseMixedBigInT8Tests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseMixedBigInT8Tests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "MixedBigInT8", Arguments = new List<FunctionArgument> { new FunctionArgument("b", "smallint"), new FunctionArgument("c", "bigint") }, ReturnType = "smallint", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int8", "mixedBigInt8", "CAST(32 AS SMALLINT), CAST(100 AS BIGINT)", "= smallint '132'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMixedBigInT8(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class MixedBigInT8TestsCSharp : BaseMixedBigInT8Tests
{
    protected override string FunctionBody => @"
return (short)(b+c);
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}