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
// DotNetProjectBuilder.cs - pldotnet F# project builder using dotnet
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Npgsql;
using NpgsqlTypes;
using PlDotNET.Common;

namespace PlDotNET
{
    /// <summary>
    /// Represents a builder for creating a .NET project.
    /// </summary>
    internal class DotNetProjectBuilder
    {
        /// <summary>
        /// The verbose level for logging detailed information during the build process.
        /// </summary>
        internal int Verbose = 0;

        /// <summary>
        /// The path to the destination project.
        /// </summary>
        internal string DestinationProjectPath = string.Empty;

        /// <summary>
        /// The path to the project template used as a basis for the new project.
        /// </summary>
        internal string ProjectTemplatePath;

        /// <summary>
        /// The root destination path where the project directory will be created.
        /// </summary>
        internal string DestinationPath = @"./tmp";

        /// <summary>
        /// The name of the project to be created.
        /// </summary>
        internal string ProjectName;

        /// <summary>
        /// The programming language used for the project.
        /// </summary>
        internal DotNETLanguage Language;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetProjectBuilder"/> class.
        /// </summary>
        /// <param name="projectTemplatePath">The path to the project template.</param>
        /// <param name="destinationPath">The path where the project will be created.</param>
        /// <param name="projectName">The name of the project.</param>
        /// <param name="language">The programming language of the project.</param>
        public DotNetProjectBuilder(string projectTemplatePath, string destinationPath, string projectName, DotNETLanguage language)
        {
            this.ProjectTemplatePath = projectTemplatePath;
            this.DestinationPath = destinationPath;
            this.ProjectName = projectName;
            this.Language = language;
        }

        /// <summary>
        /// Sets the verbose level for the project builder.
        /// </summary>
        /// <param name="v">The verbose level to set.</param>
        public void SetVerboseLevel(int v)
        {
            this.Verbose = v;
        }

        /// <summary>
        /// Sets the content of the program file in the project.
        /// </summary>
        /// <param name="newContent">The new content to be written to the program file.</param>
        /// <returns>Returns 0 if the file is successfully written, otherwise returns 1.</returns>
        public int SetProgramContent(string newContent)
        {
            string formatFile = Language == DotNETLanguage.CSharp ? "cs" : "fs";
            if (this.Verbose > 0)
            {
                Elog.Info($"Calling SetProgramContent: saving file in {this.DestinationProjectPath}/Program.{formatFile}");
            }

            try
            {
                File.WriteAllText($"{this.DestinationProjectPath}/Program.{formatFile}", newContent);
                return 0;
            }
            catch (Exception e)
            {
                Elog.Info(e.ToString());
                return 1;
            }
        }

        /// <summary>
        /// Builds the project with the specified new content.
        /// </summary>
        /// <param name="newContent">The new content to replace the placeholder program file.</param>
        /// <returns>The result of the build operation.</returns>
        public int Build(string newContent)
        {
            if (this.Verbose > 0)
            {
                Elog.Info("Calling Build()");
            }

            // Create a new directory at the destination path
            DestinationProjectPath = Path.Combine(DestinationPath, ProjectName);
            if (!Directory.Exists(DestinationProjectPath))
            {
                if (this.Verbose > 0)
                {
                    Elog.Info("Creating new directory for template project");
                }

                Directory.CreateDirectory(DestinationProjectPath);
            }
            else if (this.Verbose > 0)
            {
                Elog.Warning($"{DestinationProjectPath} already exists!");
            }

            if (this.Verbose > 0)
            {
                Elog.Info("Searching for the project template file");
            }

            if (!File.Exists(this.ProjectTemplatePath))
            {
                throw new SystemException($"Template file '{this.ProjectTemplatePath}' not found");
            }

            // Read the project template file and replace the placeholders with the actual DLL paths
            string templateProject = File.ReadAllText(this.ProjectTemplatePath);
            templateProject = templateProject.Replace(
                "$PlDotNET.Common.dll$", typeof(Elog).Assembly.Location.Replace("/", "\\"));
            templateProject = templateProject.Replace(
                "$NpgsqlTypes.dll$", typeof(NpgsqlPoint).Assembly.Location.Replace("/", "\\"));
            templateProject = templateProject.Replace(
                "$Npgsql.dll$", typeof(NpgsqlCommand).Assembly.Location.Replace("/", "\\"));

            // Save the project file to the destination path
            string formatFile = Language == DotNETLanguage.CSharp ? "csproj" : "fsproj";
            File.WriteAllText(
                $"{DestinationProjectPath}/{ProjectName}.{formatFile}", templateProject);

            if (Verbose > 2)
            {
                // Assuming Test is your Folder
                DirectoryInfo d = new DirectoryInfo(DestinationProjectPath);

                // Getting Text files
                FileInfo[] files = d.GetFiles("*");
                var fl = new List<string>();
                foreach (FileInfo f in files)
                {
                    fl.Add(f.Name);
                }

                Elog.Info($"New project created from template with these files: {string.Join(", ", fl)}");
            }

            // Replace the placeholder program file with the UserFunction code
            SetProgramContent(newContent);

            // Build the project using dotnet build
            return BuildProject(DestinationProjectPath);
        }

