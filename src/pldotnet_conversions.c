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
 * pldotnet_conversion.c
 *
 */

#include "pldotnet_conversions.h"

#include <utils/array.h>
#include <utils/cash.h>
#include <utils/date.h>
#include <utils/geo_decls.h>
#include <sys/socket.h>
#include <utils/inet.h>
#include <utils/lsyscache.h>
#include <utils/numeric.h>
#include <utils/rangetypes.h>
#include <utils/timestamp.h>
#include <utils/varbit.h>
#include <utils/xml.h>
#include <utils/uuid.h>
#include <utils/json.h>

////////////////////////////////////
/// Datum -> Npgsql or .NET type ///
////////////////////////////////////

int16_t pldotnet_GetInt16(void *datum) {
    int16_t value = DatumGetInt16((Datum)datum);
    return value;
}

int32_t pldotnet_GetInt32(void *datum) {
    int32_t value = DatumGetInt32((Datum)datum);
    return value;
}

int64_t pldotnet_GetInt64(void *datum) {
    int64_t value = DatumGetInt64((Datum)datum);
    return value;
}

float pldotnet_GetFloat(void *datum) {
    float value = DatumGetFloat4((Datum)datum);
    return value;
}

double pldotnet_GetDouble(void *datum) {
    double value = DatumGetFloat8((Datum)datum);
    return value;
}

bool pldotnet_GetBoolean(void *datum) {
    bool value = DatumGetBool((Datum)datum);
    return value;
}

void pldotnet_GetDatumPointAttributes(void *datum, double *x, double *y) {
    Point *orig_p = DatumGetPointP((Datum)datum);
    *x = orig_p->x;
    *y = orig_p->y;
}

void pldotnet_GetDatumLineAttributes(void *datum, double *a, double *b,
                                     double *c) {
    LINE *orig_l = DatumGetLineP((Datum)datum);
    *a = orig_l->A;
    *b = orig_l->B;
    *c = orig_l->C;
}

void pldotnet_GetDatumLineSegmentAttributes(void *datum, double *x1, double *y1,
                                            double *x2, double *y2) {
    LSEG *orig_l = DatumGetLsegP((Datum)datum);
    *x1 = orig_l->p[0].x;
    *y1 = orig_l->p[0].y;
    *x2 = orig_l->p[1].x;
    *y2 = orig_l->p[1].y;
}

void pldotnet_GetDatumBoxAttributes(void *datum, double *x1, double *y1,
                                    double *x2, double *y2) {
    BOX *orig_b = DatumGetBoxP((Datum)datum);
    *x1 = orig_b->high.x;
    *y1 = orig_b->high.y;
    *x2 = orig_b->low.x;
    *y2 = orig_b->low.y;
}

void pldotnet_GetDatumPathAttributes(void *datum, int32_t *pointNumber,
                                     int32_t *closed) {
    PATH *orig_p = DatumGetPathP((Datum)datum);
    *pointNumber = orig_p->npts;
    *closed = orig_p->closed;
}

void pldotnet_GetDatumPathCoordinates(void *datum, double *xCoordinates,
                                      double *yCoordinates) {
    PATH *orig_p = DatumGetPathP((Datum)datum);
    for (int32_t i = 0, npts = orig_p->npts; i < npts; i++) {
        xCoordinates[i] = orig_p->p[i].x;
        yCoordinates[i] = orig_p->p[i].y;
    }
}

void pldotnet_GetDatumPolygonAttributes(void *datum, int32_t *pointNumber) {
    POLYGON *orig_p = DatumGetPolygonP((Datum)datum);
    *pointNumber = orig_p->npts;
}

void pldotnet_GetDatumPolygonCoordinates(void *datum, double *xCoordinates,
                                         double *yCoordinates) {
    POLYGON *orig_p = DatumGetPolygonP((Datum)datum);
    for (int32_t i = 0, npts = orig_p->npts; i < npts; i++) {
        xCoordinates[i] = orig_p->p[i].x;
        yCoordinates[i] = orig_p->p[i].y;
    }
}

