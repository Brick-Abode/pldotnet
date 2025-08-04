using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseReturnDoubleArrayTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseReturnDoubleArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ReturnDoubleArray", Arguments = new List<FunctionArgument> { new FunctionArgument("doubles double", "precision[]") }, ReturnType = "double precision[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-float8-null-1array", "returnDoubleArray1", "ARRAY[21.0000000000109::double precision, null::double precision, 4.521234313421::double precision, 7.412344328978::double precision]", "= ARRAY[21.0000000000109::double precision, null::double precision, 4.521234313421::double precision, 7.412344328978::double precision]" }, new object[] { "c#-float8-null-2array-arraynull", "returnDoubleArray2", "ARRAY[[null::double precision, null::double precision], [4.521234313421::double precision, 7.412344328978::double precision]]", "= ARRAY[[null::double precision, null::double precision], [4.521234313421::double precision, 7.412344328978::double precision]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnDoubleArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Floats")]
public class ReturnDoubleArrayTestsCSharp : BaseReturnDoubleArrayTests
{
    protected override string FunctionBody => @"
return doubles;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}