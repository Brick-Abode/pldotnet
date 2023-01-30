// <copyright file="GenericHandler.cs" company="Brick Abode">
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
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PlDotNET.Handler
{
    /// <summary>
    /// A class to handle with null values in arrays.
    /// </summary>
    public static class NullMap
    {
        /// <summary>
        /// This function checks the PostgreSQL nullmap to identify if an
        /// element of the array is null.
        /// </summary>
        /// <remarks>
        /// According to the PostgreSQL convention an empty nullmap means that
        /// no elements are null.
        /// </remarks>
        /// <param name="nullmap">The PostgreSQL nullmap.</param>
        /// <param name="offset">The element offset.</param>
        /// <returns>
        /// Whether the nullmap contains any null values.
        /// </returns>
        public static unsafe bool CheckNullValue(byte[] nullmap, int offset)
        {
            int byteLen = nullmap.Length;
            if (byteLen == 0)
            {
                return false;
            }

            int bitLen = byteLen * 8;
            if (offset > bitLen)
            {
                throw new ArgumentOutOfRangeException("Illegal offset");
            }

            int byteOffset = offset / 8;
            int bitOffset = offset % 8;
            byte relevantByte = nullmap[byteOffset];
            int isNull = (relevantByte >> bitOffset) & 0x1;

            return isNull != 1;
        }
    }

    /// <summary>
    /// Base class for all classes which represent the data type handlers.
    /// </summary>
    /// <remarks>
    /// Do not use it directly. Instead see StructTypeHandler.&lt;T&gt; and ObjectTypeHandler.&lt;T&gt;.
    /// </remarks>
    public abstract class BaseTypeHandler<T>
    {
        public OID ElementOID = OID.ANYOID;

        public OID ArrayOID = OID.ANYARRAYOID;

        /// <summary>
        /// Converts a PostgreSQL datum to .NET value.
        /// </summary>
        /// <param name="datum">The PostgreSQL datum.</param>
        /// <returns> Returns the .NET value. </returns>
        public abstract T InputValue(IntPtr datum);

        /// <summary>
        /// Converts a .NET value to PostgreSQL datum.
        /// </summary>
        /// <param name="value">The .NET value.</param>
        /// <returns> Returns the PostgreSQL datum. </returns>
        public abstract IntPtr OutputValue(T value);

        /// <summary>
        /// Checks if the PostreSQL array is null. If the datum is null, it returns
        /// null. Otherwise, it calls the InputArray method.
        /// </summary>
        /// <returns>
        /// An Array Object if the Datum is not null. Otherwise returns null.
        /// </returns>
#nullable enable
        public Array? InputNullableArray(IntPtr datum, bool isnull)
        {
            return isnull ? null : this.InputArray(datum);
        }
#nullable disable

        /// <summary>
        /// Checks if Array object is null. If so, it returns a Datum Int32, otherwise
        /// it calls the OutputArray method.
        /// </summary>
        /// <returns>
        /// An Array Datum if the value is not null. Otherwise returns an Integer Datum.
        /// </returns>
#nullable enable
        public IntPtr OutputNullableArray(Array? value)
        {
            return value == null ? IntHandler.pldotnet_CreateDatumInt32(0) : this.OutputArray((Array)value);
        }
#nullable disable

        /// <summary>
        /// Converts a PostgreSQL array (multidimensional or not) in an Array object (dotnet style).
        /// If the PostgreSQL array has null values, this function calls InputArrayWithNull
        /// to correctly handle with this array.
        /// </summary>
        /// <param name="datum">The PostgreSQL array.</param>
        /// <returns> An Array object (multidimensional or not).</returns>
        public unsafe Array InputArray(IntPtr datum)
        {
            int ndims = 0;
            int[] rawDims = new int[ArrayHandler.Maxdim];
            byte* nullmap = null;
            int typeId = 0;
            ArrayHandler.pldotnet_GetArrayAttributes(datum, ref typeId, ref ndims, rawDims, ref nullmap);

            int[] dims = rawDims[..ndims];
            int nelems = 1;
            for (int i = 0; i < ndims; i++)
            {
                nelems *= dims[i];
            }

            IntPtr[] datums = new IntPtr[nelems];
            int arrayRet = ArrayHandler.pldotnet_GetArrayDatum(datum, datums, nelems, typeId);

            if (arrayRet != 0)
            {
                throw new System.Exception($"Got error from pldotnet_GetArrayDatum(): {arrayRet}");
            }

            var datumList = new List<IntPtr>();
            datumList.AddRange(datums);

            if (nullmap != null)
            {
                int nullmapLen = (nelems / 8) + 1;
                ReadOnlySpan<byte> nativeSpan = new (nullmap, nullmapLen);
                byte[] nullmapArray = nativeSpan.ToArray();
                return this.InputArrayWithNull(datumList, dims, nullmapArray);
            }

            var ret = datumList.Select((datum, index) => this.InputValue(datum)).ToArray();
            Array ret2 = Array.CreateInstance(typeof(object), dims);
            ArrayHandler.ReshapeArray(ret, ref ret2);

            return ret2;
        }

        /// <summary>
        /// Creates a PostgreSQL array from an Array object and the OID of its element.
        /// If the dotnet array has null values, the OutputArrayWithNull will be
        /// called.
        /// </summary>
        /// <param name="value">The dotnet array.</param>
        /// <returns> Returns a PostgreSQL array.</returns>
        public unsafe IntPtr OutputArray(Array value)
        {
            int nelems = value.Length;
            Array flatArray = Array.CreateInstance(typeof(object), nelems);
            ArrayHandler.FlatArray(value, ref flatArray);

            // flatArray variable is an one-dimensional array with the .NET types now
            int dimNumber = value.Rank;
            int[] dimLengths = new int[dimNumber];
            for (int i = 0; i < dimNumber; i++)
            {
                dimLengths[i] = value.GetLength(i);
            }

            for (int i = 0; i < nelems; i++)
            {
                if (flatArray.GetValue(i) == null)
                {
                    return this.OutputArrayWithNull(flatArray, dimLengths);
                }
            }

            IntPtr[] datums = new IntPtr[nelems]; // datums will be passed to C!
            for (int i = 0; i < nelems; i++)
            {
                datums[i] = this.OutputValue((T)flatArray.GetValue(i));
            }

            return ArrayHandler.pldotnet_CreateDatumArray((int)this.ElementOID, dimNumber, dimLengths, datums);
        }

        /// <summary>
        /// Converts a PostgreSQL array with null values in an Array object (dotnet style).
        /// This function is called from InputArray.
        /// </summary>
        /// <param name="datums">The flat list of datums.</param>
        /// <param name="dims">The size of each dimension.</param>
        /// <param name="nullmap">The PostgreSQL nullmap.</param>
        /// <returns> Returns an Array object (multidimensional or not).</returns>
        private Array InputArrayWithNull(List<IntPtr> datums, int[] dims, byte[] nullmap)
        {
            int nelms = datums.Count;
            object[] ret = new object[nelms];

            for (int i = 0; i < nelms; i++)
            {
                bool isNull = NullMap.CheckNullValue(nullmap, i);
                ret[i] = isNull ? null : this.InputValue(datums[i]);
            }

            Array ret2 = Array.CreateInstance(typeof(object), dims);
            ArrayHandler.ReshapeArray(ret, ref ret2);
            return ret2;
        }

        /// <summary>
        /// Creates a PostgreSQL array from an Array object with null values.
        /// This function is called from OutputArray.
        /// </summary>
        /// <param name="flatArray">The flat array with the dotnet values.</param>
        /// <param name="dims">The size of each dimension of the original dotnet array.</param>
        /// <returns> Returns a PostgreSQL array.</returns>
        private unsafe IntPtr OutputArrayWithNull(Array flatArray, int[] dims)
        {
            int nelems = flatArray.Length;
            byte[] nulls = new byte[nelems];
            IntPtr[] datums = new IntPtr[nelems];

            for (int i = 0; i < nelems; i++)
            {
                if (flatArray.GetValue(i) == null)
                {
                    nulls[i] = 1;
                    datums[i] = IntHandler.pldotnet_CreateDatumInt32(0);
                }
                else
                {
                    datums[i] = this.OutputValue((T)flatArray.GetValue(i));
                }
            }

            return ArrayHandler.pldotnet_CreateDatumArray((int)this.ElementOID, dims.Length, dims, datums, nulls);
        }
    }

    /// <summary>
    /// Base class for all classes which represent the data type handlers that use struct.
    /// </summary>
    /// <remarks>
    /// Use it for int, float, NpgsqlPoint, and other structs.
    /// </remarks>
    public abstract class StructTypeHandler<T> : BaseTypeHandler<T>
        where T : struct
    {
#nullable enable
        /// <summary>
        /// Check if the PostgreSQL datum is null. If the datum is null, it returns null.
        /// Otherwise, call the abstract method InputValue.
        /// </summary>
        /// <returns>
        /// A .NET type if the Datum is not null. Otherwise returns null.
        /// </returns>
        public T? InputNullableValue(IntPtr datum, bool isnull)
        {
            return isnull ? null : this.InputValue(datum);
        }

        /// <summary>
        /// Check if the .NET value is null. If the value is null, it returns a Datum(0).
        /// Otherwise, call the abstract method OutputValue.
        /// </summary>
        /// <returns>
        /// The suitable Datum if the .NET value is not null. Oherwise returns an Integer Datum.
        /// </returns>
        public IntPtr OutputNullableValue(T? value)
        {
            return value == null ? IntHandler.pldotnet_CreateDatumInt32(0) : this.OutputValue((T)value);
        }
#nullable disable
    }

    /// <summary>
    /// Base class for all classes which represent the data type handlers that use class.
    /// </summary>
    /// <remarks>
    /// Use it for string, PhysicalAddress, BitArray, and other classes.
    /// </remarks>
    public abstract class ObjectTypeHandler<T> : BaseTypeHandler<T>
        where T : class
    {
#nullable enable
        /// <summary>
        /// Check if the PostgreSQL datum is null. If the Datum is null, it returns null.
        /// Otherwise, call the abstract method InputValue.
        /// </summary>
        /// <returns>
        /// A .NET type if the Datum is not null. Otherwise returns null.
        /// </returns>
        public T? InputNullableValue(IntPtr datum, bool isnull)
        {
            return isnull ? null : this.InputValue(datum);
        }

        /// <summary>
        /// Check if the .NET value is null. If the value is null, it returns a Datum(0).
        /// Otherwise, call the abstract method OutputValue.
        /// </summary>
        /// <returns>
        /// The suitable Datum if the .NET value is not null. Oherwise returns an Integer Datum.
        /// </returns>
        public IntPtr OutputNullableValue(T? value)
        {
            return value == null ? IntHandler.pldotnet_CreateDatumInt32(0) : this.OutputValue((T)value);
        }
#nullable disable
    }

    /// <summary>
    ///
    /// </summary>
    public class OIDHandler : System.Attribute
    {
        public OID BaseType;
        public OID ArrayType;

        public OIDHandler(OID baseType, OID arrayType)
        {
            this.BaseType = baseType;
            this.ArrayType = arrayType;
        }
    }

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

    /// <summary>
    /// Provides a set of methods for setting the result datum of a user function to an output object.
    /// </summary>
    public class OutputResult
    {
        /// <summary>
        /// Sets the result datum of a user function to an output object.
        /// </summary>
        /// <param name="resultDatum">A pointer to the result datum to be set.</param>
        /// <param name="isNull">A value indicating whether the result datum is null. Set to `true` if
        /// the result datum is null, and `false` otherwise.</param>
        /// <param name="output">A pointer to the output object where the result datum will be set.</param>
        public static unsafe void SetDatumResult(IntPtr resultDatum, bool isNull, IntPtr output)
        {
            Result* pOutput = (Result*)output.ToPointer();
            pOutput->Value = resultDatum;
            pOutput->IsNull = isNull;
        }

        /// <summary>
        /// Represents a result object containing a value and a nullability flag.
        /// </summary>
        private struct Result
        {
            /// <summary>
            /// A pointer to the value of the result datum.
            /// </summary>
            public IntPtr Value;

            /// <summary>
            /// A value indicating whether the result is null. Set to `true` if the result is null,
            /// and `false` otherwise.
            /// </summary>
            [MarshalAs(UnmanagedType.I1)]
            public bool IsNull;
        }
    }
}
