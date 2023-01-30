CREATE OR REPLACE FUNCTION returnCompositeSumPython() RETURNS integer AS $$
exp = plpy.execute("SELECT 1 as c, 2 as b", 1)
sum = 0
for row in exp:
    sum += row['b'] + row['c']
return sum
$$ LANGUAGE plpython3u;

SELECT returnCompositeSumPython() = integer '3';

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

CREATE OR REPLACE FUNCTION checkTypesPython() RETURNS boolean AS $$
exp = plpy.execute("SELECT * from pldotnettypes", 1)
for row in exp:
    if( type(row['bcol']) != bool and type(row['i2col']) != float and
        type(row['i4col']) != int and type(row['i8col']) != float and
        type(row['f4col']) != float and type(row['f8col']) != double and
        type(row['ncol'])  != decimal and type(row['vccol']) != string ):
      return False;
return True;
$$ LANGUAGE plpython3u;

SELECT checkTypesPython() is true;

DROP TABLE IF EXISTS usersavings;
CREATE TABLE usersavings(ssnum int8, name varchar, sname varchar, balance float4);

INSERT INTO usersavings VALUES (123456789,'Homer','Simpson',2304.55);

INSERT INTO usersavings VALUES (987654321,'Charles Montgomery','Burns',3000000.65);

CREATE OR REPLACE FUNCTION getUsersWithBalancePython(searchbalance real) RETURNS varchar AS $$
exp = plpy.execute("SELECT * from usersavings", 1)
res = f"User(s) found with {searchbalance} account balance"
for user in exp:
    if(user['balance'] == searchbalance):
       res += f", {user['name']} {user['sname']} (Social Security Number {user['ssnum']})";
res += "."
return res
$$ LANGUAGE plpython3u;

SELECT getUsersWithBalancePython(2304.55) = varchar 'User(s) found with 2304.550048828125 account balance, Homer Simpson (Social Security Number 123456789).';

CREATE OR REPLACE FUNCTION getUserDescriptionPython(ssnum bigint) RETURNS varchar AS $$
exp = plpy.execute(f"SELECT * from usersavings WHERE ssnum={ssnum}", 1)
res = "No user found";
for user in exp:
    if(user['ssnum'] == ssnum):
        res = f"{user['name']} {user['sname']}, Social security Number {user['ssnum']}, has {user['balance']} account balance."
return res
$$ LANGUAGE plpython3u;

SELECT getUserDescriptionPython(123456789) = varchar 'Homer Simpson, Social security Number 123456789, has 2304.550048828125 account balance.';

SELECT getUserDescriptionPython(987654321) = varchar 'Charles Montgomery Burns, Social security Number 987654321, has 3000000.75 account balance.';
