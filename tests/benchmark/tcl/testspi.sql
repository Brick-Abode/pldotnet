CREATE OR REPLACE FUNCTION returnCompositeSumTcl() RETURNS integer AS $$
set sum 0;
spi_exec -array row "SELECT 1 as c, 2 as b" {
    set sum [expr $sum + $row(b) + $row(c)]
}
return $sum;
$$ LANGUAGE pltcl;
SELECT returnCompositeSumTcl() = integer '3';

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
CREATE OR REPLACE FUNCTION checkTypesTcl() RETURNS boolean AS $$
spi_exec -array row "SELECT * from pldotnettypes" {
    if {
        ![string is bool -strict $row(bcol)]
        && ![string is integer -strict $row(i2col)]
        && ![string is integer -strict $row(i4col)]
        && ![string is integer -strict $row(i8col)]
        && ![string is float -strict $row(f4col)]
        && ![string is float -strict $row(f8col)]
        && ![string is float -strict $row(ncol)]
        && ![string is string -strict $row(vcol)]
    } {
        return "f";
    }
}
return "t";
$$ LANGUAGE pltcl;
SELECT checkTypesTcl() is true;

DROP TABLE IF EXISTS usersavings;
CREATE TABLE usersavings(ssnum int8, name varchar, sname varchar, balance float4);
INSERT INTO usersavings VALUES (123456789,'Homer','Simpson',2304.55);
INSERT INTO usersavings VALUES (987654321,'Charles Montgomery','Burns',3000000.65);
CREATE OR REPLACE FUNCTION getUsersWithBalanceTcl(searchbalance real) RETURNS varchar AS $$
set res [concat User(s) found with $1 account balance];
spi_exec -array user "SELECT * from usersavings" { 
    if {$user(balance) == $1} {
        set res [concat $res, $user(name) $user(sname) (Social Security Number $user(ssnum))];
    }
}
set res [concat $res.];
return $res;
$$ LANGUAGE pltcl;
SELECT getUsersWithBalanceTcl(2304.55) = varchar 'User(s) found with 2304.55 account balance, Homer Simpson (Social Security Number 123456789).';

CREATE OR REPLACE FUNCTION getUserDescriptionTcl(ssnum bigint) RETURNS varchar AS $$
set res "No user found"
spi_exec -array user [concat SELECT * from usersavings WHERE ssnum=$1] { 
    set res [concat $user(name) $user(sname), Social security Number $user(ssnum), has $user(balance) account balance.]
}
return $res;
$$ LANGUAGE pltcl;

SELECT getUserDescriptionTcl(123456789) = varchar 'Homer Simpson, Social security Number 123456789, has 2304.55 account balance.';
SELECT getUserDescriptionTcl(987654321) = varchar 'Charles Montgomery Burns, Social security Number 987654321, has 3e+06 account balance.';
