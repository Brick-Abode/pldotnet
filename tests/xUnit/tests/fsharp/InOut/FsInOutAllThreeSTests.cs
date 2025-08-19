using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseFsInOutAllThreeSTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseFsInOutAllThreeSTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "FsInOutAllThreeS", Arguments = new List<FunctionArgument> { new FunctionArgument("IN a", "INT"), new FunctionArgument("INOUT b", "INT"), new FunctionArgument("OUT c", "INT") }, ReturnType = "", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-inout-allthree-S", "fs_inout_allthreeS", "3, 8", "= ROW(9, 11)" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestFsInOutAllThreeS(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "InOut")]
public class FsInOutAllThreeSTestsFSharp : BaseFsInOutAllThreeSTests
{
    protected override string FunctionBody => @"
Nullable(b+1), Nullable(a+b)
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}