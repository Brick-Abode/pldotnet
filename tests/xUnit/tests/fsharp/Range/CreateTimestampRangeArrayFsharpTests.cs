using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateTimestampRangeArrayFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateTimestampRangeArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateTimestampRangeArrayFsharp", Arguments = new List<FunctionArgument> { }, ReturnType = "TSRANGE[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-tsrange-3array", "CreateTimestampRangeArrayFSharp1", "", "= ARRAY[[['[2022-04-14 12:30:25, 2022-04-15 17:30:25)'::TSRANGE]]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateTimestampRangeArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Range")]
public class CreateTimestampRangeArrayFsharpTestsFSharp : BaseCreateTimestampRangeArrayFsharpTests
{
    protected override string FunctionBody => @"
let arr = Array.CreateInstance(typeof<NpgsqlRange<DateTime>>, 1, 1, 1)
let objects_value = NpgsqlRange<DateTime>(DateTime(2022, 4, 14, 12, 30, 25), true, false, DateTime(2022, 4, 15, 17, 30, 25), false, false)
arr.SetValue(objects_value, 0, 0, 0)
arr
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}