void pldotnet_GetDatumCircleAttributes(void *datum, double *x, double *y,
                                       double *r) {
    CIRCLE *orig_c = DatumGetCircleP((Datum)datum);
    *x = orig_c->center.x;
    *y = orig_c->center.y;
    *r = orig_c->radius;
}

void pldotnet_GetDatumTextAttributes(void *datum, int32_t *len, char **buf) {
    text *t = DatumGetTextPP((Datum)datum);
    const size_t datum_len = VARSIZE_ANY_EXHDR(t);
    *len = datum_len;
    *buf = VARDATA_ANY(t);
}

void pldotnet_GetDatumCharAttributes(void *datum, int32_t *len, char **buf) {
    BpChar *orig_bpc = DatumGetBpCharPP((Datum)datum);
    *len = VARSIZE_ANY_EXHDR(orig_bpc);
    *buf = VARDATA_ANY(orig_bpc);
}

void pldotnet_GetDatumVarCharAttributes(void *datum, int32_t *len, char **buf) {
    VarChar *orig_bpc = DatumGetVarCharPP((Datum)datum);
    *len = VARSIZE_ANY_EXHDR(orig_bpc);
    *buf = VARDATA_ANY(orig_bpc);
}

void pldotnet_GetDatumByteaAttributes(void *datum, int32_t *len, char **buf) {
    bytea *orig_b = DatumGetByteaPP((Datum)datum);
    *len = VARSIZE_ANY_EXHDR(orig_b);
    *buf = VARDATA_ANY(orig_b);
}

void pldotnet_GetDatumXmlAttributes(void *datum, int32_t *len, char **buf) {
    xmltype *orig_x = DatumGetXmlP((Datum)datum);
    *len = VARSIZE_ANY_EXHDR(orig_x);
    *buf = VARDATA_ANY(orig_x);
}

void pldotnet_GetDatumJsonAttributes(void *datum, int32_t *len, char **buf) {
    text *orig_j = DatumGetTextPP((Datum)datum);
    *len = VARSIZE_ANY_EXHDR(orig_j);
    *buf = VARDATA_ANY(orig_j);
}

void pldotnet_GetDatumDateAttributes(void *datum, int32_t *date) {
    DateADT orig_d = DatumGetDateADT((Datum)datum);
    *date = orig_d;
}

void pldotnet_GetDatumTimeAttributes(void *datum, int64_t *time) {
    TimeADT orig_t = DatumGetTimeADT((Datum)datum);
    *time = orig_t;
}

void pldotnet_GetDatumTimeTzAttributes(void *datum, int64_t *time,
                                       int32_t *zone) {
    TimeTzADT *orig_tz = DatumGetTimeTzADTP((Datum)datum);
    *time = orig_tz->time;
    *zone = orig_tz->zone;
}

void pldotnet_GetDatumTimestampAttributes(void *datum, int64_t *timestamp) {
    Timestamp orig_ts = DatumGetTimestamp((Datum)datum);
    *timestamp = orig_ts;
}

void pldotnet_GetDatumTimestampTzAttributes(void *datum, int64_t *timestamp) {
    TimestampTz orig_ts = DatumGetTimestampTz((Datum)datum);
    *timestamp = orig_ts;
}

void pldotnet_GetDatumIntervalAttributes(void *datum, int64_t *time,
                                         int32_t *day, int32_t *month) {
    Interval *orig_i = DatumGetIntervalP((Datum)datum);
    *time = orig_i->time;
    *day = orig_i->day;
    *month = orig_i->month;
}

void pldotnet_GetDatumMacAddressAttributes(void *datum, int32_t length,
                                           unsigned char *bytes) {
    macaddr *orig_ma;
    macaddr8 *orig_ma8;
    if (length == 6) {
        orig_ma = DatumGetMacaddrP((Datum)datum);
        bytes[0] = orig_ma->a;
        bytes[1] = orig_ma->b;
        bytes[2] = orig_ma->c;
        bytes[3] = orig_ma->d;
        bytes[4] = orig_ma->e;
        bytes[5] = orig_ma->f;
        return;
    }
    orig_ma8 = DatumGetMacaddr8P((Datum)datum);
    bytes[0] = orig_ma8->a;
    bytes[1] = orig_ma8->b;
    bytes[2] = orig_ma8->c;
    bytes[3] = orig_ma8->d;
    bytes[4] = orig_ma8->e;
    bytes[5] = orig_ma8->f;
    bytes[6] = orig_ma8->g;
    bytes[7] = orig_ma8->h;
}

