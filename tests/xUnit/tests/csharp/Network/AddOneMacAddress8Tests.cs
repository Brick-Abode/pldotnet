using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseAddOneMacAddress8Tests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseAddOneMacAddress8Tests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "AddOneMacAddress8", Arguments = new List<FunctionArgument> { new FunctionArgument("my_address", "MACADDR8") }, ReturnType = "MACADDR8", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-macaddr8", "addOneMacAddress8", "MACADDR8 '08:00:2b:01:02:03:04:05'", "= MACADDR8 '08:00:2b:01:02:03:04:06'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestAddOneMacAddress8(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class AddOneMacAddress8TestsCSharp : BaseAddOneMacAddress8Tests
{
    protected override string FunctionBody => @"
byte[] bytes = my_address.GetAddressBytes();
bytes[7] += 1;
return new PhysicalAddress(bytes);
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}