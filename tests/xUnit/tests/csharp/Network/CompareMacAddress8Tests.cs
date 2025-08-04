using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCompareMacAddress8Tests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCompareMacAddress8Tests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CompareMacAddress8", Arguments = new List<FunctionArgument> { new FunctionArgument("address1", "MACADDR8"), new FunctionArgument("address2", "MACADDR8") }, ReturnType = "BOOLEAN", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-macaddr8", "compareMacAddress81", "MACADDR8 '08:00:2b:01:02:03:04:06', MACADDR8 '08-00-2b-01-02-03-04-06'", "is true" }, new object[] { "c#-macaddr8-null", "compareMacAddress82", "NULL::MACADDR8, MACADDR8 'ab-01-2b-31-41-fa-ab-ac'", "is true" } };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCompareMacAddress8(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class CompareMacAddress8TestsCSharp : BaseCompareMacAddress8Tests
{
    protected override string FunctionBody => @"
if (address1 == null)
    address1 = new PhysicalAddress(new byte[8] {171, 1, 43, 49, 65, 250, 171, 172});

if (address2 == null)
    address2 = new PhysicalAddress(new byte[8] {171, 1, 43, 49, 65, 250, 171, 172});

return address1.Equals(address2);
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}