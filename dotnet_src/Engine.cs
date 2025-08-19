// PL/.NET (pldotnet) - PostgreSQL support for .NET C# and F# as
//                      procedural languages (PL)
//
//
// Copyright (c) 2023 Brick Abode
//
// This code is subject to the terms of the PostgreSQL License.
// The full text of the license can be found in the LICENSE file
// at the top level of the pldotnet repository.
//
// Engine.cs - pldotnet assembly compiler and runner
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;
using Npgsql.Tests;
using NpgsqlTypes;
using NUnit.Framework;
using PlDotNET.Common;
using PlDotNET.Handler;

#if ENABLE_FCS
using PlDotNET.FSharp;
#endif

namespace PlDotNET
{
    /// <summary>
    /// This class contains the cached information for a user function.
    /// </summary>
    public struct CachedFunction
    {
        /// <summary>
        /// The source code of the user handler.
        /// </summary>
        public string UserHandlerSourceCode;

        /// <summary>
        /// The source code of the user function.
        /// </summary>
        public string UserFunctionSourceCode;

        /// <summary>
        /// The name of the function.
        /// </summary>
        public string FunctionName;

        /// <summary>
        /// Indicates whether the function supports null input.
        /// </summary>
        public bool SupportNullInput;

        /// <summary>
        /// The user procedure delegate that will be called to execute the function.
        /// </summary>
        public Func<List<IntPtr>, IntPtr, ulong, int, bool[], int> UserProcedure;

        /// <summary>
        /// The assembly load context for the user function.
        /// </summary>
        public AssemblyLoadContext UserAssemblyLoadContext;

        /// <summary>
        /// The .NET language used for the function (C# or F#).
        /// </summary>
        public DotNETLanguage Language;
    }

    /// <summary>
    /// This class contains the cached information for a user trigger.
    /// </summary>
    public struct CachedTrigger
    {
        /// <summary>
        /// The source code of the user handler.
        /// </summary>
        public string UserHandlerSourceCode;

        /// <summary>
        /// The source code of the user function.
        /// </summary>
        public string UserFunctionSourceCode;

        /// <summary>
        /// The name of the function.
        /// </summary>
        public string FunctionName;

        /// <summary>
        /// The user procedure delegate that will be called to execute the trigger.
        /// </summary>
        public Func<IntPtr, IntPtr, string, string, string, string, int, string, string, string[], int> UserProcedure;

        /// <summary>
        /// The assembly load context for the user trigger.
        /// </summary>
        public AssemblyLoadContext UserAssemblyLoadContext;

        /// <summary>
        /// The .NET language used for the trigger (C# or F#).
        /// </summary>
        public DotNETLanguage Language;
    }

    /// <summary>
    /// This class contains the main functionality of PL.NET, including compiling and running user functions and triggers.
    /// It also manages the settings and cached information for compiled functions and triggers.
    /// </summary>
    public static partial class Engine
    {
        /// <summary>
        /// A delegate that compiles the user function code using Roslyn.
        /// </summary>
        /// <param name="functionId">The ID of the function to compile.</param>
        /// <param name="name">The name of the function.</param>
        /// <param name="returnType">The OID of the return type.</param>
        /// <param name="retset">True if the function returns a set; otherwise false.</param>
        /// <param name="isTrigger">True if the function is a trigger; otherwise false.</param>
        /// <param name="paramNames">A space-separated string of parameter names.</param>
        /// <param name="paramTypes">An array of OIDs representing the parameter types.</param>
        /// <param name="paramModes">An array of bytes representing the parameter modes.</param>
        /// <param name="numOutputValues">The number of output values.</param>
        /// <param name="body">The body of the function as a string.</param>
        /// <param name="supportNullInput">True if the function supports null input; otherwise false.</param>
        /// <param name="dotnetLanguage">The .NET language of the function (C# or F#).</param>
        public unsafe delegate int DelCompileUserFunction(
            uint functionId,
            IntPtr name,
            uint returnType,
            [MarshalAs(UnmanagedType.I1)] bool retset,
            [MarshalAs(UnmanagedType.I1)] bool isTrigger,
            IntPtr paramNames,
            uint* paramTypes,
            byte* paramModes,
            int numOutputValues,
            IntPtr body,
            [MarshalAs(UnmanagedType.I1)] bool supportNullInput,
            IntPtr dotnetLanguage);

        /// <summary>
        /// A delegate that runs a user function compiled by Roslyn.
        /// </summary>
        /// <param name="functionId">The ID of the function to run.</param>
        /// <param name="callId">The call ID for the function call.</param>
        /// <param name="callMode">The call mode for the function call.</param>
        /// <param name="arguments">A pointer to the arguments passed to the function.</param>
        /// <param name="num_arguments">The number of arguments passed to the function.</param>
        /// <param name="nullmap">A pointer to a byte array indicating which arguments are null.</param>
        /// <param name="output">A pointer to the output where the function result will be stored.</param>
        public unsafe delegate int DelRunUserFunction(
            uint functionId,
            ulong callId,
            int callMode,
            void* arguments,
            int num_arguments,
            byte* nullmap,
            IntPtr output);

