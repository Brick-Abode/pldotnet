// <copyright file="Program.cs" company="Brick Abode">
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
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using NpgsqlTypes;

#pragma warning disable CS8602
#pragma warning disable CS8625
#pragma warning disable CS8605
#pragma warning disable CS8600

namespace TestDLLFunctions
{
    public class TestClass
    {
        public static int FlatArray(Array originalArray, ref Array flatArray, int[] auxiliar = null, int contEl = 0, int loc = 1)
        {
            if (contEl >= originalArray.Length)
            {
                return contEl;
            }
            else if (contEl == 0 || auxiliar == null)
            {
                auxiliar = new int[originalArray.Rank];
            }

            int ndim = originalArray.Rank;
            int[] dim = new int[ndim];
            for (int i = 0; i < ndim; i++)
            {
                dim[i] = originalArray.GetLength(i);
            }

            if (loc == 1)
            {
                for (int i = 0; i < dim[ndim - loc]; i++)
                {
                    flatArray.SetValue(originalArray.GetValue(auxiliar), contEl++);
                    auxiliar[ndim - loc] += 1;
                }
            }

            for (int i = 1; i < loc; i++)
            {
                auxiliar[ndim - loc] += 1;
                if (auxiliar[ndim - loc] < dim[ndim - loc])
                {
                    contEl = FlatArray(originalArray, ref flatArray, auxiliar, contEl, i);
                }
            }

            auxiliar[ndim - loc] = 0;
            contEl = FlatArray(originalArray, ref flatArray, auxiliar, contEl, ++loc);
            return contEl;
        }

        public static BitArray? modifybit(BitArray? a)
        {
            if (a == null)
            {
                return null;
            }

            a[0] = !a[0];
            a[^1] = !a[^1];
            return a;
        }

        public static BitArray? modifyvarbit(BitArray? a)
        {
            if (a == null)
            {
                return null;
            }

            a[0] = !a[0];
            a[^1] = !a[^1];
            return a;
        }

        public static bool? booleanand(bool? a, bool? b)
        {
            a ??= false;
            b ??= false;

            return a & b;
        }

        public static byte[]? byteaconversions(byte[]? a, byte[]? b)
        {
            UTF8Encoding utf8_e = new ();
            if (a == null && b == null)
            {
                return null;
            }

            if (a == null)
            {
                return b;
            }

            if (b == null)
            {
                return a;
            }

            string s1 = utf8_e.GetString(a, 0, a.Length);
            string s2 = utf8_e.GetString(b, 0, b.Length);
            string result = s1 + " " + s2;
            return utf8_e.GetBytes(result);
        }

        public static DateTime? setnewdate(DateTime? orig_timestamp, DateOnly? new_date)
        {
            if (orig_timestamp == null)
            {
                orig_timestamp = new DateTime(2022, 1, 1, 8, 30, 20);
            }

            if (new_date == null)
            {
                new_date = new DateOnly(2023, 12, 25);
            }

            int new_day = ((DateOnly)new_date).Day;
            int new_month = ((DateOnly)new_date).Month;
            int new_year = ((DateOnly)new_date).Year;
            DateTime new_timestamp = new (new_year, new_month, new_day, ((DateTime)orig_timestamp).Hour, ((DateTime)orig_timestamp).Minute, ((DateTime)orig_timestamp).Second);
            return new_timestamp;
        }

        public static double? sumdoublearray(Array doubles)
        {
            Array flatten_doubles = Array.CreateInstance(typeof(object), doubles.Length);
            TestClass.FlatArray(doubles, ref flatten_doubles);
            double double_sum = 0;
            for (int i = 0; i < flatten_doubles.Length; i++)
            {
                if (flatten_doubles.GetValue(i) == null)
                {
                    continue;
                }

                double_sum += (double)flatten_doubles.GetValue(i);
            }

            return double_sum;
        }

        public static double? sumdouble(double? a, double? b)
        {
            a ??= 0;
            b ??= 0;
            return a + b;
        }

        public static short? sum2smallint(short? a, short? b)
        {
            a ??= 0;
            b ??= 0;
            return (short)(a + b); // C# requires short cast
        }

        public static int? sum2integer(int? a, int? b)
        {
            a ??= 0;
            b ??= 0;
            return a + b;
        }

        public static long? sum2bigint(long? a, long? b)
        {
            a ??= 0;
            b ??= 0;
            return a + b;
        }

        public static Array? updatearraypolygonindex(Array values_array, NpgsqlPolygon desired, Array index)
        {
            int[] arrayInteger = index.Cast<int>().ToArray();
            values_array.SetValue(desired, arrayInteger);
            return values_array;
        }

        public static Array? updatejsonarrayindex(Array values_array, string desired, Array index)
        {
            int[] arrayInteger = index.Cast<int>().ToArray();
            values_array.SetValue(desired, arrayInteger);
            return values_array;
        }

