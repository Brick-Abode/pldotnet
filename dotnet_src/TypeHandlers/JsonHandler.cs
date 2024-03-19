// <copyright file="JsonHandler.cs" company="Brick Abode">
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
using System.Buffers;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Unicode;
using PlDotNET.Common;

namespace PlDotNET.Handler
{
    /// <summary>
    /// A type handler for the PostgreSQL json data type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/datatype-json.html.
    /// </remarks>
    [OIDHandler(OID.JSONOID, OID.JSONARRAYOID)]
    public class JsonHandler : ObjectTypeHandler<string>
    {
        public static UTF8Encoding Utf8E = new ();

        public JsonHandler()
        {
            this.ElementOID = OID.JSONOID;
            this.ArrayOID = OID.JSONARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumJsonAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern unsafe void pldotnet_GetDatumJsonAttributes(IntPtr datum, ref int len, ref byte* buf);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumJson().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumJson(int len, byte[] buf);

        /// <inheritdoc />
        public override unsafe string InputValue(IntPtr datum)
        {
            int len = 0;
            byte* buf = null;
            pldotnet_GetDatumJsonAttributes(datum, ref len, ref buf);
            ReadOnlySpan<byte> nativeSpan = new (buf, len);
            string s1 = Utf8E.GetString(nativeSpan.ToArray(), 0, len);
            return s1;
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(string value)
        {
            byte[] encodedBytes = Utf8E.GetBytes(value);
            int len = encodedBytes.Length;
            return pldotnet_CreateDatumJson(len, encodedBytes);
        }
    }
}