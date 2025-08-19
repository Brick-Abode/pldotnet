using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseUpdateArraySmallInTIndexTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseUpdateArraySmallInTIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "UpdateArraySmallInTIndex", Arguments = new List<FunctionArgument> { new FunctionArgument("small_integers", "smallint[]"), new FunctionArgument("desired", "smallint"), new FunctionArgument("index", "integer[]") }, ReturnType = "smallint[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int2-null-1array", "updateArraySmallIntIndex1", "ARRAY[342::smallint, 10456::smallint, null::smallint], CAST(13212 AS smallint), ARRAY[1]", "= ARRAY[342::smallint, 13212::smallint, null::smallint]" }, new object[] { "c#-int2-null-2array-arraynull", "updateArraySmallIntIndex2", "ARRAY[[45::smallint, 11324::smallint], [null::smallint, 12464::smallint]], CAST(13212 AS smallint), ARRAY[1, 0]", "= ARRAY[[45::smallint, 11324::smallint], [13212::smallint, 12464::smallint]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArraySmallInTIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class UpdateArraySmallInTIndexTestsCSharp : BaseUpdateArraySmallInTIndexTests
{
    protected override string FunctionBody => @"
int[] arrayInteger = index.Cast<int>().ToArray();
small_integers.SetValue(desired, arrayInteger);
return small_integers;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}