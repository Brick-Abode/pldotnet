// <copyright file="IntegerHandler.cs" company="Brick Abode">
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
using System.Runtime.InteropServices;
using PlDotNET.Common;

namespace PlDotNET.Handler
{
    /// <summary>
    /// A type handler for the PostgreSQL smallint data type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/static/datatype-numeric.html.
    /// </remarks>
    [OIDHandler(OID.INT2OID, OID.INT2ARRAYOID)]
    public class ShortHandler : StructTypeHandler<short>
    {
        public ShortHandler()
        {
            this.ElementOID = OID.INT2OID;
            this.ArrayOID = OID.INT2ARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetInt16().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern short pldotnet_GetInt16(IntPtr datum);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumInt16().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumInt16(short value);

        /// <inheritdoc />
        public override short InputValue(IntPtr datum)
        {
            return pldotnet_GetInt16(datum);
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(short value)
        {
            return pldotnet_CreateDatumInt16(value);
        }
    }

    /// <summary>
    /// A type handler for the PostgreSQL integer data type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/static/datatype-numeric.html.
    /// </remarks>
    [OIDHandler(OID.INT4OID, OID.INT4ARRAYOID)]
    public class IntHandler : StructTypeHandler<int>
    {
        public IntHandler()
        {
            this.ElementOID = OID.INT4OID;
            this.ArrayOID = OID.INT4ARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetInt32().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern int pldotnet_GetInt32(IntPtr datum);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumInt32().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumInt32(int value);

        /// <inheritdoc />
        public override int InputValue(IntPtr datum)
        {
            return pldotnet_GetInt32(datum);
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(int value)
        {
            return pldotnet_CreateDatumInt32(value);
        }
    }

    /// <summary>
    /// A type handler for the PostgreSQL bigint data type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/static/datatype-numeric.html.
    /// </remarks>
    [OIDHandler(OID.INT8OID, OID.INT8ARRAYOID)]
    public class LongHandler : StructTypeHandler<long>
    {
        public LongHandler()
        {
            this.ElementOID = OID.INT8OID;
            this.ArrayOID = OID.INT8ARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetInt64().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern long pldotnet_GetInt64(IntPtr datum);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumInt64().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumInt64(long value);

        /// <inheritdoc />
        public override long InputValue(IntPtr datum)
        {
            return pldotnet_GetInt64(datum);
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(long value)
        {
            return pldotnet_CreateDatumInt64(value);
        }
    }
}
