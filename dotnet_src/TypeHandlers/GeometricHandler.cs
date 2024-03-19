// <copyright file="GeometricHandler.cs" company="Brick Abode">
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
    /// A type handler for the PostgreSQL point data type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/static/datatype-geometric.html.
    /// </remarks>
    [OIDHandler(OID.POINTOID, OID.POINTARRAYOID)]
    public class PointHandler : StructTypeHandler<NpgsqlPoint>
    {
        public PointHandler()
        {
            this.ElementOID = OID.POINTOID;
            this.ArrayOID = OID.POINTARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumPointAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern void pldotnet_GetDatumPointAttributes(IntPtr datum, ref double x, ref double y);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumPoint().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumPoint(double x, double y);

        /// <inheritdoc />
        public override NpgsqlPoint InputValue(IntPtr datum)
        {
            double x = 0.0, y = 0.0;
            pldotnet_GetDatumPointAttributes(datum, ref x, ref y);
            return new NpgsqlPoint(x, y);
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(NpgsqlPoint value)
        {
            return pldotnet_CreateDatumPoint(value.X, value.Y);
        }
    }

    /// <summary>
    /// A type handler for the PostgreSQL line data type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/static/datatype-geometric.html.
    /// </remarks>
    [OIDHandler(OID.LINEOID, OID.LINEARRAYOID)]
    public class LineHandler : StructTypeHandler<NpgsqlLine>
    {
        public LineHandler()
        {
            this.ElementOID = OID.LINEOID;
            this.ArrayOID = OID.LINEARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumLineAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern void pldotnet_GetDatumLineAttributes(IntPtr datum, ref double a, ref double b, ref double c);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumLine().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumLine(double a, double b, double c);

        /// <inheritdoc />
        public override NpgsqlLine InputValue(IntPtr datum)
        {
            double a = 0.0, b = 0.0, c = 0.0;
            pldotnet_GetDatumLineAttributes(datum, ref a, ref b, ref c);
            return new NpgsqlLine(a, b, c);
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(NpgsqlLine value)
        {
            return pldotnet_CreateDatumLine(value.A, value.B, value.C);
        }
    }

    /// <summary>
    /// A type handler for the PostgreSQL lseg data type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/static/datatype-geometric.html.
    /// </remarks>
    [OIDHandler(OID.LSEGOID, OID.LSEGARRAYOID)]
    public class LineSegmentHandler : StructTypeHandler<NpgsqlLSeg>
    {
        public LineSegmentHandler()
        {
            this.ElementOID = OID.LSEGOID;
            this.ArrayOID = OID.LSEGARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumLineSegmentAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern void pldotnet_GetDatumLineSegmentAttributes(IntPtr datum, ref double x1, ref double y1, ref double x2, ref double y2);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumLineSegment().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumLineSegment(double x1, double y1, double x2, double y2);

        /// <inheritdoc />
        public override NpgsqlLSeg InputValue(IntPtr datum)
        {
            double x1 = 0.0, y1 = 0.0, x2 = 0.0, y2 = 0.0;
            pldotnet_GetDatumLineSegmentAttributes(datum, ref x1, ref y1, ref x2, ref y2);
            return new NpgsqlLSeg(x1, y1, x2, y2);
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(NpgsqlLSeg value)
        {
            return pldotnet_CreateDatumLineSegment(value.Start.X, value.Start.Y, value.End.X, value.End.Y);
        }
    }

    /// <summary>
    /// A type handler for the PostgreSQL box data type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/static/datatype-geometric.html.
    /// </remarks>
    [OIDHandler(OID.BOXOID, OID.BOXARRAYOID)]
    public class BoxHandler : StructTypeHandler<NpgsqlBox>
    {
        public BoxHandler()
        {
            this.ElementOID = OID.BOXOID;
            this.ArrayOID = OID.BOXARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumBoxAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern void pldotnet_GetDatumBoxAttributes(IntPtr datum, ref double x1, ref double y1, ref double x2, ref double y2);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumBox().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumBox(double x1, double y1, double x2, double y2);

        /// <inheritdoc />
        public override NpgsqlBox InputValue(IntPtr datum)
        {
            double x1 = 0.0, y1 = 0.0, x2 = 0.0, y2 = 0.0;
            pldotnet_GetDatumBoxAttributes(datum, ref x1, ref y1, ref x2, ref y2);
            NpgsqlPoint upperRight = new (x1, y1);
            NpgsqlPoint lowerLeft = new (x2, y2);
            return new NpgsqlBox(upperRight, lowerLeft);
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(NpgsqlBox value)
        {
            var x1 = Math.Max(value.UpperRight.X, value.LowerLeft.X);
            var y1 = Math.Max(value.UpperRight.Y, value.LowerLeft.Y);
            var x2 = Math.Min(value.UpperRight.X, value.LowerLeft.X);
            var y2 = Math.Min(value.UpperRight.Y, value.LowerLeft.Y);
            return pldotnet_CreateDatumBox(x1, y1, x2, y2);
        }
    }

    /// <summary>
    /// A type handler for the PostgreSQL path data type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/static/datatype-geometric.html.
    /// </remarks>
    [OIDHandler(OID.PATHOID, OID.PATHARRAYOID)]
    public class PathHandler : StructTypeHandler<NpgsqlPath>
    {
        public PathHandler()
        {
            this.ElementOID = OID.PATHOID;
            this.ArrayOID = OID.PATHARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumPathAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern void pldotnet_GetDatumPathAttributes(IntPtr datum, ref int pointNumber, ref int closed);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumPathCoordinates().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern void pldotnet_GetDatumPathCoordinates(IntPtr datum, double[] xCoordinates, double[] yCoordinates);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumPath().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumPath(int npts, int closed, double[] xCoordinates, double[] yCoordinates);

        /// <inheritdoc />
        public override NpgsqlPath InputValue(IntPtr datum)
        {
            int npts = 0, closed = 0;
            pldotnet_GetDatumPathAttributes(datum, ref npts, ref closed);
            double[] xCoordinates = new double[npts];
            double[] yCoordinates = new double[npts];
            bool open = closed == 0;
            NpgsqlPath origPath = new (npts, open);
            pldotnet_GetDatumPathCoordinates(datum, xCoordinates, yCoordinates);
            for (int i = 0; i < npts; i++)
            {
                origPath.Add(new NpgsqlPoint(xCoordinates[i], yCoordinates[i]));
            }

            return origPath;
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(NpgsqlPath value)
        {
            int npts = value.Count;
            bool open = value.Open;
            int closed = open ? 0 : 1;
            double[] xCoordinates = new double[npts];
            double[] yCoordinates = new double[npts];
            for (int i = 0; i < npts; i++)
            {
                xCoordinates[i] = value[i].X;
                yCoordinates[i] = value[i].Y;
            }

            return pldotnet_CreateDatumPath(npts, closed, xCoordinates, yCoordinates);
        }
    }

    /// <summary>
    /// A type handler for the PostgreSQL polygon data type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/static/datatype-geometric.html.
    /// </remarks>
    [OIDHandler(OID.POLYGONOID, OID.POLYGONARRAYOID)]
    public class PolygonHandler : StructTypeHandler<NpgsqlPolygon>
    {
        public PolygonHandler()
        {
            this.ElementOID = OID.POLYGONOID;
            this.ArrayOID = OID.POLYGONARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumPolygonAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern void pldotnet_GetDatumPolygonAttributes(IntPtr datum, ref int pointNumber);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumPolygonCoordinates().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern void pldotnet_GetDatumPolygonCoordinates(IntPtr datum, double[] xCoordinates, double[] yCoordinates);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumPolygon().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumPolygon(int npts, double[] xCoordinates, double[] yCoordinates);

        /// <inheritdoc />
        public override NpgsqlPolygon InputValue(IntPtr datum)
        {
            int npts = 0;
            pldotnet_GetDatumPolygonAttributes(datum, ref npts);
            double[] xCoordinates = new double[npts];
            double[] yCoordinates = new double[npts];
            NpgsqlPolygon origPolygon = new (npts);
            pldotnet_GetDatumPolygonCoordinates(datum, xCoordinates, yCoordinates);
            for (int i = 0; i < npts; i++)
            {
                origPolygon.Add(new NpgsqlPoint(xCoordinates[i], yCoordinates[i]));
            }

            return origPolygon;
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(NpgsqlPolygon value)
        {
            int npts = value.Count;
            double[] xCoordinates = new double[npts];
            double[] yCoordinates = new double[npts];
            for (int i = 0; i < npts; i++)
            {
                xCoordinates[i] = value[i].X;
                yCoordinates[i] = value[i].Y;
            }

            return pldotnet_CreateDatumPolygon(npts, xCoordinates, yCoordinates);
        }
    }

    /// <summary>
    /// A type handler for the PostgreSQL circle data type.
    /// </summary>
    /// <remarks>
    /// See https://www.postgresql.org/docs/current/static/datatype-geometric.html.
    /// </remarks>
    [OIDHandler(OID.CIRCLEOID, OID.CIRCLEARRAYOID)]
    public class CircleHandler : StructTypeHandler<NpgsqlCircle>
    {
        public CircleHandler()
        {
            this.ElementOID = OID.CIRCLEOID;
            this.ArrayOID = OID.CIRCLEARRAYOID;
        }

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_GetDatumCircleAttributes().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern void pldotnet_GetDatumCircleAttributes(IntPtr datum, ref double x, ref double y, ref double r);

        /// <summary>
        /// C function declared in pldotnet_conversions.h.
        /// See ::pldotnet_CreateDatumCircle().
        /// </summary>
        [DllImport("@PKG_LIBDIR/pldotnet.so")]
        public static extern IntPtr pldotnet_CreateDatumCircle(double x, double y, double r);

        /// <inheritdoc />
        public override NpgsqlCircle InputValue(IntPtr datum)
        {
            double x = 0.0, y = 0.0, r = 0.0;
            pldotnet_GetDatumCircleAttributes(datum, ref x, ref y, ref r);
            return new NpgsqlCircle(x, y, r);
        }

        /// <inheritdoc />
        public override IntPtr OutputValue(NpgsqlCircle value)
        {
            return pldotnet_CreateDatumCircle(value.Center.X, value.Center.Y, value.Radius);
        }
    }
}