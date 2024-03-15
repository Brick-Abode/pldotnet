// <copyright file="GenericHandler.cs" company="Brick Abode">
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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.FSharp.Core;
using PlDotNET.Common;

namespace PlDotNET.Handler
{
    /// <summary>
    /// A class to handle with null values in arrays.
    /// </summary>
    public static class NullMap
    {
        /// <summary>
        /// This function checks the PostgreSQL nullmap to identify if an
        /// element of the array is null.
        /// </summary>
        /// <remarks>
        /// According to the PostgreSQL convention an empty nullmap means that
        /// no elements are null.
        /// </remarks>
        /// <param name="nullmap">The PostgreSQL nullmap.</param>
        /// <param name="offset">The element offset.</param>
        /// <returns>
        /// Whether the nullmap contains any null values.
        /// </returns>
        public static unsafe bool CheckNullValue(byte[] nullmap, int offset)
        {
            int byteLen = nullmap.Length;
            if (byteLen == 0)
            {
                return false;
            }

            int bitLen = byteLen * 8;
            if (offset > bitLen)
            {
                throw new ArgumentOutOfRangeException("Illegal offset");
            }

            int byteOffset = offset / 8;
            int bitOffset = offset % 8;
            byte relevantByte = nullmap[byteOffset];
            int isNull = (relevantByte >> bitOffset) & 0x1;

            return isNull != 1;
        }
    }

    /// <summary>
    /// Base class for all classes which represent the data type handlers.
    /// </summary>
    /// <remarks>
    /// Do not use it directly. Instead see StructTypeHandler.&lt;T&gt; and ObjectTypeHandler.&lt;T&gt;.
    /// </remarks>
    public abstract class BaseTypeHandler<T>
    {
        public OID ElementOID = OID.ANYOID;

        public OID ArrayOID = OID.ANYARRAYOID;

        /// <summary>
        /// Converts a PostgreSQL datum to .NET value.
        /// </summary>
        /// <param name="datum">The PostgreSQL datum.</param>
        /// <returns> Returns the .NET value. </returns>
        public abstract T InputValue(IntPtr datum);

        /// <summary>
        /// Converts a .NET value to PostgreSQL datum.
        /// </summary>
        /// <param name="value">The .NET value.</param>
        /// <returns> Returns the PostgreSQL datum. </returns>
        public abstract IntPtr OutputValue(T value);

        /// <summary>
        /// Gets the type of the nullable version of the type.
        /// </summary>
        /// <returns> Returns the type of the nullable version of the type. </returns>
        public abstract Type GetNullableType();

