BEGIN;

----------------------------------------------------------------------
-- First, we test simple use cases for INOUT and OUT
----------------------------------------------------------------------

-- Test inout_basic_0 for inout values (INOUT)
CREATE OR REPLACE FUNCTION inout_basic_0(INOUT argument_0 INT) AS $$
    if(argument_0 != 0){ throw new SystemException($"Failed assertion: argument_0 = {argument_0}");}
    argument_0 = 1;
$$ LANGUAGE plcsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-basic-0', 'inout_basic_0', inout_basic_0(0) = 1;

-- Test inout_basic_1 for inout values (OUT)
CREATE OR REPLACE FUNCTION inout_basic_1(OUT argument_0 INT) AS $$
    argument_0 = 1;
$$ LANGUAGE plcsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-basic-1', 'inout_basic_1', inout_basic_1() = 1;

----------------------------------------------------------------------
-- Second, we test NULL for INOUT and OUT
----------------------------------------------------------------------

-- Test that non-null input can give null output on INOUT
CREATE OR REPLACE FUNCTION inout_null_1(INOUT argument_0 INT) AS $$
    if(argument_0 is null ){ throw new SystemException($"Failed assertion: argument_0 = {argument_0}");}
    argument_0 = null;
$$ LANGUAGE plcsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-null-1', 'inout_null_1', inout_null_1(11) IS NULL;

-- Test that null input can give non-null output on INOUT
CREATE OR REPLACE FUNCTION inout_null_2(INOUT argument_0 INT) AS $$
    if(argument_0 != null ){ throw new SystemException($"Failed assertion: argument_0 = {argument_0}");}
    argument_0 = 3;
$$ LANGUAGE plcsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-null-2', 'inout_null_2', inout_null_2(NULL) = 3;

-- Test that OUT can be null
CREATE OR REPLACE FUNCTION inout_null_3(OUT argument_0 INT) AS $$
    argument_0 = null;
$$ LANGUAGE plcsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-null-3', 'inout_null_3', inout_null_3() IS NULL;

-- Test that non-null input can give null output on INOUT, with multiple args
CREATE OR REPLACE FUNCTION inout_null_4(INOUT argument_0 INT, in argument_1 INT) AS $$
    if(argument_0 is null ){ throw new SystemException($"Failed assertion: argument_0 = {argument_0}");}
    if(argument_1 != 3){ throw new SystemException($"Failed assertion: argument_0 = {argument_0}");}
    argument_0 = null;
$$ LANGUAGE plcsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-null-4', 'inout_null_4', inout_null_4(11, 3) IS NULL;

-- Test that null input can give non-null output on INOUT, with multiple args
CREATE OR REPLACE FUNCTION inout_null_5(INOUT argument_0 INT, IN argument_1 INT) AS $$
    if(argument_0 != null ){ throw new SystemException($"Failed assertion: argument_0 = {argument_0}");}
    if(argument_1 != 3){ throw new SystemException($"Failed assertion: argument_0 = {argument_0}");}
    argument_0 = 3;
$$ LANGUAGE plcsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-null-5', 'inout_null_5', inout_null_5(NULL, 3) = 3;

-- Test that OUT can be null, or not, with multiple args
CREATE OR REPLACE FUNCTION inout_null_6(IN argument_0 INT, OUT argument_1 INT) AS $$
    if(argument_0 == 1){ argument_1 = null; }
    else if(argument_0 == 2){ argument_1 = 2; }
    else { throw new SystemException($"Failed assertion: argument_0 = {argument_0}");}
$$ LANGUAGE plcsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-null-6', 'inout_null_6', inout_null_6(1) IS NULL;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-null-7', 'inout_null_6', inout_null_6(2) = 2;

----------------------------------------------------------------------
-- Third, we test large argument sets, with first arguments being
-- each of IN/INOUT/OUT
----------------------------------------------------------------------

-- Test inout_multiarg_1 for inout values (IN, INOUT, IN, OUT, OUT, INOUT, IN, OUT)
CREATE OR REPLACE FUNCTION inout_multiarg_1(IN argument_0 INT, INOUT argument_1 INT, IN argument_2 INT, OUT argument_3 INT, OUT argument_4 INT, INOUT argument_5 INT, IN argument_6 INT, OUT argument_7 INT) AS $$
    if(argument_0 != 0){ throw new SystemException($"Failed assertion: argument_0 = {argument_0}");}
    if(argument_1 != 1){ throw new SystemException($"Failed assertion: argument_1 = {argument_1}");}
    argument_1 = 2;
    if(argument_2 != 2){ throw new SystemException($"Failed assertion: argument_2 = {argument_2}");}
    argument_3 = 4;
    argument_4 = 5;
    if(argument_5 != null){ throw new SystemException($"Failed assertion: argument_5 = {argument_5}");}
    argument_5 = 6;
    if(argument_6 != 6){ throw new SystemException($"Failed assertion: argument_6 = {argument_6}");}
    argument_7 = null;
