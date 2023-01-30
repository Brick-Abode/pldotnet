CREATE OR REPLACE FUNCTION returnCompositeSumV8() RETURNS integer AS $$
var rows =  plv8.execute("SELECT 1 as c, 2 as b");
var sum = 0;
for (var r = 0; r < rows.length; r++) {
    sum += rows[r].b + rows[r].c;
}
return sum;
$$ LANGUAGE plv8;
SELECT returnCompositeSumV8() = integer '3';

DROP TABLE IF EXISTS pldotnettypes;
CREATE TABLE pldotnettypes (
    bcol  BOOLEAN,
    i2col SMALLINT,
    i4col INTEGER,
    i8col BIGINT,
    f4col FLOAT,
    f8col DOUBLE PRECISION,
    ncol  NUMERIC,
    vccol VARCHAR
);
INSERT INTO pldotnettypes VALUES (
    true,
    CAST(1 as INT2),
    CAST(32767 as INT4),
    CAST(9223372036854775707 as BIGINT),
    CAST(1.4 as FLOAT),
    CAST(10.5000000000055 as DOUBLE PRECISION),
    CAST(1.2 as NUMERIC),
    'StringSample;'
);
CREATE OR REPLACE FUNCTION checkTypesV8() RETURNS boolean AS $$
var rows = plv8.execute("SELECT * from pldotnettypes");
for (var r = 0; r < rows.length; r++)
{   
    row = rows[r];
    if(
        typeof row.bcol != "boolean"
        && typeof row.i2col != "number"
        && typeof row.i4col != "number"
        && typeof row.i8col != "number"
        && typeof row.f4col != "number"
        && typeof row.f8col != "number"
        && typeof row.ncol != "number"
        && typeof row.vccol != "string"
    )
    {
        return false;
    }
}
return true;
$$ LANGUAGE plv8;
SELECT checkTypesV8() is true;

DROP TABLE IF EXISTS usersavings;
CREATE TABLE usersavings(ssnum int8, name varchar, sname varchar, balance float4);
INSERT INTO usersavings VALUES (123456789,'Homer','Simpson',2304.55);
INSERT INTO usersavings VALUES (987654321,'Charles Montgomery','Burns',3000000.65);

CREATE OR REPLACE FUNCTION getUsersWithBalanceV8(searchbalance real) RETURNS varchar AS $$
var rows = plv8.execute("SELECT * from usersavings");
var res = "User(s) found with "+searchbalance+" account balance";
for (var r = 0; r < rows.length; r++)
{   
    var user = rows[r];
    if(user.balance == searchbalance)
    {
       res += ", "+user.name+" "+user.sname+" (Social Security Number "+user.ssnum+")";
    }
}
res += ".";
return res;
$$ LANGUAGE plv8;
SELECT getUsersWithBalanceV8(2304.55) = varchar 'User(s) found with 2304.550048828125 account balance, Homer Simpson (Social Security Number 123456789).';

CREATE OR REPLACE FUNCTION getUserDescriptionV8(ssnum bigint) RETURNS varchar AS $$
var rows = plv8.execute("SELECT * from usersavings WHERE ssnum="+ssnum);
var res = "No user found";
for (var r = 0; r < rows.length; r++)
{   
    var user = rows[r];
    if(user.ssnum == ssnum){
        plv8.elog(INFO, user.balance);
        res = user.name+" "+user.sname+", Social security Number "+user.ssnum+", has "+user.balance+" account balance.";
    }
}
return res;
$$ LANGUAGE plv8;
SELECT getUserDescriptionV8(123456789) = varchar 'Homer Simpson, Social security Number 123456789, has 2304.550048828125 account balance.';
SELECT getUserDescriptionV8(987654321) = varchar 'Charles Montgomery Burns, Social security Number 987654321, has 3000000.75 account balance.';