        /// <summary>
        /// A delegate that runs a user trigger function compiled by Roslyn.
        /// </summary>
        /// <param name="functionId">The ID of the trigger function to run.</param>
        /// <param name="callMode">The call mode for the trigger function call.</param>
        /// <param name="oldRowResult">A pointer to the old row result for the trigger.</param>
        /// <param name="newRowResult">A pointer to the new row result for the trigger.</param>
        /// <param name="triggerName">The name of the trigger.</param>
        /// <param name="triggerWhen">The timing of the trigger (e.g., BEFORE, AFTER).</param>
        /// <param name="triggerLevel">The level of the trigger (e.g., ROW, STATEMENT).</param>
        /// <param name="triggerEvent">The event that fired the trigger (e.g., INSERT, UPDATE, DELETE).</param>
        /// <param name="relationId">The OID of the relation (table) associated with the trigger.</param>
        /// <param name="tableName">The name of the table associated with the trigger.</param>
        /// <param name="tableSchema">The schema of the table associated with the trigger.</param>
        /// <param name="arguments">A pointer to an array of arguments passed to the trigger.</param>
        /// <param name="nargs">The number of arguments passed to the trigger.</param>
        public unsafe delegate int DelRunUserTFunction(
            uint functionId,
            int callMode,
            IntPtr oldRowResult,
            IntPtr newRowResult,
            string triggerName,
            string triggerWhen,
            string triggerLevel,
            string triggerEvent,
            int relationId,
            string tableName,
            string tableSchema,
            IntPtr arguments,
            int nargs);

        /// <summary>
        /// A delegate that frees a generic GC handle.
        /// </summary>
        /// <param name="p">The pointer to the GC handle to free.</param>
        public delegate void DelFreeGenericGCHandle(IntPtr p);

        /// <summary>
        /// A delegate that builds a list of Datum objects.
        /// </summary>
        public delegate System.IntPtr DelBuildDatumList();

        /// <summary>
        /// A delegate that adds a Datum object to a list of Datum objects.
        /// </summary>
        /// <param name="list">The pointer to the list of Datum objects.</param>
        /// <param name="datum">The pointer to the Datum object to add.</param>
        public delegate void DelAddDatumToList(System.IntPtr list, System.IntPtr datum);

        /// <summary>
        /// A delegate that unloads assemblies from the AssemblyLoadContext.
        /// </summary>
        /// <param name="functionId">The ID of the function whose assemblies should be unloaded.</param>
        public delegate void DelUnloadAssemblies(uint functionId);

        /// <summary>
        /// Gets or sets the PL.NET settings for the current session.
        /// </summary>
        public static PlDotNETSettings Settings { get; set; } = new();

        /// <summary>
        /// Gets or sets the dictionary that contains compiled user-defined functions.
        /// </summary>
        public static Dictionary<uint, CachedFunction> FuncBuiltCodeDict { get; set; } = [];

        /// <summary>
        /// Gets or sets the dictionary that contains compiled user-defined triggers.
        /// </summary>
        public static Dictionary<uint, CachedTrigger> TrigBuiltCodeDict { get; set; } = [];

