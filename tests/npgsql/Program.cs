using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PlDotNET
{
    public static class TestCaseGenerator
    {
        private static readonly string PathToSaveGeneratedFile = "tests/npgsql/sql/file-name.sql";

        private static readonly List<string> KeyToIgnore = new ()
        {
            "PgPostmasterMock.Start",
            "ManualResetEvent(",
            "TimeSpan.From",
            "Thread.Sleep(",
        };

        private static readonly List<string> FilesToIgnore = new ()
        {
            "PoolTests",
            "TestUtil",
        };

        private static readonly Dictionary<string, string> ConstructorArguments = new ()
        {
            { "MultiplexingMode multiplexingMode", "MultiplexingMode.NonMultiplexing" },
            { "CommandBehavior behavior", "CommandBehavior.Default" },
            { "CompatMode compatMode", "CompatMode.OnePass" },
            { "SyncOrAsync syncOrAsync", "SyncOrAsync.Sync" },
            { "bool disableDateTimeInfinityConversions", "true" },
            { "NpgsqlDbType npgsqlDbType", "NpgsqlDbType.Json" }
        };

        private static readonly Dictionary<string, string> FunctionArguments = new ()
        {
            { "PrepareOrNot prepare", "PrepareOrNot.NotPrepared" },
            { "bool async", "true" },
            { "bool withErrorBarriers", "false" },
            { "bool dispose", "true" },
            { "bool enabled", "true" },
            { "PooledOrNot pooled", "PooledOrNot.Unpooled" },
            { "bool pooling", "false" },
            { "bool openFromClose", "false" },
            { "NpgsqlRange<DateTime> input", "new NpgsqlRange<DateTime>(new DateTime(2022, 1, 1, 12, 30, 30), true, false, new DateTime(2022, 12, 25, 17, 30, 30), false, false)" },
            { "DateTimeKind kind", "DateTimeKind.Unspecified" },
            { "int count", "5" }, // NpgsqlParameterCollectionTests -> it needs to be 5 or 3
            { "bool isAsync", "false"},
        };

        public static string FormatGeneratedCode(string sourceCode)
        {
            SyntaxTree userTree = SyntaxFactory.ParseSyntaxTree(sourceCode);
            SyntaxNode node = userTree.GetRoot().NormalizeWhitespace();
            sourceCode = node.ToFullString();
            return sourceCode;
        }

        private static string CreateCSharpBody(string className, string methodName, List<string> functionArguments, string constructorArguments, string comment, bool isAsync)
        {
            string waitCall = isAsync ? ".Wait()" : string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine($"/// {className}.{methodName}:");
            sb.AppendLine(comment);
            sb.AppendLine("try {");
            sb.AppendLine($"var test = new {className}({constructorArguments});");

            if (functionArguments.Count == 0)
            {
                sb.AppendLine($"test.{methodName}(){waitCall};");
            }
            else
            {
                foreach (var arg in functionArguments)
                {
                    sb.AppendLine($"test.{methodName}({arg}){waitCall};");
                }
            }

            sb.AppendLine("Elog.Info(\"Working fine on pldotnet 👍\");} catch (Exception e) {");
            // sb.AppendLine("return e.ToString(); }");
            sb.AppendLine("Elog.Info(\"Fail on C#...\\n\"+ e.ToString()); }");
            return FormatGeneratedCode(sb.ToString());
        }

        private static string CreateSQLFunction(string className, string methodName, List<string> functionArguments, string constructorArguments, string comment, bool isAsync)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"CREATE OR REPLACE PROCEDURE {className}_{methodName}() AS $$");
            sb.AppendLine(CreateCSharpBody(className, methodName, functionArguments, constructorArguments, comment, isAsync));
            sb.AppendLine("$$ LANGUAGE plcsharp;");
            // sb.AppendLine("INSERT INTO NPGSQL_TESTS (FEATURE, TEST_NAME, RESULT)");
            // sb.AppendLine($"SELECT '{className}', '{methodName}', {className}_{methodName}();\n");
            sb.AppendLine($"CALL {className}_{methodName}();\n");

            return sb.ToString();
        }

        private static void GenerateTestCases(string filePath, string fileName)
        {
            if (FilesToIgnore.Contains(fileName))
            {
                Console.WriteLine($"Skipping the tests in the {fileName} class...");
                return;
            }

            string comment = $"/// Method defined on the line XXX of the {filePath} file.";
            var sqlFile = new StringBuilder();
            var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath));
            var root = tree.GetRoot();

            var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>().Where(c => c.Modifiers.Any(SyntaxKind.PublicKeyword));
            foreach (var classDeclaration in classDeclarations)
            {
                string className = classDeclaration.Identifier.ValueText;
                if (className != fileName)
                {
                    continue;
                }

                Console.WriteLine($"Generating SQL files for the functions in the {className} class...");

                var constructors = classDeclaration.Members.OfType<ConstructorDeclarationSyntax>();

                List<string> arguments = new ();
                try
                {
                    foreach (var constructor in constructors)
                    {
                        foreach (var p in constructor.ParameterList.Parameters)
                        {
                            arguments.Add(ConstructorArguments[p.Type + " " + p.Identifier.ValueText]);
                        }

                        // Breaking to take only the first constructor
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Fail to generate the tests in the {className} class.");
                }

                string constructorArguments = arguments.Count > 0 ? string.Join(", ", arguments) : string.Empty;

                // Filtering the function that are public and return Task or void types
                var testMethods = classDeclaration.Members
                                   .OfType<MethodDeclarationSyntax>()
                                   .Where(m => (m.ReturnType.ToString() == "Task" || m.ReturnType.ToString() == "void") &&
                                               m.Modifiers.Any(x => x.ValueText == "public"));

                foreach (var testMethod in testMethods)
                {
                    bool isAsync = testMethod.Modifiers.Any(x => x.ValueText == "async");

                    try
                    {
                        var methodBody = testMethod.Body;

                        if (methodBody != null)
                        {
                            bool toIgnore = false;
                            foreach (var key in KeyToIgnore)
                            {
                                if (methodBody.ToFullString().Contains(key))
                                {
                                    toIgnore = true;
                                    break;
                                }
                            }

                            if (toIgnore)
                            {
                                Console.WriteLine($"Skipping test: {className}.{testMethod.Identifier.ValueText}");
                                continue;
                            }
                        }

                        var parameters = testMethod.ParameterList.Parameters;

                        var testCaseAttributes = testMethod.AttributeLists
                            .SelectMany(al => al.Attributes)
                            .Where(a => a.Name.ToString() == "TestCase");

                        List<string> functionArguments = new ();
                        foreach (var testCaseAttribute in testCaseAttributes)
                        {
                            List<string> args = new ();
                            var caseArgs = testCaseAttribute.ArgumentList.Arguments;
                            int cont = 0;
                            foreach (var caseArg in caseArgs)
                            {
                                string caseArgStr = caseArg.ToString();
                                if (!caseArgStr.Contains("TestName"))
                                {
                                    args.Add(caseArgStr);
                                    cont ++;
                                    if(cont > parameters.Count)
                                    {
                                        break;
                                    }
                                }
                            }
                            functionArguments.Add(string.Join(", ", args));
                        }

                        if (functionArguments.Count == 0)
                        {
                            // There is no attribute to call the function.
                            // Let's check if the function requires an argument.
                            if (parameters.Count > 0)
                            {
                                List<string> args = new ();
                                foreach (var p in parameters)
                                {
                                    args.Add(FunctionArguments[p.Type + " " + p.Identifier.ValueText]);
                                }

                                // Creating arguments to call the function according to the FunctionArguments dict
                                functionArguments.Add(string.Join(", ", args));
                            }
                        }

                        string commentMethod = comment.Replace("XXX", (testMethod.GetLocation().GetLineSpan().StartLinePosition.Line + 2 + testCaseAttributes.Count()).ToString());

                        string methodName = testMethod.Identifier.ValueText;
                        File.WriteAllText(PathToSaveGeneratedFile.Replace("file-name", $"{className}_{methodName}"), CreateSQLFunction(className, methodName, functionArguments, constructorArguments, commentMethod, isAsync), Encoding.UTF8);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Fail to generate test: {className}.{testMethod.Identifier.ValueText}");
                    }
                }
            }
        }

        private static void Main(string[] args)
        {
            foreach (var file in args)
            {
                GenerateTestCases(file, Path.GetFileNameWithoutExtension(file));
            }
        }
    }
}
