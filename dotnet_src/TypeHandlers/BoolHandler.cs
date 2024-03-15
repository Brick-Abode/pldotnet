// <copyright file="BoolHandler.cs" company="Brick Abode">
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
    /// A type handler for the PostgreSQL bool data type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/static/datatype-boolean.html.
    /// </remarks>
    [OIDHandler(OID.BOOLOID, OID.BOOLARRAYOID)]
    public class BoolHandler : StructTypeHandler<bool>
    {
        public BoolHandler()
        {
            this.ElementOID = OID.BOOLOID;
            this.ArrayOID = OID.BOOLARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetBoolean().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool pldotnet_GetBoolean(IntPtr datum);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumBoolean().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumBoolean(bool value);

        /// <inheritdoc />
        public override bool InputValue(IntPtr datum)
        {
            return pldotnet_GetBoolean(datum);
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(bool value)
        {
            return pldotnet_CreateDatumBoolean(value);
        }
    }
}