        /// <summary>
        /// This function compiles the dynamic code using Roslyn.
        /// </summary>
        /// <param name="sourceCode">The source code to compile.</param>
        /// <param name="memStream">The memory stream to write the compiled assembly to.</param>
        /// <param name="assemblyName">The name of the assembly to create.</param>
        /// <param name="memStreamUserFunction">An optional memory stream containing the user function assembly.</param>
        /// <returns>
        /// Returns The response of the dynamic code compiled with Roslyn.
        /// </returns>
        public static Microsoft.CodeAnalysis.Emit.EmitResult CompileSourceCode(string sourceCode, MemoryStream memStream, string assemblyName, MemoryStream memStreamUserFunction = null)
        {
            SyntaxTree userTree = SyntaxFactory.ParseSyntaxTree(sourceCode);

            var trustedAssembliesPathsArray = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")).Split(Path.PathSeparator);
            List<string> trustedAssembliesPaths =
            [
                .. trustedAssembliesPathsArray,
                typeof(NpgsqlPoint).Assembly.Location,
                typeof(Elog).Assembly.Location,
                typeof(NullLoggerFactory).Assembly.Location,
                typeof(NpgsqlCommand).Assembly.Location,
                typeof(CommandTests).Assembly.Location,
                typeof(DatumConversion).Assembly.Location,
            ];

#if ENABLE_FCS
            trustedAssembliesPaths.Add(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly.Location);
#endif

            var neededAssemblies = new[]
            {
                "System.Buffers",
                "System.Collections",
                "System.Collections.Generic",
                "System.ComponentModel.Primitives",
                "System.ComponentModel.TypeConverter",
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
                "Microsoft.Extensions.Logging.Abstractions",
                "Microsoft.CSharp",
                "Npgsql",
                "Npgsql.Tests",
                "NpgsqlTypes",
                "PlDotNET.Common",
                "PlDotNET.Handlers",
            };

            List<PortableExecutableReference> references = trustedAssembliesPaths
                .Where(p => neededAssemblies.Contains(Path.GetFileNameWithoutExtension(p)))
                .Select(p => MetadataReference.CreateFromFile(p))
                .ToList();

            if (memStreamUserFunction != null)
            {
                references.Add(MetadataReference.CreateFromFile(typeof(OutputResult).Assembly.Location));
                references.Add(MetadataReference.CreateFromStream(new MemoryStream(memStreamUserFunction.GetBuffer())));
            }

            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOptimizationLevel(OptimizationLevel.Release)
                .WithConcurrentBuild(true).WithAllowUnsafe(true);

            CSharpCompilation compilation = CSharpCompilation.Create(
                $"{assemblyName}.dll",
                options: compilationOptions,
                syntaxTrees: [userTree],
                references: references);

            Microsoft.CodeAnalysis.Emit.EmitResult compileResult = compilation.Emit(memStream);

            if (!compileResult.Success)
            {
                var sb = new System.Text.StringBuilder();
                sb.AppendLine($"PL.NET could not compile the following C# generated code:");
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

        /// <summary>
        /// This function is called called from C code and tries to create and
        /// compile the dynamic code using Roslyn. It also saves the
        /// CachedFunction in FuncBuiltCodeDict so that any compiled code can
        /// be called by the user function ID.
        /// This function returns 0 if all the codes were compiled correctly.
        /// </summary>
        /// <param name="functionId">The ID of the function to compile.</param>
        /// <param name="name">The name of the function.</param>
        /// <param name="returnTypeId">The OID of the return type.</param>
        /// <param name="retset">True if the function returns a set; otherwise false.</param>
        /// <param name="isTrigger">True if the function is a trigger; otherwise false.</param>
        /// <param name="paramNames">A space-separated string of parameter names.</param>
        /// <param name="paramTypes">An array of OIDs representing the parameter types.</param>
        /// <param name="paramModes">An array of bytes representing the parameter modes.</param>
        /// <param name="numOutputValues">The number of output values.</param>
        /// <param name="body">The body of the function as a string.</param>
        /// <param name="supportNullInput">True if the function supports null input; otherwise false.</param>
        /// <param name="language">The .NET language of the function (C# or F#).</param>
        /// <returns>
        /// Returns 0 when the process succeeded, otherwise returns 1.
        /// </returns>
        public static unsafe int CompileUserFunction(
                uint functionId,
                IntPtr name,
                uint returnTypeId,
                [MarshalAs(UnmanagedType.I1)] bool retset,
                [MarshalAs(UnmanagedType.I1)] bool isTrigger,
                IntPtr paramNames,
                uint* paramTypes,
                byte* paramModes,
                int numOutputValues,
                IntPtr body,
                [MarshalAs(UnmanagedType.I1)] bool supportNullInput,
                IntPtr language)
        {
            // Get the PL.NET settings for the current session
            Settings = new PlDotNETSettings();

            // User function Data
            string funcName = Marshal.PtrToStringAuto(name);
            string auxParameters = Marshal.PtrToStringAuto(paramNames);
            string[] paramNameArray = auxParameters == null ? [] : auxParameters.Split(" ");
            uint[] paramTypeArray = auxParameters == null ? [] : new ReadOnlySpan<uint>(paramTypes, paramNameArray.Length).ToArray();
            string funcBody = Marshal.PtrToStringAuto(body);
            byte[] paramModeArray = [];

            paramModeArray = (paramModes != null) ? new ReadOnlySpan<byte>(paramModes, paramNameArray.Length).ToArray() : paramModeArray;

            // Check if PL.NET supports all the PostgreSQL types of the user function
            if (!CheckSupportedTypes(returnTypeId, paramTypeArray))
            {
                Elog.Warning($"Unsupported return type: {returnTypeId}");
                return 1;
            }

            // Check the directories access. They need to be 0700.
            try
            {
                CheckDirectoriesAccess();
            }
            catch (Exception e)
            {
                Elog.Warning($"Error encountered when executing CheckDirectoriesAccess(): {e.GetType().Name}: {e.Message}");
                return 1;
            }

            // Check if the user provides an assembly with the user function
            // The syntax for that is 'UserAssembly.dll:UserNamespace.UserClass!FunctionName'
            bool useUserAssembly = ValidateUserAssembly(funcBody);

            // The language name (just csharp or fsharp for now)
            string plLanguage = Marshal.PtrToStringAuto(language);
            DotNETLanguage dotnetLanguage = plLanguage == "csharp" ? DotNETLanguage.CSharp : DotNETLanguage.FSharp;

            // The pldotnet creates a UserHandler for each function/trigger/procedure.
            // The UserHandler is responsible for getting data out of postgres and
            // converting it to the dotnet/NPGSQL form for inputs to the function, and
            // then performing the reverse procedure for the function's outputs.
            // The UserHandler is always implemented in C#, although we previously
            // implemented F# UserHandlers for F# functions; that code is mostly
            // disabled, and we are inclined to remove it in the future.
            //
            // If the source code for the function(etc) is passed from SQL, then
            // pldotnet also creates the UserFunction, which wraps that code in
            // the necessary template for it to be compiled. Alternatively,
            // if a DLL is passed, then pldotnet simply links to that DLL
            // and does not create a UserFunction.
            //
            // For C#, compilation of the UserFunction is achieved using Roslyn.
            // For F#, compilation is done either with F# Compiler Services ("FCS"),
            // or externally via dotnet build; the external build is the default
            // for F# because of typing and linkage issues with FCS-generated code.string.Empty, userHandlerCode = string.Empty;
            string userFunctionCode = string.Empty, userHandlerCode = string.Empty;
            MemoryStream memUserFunction, memUserHandler;

            if (!useUserAssembly && dotnetLanguage == DotNETLanguage.FSharp)
            {
                CodeGenerator fs_dcg = new FSharpCodeGenerator(
                                        funcName,
                                        returnTypeId,
                                        retset,
                                        isTrigger,
                                        paramNameArray,
                                        paramTypeArray,
                                        paramModeArray,
                                        numOutputValues,
                                        funcBody,
                                        supportNullInput || Settings.AlwaysNullable);

                // Create the F# UserFunction source code
                userFunctionCode = fs_dcg.BuildUserFunctionSourceCode();

                // The path to the F# UserFunction assembly
                string userFunctionDll = string.Empty;

                // Compile the F# UserFunction and assign the assembly path to the userFunctionDll variable
                if (!Settings.CompileFSharpWithFCS)
                {
                    DotNetProjectBuilder dfp = new(
                        "@PLDOTNET_TEMPLATE_DIR/UserFunctionProject.tfsproj",
                        Settings.PathToTemporaryFiles,
                        $"FSharpUserFunctionTemplate_{functionId}",
                        DotNETLanguage.FSharp);

                    // TODO Set verbose depending on PL.NET logging config
                    dfp.SetVerboseLevel(Settings.VerboseLevel);
                    userFunctionDll = dfp.BuildAndGenDLL(userFunctionCode);
                }
                else
                {
#if ENABLE_FCS
                    List<string> extraAssemblies = new ()
                    {
                        typeof(NpgsqlPoint).Assembly.Location,
                        typeof(Elog).Assembly.Location,
                        typeof(OutputResult).Assembly.Location,
                        typeof(NpgsqlCommand).Assembly.Location,
                        typeof(FSharpCompiler).Assembly.Location,
                        typeof(System.ComponentModel.Component).Assembly.Location,
                    };
                    userFunctionDll = FSharpCompiler.CompileFSharpSourceCodeAsDLL(functionId, Settings.PathToTemporaryFiles, userFunctionCode, extraAssemblies.ToArray());
#else
                    throw new SystemException("FSharp Compiler Service is not enabled in this build");
#endif
                }

                if (userFunctionDll == string.Empty)
                {
                    return 1;
                }

                // Update function body and set the user assembly flag to true, so PL.NET will handle the F# function as a user assembly
                funcBody = $"{userFunctionDll}:PlDotNET.UserSpace.UserFunction!{funcName}";
                useUserAssembly = true;
            }

            try
            {
                // The CodeGenerator object that creates the dynamic codes according to the language (C# or F#)
                CodeGenerator dcg = new CSharpCodeGenerator(
                            funcName,
                            returnTypeId,
                            retset,
                            isTrigger,
                            paramNameArray,
                            paramTypeArray,
                            paramModeArray,
                            numOutputValues,
                            funcBody,
                            supportNullInput || Settings.AlwaysNullable,
                            Settings.CompileFSharpWithFCS, // Used to generate a UserHandler compatible with FCS DLL
                            dotnetLanguage == DotNETLanguage.FSharp); // Used to perform changes in the UserHandler code for F# language

                // Create the UserFunction source code if the user does not provide an assembly
                // If it is using an already built assembly, the source code is the assembly path in the required format
                userFunctionCode = useUserAssembly ? funcBody : dcg.BuildUserFunctionSourceCode();

                // Generate the UserHandler code
                userHandlerCode = dcg.BuildUserHandlerSourceCode();
            }
            catch (Exception e)
            {
                Elog.Warning($"{e.GetType().Name}: {e.Message}");
                return 1;
            }

            // Check if the user function exists in .NET context
            if (Engine.FuncBuiltCodeDict.TryGetValue(functionId, out CachedFunction cached))
            {
                // check PL.NET needs to recompile the source codes
                if (cached.UserHandlerSourceCode == userHandlerCode && cached.UserFunctionSourceCode == userFunctionCode && !useUserAssembly)
                {
                    return 0;
                }

                FuncBuiltCodeDict[functionId].UserAssemblyLoadContext.Unload();
                FuncBuiltCodeDict.Remove(functionId);
            }

            // Check if the user trigger exists in .NET context
            if (Engine.TrigBuiltCodeDict.TryGetValue(functionId, out CachedTrigger cachedT))
            {
                // check PL.NET needs to recompile the source codes
                if (cachedT.UserHandlerSourceCode == userHandlerCode && cachedT.UserFunctionSourceCode == userFunctionCode && !useUserAssembly)
                {
                    return 0;
                }

                TrigBuiltCodeDict[functionId].UserAssemblyLoadContext.Unload();
                TrigBuiltCodeDict.Remove(functionId);
            }

            try
            {
                // Compile the UserFunction source code, if necessary, and copy the assembly to a MemoryStream object
                memUserFunction = CreateMemoryStreamForUserFunctionCode(dotnetLanguage, functionId, useUserAssembly, userFunctionCode);

                // Compile the UserHandler source code and then copy the assembly to a MemoryStream object
                memUserHandler = CreateMemoryStreamForUserHandlerCode(DotNETLanguage.CSharp, functionId, funcName, userHandlerCode, memUserFunction);
            }
            catch (Exception e)
            {
                Elog.Warning($"Error encountered: {e.GetType().Name}: {e.Message}");
                return 1;
            }

            // Load the assemblies into AssemblyLoadContext
            AssemblyLoadContext userAlc = new($"UserFunction_{functionId}", true);
            _ = userAlc.LoadFromAssemblyPath(typeof(NpgsqlCommand).Assembly.Location); // Npgsql Assembly
            _ = userAlc.LoadFromAssemblyPath(typeof(NpgsqlPoint).Assembly.Location); // NpgsqlTypes Assembly
            _ = userAlc.LoadFromAssemblyPath(typeof(NullLoggerFactory).Assembly.Location); // Logging Abstractions Assembly
            _ = userAlc.LoadFromAssemblyPath(typeof(Elog).Assembly.Location); // PlDotNET.Common Assembly
            _ = userAlc.LoadFromAssemblyPath(typeof(OutputResult).Assembly.Location); // PlDotNET.Handlers Assembly
            _ = userAlc.LoadFromAssemblyPath(typeof(CommandTests).Assembly.Location); // Npgsql.Tests Assembly
            _ = userAlc.LoadFromAssemblyPath(typeof(NUnitAttribute).Assembly.Location); // MonoTouch.NUnitLite Assembly
            _ = dotnetLanguage == DotNETLanguage.FSharp ?
                userAlc.LoadFromAssemblyPath(typeof(Microsoft.FSharp.Core.FSharpOption<>).Assembly.Location) : null; // FSharp.Core
            _ = userAlc.LoadFromStream(new MemoryStream(memUserFunction.GetBuffer())); // UserFunction Assembly
            Assembly userHandlerAssembly = userAlc.LoadFromStream(new MemoryStream(memUserHandler.GetBuffer())); // UserHandler Assembly

            if (isTrigger)
            {
                // Create the CachedTFunction to keep the function information
                CachedTrigger newCachedTFunction = new()
                {
                    UserFunctionSourceCode = userFunctionCode,
                    UserHandlerSourceCode = userHandlerCode,
                    FunctionName = funcName,
                    UserAssemblyLoadContext = userAlc,
                    UserProcedure = GetDirectTDelegate(userHandlerAssembly),
                    Language = dotnetLanguage,
                };

                // Add the CachedTFunction in the dictionary where the key is the function Id
                Engine.TrigBuiltCodeDict.Add(functionId, newCachedTFunction);
            }
            else
            {
                // Create the CachedFunction to keep the function information
                CachedFunction newCachedFunction = new()
                {
                    UserFunctionSourceCode = userFunctionCode,
                    UserHandlerSourceCode = userHandlerCode,
                    FunctionName = funcName,
                    SupportNullInput = supportNullInput,
                    UserAssemblyLoadContext = userAlc,
                    UserProcedure = GetDirectDelegate(userHandlerAssembly),
                    Language = dotnetLanguage,
                };

                // Add the CachedFunction in the dictionary where the key is the function Id
                Engine.FuncBuiltCodeDict.Add(functionId, newCachedFunction);
            }

            memUserFunction.Close();
            memUserHandler.Close();

            return 0;
        }

        /// <summary>
        /// This function returns a MemoryStream object which contains the Assembly for the UserFunction code.
        /// </summary>
        /// <param name="language">The .NET language used for the UserFunction (C# or F#).</param>
        /// <param name="functionId">The ID of the function for which the UserFunction is being created.</param>
        /// <param name="useUserAssembly">Indicates whether the user function is using a provided assembly.</param>
        /// <param name="userFunctionCode">The source code of the UserFunction or the path to the user assembly.</param>
        /// <returns>
        /// Returns a memory stream object with the compiled UserFunction code.
        /// </returns>
        public static MemoryStream CreateMemoryStreamForUserFunctionCode(DotNETLanguage language, uint functionId, bool useUserAssembly, string userFunctionCode)
        {
            MemoryStream memUserFunction = new();
            if (!useUserAssembly)
            {
                if (language == DotNETLanguage.CSharp)
                {
                    var compileResultUserFunction = CompileSourceCode(userFunctionCode, memUserFunction, $"UserFunction_{functionId}");

                    // Verify that the C# code for UserFunction compiled correctly
                    if (!compileResultUserFunction.Success)
                    {
                        throw new SystemException("PL.NET could not compile the generated C# code.");
                    }
                }
            }
            else
            {
                string userAssemblyPath = userFunctionCode.Split(":")[0];
                using var fs = File.Open(userAssemblyPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                fs.CopyTo(memUserFunction);
            }

            return memUserFunction;
        }

        /// <summary>
        /// This function returns a MemoryStream object which contains the Assembly for the UserHandler code.
        /// </summary>
        /// <param name="language">The .NET language used for the UserHandler (C# or F#).</param>
        /// <param name="functionId">The ID of the function for which the UserHandler is being created.</param>
        /// <param name="functionName">The name of the function for which the UserHandler is being created.</param>
        /// <param name="userHandlerCode">The source code of the UserHandler.</param>
        /// <param name="assemblyToInclude">An optional memory stream containing an assembly to include in the UserHandler.</param>
        /// <returns>
        /// Returns a memory stream object with the compiled UserHandler code.
        /// </returns>
        public static MemoryStream CreateMemoryStreamForUserHandlerCode(DotNETLanguage language, uint functionId, string functionName, string userHandlerCode, MemoryStream assemblyToInclude)
        {
            MemoryStream memUserHandler = new();
#if ENABLE_FCS
            if (language == DotNETLanguage.FSharp)
            {
                List<string> extraAssemblies = new ()
                {
                    typeof(NpgsqlPoint).Assembly.Location,
                    typeof(Elog).Assembly.Location,
                    typeof(OutputResult).Assembly.Location,
                    typeof(NpgsqlCommand).Assembly.Location,
                    typeof(FSharpCompiler).Assembly.Location,
                    typeof(System.ComponentModel.Component).Assembly.Location,
                };
                return FSharpCompiler.CompileFSharpSourceCode(functionId, Settings.PathToTemporaryFiles, userHandlerCode, extraAssemblies.ToArray());
            }
#else
            if (language == DotNETLanguage.FSharp)
            {
                throw new SystemException("FSharp Compiler Service is not enabled in this build");
            }
#endif

            var compileResultUserHandler = Engine.CompileSourceCode(userHandlerCode, memUserHandler, $"UserHandler_{functionId}", assemblyToInclude);

            // Verify that the C# code for UserHandler compiled correctly
            if (compileResultUserHandler.Success)
            {
                return memUserHandler;
            }

            throw new SystemException("PL.NET could not compile the generated C# code.");
        }

        /// <summary>
        /// It creates the Delegate function for the CallUserFunction function,
        /// which was compiled by Roslyn.
        /// </summary>
        /// <param name="compiledAssembly">The assembly that contains the compiled user handler code.</param>
        /// <returns>
        /// Returns the Function object of the delegated CallUserFunction or Null for a failed proccess.
        /// </returns>
        public static Func<List<IntPtr>, IntPtr, ulong, int, bool[], int> GetDirectDelegate(Assembly compiledAssembly)
        {
            Type procClassType = compiledAssembly.GetType("PlDotNET.UserSpace.UserHandler");

            if (procClassType == null)
            {
                Elog.Warning($"Failed to get type PlDotNET.UserSpace.UserHandler");
                return null;
            }

            return (Func<List<IntPtr>, IntPtr, ulong, int, bool[], int>)Delegate.CreateDelegate(
                typeof(Func<List<IntPtr>, IntPtr, ulong, int, bool[], int>),
                null,
                procClassType.GetMethod("CallUserFunction"));
        }

        /// <summary>
        /// It creates the Delegate function for the CallUserTrigger function,
        /// which was compiled by Roslyn.
        /// </summary>
        /// <param name="compiledAssembly">The assembly that contains the compiled user handler code.</param>
        /// <returns>
        /// Returns the Function object of the delegated CallUserTrigger or null for a failed proccess.
        /// </returns>
        public static Func<IntPtr, IntPtr, string, string, string, string, int, string, string, string[], int> GetDirectTDelegate(Assembly compiledAssembly)
        {
            if (compiledAssembly == null)
            {
                // Assembly is not loaded correctly
                // Add error handling or debugging information
                Elog.Error("Assembly not loaded correctly.");
                return null; // unreached
            }

            Type procClassType = compiledAssembly.GetType("PlDotNET.UserSpace.UserHandler");

            if (procClassType == null)
            {
                Elog.Error($"Failed to get type PlDotNET.UserSpace.UserHandler");
                return null; // unreached
            }

            var del = (Func<IntPtr, IntPtr, string, string, string, string, int, string, string, string[], int>)Delegate.CreateDelegate(
                    typeof(Func<IntPtr, IntPtr, string, string, string, string, int, string, string, string[], int>),
                    null,
                    procClassType.GetMethod("CallUserTrigger"));

            return del;
        }

        /// <summary>
        /// This function is called from C code and tries to run the user trigger function
        /// compiled by Roslyn. It retrieves the cached trigger function from the
        /// TrigBuiltCodeDict dictionary using the function ID. If the trigger function is found,
        /// it calls the UserProcedure delegate with the provided parameters.
        /// If the trigger function is not found, it logs a warning and returns an error code.
        /// </summary>
        /// <param name="functionId">The ID of the trigger function to run.</param>
        /// <param name="callMode">The call mode for the trigger function call.</param>
        /// <param name="oldRowResult">A pointer to the old row result for the trigger.</param>
        /// <param name="newRowResult">A pointer to the new row result for the trigger.</param>
        /// <param name="triggerName">The name of the trigger.</param>
        /// <param name="triggerWhen">The timing of the trigger (e.g., BEFORE, AFTER).</param>
        /// <param name="triggerLevel">The level of the trigger (e.g., ROW, STATEMENT).</param>
        /// <param name="triggerEvent">The event that fired the trigger (e.g., INSERT, UPDATE, DELETE).</param>
        /// <param name="relationId">The OID of the relation (table) associated with the trigger.</param>
        /// <param name="tableName">The name of the table associated with the trigger.</param>
        /// <param name="tableSchema">The schema of the table associated with the trigger.</param>
        /// <param name="arguments">A pointer to an array of arguments passed to the trigger.</param>
        /// <param name="nargs">The number of arguments passed to the trigger.</param>
        public static unsafe int RunUserTFunction(
            uint functionId,
            int callMode,
            IntPtr oldRowResult,
            IntPtr newRowResult,
            string triggerName,
            string triggerWhen,
            string triggerLevel,
            string triggerEvent,
            int relationId,
            string tableName,
            string tableSchema,
            IntPtr arguments,
            int nargs)
        {
            if (!Engine.TrigBuiltCodeDict.TryGetValue(functionId, out CachedTrigger cachedT))
            {
                Elog.Warning($"PL.NET could not find the user trigger (ID: {functionId})");
                return (int)ReturnMode.Error;
            }

            try
            {
                string[] argumentArray = new string[nargs];

                // for (int i = 0; i < nargs; i++)
                // {
                // argumentArray[i] = Marshal.PtrToStringAnsi(arguments[i]);
                // }
                char** args = (char**)arguments.ToPointer();
                if (args == null)
                {
                    throw new SystemException($"Got null trigger argument pointer from C");
                }

                for (int i = 0; i < nargs; i++)
                {
                    char* currentArgPtr = args[i];
                    argumentArray[i] = Marshal.PtrToStringAnsi((IntPtr)currentArgPtr);
                }

                // Create TriggerData object using the provided parameters
                var retval = cachedT.UserProcedure(
                        oldRowResult,
                        newRowResult,
                        triggerName,
                        triggerWhen,
                        triggerLevel,
                        triggerEvent,
                        relationId,
                        tableName,
                        tableSchema,
                        argumentArray);

                return retval;
            }
            catch (Exception e)
            {
                Elog.Warning($"{e.GetType().Name}: {e.Message}");
                return (int)ReturnMode.Error;
            }
        }

        /// <summary>
        /// This function is called called from C code and tries to get the
        /// cached function by the function id. If the cached functions is not
        /// found, an error message is reported. Otherwise, it calls the
        /// function compiled by Roslyn.
        /// </summary>
        /// <param name="functionId">The ID of the function to run.</param>
        /// <param name="callId">The call ID for the function call.</param>
        /// <param name="callMode">The call mode for the function call.</param>
        /// <param name="arguments">A pointer to the arguments passed to the function.</param>
        /// <param name="num_arguments">The number of arguments passed to the function.</param>
        /// <param name="nullmap">A pointer to a byte array indicating which arguments are null.</param>
        /// <param name="output">A pointer to the output where the function result will be stored.</param>
        /// <returns>
        /// Returns ReturnMode.
        /// </returns>
        public static unsafe int RunUserFunction(uint functionId, ulong callId, int callMode, void* arguments, int num_arguments, byte* nullmap, IntPtr output)
        {
            // Get the PL.NET settings for the current session
            Settings = new PlDotNETSettings();

            string argaddr = ((IntPtr)arguments).ToString("X");

            IntPtr[] argumentArray = new ReadOnlySpan<IntPtr>(arguments, num_arguments).ToArray();
            List<IntPtr> argumentList = new(argumentArray);

            if (Engine.FuncBuiltCodeDict.TryGetValue(functionId, out CachedFunction cached))
            {
                try
                {
                    bool[] isnull = new bool[argumentList.Count];
                    if (cached.SupportNullInput || Settings.AlwaysNullable)
                    {
                        for (int i = 0, nargs = isnull.Length; i < nargs; i++)
                        {
                            isnull[i] = nullmap[i] != 0;
                        }
                    }

                    var retval = cached.UserProcedure(argumentList, output, callId, callMode, isnull);
                    return retval;
                }
                catch (Exception e)
                {
                    Elog.Warning($"{e.GetType().Name}: {e.Message}");
                    return (int)ReturnMode.Error;
                }
            }

            Elog.Warning($"PL.NET could not find the user function (ID: {functionId})");
            return (int)ReturnMode.Error;
        }

        /// <summary>
        /// Free memmory pointed by a IntPtr.
        /// </summary>
        /// <param name="p">The IntPtr to free.</param>
        public static unsafe void FreeGenericGCHandle(IntPtr p)
        {
            GCHandle.FromIntPtr(p).Free();
        }

        /// <summary>
        /// This functions is called from C and creates a new list of IntPtr,
        /// which pldotnet adds the Datums and passes to RunUserFunction.
        /// </summary>
        /// <returns>
        /// Returns an empty list of IntPtr.
        /// </returns>
        public static unsafe System.IntPtr BuildDatumList()
        {
            GCHandle handle = GCHandle.Alloc(new List<IntPtr>(), GCHandleType.Normal);
            return GCHandle.ToIntPtr(handle);
        }

        /// <summary>
        /// This functions is called from C and adds an IntPtr(Datum) to a list,
        /// of IntPtr. Pldotnet passes the final list to RunUserFunction.
        /// </summary>
        /// <param name="list">The IntPtr to the list of IntPtr.</param>
        /// <param name="datum">The IntPtr to the Datum to add to the list.</param>
        public static unsafe void AddDatumToList(System.IntPtr list, System.IntPtr datum)
        {
            GCHandle gchList = GCHandle.FromIntPtr(list);
            List<IntPtr> listObj = (List<IntPtr>)gchList.Target;
            listObj.Add(datum);
        }

        /// <summary>
        /// Unloads the assemblies of a specific function.
        /// </summary>
        /// <param name="functionId">The ID of the function whose assemblies should be unloaded.</param>
        public static void UnloadAssemblies(uint functionId)
        {
            if (!FuncBuiltCodeDict.TryGetValue(functionId, out CachedFunction value))
            {
                Elog.Warning($"PL.NET could not find the generated function to unload its assemblies (ID: {functionId})");
                return;
            }

            value.UserAssemblyLoadContext.Unload();
            FuncBuiltCodeDict.Remove(functionId);
        }

        /// <summary>
        /// Checks if PL.NET supports all the PostgreSQL types of the SQL user function.
        /// </summary>
        /// <param name="returnTypeId">The OID of the return type.</param>
        /// <param name="paramTypes">An array of OIDs representing the parameter types.</param>
        /// <returns>
        /// Returns true if all types are supported.
        /// </returns>
        public static bool CheckSupportedTypes(uint returnTypeId, uint[] paramTypes)
        {
            List<string> unsupportedTypes = [];

            if ((OID)returnTypeId == OID.TRIGGEROID)
            {
                return true;
            }

            if (!(DatumConversion.ArrayTypes.ContainsKey((OID)returnTypeId) || DatumConversion.SupportedTypesStr.ContainsKey((OID)returnTypeId)))
            {
                unsupportedTypes.Add($"{(OID)returnTypeId}");
            }

            foreach (var paramType in paramTypes)
            {
                if (!(DatumConversion.ArrayTypes.ContainsKey((OID)paramType) || DatumConversion.SupportedTypesStr.ContainsKey((OID)paramType)))
                {
                    unsupportedTypes.Add($"{(OID)paramType}");
                }
            }

            if (unsupportedTypes.Count == 0)
            {
                return true;
            }

            // Give a helpful error message
            unsupportedTypes = unsupportedTypes.Distinct().ToList();

            var sb = new System.Text.StringBuilder();

            for (int i = 0, length = unsupportedTypes.Count; i < length; i++)
            {
                sb.AppendLine($"PL.NET does not support the PostgreSQL {unsupportedTypes[i]} type.");
            }

            sb.AppendLine("Please contact Brick Abode to inquire about adding support. <winning@brickabode.com>");
            Elog.Warning("\n" + sb.ToString());

            return false;
        }

        /// <summary>
        /// Checks if the user provides a valid Assembly.
        /// </summary>
        /// <param name="code">The code to validate.</param>
        /// <returns>
        /// Returns true if the user provides a valid assembly, that is, 'UserAssembly.dll:UserNamespace.UserClass!FunctionName'.
        /// </returns>
        public static bool ValidateUserAssembly(string code)
        {
            return MyRegex().IsMatch(code);
        }

        /// <summary>
        /// Checks if the user provides a valid Assembly. If so, modify the arguments with the assembly path, namespace and class names, and the method name.
        /// </summary>
        /// <param name="code">The code to validate.</param>
        /// <param name="assemblyPath">The path to the assembly.</param>
        /// <param name="namespaceAndClass">The namespace and class name.</param>
        /// <param name="methodName">The method name.</param>
        /// <returns>
        /// Returns true if the user provides a valid assembly, that is, 'UserAssembly.dll:UserNamespace.UserClass!FunctionName'.
        /// </returns>
        public static bool GetInformationFromUserAssembly(string code, ref string assemblyPath, ref string namespaceAndClass, ref string methodName)
        {
            Regex regex = MyRegex1();
            if (!regex.IsMatch(code))
            {
                return false;
            }

            string[] matches = regex.Split(code);
            assemblyPath = matches[1];
            namespaceAndClass = matches[2];
            methodName = matches[3];
            return true;
        }

        /// <summary>
        /// Check the access of the specified directories.
        /// </summary>
        /// <param name="language">The language of the source code.</param>
        /// <exception cref="SystemException">Thrown if the source code directory or the temporary files directory does not have a mode of 0700.</exception>
        public static void CheckDirectoriesAccess()
        {
            // Check the access of the directory to save the source codes.
            if (Settings.SaveSourceCode)
            {
                if (!Directory.Exists(Settings.PathToSaveSourceCode))
                {
                    // Create the directory if it doesn't exist
                    Directory.CreateDirectory(Settings.PathToSaveSourceCode);
                }

                if (!CheckDirectoryMode(Settings.PathToSaveSourceCode))
                {
                    // Throw an exception if the directory doesn't have the correct mode
                    throw new SystemException($"Please specify a directory where the source codes can be saved and the directory must have a mode of 0700; current directory, '{Settings.PathToSaveSourceCode}', is no good.");
                }
            }

            // Check the access of the temporary files directory
            if (!Directory.Exists(Settings.PathToTemporaryFiles))
            {
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(Settings.PathToTemporaryFiles);
            }

            if (!CheckDirectoryMode(Settings.PathToTemporaryFiles))
            {
                // Throw an exception if the directory doesn't have the correct mode
                throw new SystemException($"Please specify a directory where the temporary files can be saved and the directory must have a mode of 0700; current directory, '{Settings.PathToTemporaryFiles}', is no good.");
            }
        }

        /// <summary>
        /// Check the mode of the specified directory.
        /// </summary>
        /// <param name="path">The path to the directory.</param>
        /// <returns>True if the mode of the directory is 0700, false otherwise.</returns>
        public static bool CheckDirectoryMode(string path)
        {
            // Get the mode of the specified directory
            string mode = string.Empty;

            // Execute the "stat" command to get information about the directory
            Process p = new();
            p.StartInfo.FileName = "/usr/bin/stat";
            p.StartInfo.Arguments = path;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();

            // Read the output of the "stat" command
            string output = p.StandardOutput.ReadToEnd();

            // Use a regular expression to parse the output and extract the mode
            Match m = MyRegex2().Match(output);
            mode = m.Success ? _ = m.Groups[1].Value : mode;

            // If Linux mode didn't work, then we do Mac mode
            return (mode == "0700") || output.Contains("drwx------");
        }

        [GeneratedRegex(@"^([-/.a-zA-Z0-9]+.dll):([a-zA-Z0-9.]+)!([a-zA-Z0-9]+)$")]
        private static partial Regex MyRegex();

        [GeneratedRegex("^([-/.a-zA-Z0-9]+.dll):([a-zA-Z0-9.]+)!([a-zA-Z0-9]+)$")]
        private static partial Regex MyRegex1();

        [GeneratedRegex(@"Access:\s+\(([0-9]+)/")]
        private static partial Regex MyRegex2();
    }

#nullable enable

}