        /// <summary>
        /// Builds the project and generates a DLL file.
        /// </summary>
        /// <param name="newContent">The new content to be built.</param>
        /// <returns>The path of the generated DLL file.</returns>
        public string BuildAndGenDLL(string newContent)
        {
            if (this.Verbose > 0)
            {
                Elog.Info("Calling BuildAndGenDLL()");
            }

            int error = this.Build(newContent);

            if (error != 0)
            {
                return string.Empty;
            }

            string releasePath = Path.GetFullPath(this.DestinationProjectPath + $"/bin/Release/net6.0/");
            return releasePath + $"{this.ProjectName}.dll";
        }

        /// <summary>
        /// Builds the specified project using the dotnet build command.
        /// </summary>
        /// <param name="projectPath">The path to the project file.</param>
        /// <returns>An integer indicating the success or failure of the build process.</returns>
        public int BuildProject(string projectPath)
        {
            int retVal = 0;

            try
            {
                // Run dotnet build command
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    // FileName = "dotnet",
                    FileName = "/usr/bin/dotnet",
                    Arguments = $"build {projectPath} --configuration Release",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                psi.EnvironmentVariables["DOTNET_CLI_HOME"] = DestinationPath;

                using (Process process = new Process { StartInfo = psi })
                {
                    process.Start();
                    process.WaitForExit();

                    // Read the output and error streams
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    if (Verbose > 1)
                    {
                        Elog.Info("Build Output:");
                        Elog.Info(output);
                    }
                    else if (output.ToLower().Contains("build failed") || !string.IsNullOrEmpty(error))
                    {
                        var errorLines = string.IsNullOrEmpty(error) ? output.Split('\n')
                            .Select(line => Regex.Match(line, @"\((\d+,\d+)\): error (\w+): ([^\[]+)"))
                            .Where(match => match.Success)
                            .Select(match => match.Value)
                            .Distinct()
                            .ToArray() : error.Split('\n');

                        string language = Language == DotNETLanguage.CSharp ? "C#" : "F#";
                        string code = File.ReadAllText(
                            $"{this.DestinationProjectPath}/Program.{(Language == DotNETLanguage.CSharp ? "cs" : "fs")}");

                        Elog.Warning(
                            $"PL.NET could not compile the following {language} generated code:" +
                            $"\n**********\n{code}\n**********\n" +
                            $"Here are the compilation results:\n{string.Join("\n", errorLines)}\n\n" +
                            $"For additional information, please consult the {language} project created at  '{this.DestinationProjectPath}'\n");

                        retVal = 1;
                    }

                    if (Verbose > 0)
                    {
                        Elog.Info("Build completed.");
                    }

                    this.DestinationProjectPath = projectPath;
                }
            }
            catch (Exception ex)
            {
                Elog.Info($"Error: {ex.Message}");
                retVal = 1;
            }

            return retVal;
        }
    }
}