void pldotnet_GetDatumInetAttributes(void *datum, int32_t *nelem,
                                     unsigned char *bytes, int32_t *netmask) {
    inet *orig_i = DatumGetInetP((Datum)datum);
    if (orig_i->inet_data.family == PGSQL_AF_INET)
        *nelem = 4;
    else if (orig_i->inet_data.family == PGSQL_AF_INET6)
        *nelem = 16;
    else
        elog(ERROR, "Unrecognized Inet family: %u", orig_i->inet_data.family);

    *netmask = orig_i->inet_data.bits;

    for (int32_t i = 0; i < *nelem; i++) {
        bytes[i] = orig_i->inet_data.ipaddr[i];
    }
}

void pldotnet_GetDatumMoneyAttributes(void *datum, int64_t *value) {
    Cash orig_c = DatumGetCash((Datum)datum);
    *value = orig_c;
}

void pldotnet_GetDatumVarBitAttributes(void *datum, int32_t *len, bits8 **dat) {
    VarBit *orig_vb = DatumGetVarBitP((Datum)datum);
    *len = orig_vb->bit_len;
    *dat = &orig_vb->bit_dat[0];
}

void pldotnet_GetDatumUuidAttributes(void *datum, unsigned char *data) {
    pg_uuid_t *orig_uuid = DatumGetUUIDP((Datum)datum);
    for (int32_t i = 0; i < 16; i++) data[i] = orig_uuid->data[i];
}

void pldotnet_GetDatumRangeAttributes(Datum inputDatum, bool *isEmpty,
                                      RangeBound **lowerRange,
                                      RangeBound **upperDange) {
    RangeType *orig_r = nullptr;
    TypeCacheEntry *typcache = nullptr;
    Oid rt_oid;

#if PG_VERSION_NUM >= 110000
    orig_r = DatumGetRangeTypeP(inputDatum);
#else
    orig_r = DatumGetRangeType(inputDatum);
#endif

    rt_oid = RangeTypeGetOid(orig_r);

    *lowerRange = palloc(sizeof(RangeBound));
    *upperDange = palloc(sizeof(RangeBound));

    typcache = lookup_type_cache(rt_oid, TYPECACHE_RANGE_INFO);
    range_deserialize(typcache, orig_r, *lowerRange, *upperDange, isEmpty);
}

void pldotnet_GetDatumRangeBoundAttributes(RangeBound *inputRange,
                                           Datum *rangeDatum, bool *infinite,
                                           bool *inclusive, bool *lower) {
    *rangeDatum = inputRange->val;
    *infinite = inputRange->infinite;
    *inclusive = inputRange->inclusive;
    *lower = inputRange->lower;
}

int32_t get_Maxdim(void) { return MAXDIM; }

void pldotnet_GetArrayAttributes(void *datum, int32_t *typeId, int32_t *nDims,
                                 int32_t *dims, uint8_t **nullmap) {
    /* the size of dims needs to be MAXDIM */
    ArrayType *array = DatumGetArrayTypeP((Datum)datum);
    int32_t *dims_in;
    int32_t i;

    *nDims = ARR_NDIM(array);
    Assert(ndim <= MAXDIM);
    *nullmap = ARR_NULLBITMAP(array);
    *typeId = ARR_ELEMTYPE(array);

    dims_in = ARR_DIMS(array);
    for (i = 0; i < *nDims; i++) {
        dims[i] = dims_in[i];
    }
}

