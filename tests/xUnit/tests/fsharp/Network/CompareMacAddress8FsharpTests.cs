
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Network")]
public class CompareMacAddress8FsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if System.Object.ReferenceEquals(a, null) && System.Object.ReferenceEquals(b, null) then true
else if System.Object.ReferenceEquals(a, null) then false
else if System.Object.ReferenceEquals(b, null) then false
else a.Equals(b)
    ";

    public CompareMacAddress8FsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CompareMacAddress8Fsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "MACADDR8"), new FunctionArgument("b", "MACADDR8") },
            ReturnType = "BOOLEAN",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
   new object[] 
        { 
            "f#-macaddr8", 
            "compareMacAddress8FSharp1", 
            "MACADDR8 '08-00-2b-01-02-03-04-06', MACADDR8 '08-00-2b-01-02-03-04-06'", 
            "is true" 
        },
        new object[] 
        { 
            "f#-macaddr8", 
            "compareMacAddress8FSharp2", 
            "MACADDR8 '08-00-2b-01-02-03-04-06', MACADDR8 '10-00-2b-01-02-03-04-06'", 
            "is false" 
        },
        new object[] 
        { 
            "f#-macaddr8-null", 
            "compareMacAddress8FSharp3", 
            "NULL::MACADDR8, MACADDR8 'ab-01-2b-31-41-fa-ab-ac'", 
            "is false" 
        },
        new object[] 
        { 
            "f#-macaddr8-null", 
            "compareMacAddress8FSharp4", 
            "MACADDR8 'ab-01-2b-31-41-fa-ab-ac', NULL::MACADDR8", 
            "is false" 
        },
        new object[] 
        { 
            "f#-macaddr8-null", 
            "compareMacAddress8FSharp5", 
            "NULL::MACADDR8, NULL::MACADDR8", 
            "is true" 
        },        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCompareMacAddress8Fsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
