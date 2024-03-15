// <copyright file="ArrayHandler.cs" company="Brick Abode">
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
    /// A generic class for all type handlers which handle PostreSQL arrays.
    /// </summary>
    public static class ArrayHandler
    {
        public static int Maxdim = get_Maxdim();

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::get_Maxdim().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern int get_Maxdim();

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetArrayAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern unsafe void pldotnet_GetArrayAttributes(IntPtr datum, ref int typeId, ref int nDims, int[] dims, ref byte* nullmap);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetArrayDatum().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern unsafe int pldotnet_GetArrayDatum(IntPtr arrayDatum, IntPtr[] results, int nElems, int typeId);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumArray().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumArray(int elementId, int dimNumber, int[] dimLengths, IntPtr[] datums, byte[] nullmap = null);
    }
}
