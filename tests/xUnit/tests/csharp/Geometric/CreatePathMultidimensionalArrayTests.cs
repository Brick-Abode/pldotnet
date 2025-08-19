using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreatePathMultidimensionalArrayTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreatePathMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreatePathMultidimensionalArray", Arguments = new List<FunctionArgument> { }, ReturnType = "PATH[]", Body = FunctionBody, Language = Language, IsStrict = true, CastFunctionAs = "TEXT", };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-path-null-3array-arraynull", "CreatePathMultidimensionalArray1", "", "= CAST(ARRAY[[['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH], [null::PATH, null::PATH]], [['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, null::PATH], ['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH]]] AS TEXT)" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreatePathMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class CreatePathMultidimensionalArrayTestsCSharp : BaseCreatePathMultidimensionalArrayTests
{
    protected override string FunctionBody => @"
NpgsqlPath objects_value = new NpgsqlPath(new NpgsqlPoint(1.5, 2.75), new NpgsqlPoint(3.0, 4.75), new NpgsqlPoint(5.0, 5.0));
NpgsqlPath?[, ,] three_dimensional_array = new NpgsqlPath?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}