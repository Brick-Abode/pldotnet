// <copyright file="Handler.cs" company="Brick Abode">
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
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using NpgsqlTypes;
using PlDotNET.Common;

namespace PlDotNET.Handler
{
    public static class DatumConversion
    {
        public static BoolHandler BoolHandlerObj = new ();
        public static ShortHandler ShortHandlerObj = new ();
        public static IntHandler IntHandlerObj = new ();
        public static LongHandler LongHandlerObj = new ();
        public static FloatHandler FloatHandlerObj = new ();
        public static DoubleHandler DoubleHandlerObj = new ();
        public static PointHandler PointHandlerObj = new ();
        public static LineHandler LineHandlerObj = new ();
        public static LineSegmentHandler LineSegmentHandlerObj = new ();
        public static BoxHandler BoxHandlerObj = new ();
        public static PathHandler PathHandlerObj = new ();
        public static PolygonHandler PolygonHandlerObj = new ();
        public static CircleHandler CircleHandlerObj = new ();
        public static DateHandler DateHandlerObj = new ();
        public static TimeHandler TimeHandlerObj = new ();
        public static TimeTzHandler TimeTzHandlerObj = new ();
        public static TimestampHandler TimestampHandlerObj = new ();
        public static TimestampTzHandler TimestampTzHandlerObj = new ();
        public static IntervalHandler IntervalHandlerObj = new ();
        public static MacaddrHandler MacaddrHandlerObj = new ();
        public static Macaddr8Handler Macaddr8HandlerObj = new ();
        public static InetHandler InetHandlerObj = new ();
        public static CidrHandler CidrHandlerObj = new ();
        public static TextHandler TextHandlerObj = new ();
        public static MoneyHandler MoneyHandlerObj = new ();
        public static BitStringHandler BitStringHandlerObj = new ();
        public static VarBitStringHandler VarBitStringHandlerObj = new ();
        public static ByteaHandler ByteaHandlerObj = new ();
        public static CharHandler CharHandlerObj = new ();
        public static CharVaryingHandler CharVaryingHandlerObj = new ();
        public static XmlHandler XmlHandlerObj = new ();
        public static JsonHandler JsonHandlerObj = new ();
        public static UuidHandler UuidHandlerObj = new ();
        public static IntRangeHandler IntRangeHandlerObj = new ();
        public static LongRangeHandler LongRangeHandlerObj = new ();
        public static TimestampRangeHandler TimestampRangeHandlerObj = new ();
        public static TimestampTzRangeHandler TimestampTzRangeHandlerObj = new ();
        public static DateRangeHandler DateRangeHandlerObj = new ();
        public static RecordHandler RecordHandlerObj = new ();