int32_t pldotnet_GetArrayDatum(Datum arrayDatum, Datum *results, int32_t nElems,
                               int32_t typeId) {
    // converts the PostgreSQL array to a linear array of `Datum`
    // returns 0 on success, other on failure
    ArrayType *array = DatumGetArrayTypeP(arrayDatum);
    int32_t ndim = ARR_NDIM(array);
    int32_t *dims = ARR_DIMS(array);
    char *dataptr = ARR_DATA_PTR(array);
    bits8 *bitmap = ARR_NULLBITMAP(array);
    int32_t bitmask = 1;
    int32_t computed_nelems = 1;
    int32_t i;
    int16 typlen;
    bool typbyval;
    char typalign;

    if (ndim > MAXDIM) {
        elog(22, "# C ERROR: ndimcomputed_nelems(%d) > MAXDIM(%d))\n", ndim,
             MAXDIM);
        return -1;
    }
    for (i = 0; i < ndim; i++) {
        computed_nelems *= dims[i];
    }
    if (computed_nelems != nElems) {
        elog(22, "# C ERROR: (computed_nelems(%d)!=nelems(%d))\n",
             computed_nelems, nElems);
        return -2;
    }

    get_typlenbyvalalign((Oid)typeId, &typlen, &typbyval, &typalign);

    for (i = 0; i < nElems; i++) {
        /* checking for NULL */
        if (bitmap && (*bitmap & bitmask) == 0) {
            results[i] = (Datum)0;
        } else {
            results[i] = fetch_att(dataptr, typbyval, typlen);
            dataptr = att_addlength_pointer(dataptr, typlen, dataptr);
            dataptr = (char *)att_align_nominal(dataptr, typalign);
        }
        /* advance bitmap pointer if any */
        /* Suggestion: subsume these branches into arithmetic */
        if (bitmap) {
            bitmask <<= 1;
            if (bitmask == 0x100) {
                bitmap++;
                bitmask = 1;
            }
        }
    }
    return 0;
}

int pldotnet_GetNumberOfRecordAttributes(Datum recordDatum) {
    HeapTupleHeader tupleHeader;
    TupleDesc tupleDesc;
    int natts;

    if (recordDatum == (Datum)0) {
        return 0;
    }

    /* Convert Datum to HeapTupleHeader */
    tupleHeader = DatumGetHeapTupleHeader(recordDatum);

    /* Extract tuple descriptor using the type OID */
    tupleDesc = lookup_rowtype_tupdesc(HeapTupleHeaderGetTypeId(tupleHeader),
                                       HeapTupleHeaderGetTypMod(tupleHeader));

    /* Get the number of attributes */
    natts = tupleDesc->natts;

    /* Release the tuple descriptor */
    ReleaseTupleDesc(tupleDesc);

    return natts;
}

int pldotnet_GetRecordAttributes(Datum recordDatum, int numAttrs,
                                 Datum *attrDatums, bool *isNulls,
                                 Oid *typeOids) {
    HeapTupleData tuple;
    HeapTupleHeader tupleHeader;
    TupleDesc tupleDesc;

    if (recordDatum == (Datum)0) {
        return -1;  // Record datum is null
    }

    /* Convert Datum to HeapTupleHeader */
    tupleHeader = DatumGetHeapTupleHeader(recordDatum);

    /* Build a HeapTuple control structure */
    tuple.t_len = HeapTupleHeaderGetDatumLength(tupleHeader);
    ItemPointerSetInvalid(&(tuple.t_self));
    tuple.t_tableOid = InvalidOid;
    tuple.t_data = tupleHeader;

    /* Get the tuple descriptor */
    tupleDesc = lookup_rowtype_tupdesc(HeapTupleHeaderGetTypeId(tupleHeader),
                                       HeapTupleHeaderGetTypMod(tupleHeader));

    /* Check if the number of attributes is valid */
    if (numAttrs != tupleDesc->natts) {
        ReleaseTupleDesc(tupleDesc);
        return -1;  // Invalid number of attributes
    }

    for (int i = 0; i < numAttrs; i++) {
        /* Get the specified attribute's datum */
        attrDatums[i] = heap_getattr(&tuple, i + 1, tupleDesc, &isNulls[i]);

        /* Get the specified attribute's type OID */
        typeOids[i] = TupleDescAttr(tupleDesc, i)->atttypid;
    }

    /* Release the tuple descriptor */
    ReleaseTupleDesc(tupleDesc);

    return 0;
}