        /// <summary>
        /// Converts the input array to a multi-dimensional array of type T.
        /// </summary>
        /// <param name="datum">The input array (the PostgreSQL datum pointer).</param>
        /// <param name="allowsNullElements">Indicates whether null elements are allowed in the array.</param>
        /// <returns>A multi-dimensional array of type T.</returns>
        public virtual object InputArrayT(IntPtr datum, bool allowsNullElements = false)
        {
            Array array = this.InputArray(datum);

            switch (array.Rank)
            {
                case 1:
                    int length0_1D = array.GetLength(0);
                    T[] result1D = new T[length0_1D];
                    for (int i = 0; i < length0_1D; i++)
                    {
                        result1D[i] = (T)Convert.ChangeType(array.GetValue(i), typeof(T));
                    }

                    return result1D;

                case 2:
                    int length0_2D = array.GetLength(0);
                    int length1_2D = array.GetLength(1);
                    T[,] result2D = new T[length0_2D, length1_2D];
                    for (int i = 0; i < length0_2D; i++)
                    {
                        for (int j = 0; j < length1_2D; j++)
                        {
                            result2D[i, j] = (T)Convert.ChangeType(array.GetValue(i, j), typeof(T));
                        }
                    }

                    return result2D;

                case 3:
                    int length0_3D = array.GetLength(0);
                    int length1_3D = array.GetLength(1);
                    int length2_3D = array.GetLength(2);
                    T[,,] result3D = new T[length0_3D, length1_3D, length2_3D];
                    for (int i = 0; i < length0_3D; i++)
                    {
                        for (int j = 0; j < length1_3D; j++)
                        {
                            for (int k = 0; k < length2_3D; k++)
                            {
                                result3D[i, j, k] = (T)Convert.ChangeType(array.GetValue(i, j, k), typeof(T));
                            }
                        }
                    }

                    return result3D;

                case 4:
                    int length0_4D = array.GetLength(0);
                    int length1_4D = array.GetLength(1);
                    int length2_4D = array.GetLength(2);
                    int length3_4D = array.GetLength(3);
                    T[,,,] result4D = new T[length0_4D, length1_4D, length2_4D, length3_4D];
                    for (int i = 0; i < length0_4D; i++)
                    {
                        for (int j = 0; j < length1_4D; j++)
                        {
                            for (int k = 0; k < length2_4D; k++)
                            {
                                for (int l = 0; l < length3_4D; l++)
                                {
                                    result4D[i, j, k, l] = (T)Convert.ChangeType(array.GetValue(i, j, k, l), typeof(T));
                                }
                            }
                        }
                    }

                    return result4D;

                case 5:
                    int length0_5D = array.GetLength(0);
                    int length1_5D = array.GetLength(1);
                    int length2_5D = array.GetLength(2);
                    int length3_5D = array.GetLength(3);
                    int length4_5D = array.GetLength(4);
                    T[,,,,] result5D = new T[length0_5D, length1_5D, length2_5D, length3_5D, length4_5D];
                    for (int i = 0; i < length0_5D; i++)
                    {
                        for (int j = 0; j < length1_5D; j++)
                        {
                            for (int k = 0; k < length2_5D; k++)
                            {
                                for (int l = 0; l < length3_5D; l++)
                                {
                                    for (int m = 0; m < length4_5D; m++)
                                    {
                                        result5D[i, j, k, l, m] = (T)Convert.ChangeType(array.GetValue(i, j, k, l, m), typeof(T));
                                    }
                                }
                            }
                        }
                    }

                    return result5D;

                case 6:
                    int length0_6D = array.GetLength(0);
                    int length1_6D = array.GetLength(1);
                    int length2_6D = array.GetLength(2);
                    int length3_6D = array.GetLength(3);
                    int length4_6D = array.GetLength(4);
                    int length5_6D = array.GetLength(5);
                    T[,,,,,] result6D = new T[length0_6D, length1_6D, length2_6D, length3_6D, length4_6D, length5_6D];
                    for (int i = 0; i < length0_6D; i++)
                    {
                        for (int j = 0; j < length1_6D; j++)
                        {
                            for (int k = 0; k < length2_6D; k++)
                            {
                                for (int l = 0; l < length3_6D; l++)
                                {
                                    for (int m = 0; m < length4_6D; m++)
                                    {
                                        for (int n = 0; n < length5_6D; n++)
                                        {
                                            result6D[i, j, k, l, m, n] = (T)Convert.ChangeType(array.GetValue(i, j, k, l, m, n), typeof(T));
                                        }
                                    }
                                }
                            }
                        }
                    }

                    return result6D;

                default:
                    throw new System.Exception(
                        $"Undefined InputArrayT for arrays of {array.Rank} dimension{(array.Rank > 1 ? "s" : string.Empty)}.");
            }
        }

        /// <summary>
        /// Converts a nullable array of a specific type from a native pointer to a managed object.
        /// </summary>
        /// <param name="datum">The PostgreSQL pointer to the array.</param>
        /// <param name="isnull">A flag indicating whether the array is null.</param>
        /// <param name="allowsNullElements">A flag indicating whether the array allows null elements.</param>
        /// <returns>The managed object representing the nullable array.</returns>
#nullable enable
        public object? InputNullableArrayT(IntPtr datum, bool isnull, bool allowsNullElements = false)
        {
            return isnull ? null : this.InputArrayT(datum, allowsNullElements);
        }
#nullable disable