        public static Dictionary<OID, OID> ArrayTypes =
               new ()
        {
            { OID.BOOLARRAYOID, OID.BOOLOID },
            { OID.INT2ARRAYOID, OID.INT2OID },
            { OID.INT4ARRAYOID, OID.INT4OID },
            { OID.INT8ARRAYOID, OID.INT8OID },
            { OID.FLOAT4ARRAYOID, OID.FLOAT4OID },
            { OID.FLOAT8ARRAYOID, OID.FLOAT8OID },
            { OID.POINTARRAYOID, OID.POINTOID },
            { OID.LINEARRAYOID, OID.LINEOID },
            { OID.LSEGARRAYOID, OID.LSEGOID },
            { OID.BOXARRAYOID, OID.BOXOID },
            { OID.POLYGONARRAYOID, OID.POLYGONOID },
            { OID.TEXTARRAYOID, OID.TEXTOID },
            { OID.PATHARRAYOID, OID.PATHOID },
            { OID.CIRCLEARRAYOID, OID.CIRCLEOID },
            { OID.DATEARRAYOID, OID.DATEOID },
            { OID.TIMEARRAYOID, OID.TIMEOID },
            { OID.TIMETZARRAYOID, OID.TIMETZOID },
            { OID.TIMESTAMPARRAYOID, OID.TIMESTAMPOID },
            { OID.TIMESTAMPTZARRAYOID, OID.TIMESTAMPTZOID },
            { OID.INTERVALARRAYOID, OID.INTERVALOID },
            { OID.MACADDRARRAYOID, OID.MACADDROID },
            { OID.MACADDR8ARRAYOID, OID.MACADDR8OID },
            { OID.INETARRAYOID, OID.INETOID },
            { OID.CIDRARRAYOID, OID.CIDROID },
            { OID.MONEYARRAYOID, OID.MONEYOID },
            { OID.VARBITARRAYOID, OID.VARBITOID },
            { OID.BITARRAYOID, OID.BITOID },
            { OID.BYTEAARRAYOID, OID.BYTEAOID },
            { OID.BPCHARARRAYOID, OID.BPCHAROID },
            { OID.VARCHARARRAYOID, OID.VARCHAROID },
            { OID.XMLARRAYOID, OID.XMLOID },
            { OID.JSONARRAYOID, OID.JSONOID },
            { OID.UUIDARRAYOID, OID.UUIDOID },
            { OID.INT4RANGEARRAYOID, OID.INT4RANGEOID },
            { OID.NUMRANGEARRAYOID, OID.NUMRANGEOID },
            { OID.TSRANGEARRAYOID, OID.TSRANGEOID },
            { OID.TSTZRANGEARRAYOID, OID.TSTZRANGEOID },
            { OID.DATERANGEARRAYOID, OID.DATERANGEOID },
            { OID.INT8RANGEARRAYOID, OID.INT8RANGEOID },
            { OID.INT4MULTIRANGEARRAYOID, OID.INT4MULTIRANGEOID },
            { OID.NUMMULTIRANGEARRAYOID, OID.NUMMULTIRANGEOID },
            { OID.TSMULTIRANGEARRAYOID, OID.TSMULTIRANGEOID },
            { OID.TSTZMULTIRANGEARRAYOID, OID.TSTZMULTIRANGEOID },
            { OID.DATEMULTIRANGEARRAYOID, OID.DATEMULTIRANGEOID },
            { OID.INT8MULTIRANGEARRAYOID, OID.INT8MULTIRANGEOID },
        };

#nullable enable
        public static Dictionary<OID, Type> SupportedTypes =
                       new ()
        {
            { OID.BOOLOID, typeof(bool) },
            { OID.INT2OID, typeof(short) },
            { OID.INT4OID, typeof(int) },
            { OID.INT8OID, typeof(long) },
            { OID.FLOAT4OID, typeof(float) },
            { OID.FLOAT8OID, typeof(double) },
            { OID.POINTOID, typeof(NpgsqlPoint) },
            { OID.LINEOID, typeof(NpgsqlLine) },
            { OID.LSEGOID, typeof(NpgsqlLSeg) },
            { OID.BOXOID, typeof(NpgsqlBox) },
            { OID.POLYGONOID, typeof(NpgsqlPolygon) },
            { OID.TEXTOID, typeof(string) },
            { OID.PATHOID, typeof(NpgsqlPath) },
            { OID.CIRCLEOID, typeof(NpgsqlCircle) },
            { OID.DATEOID, typeof(DateOnly) },
            { OID.TIMEOID, typeof(TimeOnly) },
            { OID.TIMETZOID, typeof(DateTimeOffset) },
            { OID.TIMESTAMPOID, typeof(DateTime) },
            { OID.TIMESTAMPTZOID, typeof(DateTime) },
            { OID.INTERVALOID, typeof(NpgsqlInterval) },
            { OID.MACADDROID, typeof(PhysicalAddress) },
            { OID.MACADDR8OID, typeof(PhysicalAddress) },
            { OID.INETOID, typeof((IPAddress Address, int Netmask)) },
            { OID.CIDROID, typeof((IPAddress Address, int Netmask)) },
            { OID.MONEYOID, typeof(decimal) },
            { OID.VARBITOID, typeof(BitArray) },
            { OID.BITOID, typeof(BitArray) },
            { OID.BYTEAOID, typeof(byte[]) },
            { OID.BPCHAROID, typeof(string) },
            { OID.VARCHAROID, typeof(string) },
            { OID.XMLOID, typeof(string) },
            { OID.JSONOID, typeof(string) },
            { OID.UUIDOID, typeof(Guid) },
            { OID.INT4RANGEOID, typeof(NpgsqlRange<int>) },
            { OID.INT8RANGEOID, typeof(NpgsqlRange<long>) },
            { OID.TSRANGEOID, typeof(NpgsqlRange<DateTime>) },
            { OID.TSTZRANGEOID, typeof(NpgsqlRange<DateTime>) },
            { OID.DATERANGEOID, typeof(NpgsqlRange<DateOnly>) },
            { OID.VOIDOID, typeof(void) },
            { OID.RECORDOID, typeof(object?[]) },
        };
#nullable disable