$$ LANGUAGE plcsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-multiarg-1', 'inout_multiarg_1', inout_multiarg_1(0, 1, 2, NULL, 6) = ROW(2, 4, 5, 6, NULL::INT);

-- Test inout_multiarg_2 for inout values (INOUT, OUT, INOUT, INOUT, IN, OUT, IN, OUT)
CREATE OR REPLACE FUNCTION inout_multiarg_2(INOUT argument_0 INT, OUT argument_1 INT, INOUT argument_2 INT, INOUT argument_3 INT, IN argument_4 INT, OUT argument_5 INT, IN argument_6 INT, OUT argument_7 INT) AS $$
    if(argument_0 != null){ throw new SystemException($"Failed assertion: argument_0 = {argument_0}");}
    argument_0 = 1;
    argument_1 = 2;
    if(argument_2 != 2){ throw new SystemException($"Failed assertion: argument_2 = {argument_2}");}
    argument_2 = 3;
    if(argument_3 != 3){ throw new SystemException($"Failed assertion: argument_3 = {argument_3}");}
    argument_3 = null;
    if(argument_4 != 4){ throw new SystemException($"Failed assertion: argument_4 = {argument_4}");}
    argument_5 = 6;
    if(argument_6 != 6){ throw new SystemException($"Failed assertion: argument_6 = {argument_6}");}
    argument_7 = 8;
$$ LANGUAGE plcsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-multiarg-2', 'inout_multiarg_2', inout_multiarg_2(NULL, 2, 3, 4, 6) = ROW(1, 2, 3, NULL::INT, 6, 8);

-- Test inout_multiarg_3 for inout values (OUT, IN, IN, OUT, INOUT, OUT, IN, INOUT)
CREATE OR REPLACE FUNCTION inout_multiarg_3(OUT argument_0 INT, IN argument_1 INT, IN argument_2 INT, OUT argument_3 INT, INOUT argument_4 INT, OUT argument_5 INT, IN argument_6 INT, INOUT argument_7 INT) AS $$
    argument_0 = null;
    if(argument_1 != 1){ throw new SystemException($"Failed assertion: argument_1 = {argument_1}");}
    if(argument_2 != 2){ throw new SystemException($"Failed assertion: argument_2 = {argument_2}");}
    argument_3 = 4;
    if(argument_4 != 4){ throw new SystemException($"Failed assertion: argument_4 = {argument_4}");}
    argument_4 = 5;
    argument_5 = 6;
    if(argument_6 != 6){ throw new SystemException($"Failed assertion: argument_6 = {argument_6}");}
    if(argument_7 != 7){ throw new SystemException($"Failed assertion: argument_7 = {argument_7}");}
    argument_7 = 8;
$$ LANGUAGE plcsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-multiarg-3', 'inout_multiarg_3', inout_multiarg_3(1, 2, 4, 6, 7) = ROW(NULL::INT, 4, 5, 6, 8);

-- Test inout_multiarg_4inout values (IN, INOUT, IN, OUT, OUT, INOUT, INOUT, IN)
-- This one makes sure that `STRICT` works fine.
CREATE OR REPLACE FUNCTION inout_multiarg_4(IN argument_0 INT, INOUT argument_1 INT, IN argument_2 INT, OUT argument_3 INT, OUT argument_4 INT, INOUT argument_5 INT, INOUT argument_6 INT, IN argument_7 INT) AS $$
        if(argument_0 != 0){ throw new SystemException($"Failed assertion: argument_0 = {argument_0}");}
        if(argument_1 != 1){ throw new SystemException($"Failed assertion: argument_1 = {argument_1}");}
        argument_1 = 2;
        if(argument_2 != 2){ throw new SystemException($"Failed assertion: argument_2 = {argument_2}");}
        argument_3 = 4;
        argument_4 = 5;
        if(argument_5 != 5){ throw new SystemException($"Failed assertion: argument_5 = {argument_5}");}
        argument_5 = null;
        if(argument_6 != 6){ throw new SystemException($"Failed assertion: argument_6 = {argument_6}");}
        argument_6 = 7;
        if(argument_7 != 7){ throw new SystemException($"Failed assertion: argument_7 = {argument_7}");}
$$ LANGUAGE plcsharp STRICT;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-multiarg-4', 'inout_multiarg_4', inout_multiarg_4(0, 1, 2, 5, 6, 7) = ROW(2, 4, 5, NULL::INT, 7);

----------------------------------------------------------------------
-- Fourth, we test ARRAY (with NULL) for INOUT, OUT
----------------------------------------------------------------------

