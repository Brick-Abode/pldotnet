using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseFsCreateByTeaMultidimensionalArrayTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseFsCreateByTeaMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateByTeaMultidimensionalArray", Arguments = new List<FunctionArgument> { }, ReturnType = "BYTEA[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-bytea-null-3array-arraynull", "CreateByteaMultidimensionalArray1", "", "= ARRAY[[['\\x92837465564738'::BYTEA]]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateByTeaMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "ByTea")]
public class FsCreateByTeaMultidimensionalArrayTestsFSharp : BaseFsCreateByTeaMultidimensionalArrayTests
{
    protected override string FunctionBody => @"
let objects_value = [| 0x92uy; 0x83uy; 0x74uy; 0x65uy; 0x56uy; 0x47uy; 0x38uy |]
let arr = Array.CreateInstance(typeof<byte[]>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}