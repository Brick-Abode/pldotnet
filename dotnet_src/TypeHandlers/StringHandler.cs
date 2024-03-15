// <copyright file="StringHandler.cs" company="Brick Abode">
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
using PlDotNET.Common;

namespace PlDotNET.Handler
{
    /// <summary>
    /// A type handler for PostgreSQL character data types (text, char, varchar, xml).
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/datatype-character.html.
    /// </remarks>
    public class StringHandler : ObjectTypeHandler<string>
    {
        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumTextAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern unsafe void pldotnet_GetDatumTextAttributes(IntPtr datum, ref int len, ref byte* buf);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumText().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumText(int len, byte[] buf);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumCharAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern unsafe void pldotnet_GetDatumCharAttributes(IntPtr datum, ref int len, ref byte* buf);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumChar().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumChar(int len, byte[] buf);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumVarCharAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern unsafe void pldotnet_GetDatumVarCharAttributes(IntPtr datum, ref int len, ref byte* buf);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumVarChar().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumVarChar(int len, byte[] buf);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumXmlAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern unsafe void pldotnet_GetDatumXmlAttributes(IntPtr datum, ref int len, ref byte* buf);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumXml().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumXml(int len, byte[] buf);

        /// <inheritdoc />
        public override unsafe string InputValue(IntPtr datum)
        {
            int strlen = 0;
            byte* str_p = null;
            switch ((int)this.ElementOID)
            {
                case (int)OID.TEXTOID:
                    pldotnet_GetDatumTextAttributes(datum, ref strlen, ref str_p);
                    break;
                case (int)OID.BPCHAROID:
                    pldotnet_GetDatumCharAttributes(datum, ref strlen, ref str_p);
                    break;
                case (int)OID.VARCHAROID:
                    pldotnet_GetDatumVarCharAttributes(datum, ref strlen, ref str_p);
                    break;
                case (int)OID.XMLOID:
                    pldotnet_GetDatumXmlAttributes(datum, ref strlen, ref str_p);
                    break;
                default:
                    throw new NotImplementedException($"StringConstructors doesn't support {(OID)this.ElementOID}");
            }

            return Encoding.UTF8.GetString(str_p, strlen);
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(string value)
        {
            byte[] encodedBytes = Encoding.UTF8.GetBytes(value);
            int len = encodedBytes.Length;
            return (int)this.ElementOID switch
            {
                (int)OID.TEXTOID => pldotnet_CreateDatumText(len, encodedBytes),
                (int)OID.BPCHAROID => pldotnet_CreateDatumChar(len, encodedBytes),
                (int)OID.VARCHAROID => pldotnet_CreateDatumVarChar(len, encodedBytes),
                (int)OID.XMLOID => pldotnet_CreateDatumXml(len, encodedBytes),
                _ => throw new NotImplementedException($"StringConstructors doesn't support {(OID)this.ElementOID}"),
            };
        }
    }

    /// <summary>
    /// A type handler for PostgreSQL character text data types.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/datatype-character.html.
    /// </remarks>
    [OIDHandler(OID.TEXTOID, OID.TEXTARRAYOID)]
    public class TextHandler : StringHandler
    {
        public TextHandler()
        {
            this.ElementOID = OID.TEXTOID;
            this.ArrayOID = OID.TEXTARRAYOID;
        }
    }

    /// <summary>
    /// A type handler for PostgreSQL character char data types.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/datatype-character.html.
    /// </remarks>
    [OIDHandler(OID.BPCHAROID, OID.BPCHARARRAYOID)]
    public class CharHandler : StringHandler
    {
        public CharHandler()
        {
            this.ElementOID = OID.BPCHAROID;
            this.ArrayOID = OID.BPCHARARRAYOID;
        }
    }

    /// <summary>
    /// A type handler for PostgreSQL character varchar data types.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/datatype-character.html.
    /// </remarks>
    [OIDHandler(OID.VARCHAROID, OID.VARCHARARRAYOID)]
    public class CharVaryingHandler : StringHandler
    {
        public CharVaryingHandler()
        {
            this.ElementOID = OID.VARCHAROID;
            this.ArrayOID = OID.VARCHARARRAYOID;
        }
    }

    /// <summary>
    /// A type handler for PostgreSQL character xml data types.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/datatype-xml.htm.
    /// </remarks>
    [OIDHandler(OID.XMLOID, OID.XMLARRAYOID)]
    public class XmlHandler : StringHandler
    {
        public XmlHandler()
        {
            this.ElementOID = OID.XMLOID;
            this.ArrayOID = OID.XMLARRAYOID;
        }
    }
}