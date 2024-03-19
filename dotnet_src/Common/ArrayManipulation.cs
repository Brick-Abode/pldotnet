// <copyright file="ArrayManipulation.cs" company="Brick Abode">
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

namespace PlDotNET.Common
{
    /// <summary>
    /// A generic class for all type handlers which handle PostreSQL arrays.
    /// </summary>
    public static class ArrayManipulation
    {
        /// <summary>
        /// This is a recursive function that aims to create a multidimensional
        /// array from an one-dimensional one. The dimensions of the
        /// multidimensional array must be specified when you create the "Array"
        /// object that you will pass for "multiArray". For that purpose, you
        /// can use the "Array.CreateInstance()" method. Also, to use this
        /// function, you should pass only the first two arguments.
        /// </summary>
        /// <param name="originalArray">The original one-dimensional array.</param>
        /// <param name="multiArray"> It is an empty array with the new
        /// dimensions, which must be passed by reference, so the variable you
        /// pass here will be modified to store the elements of the "originalArray".
        /// </param>
        /// <param name="auxiliar"> This one is only an integer array that helps
        /// to set the values to the new multidimensional array. You can pass
        /// an empty array with with length equal to the number of dimensions.
        /// However, you don't need. </param>
        /// <param name="contEl"> This one is an element counter, so you can pass
        /// 0, but you don't need. </param>
        /// <param name="loc"> This one is an auxiliary integer that helps to
        /// loop over the dimensions of the new array, so you can pass 1, but
        /// you don't need. </param>
        /// <returns> Returns the number of elements of the originalArray. </returns>
        public static int ReshapeArray(Array originalArray, ref Array multiArray, int[] auxiliar = null, int contEl = 0, int loc = 1)
        {
            if (contEl >= originalArray.Length)
            {
                return contEl;
            }
            else if (contEl == 0 || auxiliar == null)
            {
                auxiliar = new int[multiArray.Rank];
            }

            int ndim = multiArray.Rank;
            int[] dim = new int[ndim];
            for (int i = 0; i < ndim; i++)
            {
                dim[i] = multiArray.GetLength(i);
            }

            if (loc == 1)
            {
                for (int i = 0; i < dim[ndim - loc]; i++)
                {
                    multiArray.SetValue(originalArray.GetValue(contEl++), auxiliar);
                    auxiliar[ndim - loc] += 1;
                }
            }

            for (int i = 1; i < loc; i++)
            {
                auxiliar[ndim - loc] += 1;
                if (auxiliar[ndim - loc] < dim[ndim - loc])
                {
                    contEl = ReshapeArray(originalArray, ref multiArray, auxiliar, contEl, i);
                }
            }

            auxiliar[ndim - loc] = 0;
            contEl = ReshapeArray(originalArray, ref multiArray, auxiliar, contEl, ++loc);
            return contEl;
        }

        /// <summary>
        /// This is a recursive function that aims to create an one-dimensional
        /// array from a multidimensional one. The "flatArray" must be created
        /// with the same length as the "originalArray", and you can use the
        /// "Array.CreateInstance()" method for that.
        /// Also, to use this function, you should pass only the first two arguments.
        /// </summary>
        /// <param name="originalArray">The original multidimensional dimensional array.</param>
        /// <param name="flatArray"> It is an empty one-dimensional array with
        /// length equals to the "originalArray". This argument must be
        /// passed by reference, so the variable you pass here will be modified
        /// to store the elements of the "originalArray".</param>
        /// <param name="auxiliar"> This one is only an integer array that helps
        /// to set the values to the new flat array. You can pass an empty array
        /// with with length equal to the number of dimensions of the originalArray.
        /// However, you don't need. </param>
        /// <param name="contEl"> This one is an element counter, so you can pass
        /// 0, but you don't need. </param>
        /// <param name="loc"> This one is an auxiliary integer that helps to
        /// loop over the dimensions of the new array, so you can pass 1, but
        /// you don't need. </param>
        /// <returns> Returns the number of elements of the originalArray. </returns>
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
    }
}