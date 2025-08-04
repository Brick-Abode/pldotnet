using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseModifyBitFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseModifyBitFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ModifyBitFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "BIT(10)") }, ReturnType = "BIT(10)", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-bit", "modifybitfsharp1", "'10101'::BIT(10)", "= '0010100001'::BIT(10)" }, new object[] { "f#-bit-null", "modifybitfsharp2", "NULL::BIT(10)", "IS NULL" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyBitFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Bit")]
public class ModifyBitFsharpTestsFSharp : BaseModifyBitFsharpTests
{
    protected override string FunctionBody => @"
#line 1
    if System.Object.ReferenceEquals(a, null) then
        null
    else
        let first = a.Get(0)
        let last = a.Get(a.Length - 1)
        let result = a
        for i in 0 .. a.Length - 1 do
            result.[i] <- a.Get(i)
        result.[0] <- not first
        result.[a.Length - 1] <- not last
        new BitArray(result)
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}