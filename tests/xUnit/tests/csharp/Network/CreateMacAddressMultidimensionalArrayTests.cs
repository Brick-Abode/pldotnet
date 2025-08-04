using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateMacAddressMultidimensionalArrayTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateMacAddressMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateMacAddressMultidimensionalArray", Arguments = new List<FunctionArgument> { }, ReturnType = "MACADDR[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-macaddr-3array", "CreateMacAddressMultidimensionalArray", "", "= ARRAY[[[MACADDR 'ab-01-2b-31-41-fa', MACADDR 'ab-01-2b-31-41-fa'], [null::MACADDR, null::MACADDR]], [[MACADDR 'ab-01-2b-31-41-fa', null::MACADDR], [MACADDR 'ab-01-2b-31-41-fa', MACADDR 'ab-01-2b-31-41-fa']]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateMacAddressMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class CreateMacAddressMultidimensionalArrayTestsCSharp : BaseCreateMacAddressMultidimensionalArrayTests
{
    protected override string FunctionBody => @"
byte[] bytes = new byte[6] {171, 1, 43, 49, 65, 250};
PhysicalAddress objects_value = new PhysicalAddress(bytes);
PhysicalAddress?[, ,] three_dimensional_array = new PhysicalAddress?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}