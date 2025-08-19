// <copyright file="RecordHandler.cs" company="Brick Abode">
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
using System.Net.NetworkInformation; // for Macaddr
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Npgsql;
using NpgsqlTypes;
using PlDotNET.Common;

namespace PlDotNET.Handler
{
    /// <summary>
    /// A type handler for the PostgreSQL record data type.
    /// </summary>
    /// <remarks>
    /// Normally when we need to convert dotnet values to PostgreSQL
    /// datum, we know the types at compile time and are able to
    /// generate precise code for the conversion.  However, Records
    /// can be dynamic, so we don't always know the types at compile
    /// time, so here we dynamically examine the objects and convert
    /// them to Datum, finally converting the entire set into a single
    /// Record datum.
    /// </remarks>
    [OIDHandler(OID.RECORDOID, OID.RECORDARRAYOID)]
    public partial class RecordHandler : ObjectTypeHandler<object[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordHandler"/> class.
        /// </summary>
        public RecordHandler()
        {
            this.ElementOID = OID.RECORDOID;
            this.ArrayOID = OID.RECORDARRAYOID;
        }

        [LibraryImport("@PKG_LIBDIR/pldotnet.so")]
        public static unsafe partial void pldotnet_ResizeResult(IntPtr output, int length);

        [LibraryImport("@PKG_LIBDIR/pldotnet.so")]
        public static partial int pldotnet_GetResultLength(IntPtr result);

        [LibraryImport("@PKG_LIBDIR/pldotnet.so")]
        public static partial int pldotnet_GetResult(
                              IntPtr result,
                              int offset,
                              out IntPtr value,
                              [MarshalAs(UnmanagedType.U1)] out bool is_null,
                              out OID oid);

        [LibraryImport("@PKG_LIBDIR/pldotnet.so")]
        public static partial int pldotnet_GetRecordAttributes(
                                IntPtr recordDatum,
                                int numAttrs,
                                IntPtr[] datums,
                                byte[] isNull,
                                OID[] oid);

        [LibraryImport("@PKG_LIBDIR/pldotnet.so")]
        public static partial int pldotnet_GetNumberOfRecordAttributes(IntPtr result);

#nullable enable

        /// <summary>
        /// Converts a single value to its corresponding PostgreSQL data type and returns the datum and OID.
        /// It is for not nullable values. You must filter for nulls before calling.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <returns>A tuple containing the datum and OID.</returns>
        public static (IntPtr Datum, OID Oid) SingleValueOutput(object value)
        {
            (NpgsqlDbType dbt, object? obj) = NpgsqlHelper.GetNpgsqlTypeAndValue(value);

            if (DBNull.Value.Equals(obj))
            {
                obj = null;
            }

            OID oid = (OID)NpgsqlHelper.FindOid(dbt);
            IntPtr datum = DatumConversion.Instance.OutputNullableValue(oid, obj);

            return (datum, oid);
        }

        /// <summary>
        /// Sets the field value in the pldotnet_Result pointer at the specified offset.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <param name="output">The pldotnet_Result pointer.</param>
        /// <param name="offset">The offset at which to set the field value.</param>
        public bool OutputSetField(object value, IntPtr output, int offset)
        {
            var (datum, oid) = SingleValueOutput(value);

            OutputResult.SetDatumResult(datum, false, output, offset, (uint)oid);
            return true;
        }

        /// <summary>
        /// Convert a C `pldotnet_Result*` into a C# `object[]`.
        /// </summary>
        public object[] InputGetValue(IntPtr result)
        {
            // Return an empty array if the pointer is Null
            if (result == IntPtr.Zero)
            {
                return [];
            }

            int len = pldotnet_GetResultLength(result);
            object[] objects = new object[len];

            for (int i = 0; i < len; i++)
            {
                if (pldotnet_GetResult(result, i, out nint datum, out bool is_null, out OID oid) != 0)
                {
                    throw new SystemException($"Could not get value {i} from pldotnet_Result at {result.ToInt64():x}");
                }

                // We use the null-forgiving operator because `null` is correct here.
                objects[i] = is_null ? null! : DatumConversion.Instance.InputValue(datum, oid, true);
            }

            return objects;
        }

        /// <summary>
        /// Convert a C# `object[]` into a C `pldotnet_Result*`.
        /// </summary>
        public bool OutputSetValue(object[] values, IntPtr output)
        {
            if (values == null)
            {
                // FIXME, consider handling this better
                Elog.Info($"FIXME: returning 'true' on null input to OutputSetValue()");
                return true;
            }

            pldotnet_ResizeResult(output, values.Length);

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == null)
                {
                    // We don't know the OID of NULL, but pldotnet_main.c
                    // should handle this gracefully for NULL.
                    OutputResult.SetDatumResult((IntPtr)0, true, output, i, 0);
                }
                else
                {
                    bool isNull = false;

                    // If the value is an NpgsqlParameter, extract the Value property to check for null
                    if (values[i] is NpgsqlParameter npgsqlParameter)
                    {
                        object? value = npgsqlParameter.Value;
                        isNull = value == null || DBNull.Value.Equals(value);
                    }

                    var (datum, oid) = SingleValueOutput(values[i]);
                    OutputResult.SetDatumResult(datum, isNull, output, i, (uint)oid);
                }
            }

            return true;
        }

        /// <inheritdoc />
        public override object[] InputValue(IntPtr recordDatum)
        {
            // Return an empty array if the pointer is Null
            if (recordDatum == IntPtr.Zero)
            {
                return [];
            }

            int len = pldotnet_GetNumberOfRecordAttributes(recordDatum);

            IntPtr[] datums = new IntPtr[len];
            byte[] nullmap = new byte[len];
            OID[] oids = new OID[len];

            if (pldotnet_GetRecordAttributes(recordDatum, len, datums, nullmap, oids) != 0)
            {
                throw new SystemException($"Could not get records attributes from record datum at {recordDatum.ToInt64():x}");
            }

            object[] objects = new object[len];

            for (int i = 0; i < len; i++)
            {
                objects[i] = nullmap[i] != 0 ? null! : DatumConversion.Instance.InputValue(datums[i], oids[i], true);
            }

            return objects;
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(object[] values)
        {
            // for each
            throw new SystemException($"Do not call `OutputValue()` on a Record.");
        }
    }
}
