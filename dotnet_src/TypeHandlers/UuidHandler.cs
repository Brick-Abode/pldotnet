// <copyright file="UuidHandler.cs" company="Brick Abode">
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
    /// A type handler for the PostgreSQL UUID data type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/static/datatype-uuid.html.
    /// </remarks>
    [OIDHandler(OID.UUIDOID, OID.UUIDARRAYOID)]
    public class UuidHandler : StructTypeHandler<Guid>
    {
        public UuidHandler()
        {
            this.ElementOID = OID.UUIDOID;
            this.ArrayOID = OID.UUIDARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumUuidAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern void pldotnet_GetDatumUuidAttributes(IntPtr datum, byte[] data);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumUuid().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumUuid(byte[] data);

        /// <inheritdoc />
        public override Guid InputValue(IntPtr datum)
        {
            byte[] data = new byte[16];
            pldotnet_GetDatumUuidAttributes(datum, data);

            byte[] data1 = data[0..4];
            byte[] data2 = data[4..6];
            byte[] data3 = data[6..8];
            Array.Reverse(data1);
            Array.Reverse(data2);
            Array.Reverse(data3);

            return new Guid(
                BitConverter.ToInt32(data1, 0),
                BitConverter.ToInt16(data2, 0),
                BitConverter.ToInt16(data3, 0),
                data[8..]);
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(Guid value)
        {
            byte[] data = value.ToByteArray();
            byte[] data1 = data[0..4];
            byte[] data2 = data[4..6];
            byte[] data3 = data[6..8];
            Array.Reverse(data1);
            Array.Reverse(data2);
            Array.Reverse(data3);

            byte[] psql_data = new byte[16];
            data1.CopyTo(psql_data, 0);
            data2.CopyTo(psql_data, 4);
            data3.CopyTo(psql_data, 6);
            data[8..].CopyTo(psql_data, 8);

            return pldotnet_CreateDatumUuid(psql_data);
        }
    }
}