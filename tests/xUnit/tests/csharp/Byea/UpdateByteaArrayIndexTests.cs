
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Byea")]
public class UpdateByTeaArrayIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateByTeaArrayIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateByTeaArrayIndex",
            Arguments = new List<FunctionArgument> {
                new FunctionArgument("values_array", "BYTEA[]"),
                new FunctionArgument("desired", "BYTEA"),
                new FunctionArgument("index", "integer[]")
            },
            ReturnType = "BYTEA[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] {
                "c#-bytea-null-1array",
                "updateByteaArrayIndex1",
                "ARRAY['Brick Abode is nice!'::BYTEA, 'Test 1!'::BYTEA, null::BYTEA, '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA], 'Inserted BYTEA'::BYTEA, ARRAY[2]",
                "= ARRAY['Brick Abode is nice!'::BYTEA, 'Test 1!'::BYTEA, 'Inserted BYTEA'::BYTEA, '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]"
            },
            new object[] {
                "c#-bytea-null-2array",
                "updateByteaArrayIndex2",
                "ARRAY[['Brick Abode is nice!'::BYTEA, 'Test 1!'::BYTEA], [null::BYTEA, '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]], 'Inserted BYTEA'::BYTEA, ARRAY[1,0]",
                "= ARRAY[['Brick Abode is nice!'::BYTEA, 'Test 1!'::BYTEA], ['Inserted BYTEA'::BYTEA, '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]]"
            },
            new object[] {
                "c#-bytea-null-2array-arraynull",
                "updateByteaArrayIndex3",
                "ARRAY[[null::BYTEA, null::BYTEA], [null::BYTEA, '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]], 'Inserted BYTEA'::BYTEA, ARRAY[1,0]",
                "= ARRAY[[null::BYTEA, null::BYTEA], ['Inserted BYTEA'::BYTEA, '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]]"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateByTeaArrayIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
