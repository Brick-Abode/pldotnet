using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateMacAddressMultidimensionalArrayFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateMacAddressMultidimensionalArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateMacAddressMultidimensionalArrayFsharp", Arguments = new List<FunctionArgument> { }, ReturnType = "MACADDR[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-macaddr-3array", "CreateMacAddressMultidimensionalArrayFSharp", "", "= ARRAY[[[MACADDR 'ab-01-2b-31-41-fa']]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateMacAddressMultidimensionalArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Network")]
public class CreateMacAddressMultidimensionalArrayFsharpTestsFSharp : BaseCreateMacAddressMultidimensionalArrayFsharpTests
{
    protected override string FunctionBody => @"
let bytes = [| 171uy; 1uy; 43uy; 49uy; 65uy; 250uy |]
let objects_value = PhysicalAddress(bytes)
let arr = Array.CreateInstance(typeof<PhysicalAddress>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}