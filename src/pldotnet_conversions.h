/*
 * PL/.NET (pldotnet) - PostgreSQL support for .NET C# and F# as
 *             procedural languages (PL)
 *
 *
 * Copyright (c) 2023 Brick Abode
 *
 * This code is subject to the terms of the PostgreSQL License.
 * The full text of the license can be found in the LICENSE file
 * at the top level of the pldotnet repository.
 *
 * pldotnet_conversion.h
 *
 */

#ifndef PLDOTNET_CONVERSIONS_H_
#define PLDOTNET_CONVERSIONS_H_

#include "pldotnet_main.h"
#include <utils/rangetypes.h>

////////////////////////////////////
/// Datum -> Npgsql or .NET type ///
////////////////////////////////////

/**
 * @brief Returns the int16_t value that corresponds to a PostgreSQL small
 * integer. It is used to convert from PostgreSQL type to .NET type.
 *
 * @param datum the datum object.
 * @return int16_t value.
 */
extern PGDLLEXPORT int16_t pldotnet_GetInt16(void *datum);

/**
 * @brief Returns the int32_t value that corresponds to a PostgreSQL integer. It
 * is used to convert from PostgreSQL type to .NET type.
 *
 * @param datum the datum object.
 * @return int32_t value.
 */
extern PGDLLEXPORT int32_t pldotnet_GetInt32(void *datum);

/**
 * @brief Returns the int64_t value that corresponds to a PostgreSQL big
 * integer. It is used to convert from PostgreSQL type to .NET type.
 *
 * @param datum the datum object.
 * @return int64_t value.
 */
extern PGDLLEXPORT int64_t pldotnet_GetInt64(void *datum);

/**
 * @brief Returns the float value that corresponds to a PostgreSQL float. It is
 * used to convert from PostgreSQL type to .NET type.
 *
 * @param datum the datum object.
 * @return float value.
 */
extern PGDLLEXPORT float pldotnet_GetFloat(void *datum);

/**
 * @brief Returns the float value that corresponds to a PostgreSQL double. It is
 * used to convert from PostgreSQL type to .NET type.
 *
 * @param datum the datum object.
 * @return double value.
 */
extern PGDLLEXPORT double pldotnet_GetDouble(void *datum);

/**
 * @brief Returns the boolean value that corresponds to a PostgreSQL boolean. It
 * is used to convert from PostgreSQL type to .NET type.
 *
 * @param datum the datum object.
 * @return bool value.
 */
extern PGDLLEXPORT bool pldotnet_GetBoolean(void *datum);

/**
 * @brief Extracts the x and y coordinates from a PostgreSQL Point and
 * writes them to the reference arguments `x` and `y`, so the caller has access
 * to them.
 *
 * @param datum the datum object.
 * @param x the x coordinate.
 * @param y the y coordinate.
 */
extern PGDLLEXPORT void pldotnet_GetDatumPointAttributes(void *datum, double *x,
                                                         double *y);

/**
 * @brief Extracts the `a`, `b`, and `c` values from a PostgreSQL Line and
 * writes them to the reference arguments `a`, `b`, and `c`, so the caller has
 * access to them.
 *
 * @param datum the datum object.
 * @param a the line parameter.
 * @param b the line parameter.
 * @param c the line parameter.
 */
extern PGDLLEXPORT void pldotnet_GetDatumLineAttributes(void *datum, double *a,
                                                        double *b, double *c);

/**
 * @brief Extracts the coordinates of the points that make up a PostgreSQL Line
 * Segment and writes them to the reference arguments `x1`, `y1`, `x2`, and
 * `y2`, so the caller has access to them.
 *
 * @param datum the datum object.
 * @param x1 the x coordinate of the first point.
 * @param y1 the y coordinate of the first point.
 * @param x2 the x coordinate of the second point.
 * @param y2 the y coordinate of the second point.
 */
extern PGDLLEXPORT void pldotnet_GetDatumLineSegmentAttributes(
    void *datum, double *x1, double *y1, double *x2, double *y2);
