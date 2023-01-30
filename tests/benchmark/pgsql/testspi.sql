CREATE OR REPLACE FUNCTION returnCompositeSumPg() RETURNS integer AS $$
DECLARE 
    sum integer;
    temprow record;
BEGIN
    sum := 0;
    FOR temprow IN
        SELECT 1 as c, 2 as b
    LOOP
        sum := sum + temprow.b + temprow.c;
    END LOOP;
    RETURN sum;
END
$$ LANGUAGE plpgsql;
SELECT returnCompositeSumPg() = integer '3';

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
CREATE OR REPLACE FUNCTION checkTypesPg() RETURNS boolean AS $$
DECLARE
    temprow record;
BEGIN
    FOR temprow IN
        SELECT pg_typeof(bcol)  as bcol,
               pg_typeof(i2col) as i2col,
               pg_typeof(i4col) as i4col,
               pg_typeof(i8col) as i8col,
               pg_typeof(f4col) as f4col,
               pg_typeof(f8col) as f8col,
               pg_typeof(ncol)  as ncol,
               pg_typeof(vccol) as vccol
        FROM pldotnettypes
    LOOP
        IF 
            temprow.bcol != 'boolean'::regtype 
            AND temprow.i2col != 'smallint'::regtype 
            AND temprow.i4col != 'integer'::regtype 
            AND temprow.i8col != 'bigint'::regtype 
            AND temprow.f4col != 'double precision'::regtype 
            AND temprow.f8col != 'double precision'::regtype 
            AND temprow.ncol != 'numeric'::regtype 
            AND temprow.vccol != 'character varying'::regtype 
        THEN
            RETURN false;
        END IF;
    END LOOP;
    RETURN true;
END
$$ LANGUAGE plpgsql;
SELECT checkTypesPg() is true;

DROP TABLE IF EXISTS usersavings;
CREATE TABLE usersavings(ssnum int8, name varchar, sname varchar, balance float4);
INSERT INTO usersavings VALUES (123456789,'Homer','Simpson',2304.55);
INSERT INTO usersavings VALUES (987654321,'Charles Montgomery','Burns',3000000.65);
CREATE OR REPLACE FUNCTION getUsersWithBalancePg(searchbalance real) RETURNS varchar AS $$
DECLARE 
    res varchar;
    temprow record;
BEGIN
    res := concat('User(s) found with ',searchbalance,' account balance');
    FOR temprow IN
        SELECT * from usersavings
    LOOP
        IF temprow.balance = searchbalance THEN
            res := concat(res,', ',temprow.name,' ', temprow.sname,' (Social Security Number ',temprow.ssnum, ')');
        END IF;
    END LOOP;
    res := concat(res, '.');
    RETURN res;
END
$$ LANGUAGE plpgsql;
SELECT getUsersWithBalancePg(2304.55) = varchar 'User(s) found with 2304.55 account balance, Homer Simpson (Social Security Number 123456789).';

CREATE OR REPLACE FUNCTION getUserDescriptionPg(ssnumParam bigint) RETURNS varchar AS $$
DECLARE
    temprow record;
    res varchar;
BEGIN
    res := 'No user found';
    FOR temprow IN
        SELECT * from usersavings WHERE usersavings.ssnum=ssnumParam
    LOOP
        IF temprow.ssnum = ssnumParam THEN
            res := concat(temprow.name,' ', temprow.sname,', Social security Number ',temprow.ssnum,', has ',temprow.balance,' account balance.');
        END IF;
    END LOOP;
    RETURN res;
END
$$ LANGUAGE plpgsql;

SELECT getUserDescriptionPg(123456789) = varchar 'Homer Simpson, Social security Number 123456789, has 2304.55 account balance.';
SELECT getUserDescriptionPg(987654321) = varchar 'Charles Montgomery Burns, Social security Number 987654321, has 3e+06 account balance.';
