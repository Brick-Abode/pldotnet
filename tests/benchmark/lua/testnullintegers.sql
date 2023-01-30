/**************** Null return test functions ***************************/
CREATE OR REPLACE FUNCTION returnNullIntLua() RETURNS integer AS $$
return nil
$$ LANGUAGE pllua;
SELECT returnNullIntLua() is NULL;

CREATE OR REPLACE FUNCTION returnNullSmallIntLua() RETURNS smallint AS $$
return nil
$$ LANGUAGE pllua;
SELECT returnNullSmallIntLua() is NULL;

CREATE OR REPLACE FUNCTION returnNullBigIntLua() RETURNS bigint AS $$
return nil
$$ LANGUAGE pllua;
SELECT returnNullBigIntLua() is NULL;

/**************** Null operations test functions ***************************/
CREATE OR REPLACE FUNCTION sumNullArgIntLua(a integer, b integer) RETURNS integer AS $$
if (a == nil) then
   a = 0
end
if (b == nil) then
   b = 0
end
return a+b
$$ LANGUAGE pllua;
SELECT sumNullArgIntLua(null,null) = integer '0';
SELECT sumNullArgIntLua(null,3) = integer '3';
SELECT sumNullArgIntLua(3,null) = integer '3';
SELECT sumNullArgIntLua(3,3) = integer '6';

CREATE OR REPLACE FUNCTION sumNullArgSmallIntLua(a smallint, b smallint) RETURNS smallint AS $$
if (a == nil) then
   a = 0
end
if (b == nil) then
   b = 0
end
return a+b
$$ LANGUAGE pllua;
SELECT sumNullArgSmallIntLua(null,null) = integer '0';
SELECT sumNullArgSmallIntLua(null,CAST(101 AS smallint)) = integer '101';
SELECT sumNullArgSmallIntLua(CAST(101 AS smallint),null) = integer '101';
SELECT sumNullArgSmallIntLua(CAST(101 AS smallint),CAST(101 AS smallint)) = smallint '202';

CREATE OR REPLACE FUNCTION sumNullArgBigIntLua(a bigint, b bigint) RETURNS bigint AS $$
if (a == nil) then
   a = 0
end
if (b == nil) then
   b = 0
end
return a+b
$$ LANGUAGE pllua;
SELECT sumNullArgBigIntLua(null,null) = integer '0';
SELECT sumNullArgBigIntLua(null,100) = integer '100';
SELECT sumNullArgBigIntLua(9223372036854775707,null) = bigint '9223372036854775707';
SELECT sumNullArgBigIntLua(9223372036854775707,100) = bigint '9223372036854775807';

/**************** Conditional return test functions ***************************/
CREATE OR REPLACE FUNCTION checkedSumNullArgIntLua(a integer, b integer) RETURNS integer AS $$
if (a == nil or b == nil) then
    return nil
else
    return a + b
end
$$ LANGUAGE pllua;
SELECT checkedSumNullArgIntLua(null,null) is NULL;
SELECT checkedSumNullArgIntLua(null,3) is NULL;
SELECT checkedSumNullArgIntLua(3,null) is NULL;
SELECT checkedSumNullArgIntLua(3,3) = integer '6';

CREATE OR REPLACE FUNCTION checkedSumNullArgSmallIntLua(a smallint, b smallint) RETURNS smallint AS $$
if (a == nil or b == nil) then
    return nil
else
    return a + b
end
$$ LANGUAGE pllua;
SELECT checkedSumNullArgSmallIntLua(null,null) is NULL;
SELECT checkedSumNullArgSmallIntLua(null,CAST(133 AS smallint)) is NULL;
SELECT checkedSumNullArgSmallIntLua(CAST(133 AS smallint),null) is NULL;
SELECT checkedSumNullArgSmallIntLua(CAST(133 AS smallint),CAST(133 AS smallint)) = smallint '266';

CREATE OR REPLACE FUNCTION checkedSumNullArgBigIntLua(a bigint, b bigint) RETURNS bigint AS $$
if (a == nil or b == nil) then
    return nil
else
    return a + b
end
$$ LANGUAGE pllua;
SELECT checkedSumNullArgBigIntLua(null,null) is NULL;
SELECT checkedSumNullArgBigIntLua(null,100) is NULL;
SELECT checkedSumNullArgBigIntLua(9223372036854775707,null) is NULL;
SELECT checkedSumNullArgBigIntLua(9223372036854775707,100) = bigint '9223372036854775807';

/**************** Conditional return test functions (Mixed Args)  ***************************/
CREATE OR REPLACE FUNCTION checkedSumNullArgMixedLua(a integer, b smallint, c bigint) RETURNS bigint AS $$
if (a == nil or b == nil or c == nil) then
    return nil
else
    return a + b + c
end
$$ LANGUAGE pllua;
SELECT checkedSumNullArgMixedLua(null,null,null) is NULL;
SELECT checkedSumNullArgMixedLua(null,CAST(1313 as smallint),null) is NULL;
SELECT checkedSumNullArgMixedLua(1313,null,null) is NULL;
SELECT checkedSumNullArgMixedLua(null,null,3) is NULL;
SELECT checkedSumNullArgMixedLua(1313,CAST(1313 as smallint), 1313) = smallint '3939';