/**
 * @brief Extracts the coordinates of the points that make up a PostgreSQL Box
 * and writes them to the reference arguments `x1`, `y1`, `x2`, and `y2`, so the
 * caller has access to them.
 *
 * @param datum the datum object.
 * @param x1 the x coordinate of the upper right corner.
 * @param y1 the y coordinate of the upper right corner.
 * @param x2 the x coordinate of the lower left corner.
 * @param y2 the y coordinate of the lower left corner.
 */
extern PGDLLEXPORT void pldotnet_GetDatumBoxAttributes(void *datum, double *x1,
                                                       double *y1, double *x2,
                                                       double *y2);

/**
 * @brief Extracts the number of points and whether the PostgreSQL Path is
 * closed and writes them to the reference arguments `pointNumber` and `closed`,
 * so the caller has access to them.
 *
 * @param datum the datum object.
 * @param pointNumber is the number of points of the PostgreSQL Path.
 * @param closed whether the Path is closed or not.
 */
extern PGDLLEXPORT void pldotnet_GetDatumPathAttributes(void *datum,
                                                        int *pointNumber,
                                                        int *closed);

/**
 * @brief Extracts the coordinates of the points that make up a PostgreSQL Path
 * and writes them to arrays `xCoordinates` and `yCoordinates`, so the caller
 * has access to them.
 *
 * @param datum the datum object.
 * @param xCoordinates a pointer that references a double array with the x
 * coordinates.
 * @param yCoordinates a pointer that references a double array with the y
 * coordinates.
 */
extern PGDLLEXPORT void pldotnet_GetDatumPathCoordinates(void *datum,
                                                         double *xCoordinates,
                                                         double *yCoordinates);

/**
 * @brief Extracts the number of points of a PostgreSQL Polygon and writes it to
 * the reference arguments `pointNumber`, so the caller has access to them.
 *
 * @param datum the datum object.
 * @param pointNumber references number of points.
 */
extern PGDLLEXPORT void pldotnet_GetDatumPolygonAttributes(void *datum,
                                                           int *pointNumber);

/**
 * @brief Extracts the coordinates of the points that make up a PostgreSQL
 * Polygon and writes them to arrays `xCoordinates` and `yCoordinates`, so the
 * caller has access to them.
 *
 * @param datum the datum object.
 * @param xCoordinates the array with the x coordinates.
 * @param yCoordinates the array with the y coordinates.
 */
extern PGDLLEXPORT void pldotnet_GetDatumPolygonCoordinates(
    void *datum, double *xCoordinates, double *yCoordinates);

/**
 * @brief Extracts the coordinates of the center point and the radius of a
 * PostgreSQL Circle and writes them to the reference arguments `x`, `y`, and
 * `r`, so the caller has access to them.
 *
 * @param datum the datum object.
 * @param x the x coordinate of the center point.
 * @param y the y coordinate of the center point.
 * @param r the radius of the circle.
 */
extern PGDLLEXPORT void pldotnet_GetDatumCircleAttributes(void *datum,
                                                          double *x, double *y,
                                                          double *r);

/**
 * @brief Extracts the length and the pointer to the array character from a
 * PostgreSQL Text and writes them to the reference arguments `len` and `buf`,
 * so the caller has access to them.
 *
 * @param datum the datum object.
 * @param len the number of characters.
 * @param buf a pointer that will point to the data content (char*).
 */
extern PGDLLEXPORT void pldotnet_GetDatumTextAttributes(void *datum, int *len,
                                                        char **buf);

/**
 * @brief Extracts the length and the pointer to the array character from a
 * PostgreSQL Character (n) and writes them to the reference arguments `len` and
 * `buf`, so the caller has access to them.
 *
 * @param datum the datum object.
 * @param len the number of characters.
 * @param buf a pointer that will point to the data content (char*).
 */
extern PGDLLEXPORT void pldotnet_GetDatumCharAttributes(void *datum, int *len,
                                                        char **buf);

