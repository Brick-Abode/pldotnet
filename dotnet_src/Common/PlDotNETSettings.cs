using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PlDotNET.Common
{
    /// <summary>
    /// Loads and provides access to runtime settings for PL/.NET from PostgreSQL's configuration,
    /// using lazy initialization and interop with the PostgreSQL backend.
    /// </summary>
    public partial class PlDotNETSettings
    {
        /// <summary>
        /// Stores lazy-loaded PostgreSQL configuration settings by key.
        /// </summary>
        private readonly Dictionary<string, Lazy<string>> settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlDotNETSettings"/> class.
        /// Preloads known configuration keys with deferred access via Lazy evaluation.
        /// </summary>
        public PlDotNETSettings()
        {
            settings = [];

            var settingsKeys = new[]
            {
                "pldotnet.always_nullable",
                "pldotnet.print_source_code",
                "pldotnet.save_source_code",
                "pldotnet.compile_fsharp_with_fcs",
                "pldotnet.verbose_level",
                "pldotnet.path_to_save_source_code",
                "pldotnet.path_to_temporary_files",
            };

            foreach (var key in settingsKeys)
            {
                settings[key] = new Lazy<string>(() => GetPostgreSetting(key));
            }
        }

        /// <summary>
        /// Gets a value indicating whether all database values should be treated as nullable by default.
        /// </summary>
        public bool AlwaysNullable => ConvertToBoolean(settings["pldotnet.always_nullable"].Value);

        /// <summary>
        /// Gets a value indicating whether the generated source code should be printed to the PostgreSQL log.
        /// </summary>
        public bool PrintSourceCode => ConvertToBoolean(settings["pldotnet.print_source_code"].Value);

        /// <summary>
        /// Gets a value indicating whether the generated source code should be saved to disk.
        /// </summary>
        public bool SaveSourceCode => ConvertToBoolean(settings["pldotnet.save_source_code"].Value);

        /// <summary>
        /// Gets a value indicating whether F# compilation should use the FCS (F# Compiler Services) backend.
        /// </summary>
        public bool CompileFSharpWithFCS => ConvertToBoolean(settings["pldotnet.compile_fsharp_with_fcs"].Value);

        /// <summary>
        /// Gets the configured verbosity level for diagnostics or logging output.
        /// </summary>
        public int VerboseLevel => int.Parse(settings["pldotnet.verbose_level"].Value);

        /// <summary>
        /// Gets the file system path where generated source code should be saved.
        /// </summary>
        public string PathToSaveSourceCode => settings["pldotnet.path_to_save_source_code"].Value;

        /// <summary>
        /// Gets the directory path used to store temporary files created during compilation or execution.
        /// </summary>
        public string PathToTemporaryFiles => settings["pldotnet.path_to_temporary_files"].Value;

        /// <summary>
        /// Calls the PostgreSQL backend to retrieve the value of a server configuration setting by name.
        /// </summary>
        /// <param name="settingName">The name of the PostgreSQL configuration setting to retrieve.</param>
        /// <returns>
        /// A pointer to an ANSI string representing the setting's value, or <see cref="IntPtr.Zero"/> if not found.
        /// </returns>
        [LibraryImport("@PKG_LIBDIR/pldotnet.so", StringMarshalling = StringMarshalling.Utf8)]
        private static partial IntPtr pldotnet_GetPostgresSetting(string settingName);

        /// <summary>
        /// Converts a PostgreSQL "on"/"off" setting value to a boolean.
        /// </summary>
        /// <param name="value">The setting value, expected to be "on" or "off" (case-insensitive).</param>
        /// <returns><c>true</c> if the value is "on"; otherwise, <c>false</c>.</returns>
        private static bool ConvertToBoolean(string value)
        {
            return value?.ToLower() == "on";
        }

        /// <summary>
        /// Retrieves a PostgreSQL configuration setting by name via interop.
        /// </summary>
        /// <param name="settingName">The name of the PostgreSQL configuration setting.</param>
        /// <returns>
        /// The string representation of the setting's value, or <c>null</c> if the value is not found.
        /// </returns>
        private static string GetPostgreSetting(string settingName)
        {
            IntPtr resultPtr = pldotnet_GetPostgresSetting(settingName);
            if (resultPtr == IntPtr.Zero)
            {
                return null;
            }

            return Marshal.PtrToStringAnsi(resultPtr);
        }
    }
}
