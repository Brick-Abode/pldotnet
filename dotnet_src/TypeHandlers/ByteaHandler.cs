// <copyright file="ByteaHandler.cs" company="Brick Abode">
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
    /// A type handler for the PostgreSQL bytea data type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/static/datatype-binary.html.
    /// </remarks>
    [OIDHandler(OID.BYTEAOID, OID.BYTEAARRAYOID)]
    public class ByteaHandler : ObjectTypeHandler<byte[]>
    {
        public ByteaHandler()
        {
            this.ElementOID = OID.BYTEAOID;
            this.ArrayOID = OID.BYTEAARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumByteaAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern unsafe void pldotnet_GetDatumByteaAttributes(IntPtr datum, ref int len, ref byte* buf);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumBytea().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumBytea(int len, byte[] buf);

        /// <inheritdoc />
        public override unsafe byte[] InputValue(IntPtr datum)
        {
            int len = 0;
            byte* buf = null;
            pldotnet_GetDatumByteaAttributes(datum, ref len, ref buf);
            ReadOnlySpan<byte> nativeSpan = new (buf, len);
            return nativeSpan.ToArray();
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(byte[] value)
        {
            return pldotnet_CreateDatumBytea(value.Length, value);
        }
    }
}