        public static Dictionary<OID, string> SupportedTypesStr =
                       new ()
        {
            { OID.BOOLOID, "bool" },
            { OID.INT2OID, "short" },
            { OID.INT4OID, "int" },
            { OID.INT8OID, "long" },
            { OID.FLOAT4OID, "float" },
            { OID.FLOAT8OID, "double" },
            { OID.POINTOID, "NpgsqlPoint" },
            { OID.LINEOID, "NpgsqlLine" },
            { OID.LSEGOID, "NpgsqlLSeg" },
            { OID.BOXOID, "NpgsqlBox" },
            { OID.POLYGONOID, "NpgsqlPolygon" },
            { OID.TEXTOID, "string" },
            { OID.PATHOID, "NpgsqlPath" },
            { OID.CIRCLEOID, "NpgsqlCircle" },
            { OID.DATEOID, "DateOnly" },
            { OID.TIMEOID, "TimeOnly" },
            { OID.TIMETZOID, "DateTimeOffset" },
            { OID.TIMESTAMPOID, "DateTime" },
            { OID.TIMESTAMPTZOID, "DateTime" },
            { OID.INTERVALOID, "NpgsqlInterval" },
            { OID.MACADDROID, "PhysicalAddress" },
            { OID.MACADDR8OID, "PhysicalAddress" },
            { OID.INETOID, "(IPAddress Address, int Netmask)" },
            { OID.CIDROID, "(IPAddress Address, int Netmask)" },
            { OID.MONEYOID, "decimal" },
            { OID.VARBITOID, "BitArray" },
            { OID.BITOID, "BitArray" },
            { OID.BYTEAOID, "byte[]" },
            { OID.BPCHAROID, "string" },
            { OID.VARCHAROID, "string" },
            { OID.XMLOID, "string" },
            { OID.JSONOID, "string" },
            { OID.UUIDOID, "Guid" },
            { OID.INT4RANGEOID, "NpgsqlRange<int>" },
            { OID.INT8RANGEOID, "NpgsqlRange<long>" },
            { OID.TSRANGEOID, "NpgsqlRange<DateTime>" },
            { OID.TSTZRANGEOID, "NpgsqlRange<DateTime>" },
            { OID.DATERANGEOID, "NpgsqlRange<DateOnly>" },
            { OID.VOIDOID, "void" },
            { OID.RECORDOID, "Object?[]" },
        };

        /// <summary>
        /// Returns the handler object NAME for the specified OID.
        /// </summary>
        /// <returns>
        /// Returns The TypeHandler name.
        /// </returns>
        public static string GetTypeHandlerName(uint id)
        {
            switch (id)
            {
                case (uint)OID.BOOLOID:
                    return "BoolHandler";
                case (uint)OID.INT2OID:
                    return "ShortHandler";
                case (uint)OID.INT4OID:
                    return "IntHandler";
                case (uint)OID.INT8OID:
                    return "LongHandler";
                case (uint)OID.FLOAT4OID:
                    return "FloatHandler";
                case (uint)OID.FLOAT8OID:
                    return "DoubleHandler";
                case (uint)OID.POINTOID:
                    return "PointHandler";
                case (uint)OID.LINEOID:
                    return "LineHandler";
                case (uint)OID.LSEGOID:
                    return "LineSegmentHandler";
                case (uint)OID.BOXOID:
                    return "BoxHandler";
                case (uint)OID.PATHOID:
                    return "PathHandler";
                case (uint)OID.POLYGONOID:
                    return "PolygonHandler";
                case (uint)OID.CIRCLEOID:
                    return "CircleHandler";
                case (uint)OID.DATEOID:
                    return "DateHandler";
                case (uint)OID.TIMEOID:
                    return "TimeHandler";
                case (uint)OID.TIMETZOID:
                    return "TimeTzHandler";
                case (uint)OID.TIMESTAMPOID:
                    return "TimestampHandler";
                case (uint)OID.TIMESTAMPTZOID:
                    return "TimestampTzHandler";
                case (uint)OID.INTERVALOID:
                    return "IntervalHandler";
                case (uint)OID.MACADDROID:
                    return "MacaddrHandler";
                case (uint)OID.MACADDR8OID:
                    return "Macaddr8Handler";
                case (uint)OID.INETOID:
                    return "InetHandler";
                case (uint)OID.CIDROID:
                    return "CidrHandler";
                case (uint)OID.TEXTOID:
                    return "TextHandler";
                case (uint)OID.MONEYOID:
                    return "MoneyHandler";
                case (uint)OID.VARBITOID:
                    return "VarBitStringHandler";
                case (uint)OID.BITOID:
                    return "BitStringHandler";
                case (uint)OID.BYTEAOID:
                    return "ByteaHandler";
                case (uint)OID.BPCHAROID:
                    return "CharHandler";
                case (uint)OID.VARCHAROID:
                    return "CharVaryingHandler";
                case (uint)OID.XMLOID:
                    return "XmlHandler";
                case (uint)OID.JSONOID:
                    return "JsonHandler";
                case (uint)OID.UUIDOID:
                    return "UuidHandler";
                case (uint)OID.INT4RANGEOID:
                    return "IntRangeHandler";
                case (uint)OID.INT8RANGEOID:
                    return "LongRangeHandler";
                case (uint)OID.TSRANGEOID:
                    return "TimestampRangeHandler";
                case (uint)OID.TSTZRANGEOID:
                    return "TimestampTzRangeHandler";
                case (uint)OID.DATERANGEOID:
                    return "DateRangeHandler";
                case (uint)OID.RECORDOID:
                    return "RecordHandler";
                case (uint)OID.TRIGGEROID:
                    return "RecordHandler";
                default:
                    if (DatumConversion.ArrayTypes.ContainsKey((OID)id))
                    {
                        return GetTypeHandlerName((uint)DatumConversion.ArrayTypes[(OID)id]);
                    }

                    throw new NotImplementedException($"Datum to {(OID)id} is not supported! Check GetTypeHandler");
            }
        }

