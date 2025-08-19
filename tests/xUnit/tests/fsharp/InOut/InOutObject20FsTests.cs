using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseInOutObject20FsTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseInOutObject20FsTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "InOutObject20Fs", Arguments = new List<FunctionArgument> { new FunctionArgument("IN a", "text"), new FunctionArgument("b", "text"), new FunctionArgument("OUT c", "text") }, ReturnType = "", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-inout-object-20", "inout_object_20_fs", "'red', 'blue'", "= 'red blue'" }, new object[] { "f#-inout-object-21", "inout_object_20_fs", "'red', NULL", "= 'red '" }, new object[] { "f#-inout-object-22", "inout_object_20_fs", "NULL, 'blue'", "= ' blue'" }, new object[] { "f#-inout-object-23", "inout_object_20_fs", "'🐂', '🥰'", "= '🐂 🥰'::TEXT" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutObject20Fs(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "InOut")]
public class InOutObject20FsTestsFSharp : BaseInOutObject20FsTests
{
    protected override string FunctionBody => @"
a + "" "" + b;
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}