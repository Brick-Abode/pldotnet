using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateUUIdMultidimensionalArrayFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateUUIdMultidimensionalArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateUUIdMultidimensionalArrayFsharp", Arguments = new List<FunctionArgument> { }, ReturnType = "UUID[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-uuid-null-3array-arraynull", "CreateUUIDMultidimensionalArrayFSharp1", "", "= ARRAY[[['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID]]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateUUIdMultidimensionalArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Uuid")]
public class CreateUUIdMultidimensionalArrayFsharpTestsFSharp : BaseCreateUUIdMultidimensionalArrayFsharpTests
{
    protected override string FunctionBody => @"
let objects_value = Guid(""a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"")
let arr = Array.CreateInstance(typeof<Guid>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}