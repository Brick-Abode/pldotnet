using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateTimestampRangeArrayEmptyFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateTimestampRangeArrayEmptyFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateTimestampRangeArrayEmptyFSharp", Arguments = new List<FunctionArgument> { }, ReturnType = "TSRANGE[]", Body = FunctionBody, Language = Language, IsStrict = true, CastFunctionAs = "TEXT", };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-tsrange-null-3array-arraynull", "CreateTimestampRangeArrayEmptyFSharp1", "", "= '{{{empty}}}'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateTimestampRangeArrayEmptyFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Range")]
public class CreateTimestampRangeArrayEmptyFsharpTestsFSharp : BaseCreateTimestampRangeArrayEmptyFsharpTests
{
    protected override string FunctionBody => @"
Array.CreateInstance(typeof<NpgsqlRange<DateTime>>, 1, 1, 1)
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}