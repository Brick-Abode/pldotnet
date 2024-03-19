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
    public struct CachedFunction
    {
        public string UserHandlerSourceCode;
        public string UserFunctionSourceCode;
        public string FunctionName;
        public bool SupportNullInput;
        public Func<List<IntPtr>, IntPtr, ulong, int, bool[], int> UserProcedure;
        public AssemblyLoadContext UserAssemblyLoadContext;
        public DotNETLanguage Language;
    }

    public struct CachedTrigger
    {
        public string UserHandlerSourceCode;
        public string UserFunctionSourceCode;
        public string FunctionName;
        public Func<IntPtr, IntPtr, string, string, string, string, int, string, string, string[], int> UserProcedure;
        public AssemblyLoadContext UserAssemblyLoadContext;
        public DotNETLanguage Language;
    }

    public static class Engine
    {
        public static bool AlwaysNullable = false;

        public static bool PrintSourceCode = false;

        public static bool SaveSourceCode = true;

        public static bool CompileFSharpWithFCS = false;

        public static int VerboseLevel = 0;

        public static string PathToSaveSourceCode = "/tmp/PlDotNET/GeneratedCodes";

        public static string PathToTemporaryFiles = "/tmp/PlDotNET/";

        public static IDictionary<uint, CachedFunction> FuncBuiltCodeDict = new Dictionary<uint, CachedFunction>();
        public static IDictionary<uint, CachedTrigger> TrigBuiltCodeDict = new Dictionary<uint, CachedTrigger>();

        public unsafe delegate int DelCompileUserFunction(
            uint functionId,
            IntPtr name,
            uint returnType,
            [MarshalAs(UnmanagedType.I1)] bool retset,
            [MarshalAs(UnmanagedType.I1)] bool is_trigger,
            IntPtr paramNames,
            uint* paramTypes,
            byte* paramModes,
            int num_output_values,
            IntPtr body,
            [MarshalAs(UnmanagedType.I1)] bool supportNullInput,
            IntPtr dotnetLanguage);

        public unsafe delegate int DelRunUserFunction(
            uint functionId,
            ulong call_id,
            int call_mode,
            void* arguments,
            int num_arguments,
            byte* nullmap,
            IntPtr output);

        public unsafe delegate int DelRunUserTFunction (
            uint functionId,
            int call_mode,
            IntPtr old_row_result,
            IntPtr new_row_result,
            string triggerName,
            string triggerWhen,
            string triggerLevel,
            string triggerEvent,
            int relationId,
            string tableName,
            string tableSchema,
            IntPtr arguments,
            int nargs);

        public delegate void DelFreeGenericGCHandle(IntPtr p);

        public delegate System.IntPtr DelBuildDatumList();

        public delegate void DelAddDatumToList(System.IntPtr list, System.IntPtr datum);

        public delegate void DelUnloadAssemblies(uint functionId);

        /// <summary>
        /// This function compiles the dynamic code using Roslyn.
        /// </summary>
        /// <returns>
        /// Returns The response of the dynamic code compiled with Roslyn.
        /// </returns>
        public static Microsoft.CodeAnalysis.Emit.EmitResult CompileSourceCode(string sourceCode, MemoryStream memStream, string assemblyName, MemoryStream memStreamUserFunction = null)
        {
            SyntaxTree userTree = SyntaxFactory.ParseSyntaxTree(sourceCode);

            var trustedAssembliesPathsArray = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")).Split(Path.PathSeparator);
            List<string> trustedAssembliesPaths = new ();
            trustedAssembliesPaths.AddRange(trustedAssembliesPathsArray);
            trustedAssembliesPaths.Add(typeof(NpgsqlPoint).Assembly.Location);
            trustedAssembliesPaths.Add(typeof(Elog).Assembly.Location);
            trustedAssembliesPaths.Add(typeof(NullLoggerFactory).Assembly.Location);
            trustedAssembliesPaths.Add(typeof(NpgsqlCommand).Assembly.Location);
            trustedAssembliesPaths.Add(typeof(CommandTests).Assembly.Location);

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
                syntaxTrees: new[] { userTree },
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
        /// <returns>
        /// Returns 0 when the process succeeded, otherwise returns 1.
        /// </returns>
        public static unsafe int CompileUserFunction(
                uint functionId,
                IntPtr name,
                uint returnTypeId,
                [MarshalAs(UnmanagedType.I1)] bool retset,
                [MarshalAs(UnmanagedType.I1)] bool is_trigger,
                IntPtr paramNames,
                uint* paramTypes,
                byte* paramModes,
                int num_output_values,
                IntPtr body,
                [MarshalAs(UnmanagedType.I1)] bool supportNullInput,
                IntPtr language)
        {
            // User function Data
            string funcName = Marshal.PtrToStringAuto(name);
            string auxParameters = Marshal.PtrToStringAuto(paramNames);
            string[] paramNameArray = auxParameters == null ? Array.Empty<string>() : auxParameters.Split(" ");
            uint[] paramTypeArray = auxParameters == null ? Array.Empty<uint>() : new ReadOnlySpan<uint>(paramTypes, paramNameArray.Length).ToArray();
            string funcBody = Marshal.PtrToStringAuto(body);
            byte[] paramModeArray = Array.Empty<byte>();

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
                                        is_trigger,
                                        paramNameArray,
                                        paramTypeArray,
                                        paramModeArray,
                                        num_output_values,
                                        funcBody,
                                        supportNullInput || Engine.AlwaysNullable);

                /// Create the F# UserFunction source code
                userFunctionCode = fs_dcg.BuildUserFunctionSourceCode();

                /// The path to the F# UserFunction assembly
                string userFunctionDll = string.Empty;

                /// Compile the F# UserFunction and assign the assembly path to the userFunctionDll variable
                if (!CompileFSharpWithFCS)
                {
                    DotNetProjectBuilder dfp = new (
                                                    "@PLDOTNET_TEMPLATE_DIR/UserFunctionProject.tfsproj",
                                                    Engine.PathToTemporaryFiles,
                                                    $"FSharpUserFunctionTemplate_{functionId}",
                                                    DotNETLanguage.FSharp);

                    // TODO Set verbose depending on PL.NET logging config
                    dfp.SetVerboseLevel(VerboseLevel);
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
                    userFunctionDll = FSharpCompiler.CompileFSharpSourceCodeAsDLL(functionId, Engine.PathToTemporaryFiles, userFunctionCode, extraAssemblies.ToArray());
                    #else
                    throw new SystemException("FSharp Compiler Service is not enabled in this build");
                    #endif
                }

                if (userFunctionDll == string.Empty)
                {
                    return 1;
                }

                /// Update function body and set the user assembly flag to true, so PL.NET will handle the F# function as a user assembly
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
                            is_trigger,
                            paramNameArray,
                            paramTypeArray,
                            paramModeArray,
                            num_output_values,
                            funcBody,
                            supportNullInput || Engine.AlwaysNullable,
                            CompileFSharpWithFCS, // Used to generate a UserHandler compatible with FCS DLL
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
            AssemblyLoadContext userAlc = new ($"UserFunction_{functionId}", true);
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

            if (is_trigger)
            {
                // Create the CachedTFunction to keep the function information
                CachedTrigger newCachedTFunction = new ()
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
                CachedFunction newCachedFunction = new ()
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
        /// <returns>
        /// Returns a memory stream object with the compiled UserFunction code.
        /// </returns>
        public static MemoryStream CreateMemoryStreamForUserFunctionCode(DotNETLanguage language, uint functionId, bool useUserAssembly, string userFunctionCode)
        {
            MemoryStream memUserFunction = new ();
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
        /// <returns>
        /// Returns a memory stream object with the compiled UserHandler code.
        /// </returns>
        public static MemoryStream CreateMemoryStreamForUserHandlerCode(DotNETLanguage language, uint functionId, string functionName, string userHandlerCode, MemoryStream assemblyToInclude)
        {
            MemoryStream memUserHandler = new ();
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
                return FSharpCompiler.CompileFSharpSourceCode(functionId, Engine.PathToTemporaryFiles, userHandlerCode, extraAssemblies.ToArray());
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

        public static unsafe int RunUserTFunction(
            uint functionId,
            int call_mode,
            IntPtr old_row_result,
            IntPtr new_row_result,
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
                        old_row_result,
                        new_row_result,
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
        /// <returns>
        /// Returns ReturnMode
        /// </returns>
        public static unsafe int RunUserFunction(uint functionId, ulong call_id, int call_mode, void* arguments, int num_arguments, byte* nullmap, IntPtr output)
        {
            string argaddr = ((IntPtr)arguments).ToString("X");

            IntPtr[] argumentArray = new ReadOnlySpan<IntPtr>(arguments, num_arguments).ToArray();
            List<IntPtr> argumentList = new (argumentArray);

            if (Engine.FuncBuiltCodeDict.TryGetValue(functionId, out CachedFunction cached))
            {
                try
                {
                    bool[] isnull = new bool[argumentList.Count];
                    if (cached.SupportNullInput || Engine.AlwaysNullable)
                    {
                        for (int i = 0, nargs = isnull.Length; i < nargs; i++)
                        {
                            isnull[i] = nullmap[i] != 0;
                        }
                    }

                    var retval = cached.UserProcedure(argumentList, output, call_id, call_mode, isnull);
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
        public static unsafe void AddDatumToList(System.IntPtr list, System.IntPtr datum)
        {
            GCHandle gchList = GCHandle.FromIntPtr(list);
            List<IntPtr> listObj = (List<IntPtr>)gchList.Target;
            listObj.Add(datum);
        }

        /// <summary>
        /// Unloads the assemblies of a specific function.
        /// </summary>
        public static void UnloadAssemblies(uint functionId)
        {
            if (!FuncBuiltCodeDict.ContainsKey(functionId))
            {
                Elog.Warning($"PL.NET could not find the generated function to unload its assemblies (ID: {functionId})");
                return;
            }

            FuncBuiltCodeDict[functionId].UserAssemblyLoadContext.Unload();
            FuncBuiltCodeDict.Remove(functionId);
        }

        /// <summary>
        /// Checks if PL.NET supports all the PostgreSQL types of the SQL user function.
        /// </summary>
        /// <returns>
        /// Returns true if all types are supported.
        /// </returns>
        public static bool CheckSupportedTypes(uint returnTypeId, uint[] paramTypes)
        {
            List<string> unsupportedTypes = new ();

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
        /// <returns>
        /// Returns true if the user provides a valid assembly, that is, 'UserAssembly.dll:UserNamespace.UserClass!FunctionName'.
        /// </returns>
        public static bool ValidateUserAssembly(string code)
        {
            return Regex.IsMatch(code, @"^([-/.a-zA-Z0-9]+.dll):([a-zA-Z0-9.]+)!([a-zA-Z0-9]+)$");
        }

        /// <summary>
        /// Checks if the user provides a valid Assembly. If so, modify the arguments with the assembly path, namespace and class names, and the method name.
        /// </summary>
        /// <returns>
        /// Returns true if the user provides a valid assembly, that is, 'UserAssembly.dll:UserNamespace.UserClass!FunctionName'.
        /// </returns>
        public static bool GetInformationFromUserAssembly(string code, ref string assemblyPath, ref string namespaceAndClass, ref string methodName)
        {
            Regex regex = new ("^([-/.a-zA-Z0-9]+.dll):([a-zA-Z0-9.]+)!([a-zA-Z0-9]+)$");
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
            if (Engine.SaveSourceCode)
            {
                if (!Directory.Exists(Engine.PathToSaveSourceCode))
                {
                    // Create the directory if it doesn't exist
                    Directory.CreateDirectory(Engine.PathToSaveSourceCode);
                }

                if (!CheckDirectoryMode(Engine.PathToSaveSourceCode))
                {
                    // Throw an exception if the directory doesn't have the correct mode
                    throw new SystemException($"Please specify a directory where the source codes can be saved and the directory must have a mode of 0700; current directory, '{Engine.PathToSaveSourceCode}', is no good.");
                }
            }

            // Check the access of the temporary files directory
            if (!Directory.Exists(Engine.PathToTemporaryFiles))
            {
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(Engine.PathToTemporaryFiles);
            }

            if (!CheckDirectoryMode(Engine.PathToTemporaryFiles))
            {
                // Throw an exception if the directory doesn't have the correct mode
                throw new SystemException($"Please specify a directory where the temporary files can be saved and the directory must have a mode of 0700; current directory, '{Engine.PathToTemporaryFiles}', is no good.");
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
            Process p = new ();
            p.StartInfo.FileName = "/usr/bin/stat";
            p.StartInfo.Arguments = path;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();

            // Read the output of the "stat" command
            string output = p.StandardOutput.ReadToEnd();

            // Use a regular expression to parse the output and extract the mode
            Match m = Regex.Match(output, @"Access:\s+\(([0-9]+)/");
            mode = m.Success ? mode = m.Groups[1].Value : mode;

            // If Linux mode didn't work, then we do Mac mode
            return (mode == "0700") || output.Contains("drwx------");
        }
    }

#nullable enable

}
