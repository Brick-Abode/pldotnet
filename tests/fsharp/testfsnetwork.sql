--- MACADDROID

CREATE OR REPLACE FUNCTION returnMacAddressFSharp(my_address MACADDR) RETURNS MACADDR AS $$
my_address
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr', 'returnMacAddressFSharp', returnMacAddressFSharp(MACADDR '08-00-2b-01-02-03') = MACADDR '08-00-2b-01-02-03';

CREATE OR REPLACE FUNCTION compareMacAddressFSharp(address1 MACADDR, address2 MACADDR) RETURNS BOOLEAN AS $$
System.Object.ReferenceEquals(address1, address2)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr', 'compareMacAddressFSharp1', compareMacAddressFSharp(MACADDR '08:00:2a:01:02:03', MACADDR '08-00-2b-01-02-03') is false;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr-null', 'compareMacAddressFSharp2', compareMacAddressFSharp(NULL::MACADDR, MACADDR '08-00-2a-01-02-03') is false;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr-null', 'compareMacAddressFSharp2', compareMacAddressFSharp(MACADDR '08-00-2a-01-02-03', NULL::MACADDR) is false;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr-null', 'compareMacAddressFSharp2', compareMacAddressFSharp(NULL::MACADDR, NULL::MACADDR) is true;

--- MACADDR8OID

CREATE OR REPLACE FUNCTION addOneMacAddress8FSharp(my_address MACADDR8) RETURNS MACADDR8 AS $$
let bytes = my_address.GetAddressBytes()
bytes[7] <- bytes[7] + byte 1
PhysicalAddress(bytes)
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr8', 'addOneMacAddress8FSharp', addOneMacAddress8FSharp(MACADDR8 '08:00:2b:01:02:03:04:05') = MACADDR8 '08:00:2b:01:02:03:04:06';

CREATE OR REPLACE FUNCTION compareMacAddress8FSharp(a MACADDR8, b MACADDR8) RETURNS BOOLEAN AS $$
if System.Object.ReferenceEquals(a, null) && System.Object.ReferenceEquals(b, null) then true
else if System.Object.ReferenceEquals(a, null) then false
else if System.Object.ReferenceEquals(b, null) then false
else a.Equals(b)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr8', 'compareMacAddress8FSharp1', compareMacAddress8FSharp(MACADDR8 '08-00-2b-01-02-03-04-06', MACADDR8 '08-00-2b-01-02-03-04-06') is true;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr8', 'compareMacAddress8FSharp2', compareMacAddress8FSharp(MACADDR8 '08-00-2b-01-02-03-04-06', MACADDR8 '10-00-2b-01-02-03-04-06') is false;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr8-null', 'compareMacAddress8FSharp3', compareMacAddress8FSharp(NULL::MACADDR8, MACADDR8 'ab-01-2b-31-41-fa-ab-ac') is false;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr8-null', 'compareMacAddress8FSharp4', compareMacAddress8FSharp(MACADDR8 'ab-01-2b-31-41-fa-ab-ac', NULL::MACADDR8) is false;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr8-null', 'compareMacAddress8FSharp5', compareMacAddress8FSharp(NULL::MACADDR8, NULL::MACADDR8) is true;

--- INETOID

CREATE OR REPLACE FUNCTION modifyNetMaskFSharp(my_inet INET, n INT) RETURNS INET AS $$
let struct (address, netmask) = my_inet
struct (address, netmask + n)
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inet', 'modifyNetMaskFSharp', modifyNetMaskFSharp(INET '192.168.0.1/24', 6) = INET '192.168.0.1/30';

