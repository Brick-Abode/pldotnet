using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateCharMultidimensionalArrayTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateCharMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateCharMultidimensionalArray", Arguments = new List<FunctionArgument> { }, ReturnType = "BPCHAR[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-bpchar-null-3array-arraynull", "CreateCharMultidimensionalArray1", "", "= ARRAY[[['Multiple dimensions'::BPCHAR, 'Multiple dimensions'::BPCHAR], [null::BPCHAR, null::BPCHAR]], [['Multiple dimensions'::BPCHAR, null::BPCHAR], ['Multiple dimensions'::BPCHAR, 'Multiple dimensions'::BPCHAR]]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateCharMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class CreateCharMultidimensionalArrayTestsCSharp : BaseCreateCharMultidimensionalArrayTests
{
    protected override string FunctionBody => @"
string objects_value = ""Multiple dimensions"";
string?[, ,] three_dimensional_array = new string?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}