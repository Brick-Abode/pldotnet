using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseMixedBigInTFsharp2Tests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseMixedBigInTFsharp2Tests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "MixedBigInTFsharp2", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "int2"), new FunctionArgument("b", "int4"), new FunctionArgument("c", "int8") }, ReturnType = "int8", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-int8", "mixedBigIntFSharp2", "'32767'::int2,  '550'::int4, '200'::int8", "= '3604370000'::int8" }, new object[] { "f#-int8", "mixedBigIntFSharp2", "'32767'::int2,  '5500'::int4, '500'::int8", "= int8 '90109250000'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMixedBigInTFsharp2(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Integers")]
public class MixedBigInTFsharp2TestsFSharp : BaseMixedBigInTFsharp2Tests
{
    protected override string FunctionBody => @"
Nullable ((int64)a.Value * (int64)b.Value * c.Value)
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}