using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateLineFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateLineFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateLineFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "float8"), new FunctionArgument("b", "float8"), new FunctionArgument("c", "float8") }, ReturnType = "LINE", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-line", "createLineFSharp", "1.50,-2.750,3.25", "= LINE '{1.50,-2.750,3.25}'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateLineFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class CreateLineFsharpTestsFSharp : BaseCreateLineFsharpTests
{
    protected override string FunctionBody => @"
NpgsqlLine(a, b, c)
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}