/**
 * @brief Extracts the length and the pointer to the array character from a
 * PostgreSQL Character Varying (n) and writes them to the reference arguments
 * `len` and `buf`, so the caller has access to them.
 *
 * @param datum the datum object.
 * @param len the number of characters.
 * @param buf a pointer that will point to the data content (char*).
 */
extern PGDLLEXPORT void pldotnet_GetDatumVarCharAttributes(void *datum,
                                                           int *len,
                                                           char **buf);

/**
 * @brief Extracts the length and the pointer to the array character from a
 * PostgreSQL Bytea and writes them to the reference arguments `len` and `buf`,
 * so the caller has access to them.
 *
 * @param datum the datum object.
 * @param len the number of characters.
 * @param buf a pointer that will point to the data content (char*).
 */
extern PGDLLEXPORT void pldotnet_GetDatumByteaAttributes(void *datum, int *len,
                                                         char **buf);

/**
 * @brief Extracts the length and the pointer to the array character from a
 * PostgreSQL Xml and writes them to the reference arguments `len` and `buf`,
 * so the caller has access to them.
 *
 * @param datum the datum object.
 * @param len the number of characters.
 * @param buf a pointer that will point to the data content (char*).
 */
extern PGDLLEXPORT void pldotnet_GetDatumXmlAttributes(void *datum, int *len,
                                                       char **buf);

/**
 * @brief Extracts the length and the pointer to the array character from a
 * PostgreSQL Json and writes them to the reference arguments `len` and `buf`,
 * so the caller has access to them.
 * @remark a PostgreSQL JSON is treated as text.
 *
 * @param datum the datum object.
 * @param len the number of characters.
 * @param buf a pointer that will point to the data content (char*).
 */
extern PGDLLEXPORT void pldotnet_GetDatumJsonAttributes(void *datum, int *len,
                                                        char **buf);

/**
 * @brief Extracts the integer value corresponding to a PostgreSQL Date and
 * writes it to the reference argument `date`, so the caller has access to them.
 *
 * @param datum the datum object.
 * @param date the int32 value that represents a Date in PostgreSQL.
 */
extern PGDLLEXPORT void pldotnet_GetDatumDateAttributes(void *datum, int *date);

/**
 * @brief Extracts the long value corresponding to a PostgreSQL Time and
 * writes it to the reference argument `time`, so the caller has access to them.
 *
 * @param datum the datum object.
 * @param time the int64 value that represents a Date in PostgreSQL.
 */
extern PGDLLEXPORT void pldotnet_GetDatumTimeAttributes(void *datum,
                                                        int64_t *time);

/**
 * @brief Extracts the long and integer values corresponding to a PostgreSQL
 * Time with time zone and writes them to the reference arguments `time` and
 * `zone`, so the caller has access to them.
 *
 * @param datum the datum object.
 * @param time is the int64 value that represents a Time in PostgreSQL.
 * @param zone the time zone.
 */
extern PGDLLEXPORT void pldotnet_GetDatumTimeTzAttributes(void *datum,
                                                          int64_t *time,
                                                          int *zone);

/**
 * @brief Extracts the long value corresponding to a PostgreSQL Timestamp and
 * writes it to the reference argument `timestamp`, so the caller has access to
 * them.
 *
 * @param datum the datum object.
 * @param timestamp is the int64 value that represents a Timestamp in
 * PostgreSQL.
 */
extern PGDLLEXPORT void pldotnet_GetDatumTimestampAttributes(void *datum,
                                                             int64_t *timestamp);

/**
 * @brief Extracts the long value corresponding to a PostgreSQL Timestamp with
 * time zone and writes it to the reference argument `timestamp`, so the caller
 * has access to them.
 *
 * @param datum the datum object.
 * @param timestamp is the int64 value that represents a Timestamp in
 * PostgreSQL.
 */
extern PGDLLEXPORT void pldotnet_GetDatumTimestampTzAttributes(void *datum,
                                                               int64_t *timestamp);

/**
 * @brief Extracts the time (number of ticks), day, and moth values from a
 * PostgreSQL Interval and writes them to the reference arguments `time`, `day`,
 * and `month`, so the caller has access to them.
 *
 * @param datum the datum object.
 * @param time the ticks of time.
 * @param day the day.
 * @param month the month.
 */
