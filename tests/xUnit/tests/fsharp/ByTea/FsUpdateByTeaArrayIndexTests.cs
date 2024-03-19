
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "ByTea")]
public class FsUpdateByTeaArrayIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let dim = a.Rank
match dim with
| 1 ->
    a.SetValue(b, 0) |> ignore
    a
| 2 ->
    a.SetValue(b, 0, 0) |> ignore
    a
| 3 ->
    a.SetValue(b, 0, 0, 0) |> ignore
    a
| _ -> a
    ";

    public FsUpdateByTeaArrayIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateByTeaArrayIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "BYTEA[]"), new FunctionArgument("b", "BYTEA") },
            ReturnType = "BYTEA[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-bytea-null-1array", "updateByteaArrayIndex1", "ARRAY['Brick Abode is nice!'::BYTEA, 'Test 1!'::BYTEA, null::BYTEA, '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA], 'Inserted BYTEA'::BYTEA", "= ARRAY['Inserted BYTEA'::BYTEA, 'Test 1!'::BYTEA, null::BYTEA, '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]" },
            new object[] { "f#-bytea-null-2array", "updateByteaArrayIndex2", "ARRAY[['Brick Abode is nice!'::BYTEA, 'Test 1!'::BYTEA], [null::BYTEA, '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]], 'Inserted BYTEA'::BYTEA", "= ARRAY[['Inserted BYTEA'::BYTEA, 'Test 1!'::BYTEA], [null::BYTEA, '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]]" },
            new object[] { "f#-bytea-null-3array", "updateByteaArrayIndex3", "ARRAY[[['Brick Abode is nice!'::BYTEA, 'Test 1!'::BYTEA], [null::BYTEA, '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]]], 'Inserted BYTEA'::BYTEA", "= ARRAY[[['Inserted BYTEA'::BYTEA, 'Test 1!'::BYTEA], [null::BYTEA, '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]]]" },
            new object[] { "f#-bytea-null-1array-arraynull", "updateByteaArrayIndex4", "ARRAY[null::BYTEA, null::BYTEA, null::BYTEA, '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA], 'Inserted BYTEA'::BYTEA", "= ARRAY['Inserted BYTEA'::BYTEA, null::BYTEA, null::BYTEA, '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]" },
            new object[] { "f#-bytea-null-2array-arraynull", "updateByteaArrayIndex3", "ARRAY[[null::BYTEA, null::BYTEA], [null::BYTEA, '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]], 'Inserted BYTEA'::BYTEA", "= ARRAY[['Inserted BYTEA'::BYTEA, null::BYTEA], [null::BYTEA, '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]]" },
            new object[] { "f#-bytea-null-3array-arraynull", "updateByteaArrayIndex3", "ARRAY[[[null::BYTEA, null::BYTEA], [null::BYTEA, '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]]], 'Inserted BYTEA'::BYTEA", "= ARRAY[[['Inserted BYTEA'::BYTEA, null::BYTEA], [null::BYTEA, '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateByTeaArrayIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