        public static Array? increasemoney(Array values_array)
        {
            Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
            TestClass.FlatArray(values_array, ref flatten_values);
            for (int i = 0; i < flatten_values.Length; i++)
            {
                if (flatten_values.GetValue(i) == null)
                {
                    continue;
                }

                decimal orig_value = (decimal)flatten_values.GetValue(i);
                decimal new_value = orig_value + 1;
                flatten_values.SetValue((decimal)new_value, i);
            }

            return flatten_values;
        }

        public static Array? increasemacaddress8(Array values_array)
        {
            Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
            TestClass.FlatArray(values_array, ref flatten_values);
            for (int i = 0; i < flatten_values.Length; i++)
            {
                if (flatten_values.GetValue(i) == null)
                {
                    continue;
                }

                PhysicalAddress orig_value = (PhysicalAddress)flatten_values.GetValue(i);
                byte[] bytes = orig_value.GetAddressBytes();
                bytes[0] += 1;
                PhysicalAddress new_value = new (bytes);
                flatten_values.SetValue((PhysicalAddress)new_value, i);
            }

            return flatten_values;
        }

        public static Array? increasecidraddress(Array values_array)
        {
            Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
            TestClass.FlatArray(values_array, ref flatten_values);
            for (int i = 0; i < flatten_values.Length; i++)
            {
                if (flatten_values.GetValue(i) == null)
                {
                    continue;
                }

                (IPAddress address, int netmask) = ((IPAddress Address, int Netmask))flatten_values.GetValue(i);
                byte[] bytes = address.GetAddressBytes();
                bytes[0] += 1;
                (IPAddress Address, int Netmask) new_value = (new IPAddress(bytes), netmask);
                flatten_values.SetValue(((IPAddress Address, int Netmask))new_value, i);
            }

            return flatten_values;
        }

        public static Array? increaseint8ranges(Array values_array)
        {
            Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
            TestClass.FlatArray(values_array, ref flatten_values);
            for (int i = 0; i < flatten_values.Length; i++)
            {
                if (flatten_values.GetValue(i) == null)
                {
                    continue;
                }

                NpgsqlRange<long> orig_value = (NpgsqlRange<long>)flatten_values.GetValue(i);
                NpgsqlRange<long> new_value = new (orig_value.LowerBound + 1, orig_value.LowerBoundIsInclusive, orig_value.LowerBoundInfinite, orig_value.UpperBound + 1, orig_value.UpperBoundIsInclusive, orig_value.UpperBoundInfinite);
                flatten_values.SetValue((NpgsqlRange<long>)new_value, i);
            }

            return flatten_values;
        }

        public static Array? increasedateonlyranges(Array values_array)
        {
            Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
            TestClass.FlatArray(values_array, ref flatten_values);
            for (int i = 0; i < flatten_values.Length; i++)
            {
                if (flatten_values.GetValue(i) == null)
                {
                    continue;
                }

                NpgsqlRange<DateOnly> orig_value = (NpgsqlRange<DateOnly>)flatten_values.GetValue(i);

                NpgsqlRange<DateOnly> new_value = new (orig_value.LowerBound.AddDays(1), orig_value.LowerBoundIsInclusive, orig_value.LowerBoundInfinite, orig_value.UpperBound.AddDays(1), orig_value.UpperBoundIsInclusive, orig_value.UpperBoundInfinite);
                flatten_values.SetValue((NpgsqlRange<DateOnly>)new_value, i);
            }

            return flatten_values;
        }

        public static string? concatenatevarchars(string? a, string? b, string? c)
        {
            a ??= string.Empty;
            b ??= string.Empty;
            c ??= string.Empty;
            return (a + " " + b + " " + c).ToUpper();
        }

        public static Guid? combineuuids(Guid? a, Guid? b)
        {
            if (a == null)
            {
                a = new Guid("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11");
            }

            if (b == null)
            {
                b = new Guid("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11");
            }

            string aStr = a.ToString();
            string bStr = b.ToString();
            var aList = aStr.Split('-');
            var bList = bStr.Split('-');
            string newUuuidStr = aList[0] + aList[1] + aList[2] + bList[3] + bList[4];
            return new Guid(newUuuidStr);
        }
    }
}

namespace TestDLLFunctions.OtherTests
{
    public class TestClass
    {
        public static NpgsqlPoint? middlePointStrict(NpgsqlPoint pointa, NpgsqlPoint pointb)
        {
            double x = (pointa.X + pointb.X) * 0.5;
            double y = (pointa.Y + pointb.Y) * 0.5;
            var new_point = new NpgsqlPoint(x, y);

            return new_point;
        }

        public static NpgsqlPoint? middlePointDefault(NpgsqlPoint? pointa, NpgsqlPoint? pointb)
        {
            if (pointa == null)
            {
                pointa = new NpgsqlPoint(0, 0);
            }

            if (pointb == null)
            {
                pointb = new NpgsqlPoint(0, 0);
            }

            double x = (((NpgsqlPoint)pointa).X + ((NpgsqlPoint)pointb).X) * 0.5;
            double y = (((NpgsqlPoint)pointa).Y + ((NpgsqlPoint)pointb).Y) * 0.5;
            var new_point = new NpgsqlPoint(x, y);

            return new_point;
        }
    }
}