extern PGDLLEXPORT void pldotnet_GetDatumIntervalAttributes(void *datum,
                                                            int64_t *time,
                                                            int *day,
                                                            int *month);

/**
 * @brief Extracts the unsigned char values from a PostgreSQL MAC Address and
 * writes them to the array argument `bytes` according to the provided length (6
 * or 8), so the caller has access to them.
 *
 * @param datum the datum object.
 * @param length the MAC address length.
 * @param bytes an array with the byte value that makes up the MAC address.
 */
extern PGDLLEXPORT void pldotnet_GetDatumMacAddressAttributes(
    void *datum, int length, unsigned char *bytes);

/**
 * @brief Extracts the IPv4 or IPv6 information from a PostgreSQL INET or CIDR
 * and writes it to the reference arguments `nelem` (4 for IPv4 or 16 for IPv6),
 * `bytes` (the array in which the IP values will be written), and `netmask`
 * (the number of network bits present in the host address).
 *
 * @param datum the datum object.
 * @param nelem the number of elements.
 * @param bytes the array with byte values.
 * @param netmask the number of bits in the netmask.
 */
extern PGDLLEXPORT void pldotnet_GetDatumInetAttributes(void *datum, int *nelem,
                                                        unsigned char *bytes,
                                                        int *netmask);

/**
 * @brief Extracts the long value corresponding to a PostgreSQL Money and writes
 * it to the reference argument `value`, so the caller has access to them.
 *
 * @param datum the datum object.
 * @param value is the long integer that represents the PostgreSQL Money.
 */
extern PGDLLEXPORT void pldotnet_GetDatumMoneyAttributes(void *datum,
                                                         int64_t *value);

/**
 * @brief Extracts the length and the memory address where the bits of a
 * PostgreSQL Bit or VarBit are stored and writes them to the reference
 * arguments `len` and `dat` (a pointer to pointer), so the caller has access to
 * them.
 *
 * @param datum the datum object.
 * @param len the length of the bit string.
 * @param dat an unsigned char pointer pointing to the beginning of an array.
 */
extern PGDLLEXPORT void pldotnet_GetDatumVarBitAttributes(void *datum, int *len,
                                                          bits8 **dat);

/**
 * @brief Extracts the unsigned char values that make up a PostgreSQL UUID and
 * writes them to the array `data`, so the caller has access to them.
 *
 * @param datum the datum object.
 * @param data a pointer that references an array of unsigned char.
 */
extern PGDLLEXPORT void pldotnet_GetDatumUuidAttributes(void *datum,
                                                        unsigned char *data);

/**
 * @brief Extracts the information from a PostgreSQL Array and writes it to the
 * reference arguments `typeId`, `nDims`, `dims`, and `nullmap`, so the caller
 * has access to them.
 *
 * @param datum the datum object.
 * @param typeId references the OID of the elements.
 * @param nDims references the number of dimensions.
 * @param dims the array with the lengths of each dimension.
 * @param nullmap a pointer that references the nullmap of a PostgreSQL array.
 */
extern PGDLLEXPORT void pldotnet_GetArrayAttributes(void *datum, int *typeId,
                                                    int *nDims, int *dims,
                                                    uint8_t **nullmap);

/**
 * @brief Get a maximum number of dimensions of a PostgreSQL Array.
 *
 * @return the maximum number of dimensions of a PostgreSQL Array.
 */
extern PGDLLEXPORT int get_Maxdim(void);

/**
 * @brief Extracts the elements from a PostgreSQL Array and writes them to the
 * reference argument `results`, so the caller has access to them.
 *
 * @param arrayDatum the PostgreSQL Array.
 * @param results a pointer that references an array of Datum object.
 * @param nElems the number of elements.
 * @param typeId the OID of the elements.
 * @return int an integer to control errors: 0 on success, other on failure.
 */
extern PGDLLEXPORT int pldotnet_GetArrayDatum(Datum arrayDatum, Datum *results,
                                              int nElems, int typeId);

