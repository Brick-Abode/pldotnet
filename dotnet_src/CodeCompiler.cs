using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using NpgsqlTypes;
using PlDotNET.Handler;
using CSharp = Microsoft.CodeAnalysis.CSharp;
using VB = Microsoft.CodeAnalysis.VisualBasic;

namespace PlDotNET
{
    public abstract class CodeCompiler
    {
        protected string[] neededAssemblies = new[]
        {
            "System.Buffers",
            "System.Collections",
            "System.Collections.Generic",
            "System.Console",
            "System.Core",
            "System.Data",
            "System.Data.Common",
            "System.Data.SqlClient",
            "System.Diagnostics",
            "System.Diagnostics.CodeAnalysis",
            "System.Globalization",
            "System.Linq",
            "System.Linq.Expressions",
            "System.Net.NetworkInformation",
            "System.Net.Primitives",
            "System.Private.CoreLib",
            "System.Runtime",
            "System.Text",
            "System.Text.Unicode",
            "Npgsql",
            "PlDotNET",
        };

        protected List<string> trustedAssembliesPaths = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")).Split(Path.PathSeparator).ToList();

        public CodeCompiler()
        {
            this.trustedAssembliesPaths.Add(typeof(NpgsqlPoint).Assembly.Location);
            this.trustedAssembliesPaths.Add(typeof(Engine).Assembly.Location);
        }

        /// <summary>
        /// This function compiles the dynamic code using Roslyn.
        /// </summary>
        /// <returns>
        /// Returns The response of the dynamic code compiled with Roslyn.
        /// </returns>
        public EmitResult CompileSourceCode(string sourceCode, MemoryStream memStream, string assemblyName, MemoryStream memStreamUserFunction = null)
        {
            SyntaxTree userTree = this.GetSyntaxTree(sourceCode);

            List<PortableExecutableReference> references = this.trustedAssembliesPaths
                .Where(p => this.neededAssemblies.Contains(Path.GetFileNameWithoutExtension(p)))
                .Select(p => MetadataReference.CreateFromFile(p))
                .ToList();

            if (memStreamUserFunction != null)
            {
                references.Add(MetadataReference.CreateFromStream(new MemoryStream(memStreamUserFunction.GetBuffer())));
            }

            CompilationOptions compilationOptions = this.GetCompilationOptions();

            Compilation compilation = this.GetCompilationObject(userTree, compilationOptions, references, assemblyName);

            EmitResult compileResult = compilation.Emit(memStream);

            if (!compileResult.Success)
            {
                var sb = new System.Text.StringBuilder();
                sb.AppendLine($"PL.NET could not compile the following VisualBasic generated code:");
                sb.AppendLine($"**********");
                sb.AppendLine($"{sourceCode}");
                sb.AppendLine($"**********");
                sb.AppendLine($"Here are the compilation results:");
                foreach (var diagnostic in compileResult.Diagnostics)
                {
                    sb.AppendLine(diagnostic.ToString());
                }

                Elog.Warning(sb.ToString());
            }

            return compileResult;
        }

        protected abstract SyntaxTree GetSyntaxTree(string sourceCode);

        protected abstract CompilationOptions GetCompilationOptions();

        protected abstract Compilation GetCompilationObject(
            SyntaxTree userTree,
            CompilationOptions compilationOptions,
            List<PortableExecutableReference> references,
            string assemblyName);

        protected abstract string GetLanguageName();
    }

    public class CSharpCompiler : CodeCompiler
    {
        protected override SyntaxTree GetSyntaxTree(string sourceCode)
        {
            return CSharp.SyntaxFactory.ParseSyntaxTree(sourceCode);
        }

        protected override CompilationOptions GetCompilationOptions()
        {
            return new CSharp.CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOptimizationLevel(OptimizationLevel.Release)
                .WithConcurrentBuild(true).WithAllowUnsafe(true);
        }

        protected override Compilation GetCompilationObject(
            SyntaxTree userTree,
            CompilationOptions compilationOptions,
            List<PortableExecutableReference> references,
            string assemblyName)
        {
            return CSharp.CSharpCompilation.Create(
                $"{assemblyName}.dll",
                options: (CSharp.CSharpCompilationOptions)compilationOptions,
                syntaxTrees: new[] { userTree },
                references: references);
        }

        protected override string GetLanguageName()
        {
            return "C#";
        }
    }

    public class VisualBasicCompiler : CodeCompiler
    {
        protected override SyntaxTree GetSyntaxTree(string sourceCode)
        {
            return VB.SyntaxFactory.ParseSyntaxTree(sourceCode);
        }

        protected override CompilationOptions GetCompilationOptions()
        {
            return new VB.VisualBasicCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOptimizationLevel(OptimizationLevel.Release)
                .WithConcurrentBuild(true);
        }

        protected override Compilation GetCompilationObject(
            SyntaxTree userTree,
            CompilationOptions compilationOptions,
            List<PortableExecutableReference> references,
            string assemblyName)
        {
            return VB.VisualBasicCompilation.Create(
                $"{assemblyName}.dll",
                options: (VB.VisualBasicCompilationOptions)compilationOptions,
                syntaxTrees: new[] { userTree },
                references: references);
        }

        protected override string GetLanguageName()
        {
            return "VisualBasic";
        }
    }
}