using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseModifyBitTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseModifyBitTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ModifyBit", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "BIT(10)") }, ReturnType = "BIT(10)", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-bit", "modifybit1", "'10101'::BIT(10)", "= '0010100001'::BIT(10)" }, new object[] { "c#-bit-null", "modifybit2", "NULL::BIT(10)", "IS NULL" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyBit(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Bit")]
public class ModifyBitTestsCSharp : BaseModifyBitTests
{
    protected override string FunctionBody => @"
    if (a == null)
        return null;

    a[0] = a[0] ? false : true;
    a[a.Length-1] = a[a.Length-1] ? false : true;
    return a;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}