        public static object InputValue(IntPtr datum, OID type, bool arrayAllowsNullElements = false)
        {
            return (object)type switch
            {
                OID.BOOLOID => BoolHandlerObj.InputValue(datum),
                OID.INT2OID => ShortHandlerObj.InputValue(datum),
                OID.INT4OID => IntHandlerObj.InputValue(datum),
                OID.INT8OID => LongHandlerObj.InputValue(datum),
                OID.FLOAT4OID => FloatHandlerObj.InputValue(datum),
                OID.FLOAT8OID => DoubleHandlerObj.InputValue(datum),
                OID.POINTOID => PointHandlerObj.InputValue(datum),
                OID.LINEOID => LineHandlerObj.InputValue(datum),
                OID.LSEGOID => LineSegmentHandlerObj.InputValue(datum),
                OID.BOXOID => BoxHandlerObj.InputValue(datum),
                OID.POLYGONOID => PolygonHandlerObj.InputValue(datum),
                OID.TEXTOID => TextHandlerObj.InputValue(datum),
                OID.PATHOID => PathHandlerObj.InputValue(datum),
                OID.CIRCLEOID => CircleHandlerObj.InputValue(datum),
                OID.DATEOID => DateHandlerObj.InputValue(datum),
                OID.TIMEOID => TimeHandlerObj.InputValue(datum),
                OID.TIMETZOID => TimeTzHandlerObj.InputValue(datum),
                OID.TIMESTAMPOID => TimestampHandlerObj.InputValue(datum),
                OID.TIMESTAMPTZOID => TimestampTzHandlerObj.InputValue(datum),
                OID.INTERVALOID => IntervalHandlerObj.InputValue(datum),
                OID.MACADDROID => MacaddrHandlerObj.InputValue(datum),
                OID.MACADDR8OID => Macaddr8HandlerObj.InputValue(datum),
                OID.INETOID => InetHandlerObj.InputValue(datum),
                OID.CIDROID => CidrHandlerObj.InputValue(datum),
                OID.MONEYOID => MoneyHandlerObj.InputValue(datum),
                OID.VARBITOID => VarBitStringHandlerObj.InputValue(datum),
                OID.BITOID => BitStringHandlerObj.InputValue(datum),
                OID.BYTEAOID => ByteaHandlerObj.InputValue(datum),
                OID.BPCHAROID => CharHandlerObj.InputValue(datum),
                OID.VARCHAROID => CharVaryingHandlerObj.InputValue(datum),
                OID.XMLOID => XmlHandlerObj.InputValue(datum),
                OID.JSONOID => JsonHandlerObj.InputValue(datum),
                OID.UUIDOID => UuidHandlerObj.InputValue(datum),
                OID.INT4RANGEOID => IntRangeHandlerObj.InputValue(datum),
                OID.INT8RANGEOID => LongRangeHandlerObj.InputValue(datum),
                OID.TSRANGEOID => TimestampRangeHandlerObj.InputValue(datum),
                OID.TSTZRANGEOID => TimestampTzRangeHandlerObj.InputValue(datum),
                OID.DATERANGEOID => DateRangeHandlerObj.InputValue(datum),
                OID.RECORDOID => RecordHandlerObj.InputValue(datum),

                // Array types
                OID.BOOLARRAYOID => BoolHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.INT2ARRAYOID => ShortHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.INT4ARRAYOID => IntHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.INT8ARRAYOID => LongHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.FLOAT4ARRAYOID => FloatHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.FLOAT8ARRAYOID => DoubleHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.POINTARRAYOID => PointHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.LINEARRAYOID => LineHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.LSEGARRAYOID => LineSegmentHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.BOXARRAYOID => BoxHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.POLYGONARRAYOID => PolygonHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.TEXTARRAYOID => TextHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.PATHARRAYOID => PathHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.CIRCLEARRAYOID => CircleHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.DATEARRAYOID => DateHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.TIMEARRAYOID => TimeHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.TIMETZARRAYOID => TimeTzHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.TIMESTAMPARRAYOID => TimestampHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.TIMESTAMPTZARRAYOID => TimestampTzHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.INTERVALARRAYOID => IntervalHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.MACADDRARRAYOID => MacaddrHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.MACADDR8ARRAYOID => Macaddr8HandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.INETARRAYOID => InetHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.CIDRARRAYOID => CidrHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.MONEYARRAYOID => MoneyHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.VARBITARRAYOID => VarBitStringHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.BITARRAYOID => BitStringHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.BYTEAARRAYOID => ByteaHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.BPCHARARRAYOID => CharHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.VARCHARARRAYOID => CharVaryingHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.XMLARRAYOID => XmlHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.JSONARRAYOID => JsonHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.UUIDARRAYOID => UuidHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.INT4RANGEARRAYOID => IntRangeHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.INT8RANGEARRAYOID => LongRangeHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.TSRANGEARRAYOID => TimestampRangeHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.TSTZRANGEARRAYOID => TimestampTzRangeHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.DATERANGEARRAYOID => DateRangeHandlerObj.InputArrayT(datum, arrayAllowsNullElements),
                OID.RECORDARRAYOID => RecordHandlerObj.InputArrayT(datum, arrayAllowsNullElements),

                _ => throw new InvalidOperationException($"Failed on InputValue. Unsupported type: {type}")
            };
        }

#nullable enable
        public static object? InputNullableValue(IntPtr datum, OID type, bool isNull, bool arrayAllowsNullElements = false)
        {
            return (object)type switch
            {
                OID.BOOLOID => BoolHandlerObj.InputNullableValue(datum, isNull),
                OID.INT2OID => ShortHandlerObj.InputNullableValue(datum, isNull),
                OID.INT4OID => IntHandlerObj.InputNullableValue(datum, isNull),
                OID.INT8OID => LongHandlerObj.InputNullableValue(datum, isNull),
                OID.FLOAT4OID => FloatHandlerObj.InputNullableValue(datum, isNull),
                OID.FLOAT8OID => DoubleHandlerObj.InputNullableValue(datum, isNull),
                OID.POINTOID => PointHandlerObj.InputNullableValue(datum, isNull),
                OID.LINEOID => LineHandlerObj.InputNullableValue(datum, isNull),
                OID.LSEGOID => LineSegmentHandlerObj.InputNullableValue(datum, isNull),
                OID.BOXOID => BoxHandlerObj.InputNullableValue(datum, isNull),
                OID.POLYGONOID => PolygonHandlerObj.InputNullableValue(datum, isNull),
                OID.TEXTOID => TextHandlerObj.InputNullableValue(datum, isNull),
                OID.PATHOID => PathHandlerObj.InputNullableValue(datum, isNull),
                OID.CIRCLEOID => CircleHandlerObj.InputNullableValue(datum, isNull),
                OID.DATEOID => DateHandlerObj.InputNullableValue(datum, isNull),
                OID.TIMEOID => TimeHandlerObj.InputNullableValue(datum, isNull),
                OID.TIMETZOID => TimeTzHandlerObj.InputNullableValue(datum, isNull),
                OID.TIMESTAMPOID => TimestampHandlerObj.InputNullableValue(datum, isNull),
                OID.TIMESTAMPTZOID => TimestampTzHandlerObj.InputNullableValue(datum, isNull),
                OID.INTERVALOID => IntervalHandlerObj.InputNullableValue(datum, isNull),
                OID.MACADDROID => MacaddrHandlerObj.InputNullableValue(datum, isNull),
                OID.MACADDR8OID => Macaddr8HandlerObj.InputNullableValue(datum, isNull),
                OID.INETOID => InetHandlerObj.InputNullableValue(datum, isNull),
                OID.CIDROID => CidrHandlerObj.InputNullableValue(datum, isNull),
                OID.MONEYOID => MoneyHandlerObj.InputNullableValue(datum, isNull),
                OID.VARBITOID => VarBitStringHandlerObj.InputNullableValue(datum, isNull),
                OID.BITOID => BitStringHandlerObj.InputNullableValue(datum, isNull),
                OID.BYTEAOID => ByteaHandlerObj.InputNullableValue(datum, isNull),
                OID.BPCHAROID => CharHandlerObj.InputNullableValue(datum, isNull),
                OID.VARCHAROID => CharVaryingHandlerObj.InputNullableValue(datum, isNull),
                OID.XMLOID => XmlHandlerObj.InputNullableValue(datum, isNull),
                OID.JSONOID => JsonHandlerObj.InputNullableValue(datum, isNull),
                OID.UUIDOID => UuidHandlerObj.InputNullableValue(datum, isNull),
                OID.INT4RANGEOID => IntRangeHandlerObj.InputNullableValue(datum, isNull),
                OID.INT8RANGEOID => LongRangeHandlerObj.InputNullableValue(datum, isNull),
                OID.TSRANGEOID => TimestampRangeHandlerObj.InputNullableValue(datum, isNull),
                OID.TSTZRANGEOID => TimestampTzRangeHandlerObj.InputNullableValue(datum, isNull),
                OID.DATERANGEOID => DateRangeHandlerObj.InputNullableValue(datum, isNull),
                OID.RECORDOID => RecordHandlerObj.InputNullableValue(datum, isNull),

                // Array types
                OID.BOOLARRAYOID => BoolHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.INT2ARRAYOID => ShortHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.INT4ARRAYOID => IntHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.INT8ARRAYOID => LongHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.FLOAT4ARRAYOID => FloatHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.FLOAT8ARRAYOID => DoubleHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.POINTARRAYOID => PointHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.LINEARRAYOID => LineHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.LSEGARRAYOID => LineSegmentHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.BOXARRAYOID => BoxHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.POLYGONARRAYOID => PolygonHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.TEXTARRAYOID => TextHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.PATHARRAYOID => PathHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.CIRCLEARRAYOID => CircleHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.DATEARRAYOID => DateHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.TIMEARRAYOID => TimeHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.TIMETZARRAYOID => TimeTzHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.TIMESTAMPARRAYOID => TimestampHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.TIMESTAMPTZARRAYOID => TimestampTzHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.INTERVALARRAYOID => IntervalHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.MACADDRARRAYOID => MacaddrHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.MACADDR8ARRAYOID => Macaddr8HandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.INETARRAYOID => InetHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.CIDRARRAYOID => CidrHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.MONEYARRAYOID => MoneyHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.VARBITARRAYOID => VarBitStringHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.BITARRAYOID => BitStringHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.BYTEAARRAYOID => ByteaHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.BPCHARARRAYOID => CharHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.VARCHARARRAYOID => CharVaryingHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.XMLARRAYOID => XmlHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.JSONARRAYOID => JsonHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.UUIDARRAYOID => UuidHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.INT4RANGEARRAYOID => IntRangeHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.INT8RANGEARRAYOID => LongRangeHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.TSRANGEARRAYOID => TimestampRangeHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.TSTZRANGEARRAYOID => TimestampTzRangeHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.DATERANGEARRAYOID => DateRangeHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),
                OID.RECORDARRAYOID => RecordHandlerObj.InputNullableArrayT(datum, isNull, arrayAllowsNullElements),

