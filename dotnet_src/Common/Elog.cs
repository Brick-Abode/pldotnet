using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PlDotNET.Common
{
    /// <summary>
    /// A simple class used to report messages in PostgreSQL.
    /// </summary>
    public class Elog
    {
        /// <summary>
        /// Reports a log message in PostgreSQL.
        /// </summary>
        public static void Debug(string message)
        {
            pldotnet_Elog(14, ConvertUTF16ToUTF8(message));
        }

        /// <summary>
        /// Reports a log message in PostgreSQL.
        /// </summary>
        public static void Log(string message)
        {
            pldotnet_Elog(15, ConvertUTF16ToUTF8(message));
        }

        /// <summary>
        /// Reports an information message in PostgreSQL.
        /// </summary>
        public static void Info(string message)
        {
            pldotnet_Elog(17, ConvertUTF16ToUTF8(message));
        }

        /// <summary>
        /// Reports a notice message in PostgreSQL.
        /// </summary>
        public static void Notice(string message)
        {
            pldotnet_Elog(18, ConvertUTF16ToUTF8(message));
        }

        /// <summary>
        /// Reports a warning message in PostgreSQL.
        /// </summary>
        public static void Warning(string message)
        {
            pldotnet_Elog(19, ConvertUTF16ToUTF8(message));
        }

        /// <summary>
        /// Reports an error message in PostgreSQL.
        /// </summary>
        public static void Error(string message)
        {
            throw new Exception(message);
        }

        /// <summary>
        /// Converts a UTF-16 string to a UTF-8 string.
        /// </summary>
        /// <param name="utf16String">The UTF-16 string to convert.</param>
        /// <returns>The UTF-8 representation of the input string.</returns>
        public static string ConvertUTF16ToUTF8(string utf16String)
        {
            // Get the array of bytes that represents the UTF-16 string.
            byte[] utf16Bytes = Encoding.Unicode.GetBytes(utf16String);

            // Convert the UTF-16 bytes to UTF-8 bytes.
            byte[] utf8Bytes = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, utf16Bytes);

            // Return the UTF-8 bytes as a string.
            return Encoding.UTF8.GetString(utf8Bytes);
        }

        /// <summary>
        /// C function declared in pldotnet_main.h.
        /// See ::pldotnet_Elog().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        private static extern void pldotnet_Elog(int level, string nessage);
    }
}