        /// <summary>
        /// Checks if the PostreSQL array is null. If the datum is null, it returns
        /// null. Otherwise, it calls the InputArray method.
        /// </summary>
        /// <returns>
        /// An Array Object if the Datum is not null. Otherwise returns null.
        /// </returns>
#nullable enable
        public Array? InputNullableArray(IntPtr datum, bool isnull)
        {
            return isnull ? null : this.InputArray(datum);
        }
#nullable disable

        /// <summary>
        /// Checks if the PostreSQL array is null. If the datum is null, it returns
        /// None. Otherwise, it calls the InputArray method and returns it as Some.
        /// </summary>
        /// <returns>
        /// An Array Object if the Datum is not null. Otherwise returns None.
        /// </returns>
#nullable enable
        public FSharpOption<Array> InputOptionArray(IntPtr datum, bool isnull)
        {
            return isnull ? FSharpOption<Array>.None : FSharpOption<Array>.Some(this.InputArray(datum));
        }
#nullable disable

        /// <summary>
        /// Checks if Array object is null. If so, it returns a Datum Int32, otherwise
        /// it calls the OutputArray method.
        /// </summary>
        /// <returns>
        /// An Array Datum if the value is not null. Otherwise returns an Integer Datum.
        /// </returns>
#nullable enable
        public IntPtr OutputNullableArray(Array? value)
        {
            return (value == null) ? IntHandler.pldotnet_CreateDatumInt32(0) : this.OutputArray((Array)value);
        }
#nullable disable

        /// <summary>
        /// Checks if Array object is null. If so, it returns a Datum Int32, otherwise
        /// it calls the OutputArray method.
        /// </summary>
        /// <returns>
        /// An Array Datum if the value is not null. Otherwise returns an Integer Datum.
        /// </returns>
#nullable enable
        public IntPtr OutputOptionArray(FSharpOption<Array> value)
        {
            return FSharpOption<Array>.get_IsNone(value)
                ? IntHandler.pldotnet_CreateDatumInt32(0)
                : this.OutputArray(value.Value);
        }
#nullable disable

        /// <summary>
        /// Converts a PostgreSQL array (multidimensional or not) in an Array object (dotnet style).
        /// If the PostgreSQL array has null values, this function calls InputArrayWithNull
        /// to correctly handle with this array.
        /// </summary>
        /// <param name="datum">The PostgreSQL array.</param>
        /// <returns> An Array object (multidimensional or not).</returns>
        public unsafe Array InputArray(IntPtr datum)
        {
            int ndims = 0;
            int[] rawDims = new int[ArrayHandler.Maxdim];
            byte* nullmap = null;
            int typeId = 0;

            ArrayHandler.pldotnet_GetArrayAttributes(datum, ref typeId, ref ndims, rawDims, ref nullmap);

            int[] dims = rawDims[..ndims];
            int nelems = 1;
            for (int i = 0; i < ndims; i++)
            {
                nelems *= dims[i];
            }

            IntPtr[] datums = new IntPtr[nelems];
            int arrayRet = ArrayHandler.pldotnet_GetArrayDatum(datum, datums, nelems, typeId);

            if (arrayRet != 0)
            {
                throw new System.Exception($"Got error from pldotnet_GetArrayDatum(): {arrayRet}");
            }

            if (nullmap != null)
            {
                int nullmapLen = (nelems / 8) + 1;
                ReadOnlySpan<byte> nativeSpan = new (nullmap, nullmapLen);
                byte[] nullmapArray = nativeSpan.ToArray();
                return this.InputArrayWithNull(datums, dims, nullmapArray);
            }

            T[] ret = new T[datums.Length];
            for (int i = 0; i < datums.Length; i++)
            {
                ret[i] = this.InputValue(datums[i]);
            }

            // If the array is one-dimensional, return it as it does not need to be reshaped.
            if (ndims == 1)
            {
                return ret;
            }

            Array ret2 = Array.CreateInstance(typeof(T), dims);
            ArrayManipulation.ReshapeArray(ret, ref ret2);

            return ret2;
        }

