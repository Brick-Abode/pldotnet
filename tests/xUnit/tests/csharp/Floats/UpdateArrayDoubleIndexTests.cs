using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseUpdateArrayDoubleIndexTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseUpdateArrayDoubleIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "UpdateArrayDoubleIndex", Arguments = new List<FunctionArgument> { new FunctionArgument("doubles double", "precision[]"), new FunctionArgument("desired double", "precision"), new FunctionArgument("index", "integer[]") }, ReturnType = "double precision[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-float8-null-1array", "updateArrayDoubleIndex1", "ARRAY[4.55535544213::double precision, 10.1133254154::double precision, null::double precision], 9.8321432132, ARRAY[1]", "= ARRAY[4.55535544213::double precision, 9.8321432132::double precision, null::double precision]" }, new object[] { "c#-float8-null-2array", "updateArrayDoubleIndex2", "ARRAY[[4.55535544213::double precision, 10.1133254154::double precision], [null::double precision, 16.16155::double precision]], 9.8321432132, ARRAY[1, 0]", "= ARRAY[[4.55535544213::double precision, 10.1133254154::double precision], [9.8321432132::double precision, 16.16155::double precision]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayDoubleIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Floats")]
public class UpdateArrayDoubleIndexTestsCSharp : BaseUpdateArrayDoubleIndexTests
{
    protected override string FunctionBody => @"
int[] arrayInteger = index.Cast<int>().ToArray();
doubles.SetValue(desired, arrayInteger);
return doubles;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}