CREATE OR REPLACE FUNCTION inout_array_10(INOUT values_array MACADDR[], OUT nulls INT) AS $$
    Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
    ArrayManipulation.FlatArray(values_array, ref flatten_values);
    nulls = 0;
    for(int i = 0; i < flatten_values.Length; i++)
    {
        if (flatten_values.GetValue(i) == null){
            nulls++;
            continue;
        }

        PhysicalAddress orig_value = (PhysicalAddress)flatten_values.GetValue(i);
        byte[] bytes = orig_value.GetAddressBytes();
        bytes[0] += 1;
        PhysicalAddress new_value = new PhysicalAddress(bytes);

        flatten_values.SetValue((PhysicalAddress)new_value, i);
    }
    values_array = flatten_values;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-array-10', 'inout_array_10', inout_array_10(
    ARRAY[
        MACADDR '08-00-2b-01-02-03',
        MACADDR '09-00-2b-01-02-03',
        null::macaddr,
        MACADDR 'a8-00-2b-01-02-03',
        null::macaddr
    ]) = ROW(
        ARRAY[
            MACADDR '09-00-2b-01-02-03',
            MACADDR '0a-00-2b-01-02-03',
            null::macaddr,
            MACADDR 'a9-00-2b-01-02-03',
            null::macaddr
        ], 2
    );

CREATE OR REPLACE FUNCTION inout_array_11(OUT values_array MACADDR[], IN address MACADDR, IN count INT) AS $$
    Array output = Array.CreateInstance(typeof(object), count);
    for(int i = 0; i < count; i++)
    {
        output.SetValue((PhysicalAddress)address, i);
    }
    values_array = output;
$$ LANGUAGE plcsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-array-11', 'inout_array_11', inout_array_11(MACADDR '08-00-2b-01-02-03', 3) =
        ARRAY[
            MACADDR '08-00-2b-01-02-03',
            MACADDR '08-00-2b-01-02-03',
            MACADDR '08-00-2b-01-02-03'
        ];

----------------------------------------------------------------------
-- Fifth, we test non-trivial StructTypeHandler, here INET
----------------------------------------------------------------------

-- Test IN/INOUT/OUT for non-trivial StructTypeHandler, in this case INET

CREATE OR REPLACE FUNCTION inout_simple_10(OUT checksum INT, IN address INET) AS $$
    int i;

    // get bytes
    byte[] bytes = address.Address.GetAddressBytes();

    // compute checksum
    checksum = 0;
    for(i = 0; i<bytes.Length;i++){ checksum += bytes[i]; }
$$ LANGUAGE plcsharp STRICT;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-simple-10', 'inout_simple_10', inout_simple_10(CIDR '192.168/24') = 360;

CREATE OR REPLACE FUNCTION inout_simple_20(INOUT address INET, IN pos INT, IN delta INT) AS $$
    int i;

    // compute new address
    (IPAddress Address, int Netmask) address2 = address ?? (IPAddress.Parse("1.1.1.1"), 8);
    byte[] bytes = address2.Address.GetAddressBytes();
    bytes[pos]+=(byte)delta;
    address = (new IPAddress(bytes), address2.Netmask);
$$ LANGUAGE plcsharp STRICT;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-simple-20', 'inout_simple_20', inout_simple_20(CIDR '192.168/24', 2, 3) = INET '192.168.3.0/24';

CREATE OR REPLACE FUNCTION inout_simple_30(OUT checksum INT, INOUT address INET, IN pos INT, IN delta INT) AS $$
    int i;

    // compute new address
    (IPAddress Address, int Netmask) address2 = address ?? (IPAddress.Parse("1.1.1.1"), 8);
    byte[] bytes = address2.Address.GetAddressBytes();
    bytes[pos]+=(byte)delta;
    address = (new IPAddress(bytes), address2.Netmask);

    // compute checksum
    checksum = 0;
    for(i = 0; i<bytes.Length;i++){ checksum += bytes[i]; }
$$ LANGUAGE plcsharp STRICT;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-simple-30', 'inout_simple_30', inout_simple_30(CIDR '192.168/24', 2, 3) = ROW(363, INET '192.168.3.0/24');

----------------------------------------------------------------------
-- Fifth, we test non-trivial ObjectTypeHandler, here StringHandler
----------------------------------------------------------------------

CREATE OR REPLACE FUNCTION inout_object_10(IN a text, INOUT b text) AS $$
    if (a == null) a = "";
    if (b == null) b = "";
    b = a + " " + b;
$$ LANGUAGE plcsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-object-10', 'inout_object_10', inout_object_10('red', 'blue') = 'red blue';

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-object-11', 'inout_object_10', inout_object_10('red', NULL) = 'red ';

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-object-12', 'inout_object_10', inout_object_10(NULL, 'blue') = ' blue';

CREATE OR REPLACE FUNCTION inout_object_20(IN a text, b text, OUT c text) AS $$
    if (a == null) a = "";
    if (b == null) b = "";
    c = a + " " + b;
$$ LANGUAGE plcsharp;

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-object-20', 'inout_object_20', inout_object_20('red', 'blue') = 'red blue';

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-object-21', 'inout_object_20', inout_object_20('red', NULL) = 'red ';

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-object-22', 'inout_object_20', inout_object_20(NULL, 'blue') = ' blue';

INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'c#-inout-object-23', 'inout_object_20', inout_object_20('🐂', '🥰') = '🐂 🥰'::TEXT;

COMMIT;