        /// <summary>
        /// Creates a PostgreSQL array from an Array object and the OID of its element.
        /// If the dotnet array has null values, the OutputArrayWithNull will be
        /// called.
        /// </summary>
        /// <param name="value">The dotnet array.</param>
        /// <returns> Returns a PostgreSQL array.</returns>
        public unsafe IntPtr OutputArray(Array value)
        {
            int nelems = value.Length;
            Array flatArray = Array.CreateInstance(this.GetNullableType(), nelems);
            flatArray.SetValue(null, 0);
            ArrayManipulation.FlatArray(value, ref flatArray);

            // flatArray variable is an one-dimensional array with the .NET types now
            int dimNumber = value.Rank;
            int[] dimLengths = new int[dimNumber];
            for (int i = 0; i < dimNumber; i++)
            {
                dimLengths[i] = value.GetLength(i);
            }

            for (int i = 0; i < nelems; i++)
            {
                if (flatArray.GetValue(i) == null)
                {
                    return this.OutputArrayWithNull(flatArray, dimLengths);
                }
            }

            IntPtr[] datums = new IntPtr[nelems]; // datums will be passed to C!
            for (int i = 0; i < nelems; i++)
            {
                T val = (T)flatArray.GetValue(i);
                datums[i] = this.OutputValue(val);
            }

            return ArrayHandler.pldotnet_CreateDatumArray((int)this.ElementOID, dimNumber, dimLengths, datums);
        }

        /// <summary>
        /// Converts a PostgreSQL array with null values in an Array object (dotnet style).
        /// This function is called from InputArray.
        /// </summary>
        /// <param name="datums">The flat list of datums.</param>
        /// <param name="dims">The size of each dimension.</param>
        /// <param name="nullmap">The PostgreSQL nullmap.</param>
        /// <returns> Returns an Array object (multidimensional or not).</returns>
        private Array InputArrayWithNull(IntPtr[] datums, int[] dims, byte[] nullmap)
        {
            int nelms = datums.Length;
            Array ret = Array.CreateInstance(this.GetNullableType(), nelms);

            for (int i = 0; i < nelms; i++)
            {
                bool isNull = NullMap.CheckNullValue(nullmap, i);
                ret.SetValue(isNull ? null : this.InputValue(datums[i]), i);
            }

            Array ret2 = Array.CreateInstance(this.GetNullableType(), dims);
            ArrayManipulation.ReshapeArray(ret, ref ret2);
            return ret2;
        }

        /// <summary>
        /// Creates a PostgreSQL array from an Array object with null values.
        /// This function is called from OutputArray.
        /// </summary>
        /// <param name="flatArray">The flat array with the dotnet values.</param>
        /// <param name="dims">The size of each dimension of the original dotnet array.</param>
        /// <returns> Returns a PostgreSQL array.</returns>
        private unsafe IntPtr OutputArrayWithNull(Array flatArray, int[] dims)
        {
            int nelems = flatArray.Length;
            byte[] nulls = new byte[nelems];
            IntPtr[] datums = new IntPtr[nelems];

            for (int i = 0; i < nelems; i++)
            {
                if (flatArray.GetValue(i) == null)
                {
                    nulls[i] = 1;
                    datums[i] = IntHandler.pldotnet_CreateDatumInt32(0);
                }
                else
                {
                    datums[i] = this.OutputValue((T)flatArray.GetValue(i));
                }
            }

            return ArrayHandler.pldotnet_CreateDatumArray((int)this.ElementOID, dims.Length, dims, datums, nulls);
        }
    }

