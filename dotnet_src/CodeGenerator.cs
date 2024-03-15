// <copyright file="CodeGenerator.cs" company="Brick Abode">
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using PlDotNET.Common;
using PlDotNET.Handler;

// # BuildUserFunctionSourceCode
//    - add parameter: bool retset
//    - UserFunction needs its type changed to be IEnumerable<ret_type> UserFunction(args)
//
// # BuildUserHandlerSourceCode
//    - add parameter: bool retset
//    - SRF UserHandler needs to handle SRF:
//        + CALL_SRF_FIRST:
//            + call UserFunction to get IEnumerator<ret_type>
//            + put in cache
//            + has_more = .EnumerableNext()
//        + CALL_SRF_NEXT:
//            + if (has_more == false) return {value = 0, ReturnMode = SrfDone}
//            + get IEnumerator<ret_type> from cache, return {value = .GetEnumerator(), ReturnMode = SrfNext}
//            + has_more = .EnumerableNext()
//    - Normal (non-SRF) UserHandler needs to require CALL_NORMAL
//    - Argument handling and return values are handled the same for both cases
//        + For SRF, argument handling is only for CALL_SRF_FIRST case
namespace PlDotNET
{
    public abstract class CodeGenerator
    {
        public string FuncName;
        public uint ReturnTypeId;
        public bool Retset;
        public bool IsTrigger;
        public string[] ParamNames;
        public uint[] ParamTypes;
        public byte[] ParamModes;
        public int NumOutputValues;
        public string FuncBody;
        public bool SupportNullInput;
        public string ComplexReturnType;
        public string SimpleReturnType;
        public string[] DotnetTypes;
        public int BcsrOutputValues;
        public string TableReturnType;

        public string UserFunctionPrefix = "PlDotNET.UserSpace.UserFunction";

        public string UserHandlerTemplatePath;
        public string UserTHandlerTemplatePath;
        public string UserFunctionTemplatePath;
        public bool UserHandlerForFSharp = false;

        public DotNETLanguage Language;

        public byte[] InputModes = new byte[] { (byte)ProArgMode.In, (byte)ProArgMode.InOut };
        public byte[] OutputModes = new byte[] { (byte)ProArgMode.Out, (byte)ProArgMode.InOut };

        // This is "output modes plus table"; it has nothing to do with trigger
        public byte[] TOutputModes = new byte[] { (byte)ProArgMode.Out, (byte)ProArgMode.InOut, (byte)ProArgMode.Table };

        // taken from catalog/pg_proc.h
        public enum ProArgMode : byte
        {
            [Description("PROARGMODE_IN")]
            In = (byte)'i',
            [Description("PROARGMODE_OUT")]
            Out = (byte)'o',
            [Description("PROARGMODE_INOUT")]
            InOut = (byte)'b',
            [Description("PROARGMODE_VARIADIC")]
            Variadic = (byte)'v',
            [Description("PROARGMODE_TABLE")]
            Table = (byte)'t',
        }

        public string DebugInfoOrig()
        {
            return $@"Debug info for {this.GetType().Name}:
                FuncName: {this.FuncName}
                ReturnTypeId: {this.ReturnTypeId}
                Retset: {this.Retset}
                IsTrigger: {this.IsTrigger}
                ParamNames: {string.Join(", ", this.ParamNames)}
                paramTypes: {string.Join(", ", this.ParamTypes)}
                paramModes: {string.Join(", ", this.ParamModes)}
                num_output_values: {this.NumOutputValues}
                funcBody: {this.FuncBody.Replace(Environment.NewLine, "\\n")}
                supportNullInput: {this.SupportNullInput}
                complexReturnType: {this.ComplexReturnType}
                simpleReturnType: {this.SimpleReturnType}
                dotnetTypes: {string.Join(", ", this.DotnetTypes)}
                bcsr_output_values: {this.BcsrOutputValues}
                tableReturnType: {this.TableReturnType}";
        }

        public string DebugInfo()
        {
            var sb = new StringBuilder();
            string newline = Environment.NewLine;

            try
            {
                sb.AppendLine($"Debug info for {this.GetType().Name}:");
                sb.AppendLine($"FuncName: {this.FuncName ?? "null"}");
                sb.AppendLine($"ReturnTypeId: {this.ReturnTypeId}");
                sb.AppendLine($"Retset: {this.Retset}");
                sb.AppendLine($"IsTrigger: {this.IsTrigger}");
                sb.AppendLine($"ParamNames: {(this.ParamNames != null ? string.Join(", ", this.ParamNames) : "null")}");
                sb.AppendLine($"paramTypes: {(this.ParamTypes != null ? string.Join(", ", this.ParamTypes) : "null")}");
                sb.AppendLine($"paramModes: {(this.ParamModes != null ? string.Join(", ", this.ParamModes) : "null")}");
                sb.AppendLine($"num_output_values: {this.NumOutputValues}");
                sb.AppendLine($"funcBody: {(this.FuncBody != null ? this.FuncBody.Replace(Environment.NewLine, "\\n") : "null")}");
                sb.AppendLine($"supportNullInput: {this.SupportNullInput}");
                sb.AppendLine($"complexReturnType: {this.ComplexReturnType ?? "null"}");
                sb.AppendLine($"simpleReturnType: {this.SimpleReturnType ?? "null"}");
                sb.AppendLine($"dotnetTypes: {(this.DotnetTypes != null ? string.Join(", ", this.DotnetTypes.Select(d => d ?? "null")) : "null")}");
                sb.AppendLine($"bcsr_output_values: {this.BcsrOutputValues}");
                sb.AppendLine($"tableReturnType: {this.TableReturnType ?? "null"}");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                Elog.Error($"Exception encountered while generating debug info. " +
                    $"Current state:\n{sb.ToString()}\nException:\n{ex}");
                throw;
            }
        }

        public void baseInitializer(
            string funcName,
            uint returnTypeId,
            bool retset,
            bool is_trigger,
            string[] paramNames,
            uint[] paramTypes,
            byte[] paramModes,
            int num_output_values,
            string funcBody,
            bool supportNullInput)
        {
            // first set the simple values
            this.FuncName = funcName;
            this.ReturnTypeId = returnTypeId;
            this.Retset = retset;
            this.IsTrigger = is_trigger;
            this.ParamNames = paramNames;
            this.ParamTypes = paramTypes;
            this.ParamModes = paramModes;
            this.NumOutputValues = num_output_values;
            this.FuncBody = funcBody;
            this.SupportNullInput = supportNullInput;
            this.ComplexReturnType = this.GetReturnType(returnTypeId, retset = false);
            this.SimpleReturnType = this.GetReturnType(returnTypeId, retset = false);

            if (this.IsTrigger)
            {
                Debug.Assert(!this.Retset, "Cannot return `SETOF` from a trigger");
                Debug.Assert(this.ReturnTypeId == (uint)OID.TRIGGEROID, "Trigger functions must return TRIGGEROID");
                Debug.Assert(this.ParamNames.Length == 0, "Trigger functions can take no arguments");
                this.DotnetTypes = Array.Empty<string>();
                this.SimpleReturnType = "int";
                this.ComplexReturnType = "int";

                // triggers otherwise ignore paramNames, returnTypeId, etc, so we don't set them here
                return;
            }

            // This is done differently for C# vs F#
            this.DotnetTypes = this.GetDotNetTypes();

            ////////////////////////////////////////
            // Handle table mode
            this.BcsrOutputValues = this.ParamModes.Contains((byte)ProArgMode.Table)
                    ? this.ParamModes.Count(mode => mode == (byte)ProArgMode.Table)
                    : this.NumOutputValues;

            // Generate the `IEnumerable<a, b, c>` for table mode
            this.ComplexReturnType = this.GetComplexReturnType(this.Retset);

            // How does retset work with records?
            //     - Table mode is retset, but we need the simple type without the `IEnumerable<>` around it
            //     - record with retset returns `IEnumerable<Object? []?>`
            //     - out variables return a record, but they are just `void` (in C#)
            this.SimpleReturnType = this.ParamModes.Contains((byte)ProArgMode.Table)
                    ? this.GetComplexReturnType(false)
                    : this.SimpleReturnType;
        }

