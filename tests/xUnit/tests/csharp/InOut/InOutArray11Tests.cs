
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "InOut")]
public class InOutArray11Tests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array output = Array.CreateInstance(typeof(object), count);
    for(int i = 0; i < count; i++)
    {
        output.SetValue((PhysicalAddress)address, i);
    }
    values_array = output;
    ";

    public InOutArray11Tests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "InOutArray11",
            Arguments = new List<FunctionArgument> { new FunctionArgument("OUT values_array", "MACADDR[]"), new FunctionArgument("IN address", "MACADDR"), new FunctionArgument("IN count", "INT") },
            ReturnType = "",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
       {new object[] {
          "inout_array_11",
            "c#-inout-array-11",
            "MACADDR '08-00-2b-01-02-03', 3",
            "= ARRAY[MACADDR '08-00-2b-01-02-03',MACADDR '08-00-2b-01-02-03',MACADDR '08-00-2b-01-02-03']"
    }};
    }
    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutArray11(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
