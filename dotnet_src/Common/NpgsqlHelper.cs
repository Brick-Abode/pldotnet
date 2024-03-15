using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NpgsqlTypes;

namespace PlDotNET.Common;

/// <summary>
/// Contains helper methods related to Npgsql.
/// </summary>
public static class NpgsqlHelper
{
    /// <summary>
    /// Dictionary to map the OIDs of the PostgreSQL range types.
    /// </summary>
    public static Dictionary<string, uint> RangeArrays = new ()
    {
        { "int4range", 3905 },
        { "numrange", 3907 },
        { "tsrange", 3909 },
        { "tstzrange", 3911 },
        { "daterange", 3913 },
        { "int8range", 3927 },
    };

    /// <summary>
    /// Dictionary to map the OIDs of the PostgreSQL multi-range types.
    /// </summary>
    public static Dictionary<string, uint> MultirangeArrays = new ()
    {
        { "int4multirange", 6150 },
        { "nummultirange", 6151 },
        { "tsmultirange", 6152 },
        { "tstzmultirange", 6153 },
        { "datemultirange", 6155 },
        { "int8multirange", 6157 },
    };

    /// <summary>
    /// Returns a BuiltInPostgresType object with the attributes
    /// of a NpgsqlDbType object
    /// </summary>
    public static BuiltInPostgresType GetPostgresTypeInfo(this NpgsqlDbType npgsqlDbType)
    {
        var type = typeof(NpgsqlDbType);
        var memInfo = type.GetMember(npgsqlDbType.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(BuiltInPostgresType), false);

        return (BuiltInPostgresType)attributes[0];
    }

    /// <summary>
    /// Returns the OID according to the provided (<see cref="NpgsqlDbType"/>)
    /// </summary>
    public static uint FindOid(NpgsqlDbType type)
    {
        int array = (int)NpgsqlDbType.Array; // -2,147,483,648
        int multiRange = (int)NpgsqlDbType.Multirange; // 536,870,921
        int range = (int)NpgsqlDbType.Range; // 1,073,741,824

        int typeValue = (int)type;

        if (typeValue > range)
        {
            // it is a range!
            return ((NpgsqlDbType)(typeValue - range)).GetPostgresTypeInfo().RangeOID;
        }
        else if (typeValue > multiRange)
        {
            // it is a multirange!
            return ((NpgsqlDbType)(typeValue - multiRange)).GetPostgresTypeInfo().MultirangeOID;
        }
        else if (typeValue > 0)
        {
            // it is a base!
            return ((NpgsqlDbType)typeValue).GetPostgresTypeInfo().BaseOID;
        }
        else
        {
            // it is an array!
            int arrayAux = typeValue - array;

            if (arrayAux > range)
            {
                // it is an array of range!
                // TODO: resolve CS8604 in these lines!
                return RangeArrays[((NpgsqlDbType)(arrayAux - range)).GetPostgresTypeInfo().RangeName];
            }
            else if (arrayAux > multiRange)
            {
                // it is an array of multirange!
                return MultirangeArrays[((NpgsqlDbType)(arrayAux - multiRange)).GetPostgresTypeInfo().MultirangeName];
            }

            // It is an array of base!
            return ((NpgsqlDbType)arrayAux).GetPostgresTypeInfo().ArrayOID;
        }
    }
}