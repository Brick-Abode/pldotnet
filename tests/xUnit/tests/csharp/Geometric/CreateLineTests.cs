using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateLineTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateLineTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateLine", Arguments = new List<FunctionArgument> { new FunctionArgument("a double", "precision"), new FunctionArgument("b double", "precision"), new FunctionArgument("c double", "precision") }, ReturnType = "LINE", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-line", "createLine", "1.50,-2.750,3.25", "= LINE '{1.50,-2.750,3.25}'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateLine(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class CreateLineTestsCSharp : BaseCreateLineTests
{
    protected override string FunctionBody => @"
NpgsqlLine my_line = new NpgsqlLine(a,b,c);
return my_line;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}