    /// <summary>
    /// Base class for all classes which represent the data type handlers that use struct.
    /// </summary>
    /// <remarks>
    /// Use it for int, float, NpgsqlPoint, and other structs.
    /// </remarks>
    public abstract class StructTypeHandler<T> : BaseTypeHandler<T>
        where T : struct
    {
#nullable enable
        /// <summary>
        /// Check if the PostgreSQL datum is null. If the datum is null, it returns null.
        /// Otherwise, call the abstract method InputValue.
        /// </summary>
        /// <returns>
        /// A .NET type if the Datum is not null. Otherwise returns null.
        /// </returns>
        public T? InputNullableValue(IntPtr datum, bool isnull)
        {
            return isnull ? null : this.InputValue(datum);
        }

        /// <inheritdoc/>
        public override Type GetNullableType()
        {
            return typeof(T?);
        }

        /// <inheritdoc/>
        public override object InputArrayT(IntPtr datum, bool allowsNullElements = false)
        {
            if (!allowsNullElements)
            {
                try
                {
                    return base.InputArrayT(datum);
                }
                catch (InvalidCastException)
                {
                    throw new InvalidOperationException(
                        "Cannot read a non-nullable collection of elements because the returned array contains nulls. " +
                        "Call GetFieldValue with a nullable array instead.");
                }
                catch (Exception)
                {
                    throw;
                }
            }

            Array array = this.InputArray(datum);

            switch (array.Rank)
            {
                case 1:
                    int length0_1D = array.GetLength(0);
                    T?[] result1D = new T?[length0_1D];
                    for (int i = 0; i < length0_1D; i++)
                    {
                        object? value = array.GetValue(i);
                        if (value != null)
                        {
                            result1D[i] = (T)Convert.ChangeType(value, typeof(T));
                        }
                        else
                        {
                            result1D[i] = null;
                        }
                    }

                    return result1D;

                case 2:
                    int length0_2D = array.GetLength(0);
                    int length1_2D = array.GetLength(1);
                    T?[,] result2D = new T?[length0_2D, length1_2D];
                    for (int i = 0; i < length0_2D; i++)
                    {
                        for (int j = 0; j < length1_2D; j++)
                        {
                            object? value = array.GetValue(i, j);
                            if (value != null)
                            {
                                result2D[i, j] = (T)Convert.ChangeType(value, typeof(T));
                            }
                            else
                            {
                                result2D[i, j] = null;
                            }
                        }
                    }

                    return result2D;

                case 3:
                    int length0_3D = array.GetLength(0);
                    int length1_3D = array.GetLength(1);
                    int length2_3D = array.GetLength(2);
                    T?[,,] result3D = new T?[length0_3D, length1_3D, length2_3D];
                    for (int i = 0; i < length0_3D; i++)
                    {
                        for (int j = 0; j < length1_3D; j++)
                        {
                            for (int k = 0; k < length2_3D; k++)
                            {
                                object? value = array.GetValue(i, j, k);
                                if (value != null)
                                {
                                    result3D[i, j, k] = (T)Convert.ChangeType(value, typeof(T));
                                }
                                else
                                {
                                    result3D[i, j, k] = null;
                                }
                            }
                        }
                    }

                    return result3D;

                case 4:
                    int length0_4D = array.GetLength(0);
                    int length1_4D = array.GetLength(1);
                    int length2_4D = array.GetLength(2);
                    int length3_4D = array.GetLength(3);
                    T?[,,,] result4D = new T?[length0_4D, length1_4D, length2_4D, length3_4D];
                    for (int i = 0; i < length0_4D; i++)
                    {
                        for (int j = 0; j < length1_4D; j++)
                        {
                            for (int k = 0; k < length2_4D; k++)
                            {
                                for (int l = 0; l < length3_4D; l++)
                                {
                                    object? value = array.GetValue(i, j, k, l);
                                    if (value != null)
                                    {
                                        result4D[i, j, k, l] = (T)Convert.ChangeType(value, typeof(T));
                                    }
                                    else
                                    {
                                        result4D[i, j, k, l] = null;
                                    }
                                }
                            }
                        }
                    }

                    return result4D;

                case 5:
                    int length0_5D = array.GetLength(0);
                    int length1_5D = array.GetLength(1);
                    int length2_5D = array.GetLength(2);
                    int length3_5D = array.GetLength(3);
                    int length4_5D = array.GetLength(4);
                    T?[,,,,] result5D = new T?[length0_5D, length1_5D, length2_5D, length3_5D, length4_5D];
                    for (int i = 0; i < length0_5D; i++)
                    {
                        for (int j = 0; j < length1_5D; j++)
                        {
                            for (int k = 0; k < length2_5D; k++)
                            {
                                for (int l = 0; l < length3_5D; l++)
                                {
                                    for (int m = 0; m < length4_5D; m++)
                                    {
                                        object? value = array.GetValue(i, j, k, l, m);
                                        if (value != null)
                                        {
                                            result5D[i, j, k, l, m] = (T)Convert.ChangeType(value, typeof(T));
                                        }
                                        else
                                        {
                                            result5D[i, j, k, l, m] = null;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    return result5D;

                case 6:
                    int length0_6D = array.GetLength(0);
                    int length1_6D = array.GetLength(1);
                    int length2_6D = array.GetLength(2);
                    int length3_6D = array.GetLength(3);
                    int length4_6D = array.GetLength(4);
                    int length5_6D = array.GetLength(5);
                    T?[,,,,,] result6D = new T?[length0_6D, length1_6D, length2_6D, length3_6D, length4_6D, length5_6D];
                    for (int i = 0; i < length0_6D; i++)
                    {
                        for (int j = 0; j < length1_6D; j++)
                        {
                            for (int k = 0; k < length2_6D; k++)
                            {
                                for (int l = 0; l < length3_6D; l++)
                                {
                                    for (int m = 0; m < length4_6D; m++)
                                    {
                                        for (int n = 0; n < length5_6D; n++)
                                        {
                                            object? value = array.GetValue(i, j, k, l, m, n);
                                            if (value != null)
                                            {
                                                result6D[i, j, k, l, m, n] = (T)Convert.ChangeType(value, typeof(T));
                                            }
                                            else
                                            {
                                                result6D[i, j, k, l, m, n] = null;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    return result6D;

                default:
                    throw new System.Exception(
                        $"Undefined InputArrayT for arrays of {array.Rank} dimension{(array.Rank > 1 ? "s" : string.Empty)}.");
            }
        }

        /// <summary>
        /// Check if the PostgreSQL datum is null. If the Datum is null, it returns None.
        /// Otherwise, call the abstract method InputValue.
        /// </summary>
        /// <returns>
        /// A .NET type if the Datum is not null. Otherwise returns null.
        /// </returns>
        public FSharpOption<T> InputOptionValue(IntPtr datum, bool isnull)
        {
            return isnull ? FSharpOption<T>.None : FSharpOption<T>.Some(this.InputValue(datum));
        }

        /// <summary>
        /// Check if the .NET value is null. If the value is null, it returns a Datum(0).
        /// Otherwise, call the abstract method OutputValue.
        /// </summary>
        /// <returns>
        /// The suitable Datum if the .NET value is not null. Oherwise returns an Integer Datum.
        /// </returns>
        public IntPtr OutputNullableValue(T? value)
        {
            return value == null ? IntHandler.pldotnet_CreateDatumInt32(0) : this.OutputValue((T)value);
        }

        /// <summary>
        /// Check if the .NET value is null. If the value is null, it returns a Datum(0).
        /// Otherwise, call the abstract method OutputValue.
        /// </summary>
        /// <returns>
        /// The suitable Datum if the .NET value is not null. Oherwise returns an Integer Datum.
        /// </returns>
        public IntPtr OutputOptionValue(FSharpOption<T> value)
        {
            return (value == FSharpOption<T>.None) ? IntHandler.pldotnet_CreateDatumInt32(0) : this.OutputValue(value.Value);
        }

#nullable disable
    }

    /// <summary>
    /// Base class for all classes which represent the data type handlers that use class.
    /// </summary>
    /// <remarks>
    /// Use it for string, PhysicalAddress, BitArray, and other classes.
    ///
    /// It might appear as if this code is identical between
    /// StructTypeHandler and ObjectTypeHandler, but the meaning of
    /// T? is subtly different, so we did it this way on purpose.
    /// If you can successfully unify them, then we'd love to
    /// see a merge request, but tread carefully.  Also, it's
    /// harmless, so not super important.
    /// </remarks>
    public abstract class ObjectTypeHandler<T> : BaseTypeHandler<T>
        where T : class
    {
#nullable enable
        /// <summary>
        /// Check if the PostgreSQL datum is null. If the Datum is null, it returns null.
        /// Otherwise, call the abstract method InputValue.
        /// </summary>
        /// <returns>
        /// A .NET type if the Datum is not null. Otherwise returns null.
        /// </returns>
        public T? InputNullableValue(IntPtr datum, bool isnull)
        {
            return isnull ? null : this.InputValue(datum);
        }

        /// <inheritdoc/>
        public override Type GetNullableType()
        {
            return typeof(T);
        }

        public override object InputArrayT(IntPtr datum, bool allowsNullElements = false)
        {
            // By default, arrays of objects support null element, so use the default implementation.
            return base.InputArrayT(datum);
        }

        /// <summary>
        /// Check if the PostgreSQL datum is null. If the Datum is null, it returns None.
        /// Otherwise, call the abstract method InputValue.
        /// </summary>
        /// <returns>
        /// A .NET type if the Datum is not null. Otherwise returns null.
        /// </returns>
        public FSharpOption<T> InputOptionValue(IntPtr datum, bool isnull)
        {
            return isnull ? FSharpOption<T>.None : FSharpOption<T>.Some(this.InputValue(datum));
        }

        /// <summary>
        /// Check if the .NET value is null. If the value is null, it returns a Datum(0).
        /// Otherwise, call the abstract method OutputValue.
        /// </summary>
        /// <returns>
        /// The suitable Datum if the .NET value is not null. Oherwise returns an Integer Datum.
        /// </returns>
        public IntPtr OutputNullableValue(T? value)
        {
            return value == null ? IntHandler.pldotnet_CreateDatumInt32(0) : this.OutputValue(value!);
        }

        /// <summary>
        /// Check if the .NET value is null. If the value is null, it returns a Datum(0).
        /// Otherwise, call the abstract method OutputValue.
        /// </summary>
        /// <returns>
        /// The suitable Datum if the .NET value is not null. Oherwise returns an Integer Datum.
        /// </returns>
        public IntPtr OutputOptionValue(FSharpOption<T> value)
        {
            return
                (value == FSharpOption<T>.None)
                ? IntHandler.pldotnet_CreateDatumInt32(0)
                : this.OutputValue(value.Value);
        }

#nullable disable
    }

    /// <summary>
    ///
    /// </summary>
    public class OIDHandler : System.Attribute
    {
        public OID BaseType;
        public OID ArrayType;

        public OIDHandler(OID baseType, OID arrayType)
        {
            this.BaseType = baseType;
            this.ArrayType = arrayType;
        }
    }

    /// <summary>
    /// Provides a set of methods for setting the result datum of a user function to an output object.
    /// </summary>
    public class OutputResult
    {
        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_SetResult().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern unsafe int pldotnet_SetResult(IntPtr output, int offset, IntPtr value, bool isnull, uint oid);

        /// <summary>
        /// Sets the result datum of a user function to an output object.
        /// </summary>
        /// <param name="resultDatum">A pointer to the result datum to be set.</param>
        /// <param name="isNull">A value indicating whether the result datum is null. Set to `true` if
        /// the result datum is null, and `false` otherwise.</param>
        /// <param name="output">A pointer to the output object where the result datum will be set.</param>
        public static unsafe void SetDatumResult(IntPtr resultDatum, bool isNull, IntPtr output, int offset, uint oid)
        {
            if (pldotnet_SetResult(output, offset, resultDatum, isNull, oid) != 0)
            {
                throw new SystemException("Error received trying to set output datum");
            }
        }
    }
}
