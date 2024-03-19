
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Call")]
public class UpdateMoneyFSharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
    let flatten_floats = Array.CreateInstance(typeof<decimal>, values_array.Length)
    ArrayManipulation.FlatArray(values_array, ref flatten_floats) |> ignore
    flatten_floats.SetValue(desired, index)
    flatten_floats
    ";

    public UpdateMoneyFSharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "updateMoneyArrayFSharp",
            Arguments = new List<FunctionArgument> {
                new FunctionArgument("values_array", "MONEY[]"),
                new FunctionArgument("desired", "MONEY"),
                new FunctionArgument("index", "int"),
            },
            ReturnType = "MONEY[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] {
                "f#-money-null-1array",
                "updateMoneyArrayFSharp1",
                "ARRAY['32500.0'::MONEY, '-500.4'::MONEY, null::MONEY, '900540.2'::MONEY], '1390540.2'::MONEY, 2::int",
                "= ARRAY['32500.0'::MONEY, '-500.4'::MONEY, '1390540.2'::MONEY, '900540.2'::MONEY]"
            },
            new object[] {
                "f#-money-null-2array-arraynull",
                "updateMoneyArrayFSharp2",
                "ARRAY[['32500.0'::MONEY, '-500.4'::MONEY], [null::MONEY, null::MONEY]], '1390540.2'::MONEY, 1::int",
                "= ARRAY['32500.0'::MONEY, '1390540.2'::MONEY, 0::MONEY, 0::MONEY]"
            },
            new object[] {
                "f#-money-null-2array-arraynull",
                "updateMoneyArrayFSharp3",
                "ARRAY[[null::MONEY, null::MONEY], [null::MONEY, null::MONEY]], '1390540.2'::MONEY, 1::int",
                "= ARRAY[0::MONEY, '1390540.2'::MONEY, 0::MONEY, 0::MONEY]"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestNone(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
