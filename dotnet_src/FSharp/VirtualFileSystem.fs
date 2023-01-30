// <copyright file="VirtualFileSystem.fs" company="Brick Abode">
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
open System.IO
open System.Text
open System.Collections.Generic
open FSharp.Compiler.CodeAnalysis
open FSharp.Compiler.IO

/// <summary>
/// A virtual file system that allows reading and writing to files in memory.
/// </summary>
/// <remarks>
/// This class implements the `IFileSystem` interface and provides a virtual file system
/// that allows reading and writing to files stored in memory. It also delegates to a
/// default file system for all other file system operations.
/// </remarks>
/// <param name="container">A list of tuples representing the initial files in the virtual file system.
/// Each tuple consists of the file name and its contents.</param>
/// <param name="defaultFileSystem">The default file system to delegate to for all other file system operations.</param>
type VirtualFileSystem(container: (string * string) [], defaultFileSystem: IFileSystem) =

    /// <summary>
    /// A dictionary of the virtual files in the file system, keyed by file name.
    /// </summary>
    /// <remarks>
    /// The dictionary stores the contents of the virtual files as strings.
    /// </remarks>
    let files = dict container

    /// <summary>
    /// A dictionary of the virtual files in the file system, keyed by file name.
    /// </summary>
    /// <remarks>
    /// The dictionary stores the contents of the virtual files as `MemoryStream` objects.
    /// </remarks>
    let streams =
        let d = new Dictionary<string, MemoryStream>()
        for (key, value) in container do
            d.Add(key, new MemoryStream(Encoding.UTF8.GetBytes(value)))
        d

    /// <summary>
    /// An interface for file system operations.
    /// </summary>
    /// <remarks>
    /// This interface provides methods for performing various file system operations,
    /// such as reading and writing to files, checking file existence, and deleting files.
    /// </remarks>
    interface IFileSystem with
        /// <summary>
        /// Opens a file for reading.
        /// </summary>
        /// <param name="fileName">The name of the file to open.</param>
        /// <param name="useMemoryMappedFile">An optional flag indicating whether to use a memory-mapped file.</param>
        /// <param name="shouldShadowCopy">An optional flag indicating whether to shadow copy the file.</param>
        /// <returns>A stream representing the opened file.</returns>
        member _.OpenFileForReadShim(fileName, ?useMemoryMappedFile: bool, ?shouldShadowCopy: bool) =
            match files.TryGetValue fileName with
            | true, text -> streams.[fileName] :> Stream
            | _ ->
                defaultFileSystem.OpenFileForReadShim(
                    fileName,
                    ?useMemoryMappedFile = useMemoryMappedFile,
                    ?shouldShadowCopy = shouldShadowCopy
                )

        /// <summary>
        /// Opens a file for writing.
        /// </summary>
        /// <param name="fileName">The name of the file to open.</param>
        /// <param name="fileMode">An optional flag indicating the mode to open the file in.</param>
        /// <param name="fileAccess">An optional flag indicating the access to grant to the file.</param>
        /// <param name="fileShare">An optional flag indicating the type of sharing to allow for the file.</param>
        /// <returns>A stream representing the opened file.</returns>
        member _.OpenFileForWriteShim(fileName, ?fileMode: FileMode, ?fileAccess: FileAccess, ?fileShare: FileShare) =
            defaultFileSystem.OpenFileForWriteShim(
                fileName,
                ?fileMode = fileMode,
                ?fileAccess = fileAccess,
                ?fileShare = fileShare
            )

        /// <summary>
        /// Check if a file exists.
        /// </summary>
        /// <param name="fileName">The name of the file to check.</param>
        /// <returns>`true` if the file exists, `false` otherwise.</returns>
        member _.FileExistsShim(fileName) =
            files.ContainsKey(fileName)
            || defaultFileSystem.FileExistsShim(fileName)

        /// <summary>
        /// Get the path to the system's temporary directory.
        /// </summary>
        /// <returns>The path to the temporary directory.</returns>
        member _.GetTempPathShim() = defaultFileSystem.GetTempPathShim()

        /// <summary>
        /// Get the last write time of a file.
        /// </summary>
        /// <param name="fileName">The name of the file to check.</param>
        /// <returns>The last write time of the file.</returns>
        member _.GetLastWriteTimeShim(fileName) =
            defaultFileSystem.GetLastWriteTimeShim(fileName)

        /// <summary>
        /// Get the full path to a file.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>The full path to the file.</returns>
        member _.GetFullPathShim(fileName) =
            defaultFileSystem.GetFullPathShim(fileName)

        /// <summary>
        /// Check if a path is invalid.
        /// </summary>
        /// <param name="fileName">The path to check.</param>
        /// <returns>`true` if the path is invalid, `false` otherwise.</returns>
        member _.IsInvalidPathShim(fileName) =
            defaultFileSystem.IsInvalidPathShim(fileName)

        /// <summary>
        /// Check if a path is rooted.
        /// </summary>
        /// <param name="fileName">The path to check.</param>
        /// <returns>`true` if the path is rooted, `false` otherwise.</returns>
        member _.IsPathRootedShim(fileName) =
            defaultFileSystem.IsPathRootedShim(fileName)

        /// <summary>
        /// Delete a file.
        /// </summary>
        /// <param name="fileName">The name of the file to delete.</param>
        member _.FileDeleteShim(fileName) =
            defaultFileSystem.FileDeleteShim(fileName)

        /// <summary>
        /// Get the assembly loader to use.
        /// </summary>
        member _.AssemblyLoader = defaultFileSystem.AssemblyLoader

        /// <summary>
        /// Get the full path to a file in a directory.
        /// </summary>
        /// <param name="dir">The directory containing the file.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>The full path to the file in the directory.</returns>
        member _.GetFullFilePathInDirectoryShim dir fileName =
            defaultFileSystem.GetFullFilePathInDirectoryShim dir fileName

        /// <summary>
        /// Normalize a path.
        /// </summary>
        /// <param name="path">The path to normalize.</param>
        /// <returns>The normalized path.</returns>
        member _.NormalizePathShim(path) =
            defaultFileSystem.NormalizePathShim(path)

        /// <summary>
        /// Get the directory name from a path.
        /// </summary>
        /// <param name="path">The path to get the directory name from.</param>
        /// <returns>The directory name from the path.</returns>
        member _.GetDirectoryNameShim(path) =
            defaultFileSystem.GetDirectoryNameShim(path)

        /// <summary>
        /// Get the creation time of a file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>The creation time of the file.</returns>
        member _.GetCreationTimeShim(path) =
            defaultFileSystem.GetCreationTimeShim(path)

        /// <summary>
        /// Copy a file.
        /// </summary>
        /// <param name="src">The path to the source file.</param>
        /// <param name="dest">The path to the destination file.</param>
        /// <param name="overwrite">A flag indicating whether to overwrite the destination file if it exists.</param>
        member _.CopyShim(src, dest, overwrite) =
            defaultFileSystem.CopyShim(src, dest, overwrite)

        /// <summary>
        /// Create a directory.
        /// </summary>
        /// <param name="path">The path to the directory to create.</param>
        member _.DirectoryCreateShim(path) =
            defaultFileSystem.DirectoryCreateShim(path)

        /// <summary>
        /// Check if a directory exists.
        /// </summary>
        /// <param name="path">The path to the directory to check.</param>
        /// <returns>`true` if the directory exists, `false` otherwise.</returns>
        member _.DirectoryExistsShim(path) =
            defaultFileSystem.DirectoryExistsShim(path)

        /// <summary>
        /// Delete a directory.
        /// </summary>
        /// <param name="path">The path to the directory to delete.</param>
        member _.DirectoryDeleteShim(path) =
            defaultFileSystem.DirectoryDeleteShim(path)

        /// <summary>
        /// Enumerate the files in a directory.
        /// </summary>
        /// <param name="path">The path to the directory to enumerate.</param>
        /// <param name="pattern">The search pattern to use for the enumeration.</param>
        /// <returns>An enumerable containing the names of the files in the directory.</returns>
        member _.EnumerateFilesShim(path, pattern) =
            defaultFileSystem.EnumerateFilesShim(path, pattern)

        /// <summary>
        /// Enumerate the directories in a directory.
        /// </summary>
        /// <param name="path">The path to the directory to enumerate.</param>
        /// <returns>An enumerable containing the names of the directories in the directory.</returns>
        member _.EnumerateDirectoriesShim(path) =
            defaultFileSystem.EnumerateDirectoriesShim(path)

        /// <summary>
        /// Check if a file is stable using a heuristic.
        /// </summary>
        /// <param name="path">The path to the file to check.</param>
        /// <returns>`true` if the file is stable according to the heuristic, `false` otherwise.</returns>
        member _.IsStableFileHeuristic(path) =
            defaultFileSystem.IsStableFileHeuristic(path)