/**
 * @brief Extracts the information from a PostgreSQL Range and writes it to the
 * reference arguments `isEmpty`, `lowerRange`, and `upperDange`, so the caller
 * has access to them.
 *
 * @param inputDatum the datum object.
 * @param isEmpty whether the range is empty.
 * @param lowerRange a pointer that references the lower RangeBound object.
 * @param upperDange a pointer that references the upper RangeBound object.
 */
extern PGDLLEXPORT void pldotnet_GetDatumRangeAttributes(
    Datum inputDatum, bool *isEmpty, RangeBound **lowerRange,
    RangeBound **upperDange);

/**
 * @brief Extracts the information from the provided RangeBound object and
 * writes it to the reference arguments `rangeDatum`, `infinite`, `inclusive`,
 * and `lower`, so the caller has access to them.
 *
 * @param inputRange the RangeBound object.
 * @param rangeDatum the Datum object of the provided RangeBound.
 * @param infinite whether the range bound is infinite.
 * @param inclusive whether the range bound is inclusive.
 * @param lower whether the range bound is lower.
 */
extern PGDLLEXPORT void pldotnet_GetDatumRangeBoundAttributes(
    RangeBound *inputRange, Datum *rangeDatum, bool *infinite, bool *inclusive,
    bool *lower);

/**
 * @brief Get the number of attributes in a record.
 *
 * This function returns the number of attributes in a PostgreSQL record.
 *
 * @param recordDatum The Datum representing the record.
 * @return The number of attributes in the record.
 */
extern PGDLLEXPORT int pldotnet_GetNumberOfRecordAttributes(Datum recordDatum);

/**
 * @brief Retrieves the attribute value from a record datum.
 *
 * This function is used to extract the value of a specific attribute from a
 * record datum.
 *
 * @param recordDatum The record datum from which to extract the attribute
 * value.
 * @param attributeNumber The number of the attribute to retrieve.
 * @param value Pointer to store the retrieved attribute value.
 * @param isnull Pointer to store whether the attribute value is NULL or not.
 * @param typeId Pointer to store the OID of the attribute type.
 */
extern PGDLLEXPORT int pldotnet_GetRecordAttributes(Datum recordDatum,
                                                   int numAttrs,
                                                   Datum *attrDatums,
                                                   bool *isNulls,
                                                   Oid *typeOids);

////////////////////////////////////
/// Npgsql or .NET type -> Datum ///
////////////////////////////////////