CREATE OR REPLACE FUNCTION modifyIPFSharp(my_inet INET, n INT) RETURNS INET AS $$
let struct (address, netmask) = if my_inet.HasValue then my_inet.Value else (IPAddress.Parse("127.0.0.1"), 21)
let bytes = address.GetAddressBytes()
let size = bytes.Length
bytes[size-1] <- bytes[size-1] + byte n.Value
struct (IPAddress(bytes), netmask)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inet', 'modifyIPFSharp1', modifyIPFSharp(INET '2001:db8:3333:4444:5555:6666:1.2.3.4/25', 20) = INET '2001:db8:3333:4444:5555:6666:1.2.3.24/25';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inet-null', 'modifyIPFSharp2', modifyIPFSharp(NULL::INET, 20) = INET '127.0.0.21/21';

--- CIDROID

CREATE OR REPLACE FUNCTION modifyIP_CIDRFSharp(my_inet CIDR, pos INT, delta INT) RETURNS CIDR AS $$
let struct (address, netmask) = my_inet
let bytes = address.GetAddressBytes()
bytes[pos] <- bytes[pos] + byte delta
struct (IPAddress(bytes), netmask)
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-cidr', 'modifyIP_CIDRFSharp', modifyIP_CIDRFSharp(CIDR '192.168/24', 0, 6) = CIDR '198.168.0.0/24';

CREATE OR REPLACE FUNCTION modifyNetmask_CIDR(my_inet CIDR, delta INT) RETURNS CIDR AS $$
let struct (address, netmask) = if my_inet.HasValue then my_inet.Value else (IPAddress.Parse("127.0.0.0"), 21)
struct (address, int (netmask + delta.Value))
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-cidr', 'modifyNetmask_CIDR1', modifyNetmask_CIDR(CIDR '2001:4f8:3:ba::/64', 10) = CIDR '2001:4f8:3:ba::/74';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-cidr-null', 'modifyNetmask_CIDR2', modifyNetmask_CIDR(NULL::CIDR, 10) = CIDR '127.0.0.0/31';

--- MACADDROID Arrays