////////////////////////////////////
/// Npgsql or .NET type -> Datum ///
////////////////////////////////////

Datum pldotnet_CreateDatumInt16(int16_t value) {
    Datum dValue = Int16GetDatum(value);
    return dValue;
}

Datum pldotnet_CreateDatumInt32(int32_t value) {
    Datum dValue = Int32GetDatum(value);
    return dValue;
}

Datum pldotnet_CreateDatumInt64(int64_t value) {
    Datum dValue = Int64GetDatum(value);
    return dValue;
}

Datum pldotnet_CreateDatumFloat(float value) {
    Datum dValue = Float4GetDatum(value);
    return dValue;
}

Datum pldotnet_CreateDatumDouble(double value) {
    Datum dValue = Float8GetDatum(value);
    return dValue;
}

Datum pldotnet_CreateDatumBoolean(bool value) {
    Datum dValue = BoolGetDatum(value);
    return dValue;
}

Datum pldotnet_CreateDatumPoint(double x, double y) {
    Point *new_p = (Point *)palloc(sizeof(Point));
    new_p->x = x;
    new_p->y = y;
    return PointerGetDatum(new_p);
}

Datum pldotnet_CreateDatumLine(double a, double b, double c) {
    LINE *new_l = (LINE *)palloc(sizeof(LINE));
    new_l->A = a;
    new_l->B = b;
    new_l->C = c;
    return LinePGetDatum(new_l);
}

Datum pldotnet_CreateDatumLineSegment(double x1, double y1, double x2,
                                      double y2) {
    LSEG *new_l = (LSEG *)palloc(sizeof(LSEG));
    new_l->p[0].x = x1;
    new_l->p[0].y = y1;
    new_l->p[1].x = x2;
    new_l->p[1].y = y2;
    return LsegPGetDatum(new_l);
}

Datum pldotnet_CreateDatumBox(double x1, double y1, double x2, double y2) {
    BOX *new_b = (BOX *)palloc(sizeof(BOX));
    new_b->high.x = x1;
    new_b->high.y = y1;
    new_b->low.x = x2;
    new_b->low.y = y2;
    return BoxPGetDatum(new_b);
}

Datum pldotnet_CreateDatumPath(int32_t npts, int32_t closed,
                               double *xCoordinates, double *yCoordinates) {
    size_t path_size = sizeof(PATH) + ((size_t)npts * sizeof(Point));
    PATH *new_p = (PATH *)palloc(path_size);
    SET_VARSIZE(new_p, path_size);
    new_p->npts = npts;
    new_p->closed = closed;
    new_p->dummy = 0;
    for (int32_t i = 0; i < npts; i++) {
        new_p->p[i].x = xCoordinates[i];
        new_p->p[i].y = yCoordinates[i];
    }
    return PathPGetDatum(new_p);
}

Datum pldotnet_CreateDatumPolygon(int32_t npts, double *xCoordinates,
                                  double *yCoordinates) {
    size_t poly_size = sizeof(POLYGON) + ((size_t)npts * sizeof(Point));
    POLYGON *new_p = (POLYGON *)palloc(poly_size);
    SET_VARSIZE(new_p, poly_size);
    new_p->npts = npts;
    for (int32_t i = 0; i < npts; i++) {
        new_p->p[i].x = xCoordinates[i];
        new_p->p[i].y = yCoordinates[i];
    }
    return PolygonPGetDatum(new_p);
}

Datum pldotnet_CreateDatumCircle(double x, double y, double r) {
    CIRCLE *new_c = (CIRCLE *)palloc(sizeof(CIRCLE));
    new_c->center.x = x;
    new_c->center.y = y;
    new_c->radius = r;
    return CirclePGetDatum(new_c);
}

Datum pldotnet_CreateDatumText(int32_t len, char *buf) {
    const size_t new_size = VARHDRSZ + len;
    text *new_t = (text *)palloc(new_size);

    SET_VARSIZE(new_t, new_size);
    memcpy((void *)VARDATA(new_t), buf, len);
    PG_RETURN_TEXT_P(new_t);
}