        /// <summary>
        /// Filter the necessary handlers that need to be created in the generated code.
        /// </summary>
        /// <returns>
        /// Returns the type handlers that need to be added in the dynamic code.
        /// </returns>
        public List<string> FilterHandlers()
        {
            // We need an NPGSQL object for all arguments: IN/OUT/INOUT.  Thus, we
            // do not filter here by paramMode.
            List<string> allHandlers = new ();

            if ((OID)this.ReturnTypeId != OID.VOIDOID)
            {
                allHandlers.Add(DatumConversion.GetTypeHandlerName(this.ReturnTypeId));
            }

            for (int i = 0; i < this.ParamTypes.Length; i++)
            {
                allHandlers.Add(DatumConversion.GetTypeHandlerName(this.ParamTypes[i]));
            }

            return allHandlers.Distinct().ToList();
        }

        /// <summary>
        /// Prints the source code if Engine.PrintSourceCode is true.
        /// </summary>
        public void PrintSourceCode(string sourceCode)
        {
            if (Engine.PrintSourceCode)
            {
                Elog.Info("===========================");
                Elog.Info($"Source code:\n{sourceCode}");
                Elog.Info("===========================");
            }
        }

        /// <summary>
        /// A Nullable message to insert it into the dynamic code.
        /// </summary>
        /// <returns>
        /// Returns the Nullable message.
        /// </returns>
        public string GetNullableMessage()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"// As the SQL function named {this.FuncName} is `STRICT` or `RETURNS NULL ON NULL INPUT`,");
            sb.AppendLine("// `PL.NET` doesn't check whether any argument datum is null.");
            sb.AppendLine("// You can also set true for the `Engine.AlwaysNullable` variable");
            sb.AppendLine("// to always check whether the datum is null.");
            return sb.ToString();
        }

        /// <summary>
        /// Saves the source code if Engine.SaveSourceCode is true.
        /// </summary>
        public void SaveSourceCode(string sourceCode, string fileName)
        {
            if (Engine.SaveSourceCode)
            {
                string path = Path.Combine(Engine.PathToSaveSourceCode, fileName);
                path = Path.ChangeExtension(path, this.Language == DotNETLanguage.CSharp ? ".cs" : ".fs");
                File.WriteAllText(path, sourceCode, Encoding.UTF8);
            }
        }

        // This can be considated back into BuildUserHandlerSourceCode be mildly abstracting it
        public string BuildUserTHandlerSourceCode()
        {
            // Check if the file exists
            if (!File.Exists(this.UserTHandlerTemplatePath))
            {
                throw new SystemException($"Template file '{this.UserTHandlerTemplatePath}' not found");
            }

            string sourceCode = File.ReadAllText(this.UserTHandlerTemplatePath);

            string assemblyPath = string.Empty;
            _ = Engine.GetInformationFromUserAssembly(this.FuncBody, ref assemblyPath, ref this.UserFunctionPrefix, ref this.FuncName);

            sourceCode = sourceCode.Replace("// $handler_objects$", this.BuildHandlerObjects());

            sourceCode = sourceCode.Replace("// $user_function_call$", this.BuildFunctionCall());

            if (this.Language == DotNETLanguage.FSharp)
            {
                // Creates the UserFunction type with the SQL user function
                sourceCode = sourceCode.Replace("// $user_function_declaration$", this.BuildUserFunction());
            }

            sourceCode = this.FormatGeneratedCode(sourceCode);

            this.PrintSourceCode(sourceCode);
            this.SaveSourceCode(sourceCode, $"UserTHandler_{this.FuncName}");

            return sourceCode;
        }

        /// <summary>
        /// Creates the source code for the UserHandler according to the programming language.
        /// </summary>
        /// <returns>
        /// Returns the generated UserHandler source code.
        /// </returns>
        public string BuildUserHandlerSourceCode()
        {
            if (this.IsTrigger)
            {
                return this.BuildUserTHandlerSourceCode();
            }

            // Check if the file exists
            if (!File.Exists(this.UserHandlerTemplatePath))
            {
                throw new SystemException($"Template file '{this.UserHandlerTemplatePath}' not found");
            }

            string assemblyPath = string.Empty;
            _ = Engine.GetInformationFromUserAssembly(this.FuncBody, ref assemblyPath, ref this.UserFunctionPrefix, ref this.FuncName);

            string sourceCode = File.ReadAllText(this.UserHandlerTemplatePath);
            sourceCode = sourceCode.Replace("// $srf_cache$", this.BuildSRFCache());
            sourceCode = sourceCode.Replace("// $handler_objects$", this.BuildHandlerObjects());
            sourceCode = sourceCode.Replace("// $srf_begin$", this.BuildSRFBegin());
            sourceCode = sourceCode.Replace("// $create_arguments$", this.BuildCreateArguments(this.InputModes));
            sourceCode = sourceCode.Replace("// $user_function_call$", this.BuildFunctionCall());
            sourceCode = sourceCode.Replace("// $srf_middle$", this.BuildSRFMiddle());
            sourceCode = sourceCode.Replace("// $call_set_result$", this.BuildCallSetResult());
            sourceCode = sourceCode.Replace("// $srf_end$", this.BuildSRFEnd());
            sourceCode = sourceCode.Replace("// $fsharp_wrapper_for_inout$", this.BuildWrapperFunctionForInoutOut());

            if (this.Language == DotNETLanguage.FSharp)
            {
                // Creates the UserFunction type with the SQL user function
                sourceCode = sourceCode.Replace("// $user_function_declaration$", this.BuildUserFunction());
            }

            sourceCode = this.FormatGeneratedCode(sourceCode);

            this.PrintSourceCode(sourceCode);
            this.SaveSourceCode(sourceCode, $"UserHandler_{this.FuncName}");

            return sourceCode;
        }

        /// <summary>
        /// Creates the SRF cache, or else a small comment if not SRF
        /// </summary>
        /// <remarks>
        /// We need the cache so that the IEnumerator is not garbage
        /// collected.  We store it here and then free it (delete from
        /// the cache) when we get CALL_SRF_CLEANUP.
        /// </remarks>
        /// <returns>
        /// Returns the generated source code.
        /// </returns>
        public string BuildSRFCache()
        {
            return this.Retset ?
                $"public static Dictionary<ulong, IEnumerator<{this.SimpleReturnType}>> EnumeratorCache = new Dictionary<ulong, IEnumerator<{this.SimpleReturnType}>>();\n" :
                    "// skipping SRF cache; not a set-returning function";
        }

        /// <summary>
        /// Creates the opening for SRF handling, or else a small comment if not SRF
        /// </summary>
        /// <remarks>
        /// Generated SRF-handling code:
        ///     1. If CALL_SRF_FIRST, then
        /// that is the end, because the outer code cascades into input
        /// value setup and calling the function.
        /// </remarks>
        /// <returns>
        /// Returns the generated source code.
        /// </returns>
        public string BuildSRFBegin()
        {
            if (!this.Retset)
            {
                return "// skipping SRF setup; not a set-returning function";
            }

            return @"// Elog.Info($""Entering inside SRF code. call_id: {call_id}"");
                    if (call_mode == (int)CallMode.SrfFirst){
                        // Elog.Info($""Got SRF_FIRST on call_id {call_id}"");
                        ";
        }

        /// <summary>
        /// Creates the middle of the SRF handling, or else a small comment if not SRF
        /// </summary>
        /// <remarks>
        /// 1. we end the SRF_FIRST handling
        /// 2. we open the SRF_NEXT handling
        /// </remarks>
        /// <returns>
        /// Returns the generated source code.
        /// </returns>
        public string BuildSRFMiddle()
        {
            if (!this.Retset)
            {
                return "// skipping SRF middle; not a set-returning function";
            }

            var ret = @"
                    // Elog.Info($""Got SRF result ({result.GetType().Name}) {result}"");
                    var enumerator = result.GetEnumerator();
                    // Elog.Info($""Got SRF enumerator ({enumerator.GetType().Name}) {enumerator}; adding to cache under [{call_id}]"");
                    EnumeratorCache.Add(call_id, enumerator);
                    // Elog.Info($""Returning SRF_NEXT"");
                    return (int)ReturnMode.SrfNext;
                } else if (call_mode == (int)CallMode.SrfNext){
                    // Elog.Info($""Getting SRF enumerator (call_id {call_id})"");
                    // List<ulong> keyList = new List<ulong>(EnumeratorCache.Keys);
                    // Elog.Info($""EnumeratorCache.Keys: {string.Join(""\t"", keyList)}, call_id: {call_id}"");

                    var enumerator = EnumeratorCache[call_id];
                    // Elog.Info($""Got SRF enumerator ({enumerator.GetType().Name}) {enumerator}"");
                    if (enumerator.MoveNext() == false) {
                        // Elog.Info($""Enumerator is done; returning SRF_DONE"");
                        return (int)ReturnMode.SrfDone;
                    }
                    // Elog.Info($""Getting SRF current value"");
                    ";

            if (this.ParamModes.Contains((byte)ProArgMode.Table))
            {
                List<string> table_args = new List<string>();
                int argc = this.ParamTypes.Length;

                // assign them
                for (int i = 0; i < argc; i++)
                {
                    if (this.ParamModes[i] == (char)ProArgMode.Table)
                    {
                        table_args.Add($"argument_{i}");
                    }
                }

                string args = string.Join(", ", table_args);
                ret += $"\nvar ({args}) = enumerator.Current;\n";
            }
            else
            {
                ret += "var result = enumerator.Current;\n";
            }

            ret += @"
                    // Elog.Info($""Got next result {result}"");
                    // we now cascade to returning the value";
            return ret;
        }

        /// <summary>
        /// Creates the end of the SRF handling, or else a small comment if not SRF
        /// </summary>
        /// <remarks>
        /// 1. we return mode SrfNext
        /// 2. we close the SRF_NEXT handling
        /// 3. we handle CALL_SRF_CLEANUP
        /// 4. we error on all other cases
        /// </remarks>
        /// <returns>
        /// Returns the generated source code.
        /// </returns>
        public string BuildSRFEnd()
        {
            if (!this.Retset)
            {
                return "return (int)ReturnMode.Normal;";
            }

            return @"
                            return (int)ReturnMode.SrfNext;
                        } else if (call_mode == (int)CallMode.SrfCleanup){
                            EnumeratorCache.Remove(call_id);
                            return (int)ReturnMode.SrfDone;
                        }
                        Elog.Warning($""Unrecognized call mode: {call_mode}"");
                        return (int)ReturnMode.Error;
                        ";
        }

        /// <summary>
        /// Creates the source code for the UserFunction.
        /// </summary>
        /// <remarks>
        /// If the user provides an assembly file, this function returns the SQL user function body,
        /// i.e., 'UserAssembly.dll:UserNamespace.UserClass!FunctionName'.
        /// If the user function uses F#, this function returns an empty string.
        /// </remarks>
        /// <returns>
        /// Returns the generated UserFunction source code.
        /// </returns>
        public string BuildUserFunctionSourceCode()
        {
            if (Engine.ValidateUserAssembly(this.FuncBody))
            {
                return this.FuncBody;
            }

            if (!File.Exists(this.UserFunctionTemplatePath))
            {
                string msg = $"Template file '{this.UserFunctionTemplatePath}' not found";
                throw new SystemException(msg);
            }

            string sourceCode = File.ReadAllText(this.UserFunctionTemplatePath);
            sourceCode = sourceCode.Replace("// $user_function_declaration$", this.BuildUserFunction());

            sourceCode = this.FormatGeneratedCode(sourceCode);

            this.PrintSourceCode(sourceCode);
            this.SaveSourceCode(sourceCode, $"UserFunction_{this.FuncName}");

            return sourceCode;
        }

        public string GetReturnType(uint returnTypeId, bool retset)
        {
            if ((OID)returnTypeId == OID.TRIGGEROID)
            {
                return "int";
            }

            string returnType = DatumConversion.ArrayTypes.ContainsKey((OID)returnTypeId) ? "Array" : DatumConversion.SupportedTypesStr[(OID)returnTypeId];
            string nullAbleOutput = SetTypeAsNullable(returnType);
            returnType = retset ? SetTypeAsSequence(returnType, retset) : nullAbleOutput;
            return returnType;
        }

        /// <summary>
        /// Builds a wrapper function for handling inout and out parameters.
        /// </summary>
        /// <returns>The generated wrapper function as a string.</returns>
        public string BuildWrapperFunctionForInoutOut()
        {
            if (!this.UserHandlerForFSharp || !this.ParamModes.Any(mode => mode == (byte)ProArgMode.InOut || mode == (byte)ProArgMode.Out))
            {
                return string.Empty;
            }

            string aux = this.SupportNullInput ? "?" : string.Empty;
            List<string> parameters = new List<string>();
            List<string> outputParameters = new List<string>();
            List<string> inputParameters = new List<string>();

            for (int i = 0, argc = this.DotnetTypes.Length; i < argc; i++)
            {
                byte mode = this.ParamModes[i];

                if (mode == (byte)ProArgMode.In)
                {
                    parameters.Add($"{this.DotnetTypes[i]}{aux} argument_{i}");
                    inputParameters.Add($"({this.DotnetTypes[i]}{aux})argument_{i}");
                }
                else if (mode == (byte)ProArgMode.InOut)
                {
                    parameters.Add($"ref {this.DotnetTypes[i]}? argument_{i}");
                    inputParameters.Add($"({this.DotnetTypes[i]}{aux})argument_{i}");
                    outputParameters.Add($"argument_{i}");
                }
                else if (mode == (byte)ProArgMode.Out)
                {
                    parameters.Add($"out {this.DotnetTypes[i]}? argument_{i}");
                    outputParameters.Add($"argument_{i}");
                }
                else if (mode == (byte)ProArgMode.Table)
                {
                    // Table arguments are actually records
                    // No action here
                }
                else
                {
                    throw new SystemException(
                        $"Unsupported mode {mode} on parameter number {i} of {this.FuncName}: all modes are {string.Join(", ", this.ParamModes)}.  [3]");
                }
            }

            var sb = new System.Text.StringBuilder();
            sb.Append($"public static void {this.FuncName}_wrapper({string.Join(", ", parameters)}) {{\n");
            sb.Append($"({string.Join(", ", outputParameters)}) = {this.UserFunctionPrefix}.{this.FuncName}({string.Join(", ", inputParameters)});\n}}");

            return sb.ToString();
        }

        /// <summary>
        /// Sets the specified type as nullable.
        /// </summary>
        /// <param name="type">The type to set as nullable.</param>
        /// <returns>The nullable type.</returns>
        public abstract string SetTypeAsNullable(string type);

        /// <summary>
        /// Sets the type as a sequence.
        /// </summary>
        /// <param name="type">The type to set.</param>
        /// <param name="retset">A flag indicating whether to return the type as a set.</param>
        /// <returns>The type as a sequence.</returns>
        public abstract string SetTypeAsSequence(string type, bool retset);

        /// <summary>
        /// Gets the complex return type based on the specified conditions.
        /// </summary>
        /// <param name="retset">A boolean value indicating if it's a result set.</param>
        /// <returns>
        /// The complex return type. If it's not a result set, it returns the simple return type.
        /// If it's a result set, it returns the complex return type in the form of `IEnumerable<T>`
        /// or seq<T> for C# and F# respectively.
        /// </returns>
        public abstract string GetComplexReturnType(bool retset);

        /// <summary>
        /// Creates the code to create the handler objects that will be used to make the conversions.
        /// </summary>
        /// <returns>
        /// Returns the code to create the handler objects.
        /// </returns>
        public abstract string BuildHandlerObjects();

        /// <summary>
        /// This function creates the code to call the handler objects, which
        /// do the process of converting a Postgres type to an equivalente .NET
        /// type.
        /// </summary>
        /// <returns>
        /// Returns the user function arguments.
        /// </returns>
        public abstract string BuildCreateArguments(byte[] paramModes);

        /// <summary>
        /// This function creates code to call the user function.
        /// </summary>
        /// <returns>
        /// Returns the code to call the user function.
        /// </returns>
        public abstract string BuildFunctionCall();

        /// <summary>
        /// This function creates the code to create the Datum result according
        /// to the OID of the result function. It also adds the code to set the
        /// Datum object to the function output.
        /// </summary>
        /// <returns>
        /// Returns the created code as string.
        /// </returns>
        public abstract string BuildCallSetResult();

        /// <summary>
        /// This function creates user function.
        /// </summary>
        /// <returns>
        /// Returns user function as string.
        /// </returns>
        public abstract string BuildUserFunction();

        /// <summary>
        /// Get the .NET types of the SQL user function according to the language.
        /// </summary>
        /// <returns>
        /// Returns the types of each function argument.
        /// </returns>
        public abstract string[] GetDotNetTypes();

        /// <summary>
        /// This function formats the generated code.
        /// </summary>
        /// <returns>
        /// Returns the formatted code.
        /// </returns>
        public abstract string FormatGeneratedCode(string sourceCode);
    }

    public class CSharpCodeGenerator : CodeGenerator
    {
        public bool UserHandlerFSC;

        public CSharpCodeGenerator(
            string funcName,
            uint returnTypeId,
            bool retset,
            bool is_trigger,
            string[] paramNames,
            uint[] paramTypes,
            byte[] paramModes,
            int num_output_values,
            string funcBody,
            bool supportNullInput,
            bool userHandlerFSC,
            bool userHandlerForFSharp = false)
        {
            this.Language = DotNETLanguage.CSharp;
            this.UserHandlerFSC = userHandlerFSC;
            this.UserHandlerForFSharp = userHandlerForFSharp;
            this.baseInitializer(
                    funcName,
                    returnTypeId,
                    retset,
                    is_trigger,
                    paramNames,
                    paramTypes,
                    paramModes,
                    num_output_values,
                    funcBody,
                    supportNullInput);

            // Handle OUT/INOUT as `void` for C#, which is different than F#
            if (this.ParamModes.Intersect(this.OutputModes).Any())
            {
                this.SimpleReturnType = "void";
                this.ComplexReturnType = "void";
            }

            this.UserHandlerTemplatePath = "@PLDOTNET_TEMPLATE_DIR/UserHandler.tcs";
            this.UserTHandlerTemplatePath = "@PLDOTNET_TEMPLATE_DIR/UserTHandler.tcs";
            this.UserFunctionTemplatePath = "@PLDOTNET_TEMPLATE_DIR/UserFunction.tcs";
        }

        /// <inheritdoc />
        public override string BuildHandlerObjects()
        {
            var sb = new System.Text.StringBuilder();
            foreach (string handler in this.FilterHandlers())
            {
                sb.AppendLine($"public static {handler} {handler}Obj = new {handler}();");
            }

            return sb.ToString();
        }

        /// <inheritdoc />
        public override string BuildCreateArguments(byte[] paramModes)
        {
            var sb = new System.Text.StringBuilder();
            int argc = this.ParamTypes.Length;
            int skips = 0;

            sb.AppendLine(this.SupportNullInput ? string.Empty : this.GetNullableMessage());
            sb.AppendLine($"// BEGIN create arguments for {this.FuncName}");

            for (int i = 0; i < argc; i++)
            {
                string argType = (this.OutputModes.Contains(this.ParamModes[i]) || this.SupportNullInput) ? $"{this.DotnetTypes[i]}?" : this.DotnetTypes[i];
                byte paramMode = this.ParamModes[i];
                if (!this.ParamModes.Contains(paramMode))
                {
                    continue;
                }

                if (this.InputModes.Contains(paramMode))
                {
                    string handler = DatumConversion.GetTypeHandlerName(this.ParamTypes[i]);
                    string null_input = this.SupportNullInput ? $", isnull[{i - skips}]" : string.Empty;

                    string inputMethod = DatumConversion.ArrayTypes.ContainsKey((OID)this.ParamTypes[i]) ?
                        (this.SupportNullInput ? "InputNullableArray" : "InputArray") :
                        (this.SupportNullInput ? "InputNullableValue" : "InputValue");
                    sb.AppendLine($"{argType} argument_{i} = {handler}Obj.{inputMethod}(arguments[{i - skips}]{null_input});");
                }
                else if (paramMode == (byte)ProArgMode.Out)
                {
                    sb.AppendLine($"// Argument argument_{i} is {argType} because it's an output ('{((char)paramMode).ToString()}') variable");
                    sb.AppendLine($"{argType} argument_{i};");
                    skips += 1;
                }
                else if (paramMode == (byte)ProArgMode.Table)
                {
                    sb.AppendLine($"// Argument argument_{i} is a table argument; skipped");
                }
                else
                {
                    throw new SystemException($"Unsupported mode {paramMode} on parameter number {i} of {this.FuncName}: all modes are {string.Join(", ", this.ParamModes)}.  [1]");
                }
            }

            sb.Append($"// END create arguments for {this.FuncName}");
            return sb.ToString();
        }

        /// <inheritdoc />
        public override string BuildFunctionCall()
        {
            if (this.UserHandlerFSC)
            {
                return this.BuildFunctionCallForFSC();
            }

            var sb = new System.Text.StringBuilder();
            bool has_output_var = this.ParamModes.Intersect(this.OutputModes).Any();
            string aux = this.SupportNullInput ? "?" : string.Empty;
            List<string> parameters = new List<string>();

            sb.AppendLine(string.Empty);

            if (this.IsTrigger)
            {
                // Triggers are simple:
                //     - they take no arguments, other than the TriggerData
                //     - they return an integer, same as UserHandler
                string trigger_string = $@"
                    try
                    {{
                        rv = (int){this.UserFunctionPrefix}.{this.FuncName}(tg);

                    }}
                    catch (Exception ex)
                    {{
                        Elog.Warning(""Trigger gave an error: "" + ex.ToString());
                        return (int)ReturnMode.Error;
                    }}

                ";
                sb.Append(trigger_string);
                return sb.ToString();
            }

            if (this.SimpleReturnType != "void")
            {
                sb.AppendLine("var result = ");
            }

            if (this.UserHandlerForFSharp && this.ParamModes.Any(mode => mode == (byte)ProArgMode.InOut || mode == (byte)ProArgMode.Out))
            {
                // If the user handler is for F#, we need to call the wrapper function for INOUT/OUT parameters
                sb.Append($"{this.FuncName}_wrapper(");
            }
            else
            {
                sb.Append($"{this.UserFunctionPrefix}.{this.FuncName}(");
            }

            /*****************************************
            for (int i = 0, argc = this.DotnetTypes.Length; i < argc; i++)
            {
                switch (paramModes[i])
                {
                    case (byte)ProArgMode.In:
                        sb.Append($"({this.DotnetTypes[i]}{aux}) argument_{i}");
                        sb.Append((i < argc - 1) ? ", " : string.Empty);
                        break;
                    case (byte)ProArgMode.InOut:
                        sb.Append($"ref argument_{i}");
                        sb.Append((i < argc - 1) ? ", " : string.Empty);
                        break;
                    case (byte)ProArgMode.Out:
                        sb.Append($"out argument_{i}");
                        sb.Append((i < argc - 1) ? ", " : string.Empty);
                        break;
                    case (byte)ProArgMode.Table:
                        // not actually arguments
                        break;
                    default:
                        throw new SystemException($"Unrecognized parameter mode: {this.ParamModes[i]}, slot {i}");
                }

            }
            *****************************************/

            for (int i = 0, argc = this.DotnetTypes.Length; i < argc; i++)
            {
                byte mode = this.ParamModes[i];

                if (mode == (byte)ProArgMode.In)
                {
                    parameters.Add($"({this.DotnetTypes[i]}{aux}) argument_{i}");
                }
                else if (mode == (byte)ProArgMode.InOut || mode == (byte)ProArgMode.Out)
                {
                    string prefix = mode == (byte)ProArgMode.InOut ? "ref" : "out";
                    parameters.Add($"{prefix} argument_{i}");
                }
                else if (mode == (byte)ProArgMode.Table)
                {
                    // Table arguments are not actually arguments
                    // No action here
                }
                else
                {
                    throw new SystemException($"Unrecognized parameter mode: {mode}, slot {i}");
                }
            }

            sb.Append(string.Join(", ", parameters));

            sb.Append(");");
            return sb.ToString();
        }

        /// <inheritdoc />
        public string BuildFunctionCallForFSC()
        {
            var sb = new System.Text.StringBuilder();
            bool has_output_var = this.ParamModes.Intersect(this.OutputModes).Any();
            string aux = this.SupportNullInput ? "?" : string.Empty;
            List<string> parameters = new List<string>();

            sb.AppendLine(string.Empty);

            sb.AppendLine($"var type = typeof({this.UserFunctionPrefix});");
            sb.AppendLine($"var method = type.GetMethod(\"{this.FuncName}\");");

            if (this.IsTrigger)
            {
                // Triggers are simple:
                //     - they take no arguments, other than the TriggerData
                //     - they return an integer, same as UserHandler
                string trigger_string = $@"
                    try
                    {{
                        object[] parameters = new object[] {{tg}};
                        rv = (int) method.Invoke(null, parameters);
                    }}
                    catch (Exception ex)
                    {{
                        Elog.Warning(""Trigger gave an error: "" + ex.ToString());
                        return (int)ReturnMode.Error;
                    }}

                ";
                sb.Append(trigger_string);
                return sb.ToString();
            }

            for (int i = 0, argc = this.DotnetTypes.Length; i < argc; i++)
            {
                byte mode = this.ParamModes[i];

                if (mode == (byte)ProArgMode.In)
                {
                    parameters.Add($"({this.DotnetTypes[i]}{aux}) argument_{i}");
                }
                else if (mode == (byte)ProArgMode.InOut || mode == (byte)ProArgMode.Out)
                {
                    string prefix = mode == (byte)ProArgMode.InOut ? "ref" : "out";
                    parameters.Add($"{prefix} argument_{i}");
                }
                else if (mode == (byte)ProArgMode.Table)
                {
                    // Table arguments are not actually arguments
                    // No action here
                }
                else
                {
                    throw new SystemException($"Unrecognized parameter mode: {mode}, slot {i}");
                }
            }

            sb.Append($"object[] parameters = new object[] {{");
            sb.Append(string.Join(", ", parameters));
            sb.AppendLine($"}};");

            var rettype = this.GetReturnType(this.ReturnTypeId, retset: false);
            if (rettype != "void")
            {
                sb.AppendLine($"var result = ({rettype}) method.Invoke(null, parameters);");
            }
            else
            {
                sb.AppendLine($"var result = method.Invoke(null, parameters);");
            }

            return sb.ToString();
        }

        public override string BuildCallSetResult()
        {
            var sb = new System.Text.StringBuilder();
            bool is_out = this.ParamModes.Any(mode => (mode == (byte)ProArgMode.Out)
                    || (mode == (byte)ProArgMode.InOut)
                    || (mode == (byte)ProArgMode.Table));

            if ((OID)this.ReturnTypeId == OID.VOIDOID)
            {
                return string.Empty;
            }

            if ((OID)this.ReturnTypeId == OID.RECORDOID)
            {
                if (!is_out)
                {
                    sb.AppendLine("// Records are handled and set in one step");
                    sb.AppendLine("RecordHandlerObj.OutputSetValue(result, output);");
                    return sb.ToString();
                }
            }

            // This just assigns the Datum in the case where we want the return value.
            // That is, not using INOUT or OUT arguments.
            sb.AppendLine(string.Empty);
            sb.AppendLine($"// Handling {this.NumOutputValues} output values");
            if (this.NumOutputValues == 0)
            {
                sb.AppendLine($"// Handling normal function return (no INOUT/OUT arguments)");
                string output_handler = DatumConversion.ArrayTypes.ContainsKey((OID)this.ReturnTypeId) ? "OutputNullableArray" : "OutputNullableValue";
                sb.AppendLine($"IntPtr resultDatum = {DatumConversion.GetTypeHandlerName(this.ReturnTypeId)}Obj.{output_handler}(result);");
                sb.AppendLine($"OutputResult.SetDatumResult(resultDatum, result == null, output, 0, {this.ReturnTypeId});");
            }
            else if (this.NumOutputValues == 1)
            {
                // find the 1 output value and return it
                int output_parameter_offset = Enumerable.Range(0, this.ParamModes.Length).FirstOrDefault(i => this.OutputModes.Contains(this.ParamModes[i]), -1);
                var outResultName = $"argument_{output_parameter_offset}";
                string outHandler = DatumConversion.ArrayTypes.ContainsKey((OID)this.ReturnTypeId) ? "OutputNullableArray" : "OutputNullableValue";

                if (output_parameter_offset == -1)
                {
                    throw new SystemException($"Could not find output parameter from modes: {string.Join(", ", this.ParamModes)}");
                }

                sb.AppendLine($"// Handling single OUT return value `{outResultName}`, in slot {output_parameter_offset}");
                sb.AppendLine($"IntPtr resultDatum = {DatumConversion.GetTypeHandlerName(this.ReturnTypeId)}Obj.{outHandler}({outResultName});");
                sb.AppendLine($"OutputResult.SetDatumResult(resultDatum, {outResultName} == null, output, 0, {this.ReturnTypeId});");
            }
            else if (this.NumOutputValues > 1)
            {
                int i;
                int skips = 0;

                for (i = 0; i < this.ParamNames.Length; i++)
                {
                    if (!this.TOutputModes.Contains(this.ParamModes[i]))
                    {
                        sb.AppendLine($"// Skipping non-output-mode ({((char)this.ParamModes[i]).ToString()}) argument {i}");
                        skips += 1;
                        continue;
                    }

                    string handler = DatumConversion.GetTypeHandlerName(this.ParamTypes[i]);
                    var outResultName = $"argument_{i}";
                    var outputHandler = DatumConversion.ArrayTypes.ContainsKey((OID)this.ParamTypes[i]) ? "OutputNullableArray" : "OutputNullableValue";
                    sb.AppendLine($"// Adding output-mode ({((char)this.ParamModes[i]).ToString()}) argument {i} for oid {this.ReturnTypeId}");
                    sb.AppendLine($"IntPtr resultDatum_{i} = {handler}Obj.{outputHandler}({outResultName});");
                    sb.AppendLine($"OutputResult.SetDatumResult(resultDatum_{i}, argument_{i} == null, output, {i - skips}, {(int)this.ParamTypes[i]});");
                }
            }
            else
            {
                throw new SystemException($"Unsupported number of arguments: {this.NumOutputValues}");
            }

            return sb.ToString();
        }

        /// <inheritdoc />
        public override string BuildUserFunction()
        {
            var sb = new System.Text.StringBuilder();
            string aux = this.SupportNullInput ? "?" : string.Empty;
            List<string> parameters = new List<string>();

            // for Set-Returning Functions, we create a C# generator
            if (this.IsTrigger)
            {
                sb.Append($"public static ReturnMode {this.FuncName}(TriggerData tg");
                sb.Append($") {{\n#line 1\n{this.FuncBody}\n}}");
                return sb.ToString();
            }

            sb.Append($"public static {this.ComplexReturnType} {this.FuncName}(");

            for (int i = 0, argc = this.DotnetTypes.Length; i < argc; i++)
            {
                byte mode = this.ParamModes[i];

                if (mode == (byte)ProArgMode.In)
                {
                    parameters.Add($"{this.DotnetTypes[i]}{aux} {this.ParamNames[i]}");
                }
                else if (mode == (byte)ProArgMode.InOut || mode == (byte)ProArgMode.Out)
                {
                    string prefix = mode == (byte)ProArgMode.InOut ? "ref" : "out";
                    parameters.Add($"{prefix} {this.DotnetTypes[i]}? {this.ParamNames[i]}");
                }
                else if (mode == (byte)ProArgMode.Table)
                {
                    // Table arguments are actually records
                    // No action here
                }
                else
                {
                    throw new SystemException($"Unsupported mode {mode} on parameter number {i} of {this.FuncName}: all modes are {string.Join(", ", this.ParamModes)}.  [2]");
                }
            }

            sb.Append(string.Join(", ", parameters));

            sb.Append($") {{\n#line 1\n{this.FuncBody}\n}}");
            return sb.ToString();
        }

        /// <inheritdoc />
        public override string[] GetDotNetTypes()
        {
            return this.ParamTypes.Select(t => DatumConversion.ArrayTypes.ContainsKey((OID)t) ? "Array" : DatumConversion.SupportedTypesStr[(OID)t]).ToArray();
        }

        /// <inheritdoc />
        public override string FormatGeneratedCode(string sourceCode)
        {
            SyntaxTree userTree = SyntaxFactory.ParseSyntaxTree(sourceCode);
            SyntaxNode node = userTree.GetRoot().NormalizeWhitespace();
            sourceCode = node.ToFullString();
            return sourceCode;
        }

        /// <inheritdoc />
        public override string GetComplexReturnType(bool retset)
        {
            // ComplexReturnType is:
            //     - `IEnumerable<int, string>` if it's retset
            //     - `int`/`void`/etc otherwise
            // otherwise the plain type.
            string returnType;

            if (!this.ParamModes.Any(mode => this.TOutputModes.Contains(mode)))
            {
                returnType = this.SimpleReturnType;
            }
            else
            {
                // Filter by paramModes to get INOUT, OUT, and TABLE arguments
                var filteredParams = this.ParamNames.Zip(this.ParamTypes, (name, typeId) => new { Name = name, TypeId = typeId })
                    .Zip(this.ParamModes, (pair, mode) => new { pair.Name, pair.TypeId, Mode = mode })
                    .Where(param => this.TOutputModes.Contains(param.Mode));

                string filteredParamsStr = string.Join(", ", filteredParams.Select(p => $"Name:{p.Name}, TypeId:{p.TypeId}, Mode:{p.Mode}"));

                // does not handle `void` return types, as they're meaningless
                var args = filteredParams.Select(param =>
                        $"{(DatumConversion.ArrayTypes.ContainsKey((OID)param.TypeId) ? "Array" : DatumConversion.SupportedTypesStr[(OID)param.TypeId])}? {param.Name}")
                    .ToList();
                string argsStr = string.Join(", ", args);

                // join the args and put parentheses around compound types
                returnType = string.Join(", ", args);
                returnType = (args.Count > 1) ? $"({returnType})" : returnType;
            }

            // make it an IEnumerable if it's a retset
            returnType = retset ? $"IEnumerable<{returnType}>" : $"{returnType}";

            return returnType;
        }

        /// <inheritdoc />
        public override string SetTypeAsNullable(string type)
        {
            return (type == "void") ? type : $"{type}?";
        }

        /// <inheritdoc />
        public override string SetTypeAsSequence(string type, bool retset)
        {
            string nullableType = SetTypeAsNullable(type);
            return retset ? $"IEnumerable<{nullableType}>" : nullableType;
        }
    }

    public class FSharpCodeGenerator : CodeGenerator
    {
        /// <summary>
        /// This Dictionary contains the C# types that differs from F# type names.
        /// </summary>
        private static readonly Dictionary<string, string> FSharpTypes =
               new ()
        {
            // TODO: Check if DatumConversion can return any other Nullable or other anomaly
            { "Object?[]", "obj[]" },
            { "float", "float32" },
            { "short", "int16" },
            { "long", "int64" },
            { "NpgsqlRange<long>", "NpgsqlRange<int64>" },
            { "(IPAddress Address, int Netmask)", "struct(IPAddress*int)" },
        };

        /// <summary>
        /// This List contains object types which are inherently Nullable.
        /// </summary>
        private static readonly List<string> ClassTypes =
               new ()
        {
            "Nullable<obj[]>",
            "obj[]",
            "Array",
            "byte[]",
            "BitArray",
            "string",
            "PhysicalAddress",
        };

        public FSharpCodeGenerator(
            string funcName,
            uint returnTypeId,
            bool retset,
            bool is_trigger,
            string[] paramNames,
            uint[] paramTypes,
            byte[] paramModes,
            int num_output_values,
            string funcBody,
            bool supportNullInput)
        {
            this.Language = DotNETLanguage.FSharp;
            this.baseInitializer(
                    funcName,
                    returnTypeId,
                    retset,
                    is_trigger,
                    paramNames,
                    paramTypes,
                    paramModes,
                    num_output_values,
                    funcBody,
                    supportNullInput);

            this.UserHandlerTemplatePath = "@PLDOTNET_TEMPLATE_DIR/UserHandler.tfs";
            this.UserTHandlerTemplatePath = "@PLDOTNET_TEMPLATE_DIR/UserTHandler.tfs";
            this.UserFunctionTemplatePath = "@PLDOTNET_TEMPLATE_DIR/UserFunction.tfs";
        }

        /// <summary>
        /// Use the FSharpTypes Dictionary to convert C# types that differs from F# type names if necessary
        /// </summary>
        public static string EnsureFSharpType(string type)
        {
            return FSharpTypes.ContainsKey(type) ? FSharpTypes[type] : type;
        }

        /// <summary>
        /// Indents code according to the provided number of space.
        /// </summary>
        /// <returns>
        /// Returns the indented code.
        /// </returns>
        public static string IndentCode(string code, uint spaceNumber)
        {
            string indentation = new (' ', (int)spaceNumber);
            string newline = string.Empty;
            var sb = new System.Text.StringBuilder();

            foreach (string line in code.Split("\n"))
            {
                sb.Append(string.IsNullOrWhiteSpace(line) ? newline : $"{newline}{indentation}{line}");
                newline = "\n";
            }

            return sb.ToString();
        }

        /// <inheritdoc />
        public override string BuildHandlerObjects()
        {
            var sb = new System.Text.StringBuilder();
            this.FilterHandlers().ToList().ForEach(handler => sb.AppendLine($"let {handler}Obj = new {handler}()"));
            return "// handler objects\n" + IndentCode(sb.ToString(), 8);
        }

        /// <inheritdoc />
        public override string BuildCreateArguments(byte[] paramModes)
        {
            int skips = 0, argc = this.ParamTypes.Length;
            var sb = new System.Text.StringBuilder();

            sb.AppendLine($"// BEGIN create arguments for {this.FuncName}");

            for (int i = 0; i < argc; i++)
            {
                // Because F# is a functional language, it does not support INOUT or OUT arguments like C# does.
                // Instead, IN and INOUT are treated as normal arguments, and INOUT and OUT get `output_{i}` variables
                // to receive their return values.
                string handler = DatumConversion.GetTypeHandlerName(this.ParamTypes[i]);
                string argType = (this.OutputModes.Contains(this.ParamModes[i]) || this.SupportNullInput) ? $"{this.DotnetTypes[i]}?" : this.DotnetTypes[i];

                if (this.InputModes.Contains(this.ParamModes[i]))
                {
                    string handlerName = DatumConversion.GetTypeHandlerName(this.ParamTypes[i]);
                    string null_input = this.SupportNullInput ? $", isnull[{i - skips}]" : string.Empty;
                    string inputMethod = DatumConversion.ArrayTypes.ContainsKey((OID)this.ParamTypes[i]) ?
                        (this.SupportNullInput ? "InputNullableArray" : "InputArray") :
                        (this.SupportNullInput ? "InputNullableValue" : "InputValue");
                    sb.AppendLine($"let argument_{i} = {handler}Obj.{inputMethod}(arguments[{i - skips}]{null_input});");
                }
                else if (this.ParamModes[i] == (byte)ProArgMode.Out)
                {
                    sb.AppendLine($"// skipping output argument {i}");
                    skips += 1;
                }
                else
                {
                    throw new SystemException($"Unsupported mode {this.ParamModes[i]} on parameter number {i} of {this.FuncName}: all modes are {string.Join(", ", this.ParamModes)}.  [1]");
                }
            }

            sb.AppendLine($"// END create arguments for {this.FuncName}");
            return "\n" + IndentCode(sb.ToString(), 8);
        }

        /// <inheritdoc />
        public override string BuildFunctionCall()
        {
            List<string> retvals = new List<string>();
            List<string> arguments = new List<string>();
            List<string> variables = new List<string>();
            string let_result;
            int i, output_num = 0;

            for (i = 0; i < this.DotnetTypes.Length; i++)
            {
                if (this.InputModes.Contains(this.ParamModes[i]))
                {
                    arguments.Add($"argument_{i}");
                }

                if (this.OutputModes.Contains(this.ParamModes[i]))
                {
                    variables.Add($"{this.DotnetTypes[i]} output_{i};");
                    retvals.Add($"output_{i}");
                    output_num++;
                }
            }

            // set the `let_result` string to get the return values
            if (retvals.Count > 1)
            {
                let_result = $"let {string.Join(", ", retvals)} = ";
            }
            else if (retvals.Count == 1)
            {
                let_result = $"let {retvals[0]} = ";
            }
            else if (retvals.Count == 0)
            {
                let_result = ((OID)this.ReturnTypeId != OID.VOIDOID) ? "let result = " : string.Empty;
            }
            else
            {
                throw new SystemException($"Bizarre retvals.Count {retvals.Count}");
            }

            string argstring = string.Join(" ", arguments);

            return "// Calling user function\n" + IndentCode($"{let_result}UserFunction.{this.FuncName} {argstring}\n", 8);
        }

        /// <inheritdoc />
        public override string BuildCallSetResult()
        {
            int i, output_num = 0;
            var sb = new System.Text.StringBuilder();

            if ((OID)this.ReturnTypeId == OID.VOIDOID)
            {
                return string.Empty;
            }

            if (this.NumOutputValues < 0)
            {
                throw new SystemException($"Unrecognized num_output_values: {this.NumOutputValues}");
            }

            if (this.NumOutputValues == 0)
            {
                // use "result"
                string type = DatumConversion.ArrayTypes.ContainsKey((OID)this.ReturnTypeId) ? "Array" : DatumConversion.SupportedTypesStr[(OID)this.ReturnTypeId];
                string returnType = EnsureFSharpType(type);
                string outputHandler = DatumConversion.ArrayTypes.ContainsKey((OID)this.ReturnTypeId) ? "OutputNullableArray" : "OutputNullableValue";
                string isnull = ClassTypes.Contains(returnType) ? "Object.ReferenceEquals(result, null)" : "not result.HasValue";
                sb.AppendLine($"// Handling normal function return (no INOUT/OUT arguments)");

                string makeDatum = $"let resultDatum = {DatumConversion.GetTypeHandlerName(this.ReturnTypeId)}Obj.{outputHandler}(result)";
                sb.AppendLine(makeDatum);
                string setDatum = $"OutputResult.SetDatumResult(resultDatum, {isnull}, output, 0, uint32 {this.ReturnTypeId})";
                sb.AppendLine(setDatum);
                return "// Create PostgreSQL datum\n" + IndentCode(sb.ToString(), 8);
            }

            // num_output_values > 1, so use "output_0", "output_1", etc
            for (i = 0; i < this.ParamTypes.Length; i++)
            {
                if (this.OutputModes.Contains(this.ParamModes[i]))
                {
                    string outputTypeHandler = DatumConversion.GetTypeHandlerName(this.ParamTypes[i]);
                    string type = DatumConversion.ArrayTypes.ContainsKey((OID)this.ParamTypes[i]) ? "Array" : DatumConversion.SupportedTypesStr[(OID)this.ParamTypes[i]];
                    string returnType = EnsureFSharpType(type);
                    string outputHandlerMethod = DatumConversion.ArrayTypes.ContainsKey((OID)this.ParamTypes[i]) ? "OutputNullableArray" : "OutputNullableValue";
                    string isnull = ClassTypes.Contains(returnType) ? $"Object.ReferenceEquals(output_{i}, null)" : $"not output_{i}.HasValue";

                    sb.AppendLine($"let resultDatum_{output_num} = {outputTypeHandler}Obj.{outputHandlerMethod}(output_{i})");
                    sb.AppendLine($"OutputResult.SetDatumResult(resultDatum_{output_num}, {isnull}, output, {output_num}, uint32 {(int)this.ParamTypes[i]})");

                    output_num++;
                }
            }

            return "// Create PostgreSQL datums for INOUT/OUT parameters\n" + IndentCode(sb.ToString(), 8);
        }

        /// <inheritdoc />
        public override string BuildUserFunction()
        {
            var sb = new System.Text.StringBuilder();

            if (this.IsTrigger)
            {
                sb.Append($"static member {this.FuncName} (tg: TriggerData) : ReturnMode = \n");
                sb.Append($"#line 1{(this.FuncBody.StartsWith("\n") ? string.Empty : "\n")}{this.FuncBody}\n");
                return sb.ToString();
            }

            string return_type = this.SimpleReturnType;
            if (this.Retset)
            {
                return_type = this.ComplexReturnType;
            }

            List<string> outputTypes = new ();

            sb.Append($"static member {this.FuncName}");

            int totalUsedParameters = 0;
            for (int i = 0, length = this.ParamNames.Length; i < length; i++)
            {
                if (this.OutputModes.Contains(this.ParamModes[i]))
                {
                    string outputParamType = EnsureFSharpType(this.DotnetTypes[i]);
                    outputParamType = ClassTypes.Contains(this.DotnetTypes[i]) ? this.DotnetTypes[i] : $"Nullable<{this.DotnetTypes[i]}>";
                    outputTypes.Add(outputParamType);
                }

                if (this.InputModes.Contains(this.ParamModes[i]))
                {
                    string inputParamType = this.DotnetTypes[i];
                    if (this.SupportNullInput && (!ClassTypes.Contains(inputParamType)))
                    {
                       inputParamType = $"Nullable<{inputParamType}>";
                    }

                    sb.Append($" ({this.ParamNames[i]}: {inputParamType})");
                    totalUsedParameters += 1;
                }
            }

            if (totalUsedParameters == 0)
            {
                sb.Append("()"); // void equivalent
            }

            if (outputTypes.Count > 1)
            {
                return_type = string.Join(" * ", outputTypes);
            }
            else if (outputTypes.Count == 1)
            {
                return_type = outputTypes[0];
            }

            if (return_type == "void")
            {
                sb.Append($" = \n#line 1{(this.FuncBody.StartsWith("\n") ? string.Empty : "\n")}{IndentCode(this.FuncBody, 8)}");
            }
            else
            {
                sb.Append($" : {return_type} = \n#line 1{(this.FuncBody.StartsWith("\n") ? string.Empty : "\n")}{IndentCode(this.FuncBody, 8)}");
            }

            return sb.ToString();
        }

        /// <inheritdoc />
        public override string[] GetDotNetTypes()
        {
            string[] dotnetTypes = new string[this.ParamTypes.Length];
            for (int i = 0, length = this.ParamTypes.Length; i < length; i++)
            {
                string type = DatumConversion.ArrayTypes.ContainsKey((OID)this.ParamTypes[i]) ? "Array" : DatumConversion.SupportedTypesStr[(OID)this.ParamTypes[i]];
                dotnetTypes[i] = EnsureFSharpType(type);
            }

            return dotnetTypes;
        }

        /// <inheritdoc />
        public override string FormatGeneratedCode(string sourceCode)
        {
            return sourceCode;
        }

        /// <inheritdoc />
        public override string GetComplexReturnType(bool retset)
        {
            // ComplexReturnType is:
            //     - `IEnumerable<int, string>` if it's retset
            //     - `int`/`void`/etc otherwise
            // otherwise the plain type.
            string returnType;

            if (!this.ParamModes.Any(mode => this.TOutputModes.Contains(mode)))
            {
                returnType = this.SimpleReturnType;
            }
            else
            {
                // Filter by paramModes to get INOUT, OUT, and TABLE arguments
                var filteredParams = this.ParamNames.Zip(this.ParamTypes, (name, typeId) => new { Name = name, TypeId = typeId })
                    .Zip(this.ParamModes, (pair, mode) => new { pair.Name, pair.TypeId, Mode = mode })
                    .Where(param => this.TOutputModes.Contains(param.Mode));

                string filteredParamsStr = string.Join(", ", filteredParams.Select(p => $"Name:{p.Name}, TypeId:{p.TypeId}, Mode:{p.Mode}"));

                // does not handle `void` return types, as they're meaningless
                var args = filteredParams.Select(param =>
                        $"{(DatumConversion.ArrayTypes.ContainsKey((OID)param.TypeId) ? "Array" : SetTypeAsNullable(EnsureFSharpType(DatumConversion.SupportedTypesStr[(OID)param.TypeId])))}")
                    .ToList();
                string argsStr = string.Join(", ", args);

                // join the args and put parentheses around compound types
                returnType = string.Join(" * ", args);
                returnType = (args.Count > 1) ? $"({returnType})" : returnType;
            }

            if (this.ParamModes.Contains((byte)ProArgMode.Table))
            {
                returnType = $"seq<struct {returnType}>";
            }
            else
            {
                // make it an seq if it's a retset
                returnType = retset ? $"seq<{returnType}>" : $"{returnType}";
            }

            return returnType;
        }

        /// <inheritdoc />
        public override string SetTypeAsNullable(string type)
        {
            // Verify if there is a difference in C# and F# to the same type and convert if necessary
            type = EnsureFSharpType(type);

            // Classes already are Nullable
            if (ClassTypes.Contains(type))
            {
                return type;
            }

            return (type == "void") ? type : $"Nullable<{type}>";
        }

        /// <inheritdoc />
        public override string SetTypeAsSequence(string type, bool retset)
        {
            string nullableType = SetTypeAsNullable(type);

            // Assumes that the type is nullable
            return retset ? $"seq<{nullableType}>" : nullableType;
        }
    }
}
