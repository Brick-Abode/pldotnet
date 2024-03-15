// <copyright file="FloatHandler.cs" company="Brick Abode">
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
    /// A type handler for the PostgreSQL float data type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/static/datatype-numeric.html.
    /// </remarks>
    [OIDHandler(OID.FLOAT4OID, OID.FLOAT4ARRAYOID)]
    public class FloatHandler : StructTypeHandler<float>
    {
        public FloatHandler()
        {
            this.ElementOID = OID.FLOAT4OID;
            this.ArrayOID = OID.FLOAT4ARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetFloat().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern float pldotnet_GetFloat(IntPtr datum);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumFloat().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumFloat(float value);

        /// <inheritdoc />
        public override float InputValue(IntPtr datum)
        {
            return pldotnet_GetFloat(datum);
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(float value)
        {
            return pldotnet_CreateDatumFloat(value);
        }
    }

    /// <summary>
    /// A type handler for the PostgreSQL double precision data type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/static/datatype-numeric.html.
    /// </remarks>
    [OIDHandler(OID.FLOAT8OID, OID.FLOAT8ARRAYOID)]
    public class DoubleHandler : StructTypeHandler<double>
    {
        public DoubleHandler()
        {
            this.ElementOID = OID.FLOAT8OID;
            this.ArrayOID = OID.FLOAT8ARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDouble().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern double pldotnet_GetDouble(IntPtr datum);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumDouble().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumDouble(double value);

        /// <inheritdoc />
        public override double InputValue(IntPtr datum)
        {
            return pldotnet_GetDouble(datum);
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(double value)
        {
            return pldotnet_CreateDatumDouble(value);
        }
    }
}