                _ => throw new InvalidOperationException($"Failed on InputNullableValue. Unsupported type: {type}")
            };
        }
#nullable disable

#nullable enable
        public static IntPtr OutputNullableValue(OID type, object? value)
        {
            if (DBNull.Value.Equals(value))
            {
                value = null;
            }

            return type switch
            {
                OID.BOOLOID => BoolHandlerObj.OutputNullableValue((bool?)value),
                OID.INT2OID => ShortHandlerObj.OutputNullableValue((short?)value),
                OID.INT4OID => IntHandlerObj.OutputNullableValue((int?)value),
                OID.INT8OID => LongHandlerObj.OutputNullableValue((long?)value),
                OID.FLOAT4OID => FloatHandlerObj.OutputNullableValue((float?)value),
                OID.FLOAT8OID => DoubleHandlerObj.OutputNullableValue((double?)value),
                OID.POINTOID => PointHandlerObj.OutputNullableValue((NpgsqlPoint?)value),
                OID.LINEOID => LineHandlerObj.OutputNullableValue((NpgsqlLine?)value),
                OID.LSEGOID => LineSegmentHandlerObj.OutputNullableValue((NpgsqlLSeg?)value),
                OID.BOXOID => BoxHandlerObj.OutputNullableValue((NpgsqlBox?)value),
                OID.POLYGONOID => PolygonHandlerObj.OutputNullableValue((NpgsqlPolygon?)value),
                OID.TEXTOID => TextHandlerObj.OutputNullableValue((string?)value),
                OID.PATHOID => PathHandlerObj.OutputNullableValue((NpgsqlPath?)value),
                OID.CIRCLEOID => CircleHandlerObj.OutputNullableValue((NpgsqlCircle?)value),
                OID.DATEOID => DateHandlerObj.OutputNullableValue((DateOnly?)value),
                OID.TIMEOID => TimeHandlerObj.OutputNullableValue((TimeOnly?)value),
                OID.TIMETZOID => TimeTzHandlerObj.OutputNullableValue((DateTimeOffset?)value),
                OID.TIMESTAMPOID => TimestampHandlerObj.OutputNullableValue((DateTime?)value),
                OID.TIMESTAMPTZOID => TimestampTzHandlerObj.OutputNullableValue((DateTime?)value),
                OID.INTERVALOID => IntervalHandlerObj.OutputNullableValue((NpgsqlInterval?)value),
                OID.MACADDROID => MacaddrHandlerObj.OutputNullableValue((PhysicalAddress?)value),
                OID.MACADDR8OID => Macaddr8HandlerObj.OutputNullableValue((PhysicalAddress?)value),
                OID.INETOID => InetHandlerObj.OutputNullableValue(((IPAddress Address, int Netmask)?)value),
                OID.CIDROID => CidrHandlerObj.OutputNullableValue(((IPAddress Address, int Netmask)?)value),
                OID.MONEYOID => MoneyHandlerObj.OutputNullableValue((decimal?)value),
                OID.VARBITOID => VarBitStringHandlerObj.OutputNullableValue((BitArray?)value),
                OID.BITOID => BitStringHandlerObj.OutputNullableValue((BitArray?)value),
                OID.BYTEAOID => ByteaHandlerObj.OutputNullableValue((byte[]?)value),
                OID.BPCHAROID => CharHandlerObj.OutputNullableValue((string?)value),
                OID.VARCHAROID => CharVaryingHandlerObj.OutputNullableValue((string?)value),
                OID.XMLOID => XmlHandlerObj.OutputNullableValue((string?)value),
                OID.JSONOID => JsonHandlerObj.OutputNullableValue((string?)value),
                OID.UUIDOID => UuidHandlerObj.OutputNullableValue((Guid?)value),
                OID.INT4RANGEOID => IntRangeHandlerObj.OutputNullableValue((NpgsqlRange<int>?)value),
                OID.INT8RANGEOID => LongRangeHandlerObj.OutputNullableValue((NpgsqlRange<long>?)value),
                OID.TSRANGEOID => TimestampRangeHandlerObj.OutputNullableValue((NpgsqlRange<DateTime>?)value),
                OID.TSTZRANGEOID => TimestampTzRangeHandlerObj.OutputNullableValue((NpgsqlRange<DateTime>?)value),
                OID.DATERANGEOID => DateRangeHandlerObj.OutputNullableValue((NpgsqlRange<DateOnly>?)value),
                OID.RECORDOID => RecordHandlerObj.OutputNullableValue(value as object?[]),

                // Array types
                OID.BOOLARRAYOID => BoolHandlerObj.OutputNullableArray((Array?)value),
                OID.INT2ARRAYOID => ShortHandlerObj.OutputNullableArray((Array?)value),
                OID.INT4ARRAYOID => IntHandlerObj.OutputNullableArray((Array?)value),
                OID.INT8ARRAYOID => LongHandlerObj.OutputNullableArray((Array?)value),
                OID.FLOAT4ARRAYOID => FloatHandlerObj.OutputNullableArray((Array?)value),
                OID.FLOAT8ARRAYOID => DoubleHandlerObj.OutputNullableArray((Array?)value),
                OID.POINTARRAYOID => PointHandlerObj.OutputNullableArray((Array?)value),
                OID.LINEARRAYOID => LineHandlerObj.OutputNullableArray((Array?)value),
                OID.LSEGARRAYOID => LineSegmentHandlerObj.OutputNullableArray((Array?)value),
                OID.BOXARRAYOID => BoxHandlerObj.OutputNullableArray((Array?)value),
                OID.POLYGONARRAYOID => PolygonHandlerObj.OutputNullableArray((Array?)value),
                OID.TEXTARRAYOID => TextHandlerObj.OutputNullableArray((Array?)value),
                OID.PATHARRAYOID => PathHandlerObj.OutputNullableArray((Array?)value),
                OID.CIRCLEARRAYOID => CircleHandlerObj.OutputNullableArray((Array?)value),
                OID.DATEARRAYOID => DateHandlerObj.OutputNullableArray((Array?)value),
                OID.TIMEARRAYOID => TimeHandlerObj.OutputNullableArray((Array?)value),
                OID.TIMETZARRAYOID => TimeTzHandlerObj.OutputNullableArray((Array?)value),
                OID.TIMESTAMPARRAYOID => TimestampHandlerObj.OutputNullableArray((Array?)value),
                OID.TIMESTAMPTZARRAYOID => TimestampTzHandlerObj.OutputNullableArray((Array?)value),
                OID.INTERVALARRAYOID => IntervalHandlerObj.OutputNullableArray((Array?)value),
                OID.MACADDRARRAYOID => MacaddrHandlerObj.OutputNullableArray((Array?)value),
                OID.MACADDR8ARRAYOID => Macaddr8HandlerObj.OutputNullableArray((Array?)value),
                OID.INETARRAYOID => InetHandlerObj.OutputNullableArray((Array?)value),
                OID.CIDRARRAYOID => CidrHandlerObj.OutputNullableArray((Array?)value),
                OID.MONEYARRAYOID => MoneyHandlerObj.OutputNullableArray((Array?)value),
                OID.VARBITARRAYOID => VarBitStringHandlerObj.OutputNullableArray((Array?)value),
                OID.BITARRAYOID => BitStringHandlerObj.OutputNullableArray((Array?)value),
                OID.BYTEAARRAYOID => ByteaHandlerObj.OutputNullableArray((Array?)value),
                OID.BPCHARARRAYOID => CharHandlerObj.OutputNullableArray((Array?)value),
                OID.VARCHARARRAYOID => CharVaryingHandlerObj.OutputNullableArray((Array?)value),
                OID.XMLARRAYOID => XmlHandlerObj.OutputNullableArray((Array?)value),
                OID.JSONARRAYOID => JsonHandlerObj.OutputNullableArray((Array?)value),
                OID.UUIDARRAYOID => UuidHandlerObj.OutputNullableArray((Array?)value),
                OID.INT4RANGEARRAYOID => IntRangeHandlerObj.OutputNullableArray((Array?)value),
                OID.INT8RANGEARRAYOID => LongRangeHandlerObj.OutputNullableArray((Array?)value),
                OID.TSRANGEARRAYOID => TimestampRangeHandlerObj.OutputNullableArray((Array?)value),
                OID.TSTZRANGEARRAYOID => TimestampTzRangeHandlerObj.OutputNullableArray((Array?)value),
                OID.DATERANGEARRAYOID => DateRangeHandlerObj.OutputNullableArray((Array?)value),
                OID.RECORDARRAYOID => RecordHandlerObj.OutputNullableArray((Array?)value),

                _ => throw new InvalidOperationException($"Failed on OutputNullableValue. Unsupported type: {type}")
            };
        }
#nullable disable

        public static Type GetFieldType(OID type)
        {
            if (ArrayTypes.ContainsKey(type))
            {
                return typeof(Array);
            }

            if (SupportedTypes.ContainsKey(type))
            {
                return SupportedTypes[type];
            }

            throw new NotImplementedException($"pldotnet does not support the {(OID)type} type.");
        }
    }
}