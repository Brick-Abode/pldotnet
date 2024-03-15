
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Network")]
public class CreateMacAddress8MultidimensionalArrayFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let arr = Array.CreateInstance(typeof<PhysicalAddress>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
    ";

    public CreateMacAddress8MultidimensionalArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateMacAddress8MultidimensionalArrayFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("objects_value", "MACADDR8") },
            ReturnType = "MACADDR8[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-macaddr8-3array", "CreateMacAddress8MultidimensionalArrayFSharp", "MACADDR8 'ab-01-2b-31-41-fa-ab-ac'", "= ARRAY[[[MACADDR8 'ab-01-2b-31-41-fa-ab-ac']]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateMacAddress8MultidimensionalArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