CREATE OR REPLACE FUNCTION returnMacAddressArrayFSharp(addresses MACADDR[]) RETURNS MACADDR[] AS $$
addresses
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr-null-2array-arraynull', 'returnMacAddressArrayFSharp1', returnMacAddressArrayFSharp(ARRAY[[null::macaddr, null::macaddr], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']]) = ARRAY[[null::macaddr, null::macaddr], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']];

CREATE OR REPLACE FUNCTION updateArrayMacAddressIndexFSharp(a MACADDR[], b MACADDR) RETURNS MACADDR[] AS $$
let dim = a.Rank
match dim with
| 1 ->
    a.SetValue(b, 0) |> ignore
    a
| 2 ->
    a.SetValue(b, 0, 0) |> ignore
    a
| 3 ->
    a.SetValue(b, 0, 0, 0) |> ignore
    a
| _ -> a
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr-1array', 'updateArrayMacAddressIndexFSharp1', updateArrayMacAddressIndexFSharp(ARRAY[MACADDR '08-00-2b-01-02-03', MACADDR '09-00-2b-01-02-03', null::macaddr, MACADDR 'a8-00-2b-01-02-03'], MACADDR 'd1-00-2b-01-02-03') = ARRAY[MACADDR 'd1-00-2b-01-02-03', MACADDR '09-00-2b-01-02-03', null::macaddr, MACADDR 'a8-00-2b-01-02-03'];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr-2array', 'updateArrayMacAddressIndexFSharp2', updateArrayMacAddressIndexFSharp(ARRAY[[MACADDR '08-00-2b-01-02-03', MACADDR '09-00-2b-01-02-03'], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']], MACADDR 'd1-00-2b-01-02-03') = ARRAY[[MACADDR 'd1-00-2b-01-02-03', MACADDR '09-00-2b-01-02-03'], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr-3array', 'updateArrayMacAddressIndexFSharp3', updateArrayMacAddressIndexFSharp(ARRAY[[[MACADDR '08-00-2b-01-02-03', MACADDR '09-00-2b-01-02-03'], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']]], MACADDR 'd1-00-2b-01-02-03') = ARRAY[[[MACADDR 'd1-00-2b-01-02-03', MACADDR '09-00-2b-01-02-03'], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr-null-1array-arraynull', 'updateArrayMacAddressIndexFSharp4', updateArrayMacAddressIndexFSharp(ARRAY[null::macaddr, null::macaddr, null::macaddr, MACADDR 'a8-00-2b-01-02-03'], MACADDR 'd1-00-2b-01-02-03') = ARRAY[MACADDR 'd1-00-2b-01-02-03', null::macaddr, null::macaddr, MACADDR 'a8-00-2b-01-02-03'];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr-null-2array-arraynull', 'updateArrayMacAddressIndexFSharp5', updateArrayMacAddressIndexFSharp(ARRAY[[null::macaddr, null::macaddr], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']], MACADDR 'd1-00-2b-01-02-03') = ARRAY[[MACADDR 'd1-00-2b-01-02-03', null::macaddr], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr-null-3array-arraynull', 'updateArrayMacAddressIndexFSharp6', updateArrayMacAddressIndexFSharp(ARRAY[[[null::macaddr, null::macaddr], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']]], MACADDR 'd1-00-2b-01-02-03') = ARRAY[[[MACADDR 'd1-00-2b-01-02-03', null::macaddr], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']]];

CREATE OR REPLACE FUNCTION CreateMacAddressMultidimensionalArrayFSharp() RETURNS MACADDR[] AS $$
let bytes = [| 171uy; 1uy; 43uy; 49uy; 65uy; 250uy |]
let objects_value = PhysicalAddress(bytes)
let arr = Array.CreateInstance(typeof<PhysicalAddress>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr-3array', 'CreateMacAddressMultidimensionalArrayFSharp', CreateMacAddressMultidimensionalArrayFSharp() = ARRAY[[[MACADDR 'ab-01-2b-31-41-fa']]];

--- MACADDR8OID Arrays

CREATE OR REPLACE FUNCTION updateArrayMacAddress8IndexFSharp(a MACADDR8[], b MACADDR8) RETURNS MACADDR8[] AS $$
let dim = a.Rank
match dim with
| 1 ->
    a.SetValue(b, 0) |> ignore
    a
| 2 ->
    a.SetValue(b, 0, 0) |> ignore
    a
| 3 ->
    a.SetValue(b, 0, 0, 0) |> ignore
    a
| _ -> a
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr8-1array', 'updateArrayMacAddress8IndexFSharp1', updateArrayMacAddress8IndexFSharp(ARRAY[MACADDR8 '08-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac', null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac'], MACADDR8 'd1-00-2b-01-02-03-ab-ac') = ARRAY[MACADDR8 'd1-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac', null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac'];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr8-null-1array-arraynull', 'updateArrayMacAddress8IndexFSharp2', updateArrayMacAddress8IndexFSharp(ARRAY[null::MACADDR8, null::MACADDR8, null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac'], MACADDR8 'd1-00-2b-01-02-03-ab-ac') = ARRAY[MACADDR8 'd1-00-2b-01-02-03-ab-ac', null::MACADDR8, null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac'];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr8-2array', 'updateArrayMacAddress8IndexFSharp3', updateArrayMacAddress8IndexFSharp(ARRAY[[MACADDR8 '08-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac'], [null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']], MACADDR8 'd1-00-2b-01-02-03-ab-ac') = ARRAY[[MACADDR8 'd1-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac'], [null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr8-null-2array-arraynull', 'updateArrayMacAddress8IndexFSharp4', updateArrayMacAddress8IndexFSharp(ARRAY[[null::MACADDR8, null::MACADDR8], [null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']], MACADDR8 'd1-00-2b-01-02-03-ab-ac') = ARRAY[[MACADDR8 'd1-00-2b-01-02-03-ab-ac', null::MACADDR8], [null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr8-3array', 'updateArrayMacAddress8IndexFSharp5', updateArrayMacAddress8IndexFSharp(ARRAY[[[MACADDR8 '08-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac'], [null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']]], MACADDR8 'd1-00-2b-01-02-03-ab-ac') = ARRAY[[[MACADDR8 'd1-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac'], [null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr8-null-3array-arraynull', 'updateArrayMacAddress8IndexFSharp6', updateArrayMacAddress8IndexFSharp(ARRAY[[[null::MACADDR8, null::MACADDR8], [null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']]], MACADDR8 'd1-00-2b-01-02-03-ab-ac') = ARRAY[[[MACADDR8 'd1-00-2b-01-02-03-ab-ac', null::MACADDR8], [null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']]];

CREATE OR REPLACE FUNCTION CreateMacAddress8MultidimensionalArrayFSharp(objects_value MACADDR8) RETURNS MACADDR8[] AS $$
let arr = Array.CreateInstance(typeof<PhysicalAddress>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-macaddr8-3array', 'CreateMacAddress8MultidimensionalArrayFSharp', CreateMacAddress8MultidimensionalArrayFSharp(MACADDR8 'ab-01-2b-31-41-fa-ab-ac') = ARRAY[[[MACADDR8 'ab-01-2b-31-41-fa-ab-ac']]];

--- INETOID Arrays

CREATE OR REPLACE FUNCTION updateArrayNetMaskIndexFSharp(a INET[], b INET) RETURNS INET[] AS $$
let dim = a.Rank
match dim with
| 1 ->
    a.SetValue(b, 0) |> ignore
    a
| 2 ->
    a.SetValue(b, 0, 0) |> ignore
    a
| 3 ->
    a.SetValue(b, 0, 0, 0) |> ignore
    a
| _ -> a
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inet-1array', 'updateArrayNetMaskIndexFSharp1', updateArrayNetMaskIndexFSharp(ARRAY[INET '192.168.0.1/24', INET '192.170.0.1/24', null::inet, INET '170.168.0.1/24'], INET '192.168.0.120/24') = ARRAY[INET '192.168.0.120/24', INET '192.170.0.1/24', null::inet, INET '170.168.0.1/24'];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inet-2array', 'updateArrayNetMaskIndexFSharp2', updateArrayNetMaskIndexFSharp(ARRAY[[INET '192.168.0.1/24', INET '192.170.0.1/24'], [null::inet, INET '170.168.0.1/24']], INET '192.168.0.120/24') = ARRAY[[INET '192.168.0.120/24', INET '192.170.0.1/24'], [null::inet, INET '170.168.0.1/24']];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inet-2array', 'updateArrayNetMaskIndexFSharp3', updateArrayNetMaskIndexFSharp(ARRAY[[[INET '192.168.0.1/24', INET '192.170.0.1/24'], [null::inet, INET '170.168.0.1/24']]], INET '192.168.0.120/24') = ARRAY[[[INET '192.168.0.120/24', INET '192.170.0.1/24'], [null::inet, INET '170.168.0.1/24']]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inet-null-1array-arraynull', 'updateArrayNetMaskIndexFSharp4', updateArrayNetMaskIndexFSharp(ARRAY[null::INET, null::INET, null::inet, INET '170.168.0.1/24'], INET '192.168.0.120/24') = ARRAY[INET '192.168.0.120/24', null::INET, null::inet, INET '170.168.0.1/24'];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inet-null-2array-arraynull', 'updateArrayNetMaskIndexFSharp5', updateArrayNetMaskIndexFSharp(ARRAY[[null::INET, null::INET], [null::inet, INET '170.168.0.1/24']], INET '192.168.0.120/24') = ARRAY[[INET '192.168.0.120/24', null::INET], [null::inet, INET '170.168.0.1/24']];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inet-null-3array-arraynull', 'updateArrayNetMaskIndexFSharp6', updateArrayNetMaskIndexFSharp(ARRAY[[[null::INET, null::INET], [null::inet, INET '170.168.0.1/24']]], INET '192.168.0.120/24') = ARRAY[[[INET '192.168.0.120/24', null::INET], [null::inet, INET '170.168.0.1/24']]];

CREATE OR REPLACE FUNCTION CreateInetMultidimensionalArrayFSharp() RETURNS INET[] AS $$
let objects_value = struct (IPAddress.Parse("127.0.0.1"), 21)
let arr = Array.CreateInstance(typeof<struct(IPAddress*int)>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-inet-3array', 'CreateInetMultidimensionalArrayFSharp', CreateInetMultidimensionalArrayFSharp() = ARRAY[[[INET '127.0.0.1/21']]];

--- CIDROID Arrays

CREATE OR REPLACE FUNCTION updateArrayCIDRIndexFSharp(a CIDR[], b CIDR) RETURNS CIDR[] AS $$
let dim = a.Rank
match dim with
| 1 ->
    a.SetValue(b, 0) |> ignore
    a
| 2 ->
    a.SetValue(b, 0, 0) |> ignore
    a
| 3 ->
    a.SetValue(b, 0, 0, 0) |> ignore
    a
| _ -> a
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-cidr-1array', 'updateArrayCIDRIndexFSharp1', updateArrayCIDRIndexFSharp(ARRAY[CIDR '192.168/24', CIDR '170.168/24', null::cidr, CIDR '142.168/24'], CIDR '192.169/24') = ARRAY[CIDR '192.169/24', CIDR '170.168/24', null::cidr, CIDR '142.168/24'];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-cidr-2array', 'updateArrayCIDRIndexFSharp2', updateArrayCIDRIndexFSharp(ARRAY[[CIDR '192.168/24', CIDR '170.168/24'], [null::cidr, CIDR '142.168/24']], CIDR '192.169/24') = ARRAY[[CIDR '192.169/24', CIDR '170.168/24'], [null::cidr, CIDR '142.168/24']];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-cidr-2array', 'updateArrayCIDRIndexFSharp3', updateArrayCIDRIndexFSharp(ARRAY[[[CIDR '192.168/24', CIDR '170.168/24'], [null::cidr, CIDR '142.168/24']]], CIDR '192.169/24') = ARRAY[[[CIDR '192.169/24', CIDR '170.168/24'], [null::cidr, CIDR '142.168/24']]];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-cidr-null-1array-arraynull', 'updateArrayCIDRIndexFSharp4', updateArrayCIDRIndexFSharp(ARRAY[null::CIDR, null::CIDR, null::cidr, CIDR '142.168/24'], CIDR '192.169/24') = ARRAY[CIDR '192.169/24', null::CIDR, null::cidr, CIDR '142.168/24'];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-cidr-null-2array-arraynull', 'updateArrayCIDRIndexFSharp5', updateArrayCIDRIndexFSharp(ARRAY[[null::CIDR, null::CIDR], [null::cidr, CIDR '142.168/24']], CIDR '192.169/24') = ARRAY[[CIDR '192.169/24', null::CIDR], [null::cidr, CIDR '142.168/24']];
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-cidr-null-3array-arraynull', 'updateArrayCIDRIndexFSharp6', updateArrayCIDRIndexFSharp(ARRAY[[[null::CIDR, null::CIDR], [null::cidr, CIDR '142.168/24']]], CIDR '192.169/24') = ARRAY[[[CIDR '192.169/24', null::CIDR], [null::cidr, CIDR '142.168/24']]];

CREATE OR REPLACE FUNCTION CreateCIDRMultidimensionalArrayFSharp() RETURNS CIDR[] AS $$
let objects_value = struct (IPAddress.Parse("127.123.54.0"), 24)
let arr = Array.CreateInstance(typeof<struct(IPAddress*int)>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
$$ LANGUAGE plfsharp STRICT;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-cidr-3array', 'CreateCIDRMultidimensionalArrayFSharp', CreateCIDRMultidimensionalArrayFSharp() = ARRAY[[[CIDR '127.123.54.0/24']]];
