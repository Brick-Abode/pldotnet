using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide the directory path as an argument.");
            return;
        }

        var directoryPath = args[0];

        var csFiles = Directory.GetFiles(directoryPath, "*.cs", SearchOption.AllDirectories);

        foreach (var file in csFiles)
        {
            var isDllTest = file.Split(Path.DirectorySeparatorChar)
                .Any(folder => folder.Equals("Dll", StringComparison.OrdinalIgnoreCase));
            var code = File.ReadAllText(file);
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var root = syntaxTree.GetRoot() as CompilationUnitSyntax;
            if (root == null)
                continue; // Skip if the file does not contain a valid syntax tree

            var classDeclarations = root.DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .ToList();

            foreach (var classDeclaration in classDeclarations)
            {
                var className = classDeclaration.Identifier.Text;
                var baseClassName = $"Base{className}";
                var derivedClassName = $"{className}FSharp";

                // Prepare the abstract base class without attributes
                var baseClass = SyntaxFactory
                    .ClassDeclaration(baseClassName)
                    .AddModifiers(
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.AbstractKeyword)
                    )
                    .AddBaseListTypes(
                        SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName("PlDotNetTest"))
                    )
                    .WithAttributeLists(SyntaxFactory.List<AttributeListSyntax>()); // Ensure no attributes for the base class

                // Create abstract properties
                baseClass = baseClass.AddMembers(
                    SyntaxFactory
                        .PropertyDeclaration(SyntaxFactory.ParseTypeName("string"), "FunctionBody")
                        .AddModifiers(
                            SyntaxFactory.Token(SyntaxKind.ProtectedKeyword),
                            SyntaxFactory.Token(SyntaxKind.AbstractKeyword)
                        )
                        .AddAccessorListAccessors(
                            SyntaxFactory
                                .AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                        ),
                    SyntaxFactory
                        .PropertyDeclaration(
                            SyntaxFactory.ParseTypeName("LanguageType"),
                            "Language"
                        )
                        .AddModifiers(
                            SyntaxFactory.Token(SyntaxKind.ProtectedKeyword),
                            SyntaxFactory.Token(SyntaxKind.AbstractKeyword)
                        )
                        .AddAccessorListAccessors(
                            SyntaxFactory
                                .AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                        )
                );

                // Constructor
                var constructor = classDeclaration
                    .Members.OfType<ConstructorDeclarationSyntax>()
                    .FirstOrDefault();
                if (constructor != null)
                {
                    baseClass = baseClass.AddMembers(
                        constructor.WithIdentifier(SyntaxFactory.Identifier(baseClassName))
                    );
                }

                // TestCases method
                var testCasesMethod = classDeclaration
                    .Members.OfType<MethodDeclarationSyntax>()
                    .FirstOrDefault(m => m.Identifier.Text == "TestCases");
                if (testCasesMethod != null)
                {
                    baseClass = baseClass.AddMembers(testCasesMethod);
                }

                // Test method
                var testMethod = classDeclaration
                    .Members.OfType<MethodDeclarationSyntax>()
                    .FirstOrDefault(
                        m =>
                            m.AttributeLists.SelectMany(a => a.Attributes)
                                .Any(
                                    attr =>
                                        attr.Name.ToString() == "Theory"
                                        || attr.Name.ToString().EndsWith(".Theory")
                                )
                    );
                if (testMethod != null)
                {
                    baseClass = baseClass.AddMembers(testMethod);
                }

                // Prepare the derived class with original class attributes
                var derivedClass = SyntaxFactory
                    .ClassDeclaration(derivedClassName)
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddBaseListTypes(
                        SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(baseClassName))
                    )
                    .WithAttributeLists(classDeclaration.AttributeLists) // Apply attributes to the derived class
                    .AddMembers(
                        SyntaxFactory
                            .PropertyDeclaration(
                                SyntaxFactory.ParseTypeName("string"),
                                "FunctionBody"
                            )
                            .AddModifiers(
                                SyntaxFactory.Token(SyntaxKind.ProtectedKeyword),
                                SyntaxFactory.Token(SyntaxKind.OverrideKeyword)
                            )
                            .WithExpressionBody(
                                SyntaxFactory.ArrowExpressionClause(
                                    SyntaxFactory.ParseExpression(
                                        $"{classDeclaration.Members.OfType<FieldDeclarationSyntax>().FirstOrDefault()?.Declaration.Variables.FirstOrDefault()?.Initializer.Value.ToString()}"
                                    )
                                )
                            )
                            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                        SyntaxFactory
                            .PropertyDeclaration(
                                SyntaxFactory.ParseTypeName("LanguageType"),
                                "Language"
                            )
                            .AddModifiers(
                                SyntaxFactory.Token(SyntaxKind.ProtectedKeyword),
                                SyntaxFactory.Token(SyntaxKind.OverrideKeyword)
                            )
                            .WithExpressionBody(
                                SyntaxFactory.ArrowExpressionClause(
                                    SyntaxFactory.ParseExpression("LanguageType.PlfSharp")
                                )
                            )
                            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                    );

                // The DLL tests run without $$,
                // so we need to keep the GetFunctionDefinition from the original .cs test
                if (isDllTest)
                {
                    var getFunctionDefinitionMethod = classDeclaration
                        .Members.OfType<MethodDeclarationSyntax>()
                        .FirstOrDefault(m => m.Identifier.Text == "GetFunctionDefinition");

                    if (getFunctionDefinitionMethod != null)
                    {
                        derivedClass = derivedClass.AddMembers(getFunctionDefinitionMethod);
                    }
                }

                // Replace the original class with the new base and derived classes
                root = root.ReplaceNode(
                    classDeclaration,
                    new SyntaxNode[] { baseClass, derivedClass }
                );
            }

            File.WriteAllText(file, root.NormalizeWhitespace().ToFullString());
        }

        Console.WriteLine("xUnit test refactoring complete.");
    }
}