Datum pldotnet_CreateDatumChar(int32_t len, char *buf) {
    const size_t new_size = VARHDRSZ + len;
    BpChar *new_bpc = (BpChar *)palloc(new_size);

    SET_VARSIZE(new_bpc, new_size);
    memcpy((void *)VARDATA(new_bpc), buf, len);
    PG_RETURN_BPCHAR_P(new_bpc);
}

Datum pldotnet_CreateDatumVarChar(int32_t len, char *buf) {
    const size_t new_size = VARHDRSZ + len;
    VarChar *new_bpc = (VarChar *)palloc(new_size);

    SET_VARSIZE(new_bpc, new_size);
    memcpy((void *)VARDATA(new_bpc), buf, len);
    PG_RETURN_VARCHAR_P(new_bpc);
}

Datum pldotnet_CreateDatumBytea(int32_t len, char *buf) {
    const size_t new_size = VARHDRSZ + len;
    bytea *new_b = (bytea *)palloc(new_size);

    SET_VARSIZE(new_b, new_size);
    memcpy((void *)VARDATA(new_b), buf, len);
    PG_RETURN_BYTEA_P(new_b);
}

Datum pldotnet_CreateDatumXml(int32_t len, char *buf) {
    const size_t new_size = VARHDRSZ + len;
    xmltype *new_x = (xmltype *)palloc(new_size);

    SET_VARSIZE(new_x, new_size);
    memcpy((void *)VARDATA(new_x), buf, len);
    PG_RETURN_XML_P(new_x);
}

Datum pldotnet_CreateDatumJson(int32_t len, char *buf) {
    const size_t new_size = VARHDRSZ + len;
    text *new_j = (text *)palloc(new_size);

    SET_VARSIZE(new_j, new_size);
    memcpy((void *)VARDATA(new_j), buf, len);
    PG_RETURN_TEXT_P(new_j);
}

Datum pldotnet_CreateDatumDate(int32_t date) { return DateADTGetDatum(date); }

Datum pldotnet_CreateDatumTime(int64_t time) { return TimeADTGetDatum(time); }

Datum pldotnet_CreateDatumTimeTz(int64_t time, int32_t zone) {
    TimeTzADT *new_tz = (TimeTzADT *)palloc(sizeof(TimeTzADT));
    new_tz->time = time;
    new_tz->zone = zone;
    return TimeTzADTPGetDatum(new_tz);
}

Datum pldotnet_CreateDatumTimestamp(int64_t timestamp) {
    return TimestampGetDatum(timestamp);
}

Datum pldotnet_CreateDatumTimestampTz(int64_t timestamp) {
    return TimestampTzGetDatum(timestamp);
}

Datum pldotnet_CreateDatumInterval(int64_t time, int32_t day, int32_t month) {
    Interval *new_i = (Interval *)palloc(sizeof(Interval));
    new_i->time = time;
    new_i->day = day;
    new_i->month = month;
    return IntervalPGetDatum(new_i);
}

Datum pldotnet_CreateDatumMacAddress(int32_t length, unsigned char *bytes) {
    macaddr *new_ma;
    macaddr8 *new_ma8;
    if (length == 6) {
        new_ma = (macaddr *)palloc(sizeof(macaddr));
        new_ma->a = bytes[0];
        new_ma->b = bytes[1];
        new_ma->c = bytes[2];
        new_ma->d = bytes[3];
        new_ma->e = bytes[4];
        new_ma->f = bytes[5];
        return MacaddrPGetDatum(new_ma);
    }
    new_ma8 = (macaddr8 *)palloc(sizeof(macaddr8));
    new_ma8->a = bytes[0];
    new_ma8->b = bytes[1];
    new_ma8->c = bytes[2];
    new_ma8->d = bytes[3];
    new_ma8->e = bytes[4];
    new_ma8->f = bytes[5];
    new_ma8->g = bytes[6];
    new_ma8->h = bytes[7];
    return Macaddr8PGetDatum(new_ma8);
}

