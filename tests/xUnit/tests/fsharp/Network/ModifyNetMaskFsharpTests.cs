using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseModifyNetMaskFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseModifyNetMaskFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ModifyNetMaskFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("my_inet", "INET"), new FunctionArgument("n", "INT") }, ReturnType = "INET", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-inet", "modifyNetMaskFSharp", "INET '192.168.0.1/24', 6", "= INET '192.168.0.1/30'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyNetMaskFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Network")]
public class ModifyNetMaskFsharpTestsFSharp : BaseModifyNetMaskFsharpTests
{
    protected override string FunctionBody => @"
let struct (address, netmask) = my_inet
struct (address, netmask + n)
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}