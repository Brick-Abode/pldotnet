// <copyright file="DateTimeHandler.cs" company="Brick Abode">
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
using System.Diagnostics;
using System.Runtime.InteropServices;
using NpgsqlTypes;
using PlDotNET.Common;

namespace PlDotNET.Handler
{
    /// <summary>
    /// A class to control PostgreSQL parameters.
    /// </summary>
    public class ConfigDateTime
    {
        public static readonly bool DisableDateTimeInfinityConversions = true;
        public static readonly bool LegacyTimestampBehavior = true;
        public static readonly string InfinityExceptionMessage =
        "Can't read infinity value since UserClass.DisableDateTimeInfinityConversions is enabled";

        public static readonly long PostgresTimestampOffsetTicks = 630822816000000000L;
    }

    /// <summary>
    /// A type handler for the PostgreSQL date data type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/static/datatype-datetime.html.
    /// </remarks>
    [OIDHandler(OID.DATEOID, OID.DATEARRAYOID)]
    public class DateHandler : StructTypeHandler<DateOnly>
    {
        public DateHandler()
        {
            this.ElementOID = OID.DATEOID;
            this.ArrayOID = OID.DATEARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumDateAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern void pldotnet_GetDatumDateAttributes(IntPtr datum, ref int date);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumDate().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumDate(int date);

        /// <inheritdoc />
        public override DateOnly InputValue(IntPtr datum)
        {
            int date = 0;
            pldotnet_GetDatumDateAttributes(datum, ref date);
            return date switch
            {
                int.MaxValue => ConfigDateTime.DisableDateTimeInfinityConversions ?
                    throw new InvalidCastException(ConfigDateTime.InfinityExceptionMessage) : DateOnly.MaxValue,
                int.MinValue => ConfigDateTime.DisableDateTimeInfinityConversions ?
                    throw new InvalidCastException(ConfigDateTime.InfinityExceptionMessage) : DateOnly.MinValue,
                var value => DateOnly.FromDayNumber(value + 730119)
            };
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(DateOnly value)
        {
            return pldotnet_CreateDatumDate(value.DayNumber - 730119);
        }
    }

    /// <summary>
    /// A type handler for the PostgreSQL time data type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/static/datatype-datetime.html.
    /// </remarks>
    [OIDHandler(OID.TIMEOID, OID.TIMEARRAYOID)]
    public class TimeHandler : StructTypeHandler<TimeOnly>
    {
        public TimeHandler()
        {
            this.ElementOID = OID.TIMEOID;
            this.ArrayOID = OID.TIMEARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumTimeAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern void pldotnet_GetDatumTimeAttributes(IntPtr datum, ref long time);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumTime().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumTime(long time);

        /// <inheritdoc />
        public override TimeOnly InputValue(IntPtr datum)
        {
            long time = 0;
            pldotnet_GetDatumTimeAttributes(datum, ref time);
            return new TimeOnly(time * 10);
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(TimeOnly value)
        {
            return pldotnet_CreateDatumTime(value.Ticks / 10);
        }
    }

    /// <summary>
    /// A type handler for the PostgreSQL timetz data type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/static/datatype-datetime.html.
    /// </remarks>
    [OIDHandler(OID.TIMETZOID, OID.TIMETZARRAYOID)]
    public class TimeTzHandler : StructTypeHandler<DateTimeOffset>
    {
        public TimeTzHandler()
        {
            this.ElementOID = OID.TIMETZOID;
            this.ArrayOID = OID.TIMETZARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumTimeTzAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern void pldotnet_GetDatumTimeTzAttributes(IntPtr datum, ref long time, ref int zone);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumTimeTz().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumTimeTz(long time, int zone);

        /// <inheritdoc />
        public override DateTimeOffset InputValue(IntPtr datum)
        {
            long time = 0;
            int zone = 0;
            pldotnet_GetDatumTimeTzAttributes(datum, ref time, ref zone);
            return new DateTimeOffset((time * 10) + TimeSpan.TicksPerDay, new TimeSpan(0, 0, -zone));
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(DateTimeOffset value)
        {
            return pldotnet_CreateDatumTimeTz(value.TimeOfDay.Ticks / 10, -(int)(value.Offset.Ticks / TimeSpan.TicksPerSecond));
        }
    }

    /// <summary>
    /// A type handler for the PostgreSQL timestamp data type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/static/datatype-datetime.html.
    /// </remarks>
    [OIDHandler(OID.TIMESTAMPOID, OID.TIMESTAMPARRAYOID)]
    public class TimestampHandler : StructTypeHandler<DateTime>
    {
        public TimestampHandler()
        {
            this.ElementOID = OID.TIMESTAMPOID;
            this.ArrayOID = OID.TIMESTAMPARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumTimestampAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern void pldotnet_GetDatumTimestampAttributes(IntPtr datum, ref long timestamp);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumTimestamp().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumTimestamp(long timestamp);

        /// <summary>
        /// Creates the DateTime object according to the provided parameters.
        /// </summary>
        public static DateTime CreateDateTimeObject(long timestamp, DateTimeKind kind)
        {
            try
            {
                return timestamp switch
                {
                    long.MaxValue => ConfigDateTime.DisableDateTimeInfinityConversions ? throw new InvalidCastException(ConfigDateTime.InfinityExceptionMessage) : DateTime.MaxValue,
                    long.MinValue => ConfigDateTime.DisableDateTimeInfinityConversions ? throw new InvalidCastException(ConfigDateTime.InfinityExceptionMessage) : DateTime.MinValue,
                    var value => new DateTime((value * 10) + ConfigDateTime.PostgresTimestampOffsetTicks, kind)
                };
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new InvalidCastException("Out of the range of DateTime (year must be between 1 and 9999)", e);
            }
        }

        /// <inheritdoc />
        public override DateTime InputValue(IntPtr datum)
        {
            long timestamp = 0;
            pldotnet_GetDatumTimestampAttributes(datum, ref timestamp);
            return CreateDateTimeObject(timestamp, DateTimeKind.Unspecified);
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(DateTime value)
        {
            if (!ConfigDateTime.DisableDateTimeInfinityConversions)
            {
                if (value == DateTime.MaxValue)
                {
                    return pldotnet_CreateDatumTimestamp(long.MaxValue);
                }

                if (value == DateTime.MinValue)
                {
                    return pldotnet_CreateDatumTimestamp(long.MinValue);
                }
            }

            return pldotnet_CreateDatumTimestamp((long)((value.Ticks - ConfigDateTime.PostgresTimestampOffsetTicks) / 10));
        }
    }

    /// <summary>
    /// A type handler for the PostgreSQL timestamptz data type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/static/datatype-datetime.html.
    /// </remarks>
    [OIDHandler(OID.TIMESTAMPTZOID, OID.TIMESTAMPTZARRAYOID)]
    public class TimestampTzHandler : StructTypeHandler<DateTime>
    {
        public TimestampTzHandler()
        {
            this.ElementOID = OID.TIMESTAMPTZOID;
            this.ArrayOID = OID.TIMESTAMPTZARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumTimestampTzAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern void pldotnet_GetDatumTimestampTzAttributes(IntPtr datum, ref long timestamp);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumTimestampTz().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumTimestampTz(long timestamp);

        /// <inheritdoc />
        public override DateTime InputValue(IntPtr datum)
        {
            long timestamp = 0;
            pldotnet_GetDatumTimestampTzAttributes(datum, ref timestamp);
            DateTime dateTime = TimestampHandler.CreateDateTimeObject(timestamp, DateTimeKind.Utc);
            return ConfigDateTime.LegacyTimestampBehavior && (ConfigDateTime.DisableDateTimeInfinityConversions || (dateTime != DateTime.MaxValue && dateTime != DateTime.MinValue))
            ? dateTime.ToLocalTime()
            : dateTime;
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(DateTime value)
        {
            if (ConfigDateTime.LegacyTimestampBehavior)
            {
                switch (value.Kind)
                {
                    case DateTimeKind.Unspecified:
                    case DateTimeKind.Utc:
                        break;
                    case DateTimeKind.Local:
                        value = value.ToUniversalTime();
                        break;
                    default:
                        throw new InvalidOperationException($"Internal Npgsql bug: unexpected value {value.Kind} of enum {nameof(DateTimeKind)}. Please file a bug.");
                }
            }
            else
            {
                Debug.Assert(value.Kind == DateTimeKind.Utc || value == DateTime.MinValue || value == DateTime.MaxValue);
            }

            if (!ConfigDateTime.DisableDateTimeInfinityConversions)
            {
                if (value == DateTime.MaxValue)
                {
                    return pldotnet_CreateDatumTimestampTz(long.MaxValue);
                }

                if (value == DateTime.MinValue)
                {
                    return pldotnet_CreateDatumTimestampTz(long.MinValue);
                }
            }

            return pldotnet_CreateDatumTimestampTz((long)((value.Ticks - ConfigDateTime.PostgresTimestampOffsetTicks) / 10));
        }
    }

    /// <summary>
    /// A type handler for the PostgreSQL date interval type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/static/datatype-datetime.html.
    /// </remarks>
    [OIDHandler(OID.INTERVALOID, OID.INTERVALARRAYOID)]
    public class IntervalHandler : StructTypeHandler<NpgsqlInterval>
    {
        public IntervalHandler()
        {
            this.ElementOID = OID.INTERVALOID;
            this.ArrayOID = OID.INTERVALARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumIntervalAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern void pldotnet_GetDatumIntervalAttributes(IntPtr datum, ref long time, ref int day, ref int month);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumInterval().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumInterval(long time, int day, int month);

        /// <inheritdoc />
        public override NpgsqlInterval InputValue(IntPtr datum)
        {
            long time = 0;
            int day = 0, month = 0;
            pldotnet_GetDatumIntervalAttributes(datum, ref time, ref day, ref month);
            return new NpgsqlInterval(month, day, time);
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(NpgsqlInterval value)
        {
            return pldotnet_CreateDatumInterval(value.Time, value.Days, value.Months);
        }
    }
}