Datum pldotnet_CreateDatumInet(int32_t length, unsigned char *bytes,
                               int32_t netmask) {
    inet *new_i = (inet *)palloc(sizeof(inet));
    SET_VARSIZE(new_i, sizeof(inet));
    if (length == 4)
        new_i->inet_data.family = PGSQL_AF_INET;
    else if (length == 16)
        new_i->inet_data.family = PGSQL_AF_INET6;
    else
        elog(ERROR, "Unrecognized Inet family with %d items.", length);

    new_i->inet_data.bits = netmask;
    for (int32_t i = 0; i < length; i++) {
        new_i->inet_data.ipaddr[i] = bytes[i];
    }
    return InetPGetDatum(new_i);
}

Datum pldotnet_CreateDatumMoney(int64_t value) {
    Datum new_m = CashGetDatum(value);
    return CashGetDatum(new_m);
}

Datum pldotnet_CreateDatumVarBit(int32_t len, bits8 *dat) {
    size_t size = VARBITTOTALLEN(len);
    VarBit *new_vb = (VarBit *)palloc(size);
    SET_VARSIZE(new_vb, size);
    new_vb->bit_len = len;
    for (int32_t i = 0; i < len; i++) new_vb->bit_dat[i] = dat[i];
    return VarBitPGetDatum(new_vb);
}

Datum pldotnet_CreateDatumUuid(unsigned char *data) {
    pg_uuid_t *new_uuid = (pg_uuid_t *)palloc(sizeof(pg_uuid_t));
    for (int32_t i = 0; i < 16; i++) new_uuid->data[i] = data[i];
    return UUIDPGetDatum(new_uuid);
}

Datum pldotnet_CreateEmptyDatumRange(Oid rangeTypeId) {
    // return an empty range for the oid
    size_t len = sizeof(RangeType) + sizeof(char);  // header plus flag
    RangeType *retval = (RangeType *)palloc(len);
    char *flag_ptr = (char *)retval;

    SET_VARSIZE(retval, len);
    retval->rangetypid = rangeTypeId;
    flag_ptr[len - 1] = RANGE_EMPTY;

#if PG_VERSION_NUM >= 110000
    PG_RETURN_RANGE_P(retval);
#else
    PG_RETURN_RANGE(retval);
#endif
}

Datum pldotnet_CreateDatumRange(Oid rtOid, Datum lowerDatum, bool lowerInfinite,
                                bool lowerInclusive, Datum upperDatum,
                                bool upperInfinite, bool upperInclusive) {
    TypeCacheEntry *typcache;
    RangeBound lower;
    RangeBound upper;

    lower.val = lowerDatum;
    lower.infinite = lowerInfinite;
    lower.inclusive = lowerInclusive;
    lower.lower = 1;

    upper.val = upperDatum;
    upper.infinite = upperInfinite;
    upper.inclusive = upperInclusive;
    upper.lower = 0;

    typcache = lookup_type_cache(rtOid, TYPECACHE_RANGE_INFO);

#if PG_VERSION_NUM >= 160000
    PG_RETURN_RANGE_P(
        range_serialize(typcache, &lower, &upper, false, nullptr));
#elif PG_VERSION_NUM >= 110000
    PG_RETURN_RANGE_P(range_serialize(typcache, &lower, &upper, false));
#else
    PG_RETURN_RANGE(range_serialize(typcache, &lower, &upper, false));
#endif
}

Datum pldotnet_CreateDatumArray(int32_t elementId, int32_t dimNumber,
                                int32_t *dimLengths, Datum *datums,
                                bool *nulls) {
    Oid element_type = (Oid)elementId;
    ArrayType *at;
    int16 typlen;
    bool typbyval;
    char typalign;
    int32_t *lsb = (int32_t *)palloc(sizeof(int32_t) * dimNumber);

    get_typlenbyvalalign(element_type, &typlen, &typbyval, &typalign);

    for (int32_t i = 0; i < dimNumber; i++) {
        lsb[i] = 1;
    }

    at = construct_md_array(datums, nulls, dimNumber, dimLengths, lsb,
                            element_type, typlen, typbyval, typalign);

    PG_RETURN_ARRAYTYPE_P(at);
}