/**
 * @brief Creates a PostgreSQL small integer from an int16_t value. It is used
 * to convert from a .NET type to a PostgreSQL Datum.
 *
 * @param value
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumInt16(int16_t value);

/**
 * @brief Creates a PostgreSQL integer from an int32_t value. It is used to
 * convert from a .NET type to a PostgreSQL Datum.
 *
 * @param value
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumInt32(int32_t value);

/**
 * @brief Creates a PostgreSQL big integer from an int64_t value. It is used to
 * convert from a .NET type to a PostgreSQL Datum.
 *
 * @param value
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumInt64(int64_t value);

/**
 * @brief Creates a PostgreSQL float from a float value. It is used to convert
 * from a .NET type to a PostgreSQL Datum.
 *
 * @param value
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumFloat(float value);

/**
 * @brief Creates a PostgreSQL double from a double value. It is used to convert
 * from a .NET type to a PostgreSQL Datum.
 *
 * @param value
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumDouble(double value);

/**
 * @brief Creates a PostgreSQL double from a double. It is used to convert from
 * a .NET type to a PostgreSQL Datum.
 *
 * @param value
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumBoolean(bool value);

/**
 * @brief Creates a PostgreSQL Point from its coordinates. It is used to convert
 * from a .NET type to a PostgreSQL Datum.
 *
 * @param x
 * @param y
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumPoint(double x, double y);

/**
 * @brief Creates a PostgreSQL Line from its parameters. It is used to convert
 * from a .NET type to a PostgreSQL Datum.
 *
 * @param a
 * @param b
 * @param c
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumLine(double a, double b, double c);

/**
 * @brief Creates a PostgreSQL Line Segment from its point coordinates. It is
 * used to convert from a .NET type to a PostgreSQL Datum.
 *
 * @param x1 the x coordinate of the first point.
 * @param y1 the y coordinate of the first point.
 * @param x2 the x coordinate of the second point.
 * @param y2 the y coordinate of the second point.
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumLineSegment(double x1, double y1,
                                                         double x2, double y2);

/**
 * @brief Creates a PostgreSQL Box from its point coordinates. It is used to
 * convert from a .NET type to a PostgreSQL Datum.
 *
 * @param x1 the x coordinate of the upper right corner.
 * @param y1 the y coordinate of the upper right corner.
 * @param x2 the x coordinate of the lower left corner.
 * @param y2 the y coordinate of the lower left corner.
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumBox(double x1, double y1,
                                                 double x2, double y2);

/**
 * @brief Creates a PostgreSQL Path from its point coordinates. It is used to
 * convert from a .NET type to a PostgreSQL Datum.
 *
 * @param npts the number of points.
 * @param closed whether the path is closed or not
 * @param xCoordinates the array with the x coordinate of the points.
 * @param yCoordinates the array with the y coordinate of the points.
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumPath(int npts, int closed,
                                                  double *xCoordinates,
                                                  double *yCoordinates);

/**
 * @brief Creates a PostgreSQL Polygon from its point coordinates. It is used to
 * convert from a .NET type to a PostgreSQL Datum.
 *
 * @param npts the number of points.
 * @param xCoordinates the array with the x coordinate of the points.
 * @param yCoordinates the array with the y coordinate of the points.
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumPolygon(int npts,
                                                     double *xCoordinates,
                                                     double *yCoordinates);

/**
 * @brief Creates a PostgreSQL Circle from its properties. It is used to convert
 * from a .NET type to a PostgreSQL Datum.
 *
 * @param x the x coordinate of the center point.
 * @param y the y coordinate of the center point.
 * @param r the radius of the circle.
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumCircle(double x, double y,
                                                    double r);

/**
 * @brief Creates a PostgreSQL Text. It is used to convert from a .NET type to a
 * PostgreSQL Datum.
 *
 * @param len the number of characters.
 * @param buf the array with byte values.
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumText(int len, char *buf);

/**
 * @brief Creates a PostgreSQL Character(n). It is used to convert from a .NET
 * type to a PostgreSQL Datum.
 *
 * @param len the number of characters.
 * @param buf the array with byte values.
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumChar(int len, char *buf);

/**
 * @brief Creates a PostgreSQL Character Varying(n). It is used to convert from
 * a .NET type to a PostgreSQL Datum.
 *
 * @param len the number of characters.
 * @param buf the array with byte values.
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumVarChar(int len, char *buf);

/**
 * @brief Creates a PostgreSQL Bytea. It is used to convert from a .NET type to
 * a PostgreSQL Datum.
 *
 * @param len the number of characters.
 * @param buf the array with byte values.
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumBytea(int len, char *buf);

/**
 * @brief Creates a PostgreSQL Xml. It is used to convert from a .NET type to a
 * PostgreSQL Datum.
 *
 * @param len the number of characters.
 * @param buf the array with byte values.
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumXml(int len, char *buf);

/**
 * @brief Creates a PostgreSQL JSON. It is used to convert from a .NET type to a
 * PostgreSQL Datum.
 * @remark a PostgreSQL JSON is treated as text.
 *
 * @param len the number of characters.
 * @param buf the array with byte values.
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumJson(int len, char *buf);

/**
 * @brief Creates a PostgreSQL Date. It is used to convert from a .NET type to a
 * PostgreSQL Datum.
 *
 * @param date the integer value that represents a Date in PostgreSQL.
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumDate(int date);

/**
 * @brief Creates a PostgreSQL Time. It is used to convert from a .NET type to a
 * PostgreSQL Datum.
 *
 * @param time the long integer value that represents a Time in PostgreSQL.
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumTime(int64_t time);

/**
 * @brief Creates a PostgreSQL Time with the time zone. It is used to convert
 * from a .NET type to a PostgreSQL Datum.
 *
 * @param time is the long integer value that represents a Time in PostgreSQL.
 * @param zone the zone value.
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumTimeTz(int64_t time, int zone);

/**
 * @brief Creates a PostgreSQL Timestamp without a time zone. It is used to
 * convert from a .NET type to a PostgreSQL Datum.
 *
 * @param timestamp the long integer value that represents a Timestamp in
 * PostgreSQL.
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumTimestamp(int64_t timestamp);

/**
 * @brief Creates a PostgreSQL Timestamp with the time zone. It is used to
 * convert from a .NET type to a PostgreSQL Datum.
 *
 * @param timestamp the long integer value that represents a Timestamp in
 * PostgreSQL.
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumTimestampTz(int64_t timestamp);

/**
 * @brief Creates a PostgreSQL Interval. It is used to convert from a .NET type
 * to a PostgreSQL Datum.
 *
 * @param time the long integer value that represents a time in PostgreSQL.
 * @param day the number of days.
 * @param month the number of months.
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumInterval(int64_t time, int day,
                                                      int month);

/**
 * @brief Creates a PostgreSQL MAC Address. It is used to convert from a .NET
 * type to a PostgreSQL Datum.
 *
 * @param length the length of the MAC address; it must be 6 or 8.
 * @param bytes the array with byte values.
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumMacAddress(int length,
                                                        unsigned char *bytes);

/**
 * @brief Creates a PostgreSQL INET. It is used to convert from a .NET type to a
 * PostgreSQL Datum.
 *
 * @param length the number of elements.
 * @param bytes the array with byte values.
 * @param netmask the number of bits in the netmask.
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumInet(int length,
                                                  unsigned char *bytes,
                                                  int netmask);

/**
 * @brief Creates PostgreSQL Money. It is used to convert from a .NET type to
 * a PostgreSQL Datum.
 *
 * @param value the long integer that represents the money in PostgreSQL
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumMoney(int64_t value);

/**
 * @brief Creates a PostgreSQL VarBit or Bit. It is used to convert from a .NET
 * type to a PostgreSQL Datum.
 *
 * @param len the number of elements.
 * @param dat the array with byte values.
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumVarBit(int len, bits8 *dat);

/**
 * @brief Creates a PostgreSQL UUID. It is used to convert from a .NET type to a
 * PostgreSQL Datum.
 *
 * @param data the array with the UUID values.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumUuid(unsigned char *data);

/**
 * @brief Creates an empty PostgreSQL range. It is used to convert from a .NET
 * type to a PostgreSQL Datum.
 *
 * @param rangeTypeId the Oid of the range.
 * @return Datum the empty datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateEmptyDatumRange(Oid rangeTypeId);

/**
 * @brief Creates a PostgreSQL range according to the provided Oid. It is used
 * to convert from a .NET type to a PostgreSQL Datum.
 *
 * @param rtOid
 * @param lowerDatum
 * @param lowerInfinite
 * @param lowerInclusive
 * @param upperDatum
 * @param upperInfinite
 * @param upperInclusive
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumRange(
    Oid rtOid, Datum lowerDatum, bool lowerInfinite, bool lowerInclusive,
    Datum upperDatum, bool upperInfinite, bool upperInclusive);

/**
 * @brief Creates a PostgreSQL Array of the specified type. It is used to
 * convert from a .NET type to a PostgreSQL Datum.
 *
 * @param elementId the OID of the elements.
 * @param dimNumber the number of dimensions.
 * @param dimLengths the array with the lengths in each dimension.
 * @param datums the flat array with the datum objects.
 * @param nulls the array that maps the null values of the datums variable.
 * @return Datum the datum object.
 */
extern PGDLLEXPORT Datum pldotnet_CreateDatumArray(int elementId, int dimNumber,
                                                   int *dimLengths,
                                                   Datum *datums, bool *nulls);

#endif  // PLDOTNET_CONVERSIONS_H_
