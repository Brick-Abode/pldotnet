// <copyright file="RangeHandler.cs" company="Brick Abode">
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
using NpgsqlTypes;
using PlDotNET.Common;

namespace PlDotNET.Handler
{
    /// <summary>
    /// A generic type handler for the PostgreSQL range.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/rangetypes.html.
    /// </remarks>
    public abstract class RangeHandler<T, THandler> : StructTypeHandler<NpgsqlRange<T>>
            where THandler : BaseTypeHandler<T>, new()
    {
        public static THandler HandlerObj = new ();

        /// <inheritdoc />
        public override unsafe NpgsqlRange<T> InputValue(IntPtr datum)
        {
            byte isEmpty;
            IntPtr upperDange, lower_range;
            IntPtr upperDatum, lowerDatum;
            byte upperInfinite, lowerInfinite;
            byte upperInclusive, lowerInclusive;
            byte upperLower, lowerLower;
            T upper, lower;

            RangeConstructors.pldotnet_GetDatumRangeAttributes(datum, &isEmpty, &lower_range, &upperDange);

            RangeConstructors.pldotnet_GetDatumRangeBoundAttributes(
                upperDange, &upperDatum, &upperInfinite, &upperInclusive, &upperLower);
            RangeConstructors.pldotnet_GetDatumRangeBoundAttributes(
                lower_range, &lowerDatum, &lowerInfinite, &lowerInclusive, &lowerLower);

            lower = HandlerObj.InputValue(lowerDatum);
            upper = HandlerObj.InputValue(upperDatum);

            return new NpgsqlRange<T>(
                lower,
                lowerInclusive > 0,
                lowerInfinite > 0,
                upper,
                upperInclusive > 0,
                upperInfinite > 0);
        }

        /// <summary>
        /// Formats the output range by adjusting the lower and upper bounds based on PostgreSQL standards.
        /// </summary>
        /// <param name="range">The input range to be formatted.</param>
        /// <returns>A new NpgsqlRange object with adjusted bounds.</returns>
        public NpgsqlRange<T> FormatOutputRange(NpgsqlRange<T> range)
        {
            T newLowerBound = range.LowerBound;
            T newUpperBound = range.UpperBound;
            bool lowerBoundIsInclusive = range.LowerBoundIsInclusive;
            bool upperBoundIsInclusive = range.UpperBoundIsInclusive;
            bool lowerBoundInfinite = range.LowerBoundInfinite;
            bool upperBoundInfinite = range.UpperBoundInfinite;

            // Adjust the lower bound
            if (!lowerBoundIsInclusive && !lowerBoundInfinite)
            {
                if (typeof(T) == typeof(DateOnly))
                {
                    DateOnly lowerBound = (DateOnly)(object)range.LowerBound;
                    newLowerBound = (T)(object)lowerBound.AddDays(1);
                    lowerBoundIsInclusive = true;
                }
                else if (typeof(T) == typeof(int) || typeof(T) == typeof(long))
                {
                    dynamic lowerBound = range.LowerBound;
                    newLowerBound = (T)(lowerBound + 1);
                    lowerBoundIsInclusive = true;
                }
            }

            // Adjust the upper bound
            if (upperBoundIsInclusive && !upperBoundInfinite)
            {
                if (typeof(T) == typeof(DateOnly))
                {
                    DateOnly upperBound = (DateOnly)(object)range.UpperBound;
                    newUpperBound = (T)(object)upperBound.AddDays(1);
                    upperBoundIsInclusive = false;
                }
                else if (typeof(T) == typeof(int) || typeof(T) == typeof(long))
                {
                    dynamic upperBound = range.UpperBound;
                    newUpperBound = (T)(upperBound + 1);
                    upperBoundIsInclusive = false;
                }
            }

            // Return a new range with adjusted bounds
            return new NpgsqlRange<T>(
                newLowerBound,
                lowerBoundIsInclusive,
                lowerBoundInfinite,
                newUpperBound,
                upperBoundIsInclusive,
                upperBoundInfinite);
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(NpgsqlRange<T> value)
        {
            if (value.IsEmpty)
            {
                return RangeConstructors.pldotnet_CreateEmptyDatumRange(this.ElementOID);
            }

            value = FormatOutputRange(value);
            byte upperInfinite = (byte)(value.UpperBoundInfinite ? 1 : 0);
            byte lowerInfinite = (byte)(value.LowerBoundInfinite ? 1 : 0);
            byte upperInclusive = (byte)(value.UpperBoundIsInclusive ? 1 : 0);
            byte lowerInclusive = (byte)(value.LowerBoundIsInclusive ? 1 : 0);
            T upper = value.UpperBound;
            T lower = value.LowerBound;
            IntPtr upperDatum = HandlerObj.OutputValue(upper);
            IntPtr lowerDatum = HandlerObj.OutputValue(lower);

            return RangeConstructors.pldotnet_CreateDatumRange(
                this.ElementOID,
                lowerDatum,
                lowerInfinite,
                lowerInclusive,
                upperDatum,
                upperInfinite,
                upperInclusive);
        }
    }

    /// <summary>
    /// This class contains the C methods used to convert from PostgreSQL range for NpgsqlRange,
    /// as well as the opposite way.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/rangetypes.html.
    /// </remarks>
    public class RangeConstructors
    {
        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumRangeAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static unsafe extern void pldotnet_GetDatumRangeAttributes(
                IntPtr inputDatum,
                byte* isEmpty,
                IntPtr* lowerRange,
                IntPtr* upperDange);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumRangeBoundAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static unsafe extern void pldotnet_GetDatumRangeBoundAttributes(
                IntPtr inputRange,
                IntPtr* rangeDatum,
                byte* infinite,
                byte* inclusive,
                byte* lower);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumRange().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static unsafe extern IntPtr pldotnet_CreateDatumRange(
            OID rtOid,
            IntPtr lowerDatum,
            byte lowerInfinite,
            byte lowerInclusive,
            IntPtr upperDatum,
            byte upperInfinite,
            byte upperInclusive);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateEmptyDatumRange().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static unsafe extern IntPtr pldotnet_CreateEmptyDatumRange(OID rangeTypeId);
    }

    /// <summary>
    /// A type handler for the PostgreSQL range of integer.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/rangetypes.html.
    /// </remarks>
    [OIDHandler(OID.INT4RANGEOID, OID.INT4RANGEARRAYOID)]
    public class IntRangeHandler : RangeHandler<int, IntHandler>
    {
        public IntRangeHandler()
        {
            this.ElementOID = OID.INT4RANGEOID;
            this.ArrayOID = OID.INT4RANGEARRAYOID;
        }
    }

    /// <summary>
    /// A type handler for the PostgreSQL range of bigint.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/rangetypes.html.
    /// </remarks>
    [OIDHandler(OID.INT8RANGEOID, OID.INT8RANGEARRAYOID)]
    public class LongRangeHandler : RangeHandler<long, LongHandler>
    {
        public LongRangeHandler()
        {
            this.ElementOID = OID.INT8RANGEOID;
            this.ArrayOID = OID.INT8RANGEARRAYOID;
        }
    }

    /// <summary>
    /// A type handler for the PostgreSQL range of timestamp without time zone.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/rangetypes.html.
    /// </remarks>
    [OIDHandler(OID.TSRANGEOID, OID.TSRANGEARRAYOID)]
    public class TimestampRangeHandler : RangeHandler<DateTime, TimestampHandler>
    {
        public TimestampRangeHandler()
        {
            this.ElementOID = OID.TSRANGEOID;
            this.ArrayOID = OID.TSRANGEARRAYOID;
        }
    }

    /// <summary>
    /// A type handler for the PostgreSQL range of timestamp with time zone.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/rangetypes.html.
    /// </remarks>
    [OIDHandler(OID.TSTZRANGEOID, OID.TSTZRANGEARRAYOID)]
    public class TimestampTzRangeHandler : RangeHandler<DateTime, TimestampTzHandler>
    {
        public TimestampTzRangeHandler()
        {
            this.ElementOID = OID.TSTZRANGEOID;
            this.ArrayOID = OID.TSTZRANGEARRAYOID;
        }
    }

    /// <summary>
    /// A type handler for the PostgreSQL range of date.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/rangetypes.html.
    /// </remarks>
    [OIDHandler(OID.DATERANGEOID, OID.DATERANGEARRAYOID)]
    public class DateRangeHandler : RangeHandler<DateOnly, DateHandler>
    {
        public DateRangeHandler()
        {
            this.ElementOID = OID.DATERANGEOID;
            this.ArrayOID = OID.DATERANGEARRAYOID;
        }
    }
}
