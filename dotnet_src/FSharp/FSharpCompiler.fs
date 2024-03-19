// <copyright file="FSharpCompiler.fs" company="Brick Abode">
//
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
// </copyright>

#if INTERACTIVE
#r "FSharp.Compiler.Service.dll"
#endif

namespace PlDotNET.FSharp

open System
open System.Collections.Generic
open System.Diagnostics
open System.IO
open System.Reflection
open System.Runtime.InteropServices
open System.Runtime.Loader
open System.Text
open FSharp.Compiler.CodeAnalysis
open FSharp.Compiler.Text
open FSharp.Compiler.IO
open PlDotNET.Common

/// <summary>
/// The FSharpCompiler type provides methods for compiling F# source code.
/// </summary>
type FSharpCompiler() =

    /// <summary>
    /// The F# checker used for compiling F# source code.
    /// </summary>
    /// <seealso cref="FSharpChecker"/>
    /// <seealso cref="FSharpChecker.Create"/>
    static member checker = FSharpChecker.Create()

    /// <summary>
    /// Compiles the given F# source code and returns the path to the generated DLL.
    /// If the compilation fails, an error message is logged and an exception is thrown.
    /// </summary>
    /// <param name="functionId">The ID of the function.</param>
    /// <param name="temporaryPath">The temporary path to save the compiled assembly.</param>
    /// <param name="sourceCode">The F# source code to be compiled.</param>
    /// <param name="extraAssemblies">An array of strings containing the paths to any extra assemblies that should be included in the compilation.</param>
    /// <returns> A MemoryStream with the content of the generated DLL, or null compilation failed.</returns>
    static member CompileFSharpSourceCode (functionId: uint) (temporaryPath: string) (sourceCode: string) (extraAssemblies: string[]) : MemoryStream =
        let functionIdString = string functionId
        let fileName = "UserHandler_" + functionIdString
        let fakeInputFile : string = Path.ChangeExtension(Path.Combine(temporaryPath, fileName), ".fs")
        let outputFile : string = Path.ChangeExtension(fakeInputFile, ".dll")
        let options = FSharpCompiler.GetAllFlags fakeInputFile outputFile extraAssemblies

        let files = [| (fakeInputFile, sourceCode) |]
        let virtualFileSystem = VirtualFileSystem(files, FileSystem)
        FileSystem <- VirtualFileSystem(files, FileSystem)

        let errors, exitCode =
            FSharpCompiler.checker.Compile(options)
                |> Async.RunSynchronously

        match (exitCode) with
        | 0 ->
            let memUserHandler = new MemoryStream()
            let fs = File.Open(outputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            fs.CopyTo(memUserHandler) |> ignore
            File.Delete(outputFile) |> ignore
            File.Delete(Path.ChangeExtension(outputFile, ".pdb")) |> ignore
            memUserHandler
        | _ ->
            let sb = new System.Text.StringBuilder()
            sb.AppendLine($"PL.NET could not compile the following F# generated code:") |> ignore
            sb.AppendLine($"**********") |> ignore
            sb.AppendLine(sourceCode) |> ignore
            sb.AppendLine($"**********") |> ignore
            sb.AppendLine($"Here are the compilation results:") |> ignore
            for e in errors do
                sb.AppendLine(e.ToString()) |> ignore
            Elog.Warning(sb.ToString())
            failwith "PL.NET could not compile the generated F# code."
            null

    /// <summary>
    /// Compiles the given F# source code and returns the path to the generated DLL.
    /// If the compilation fails, an error message is logged and an exception is thrown.
    /// </summary>
    /// <param name="functionId">The ID of the function.</param>
    /// <param name="temporaryPath">The temporary path to save the compiled assembly.</param>
    /// <param name="sourceCode">The F# source code to be compiled.</param>
    /// <param name="extraAssemblies">An array of strings containing the paths to any extra assemblies that should be included in the compilation.</param>
    /// <returns>The path to the generated DLL, or null if the compilation failed.</returns>
    static member CompileFSharpSourceCodeAsDLL (functionId: uint) (temporaryPath: string) (sourceCode: string) (extraAssemblies: string[]) : string =
        let functionIdString = string functionId
        let fileName = "UserFunction" + functionIdString
        let fakeInputFile : string = Path.ChangeExtension(Path.Combine(temporaryPath, fileName), ".fs")
        let outputFile : string = Path.ChangeExtension(fakeInputFile, ".dll")
        let options = FSharpCompiler.GetAllFlags fakeInputFile outputFile extraAssemblies

        let files = [| (fakeInputFile, sourceCode) |]
        let virtualFileSystem = VirtualFileSystem(files, FileSystem)
        FileSystem <- VirtualFileSystem(files, FileSystem)

        Elog.Info("Executing f# compiler");

        let errors, exitCode =
            FSharpCompiler.checker.Compile(options)
                |> Async.RunSynchronously

        match (exitCode) with
        | 0 ->
            outputFile
        | _ ->
            let sb = new System.Text.StringBuilder()
            sb.AppendLine($"PL.NET could not compile the following F# generated code:") |> ignore
            sb.AppendLine($"**********") |> ignore
            sb.AppendLine(sourceCode) |> ignore
            sb.AppendLine($"**********") |> ignore
            sb.AppendLine($"Here are the compilation results:") |> ignore
            for e in errors do
                sb.AppendLine(e.ToString()) |> ignore
            Elog.Warning(sb.ToString())
            failwith "PL.NET could not compile the generated F# code."
            null

    /// <summary>
    /// Generates an array of strings containing the command-line flags to be passed to the F# compiler.
    /// </summary>
    /// <param name="input">The path to the input F# source file.</param>
    /// <param name="output">The path to the output DLL file.</param>
    /// <param name="extraAssemblies">An array of strings containing the paths to any extra assemblies that should be included in the compilation.</param>
    /// <returns>An array of strings containing the command-line flags to be passed to the F# compiler.</returns>
    /// <remarks>The generated array includes the paths to the required system assemblies and the F# Core library.</remarks>
    static member GetAllFlags (input : string) (output : string) (extraAssemblies : string[]) =
        let sysLib nm =
            let sysDir = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory()
            let (++) a b = System.IO.Path.Combine(a,b)
            sysDir ++ nm + ".dll"

        let allFlags =
            [| yield "fsc.exe";
               yield "-o"; yield output;
               yield "-a"; yield input;
               yield "--optimize-";
               yield "--noframework";
               yield "--debug:full";
               yield "--define:DEBUG";
               yield "--simpleresolution";
               yield "--doc:test.xml";
               yield "--warn:3";
               yield "--fullpaths";
               yield "--flaterrors";
               let references =
                 [ sysLib "mscorlib"
                   sysLib "System"
                   sysLib "System.Core"
                   sysLib "System.Linq"
                   sysLib "System.Data"
                   sysLib "System.Data.Common"
                   sysLib "System.Linq.Expressions"
                   sysLib "System.Runtime"
                   sysLib "System.Runtime.Numerics"
                   sysLib "System.Private.CoreLib"
                   sysLib "System.Collections"
                   sysLib "System.Net.Requests"
                   sysLib "System.Net.WebClient"
                   sysLib "System.Globalization"
                   sysLib "System.Runtime.InteropServices"
                   sysLib "System.Runtime.Extensions"
                   sysLib "System.Net.NetworkInformation"
                   sysLib "System.Net.Primitives" ]
               for r in references do
                     yield "-r:" + r
               for r in extraAssemblies do
                     yield "-r:" + r |]
        allFlags
