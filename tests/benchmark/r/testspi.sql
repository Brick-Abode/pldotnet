CREATE OR REPLACE FUNCTION returnCompositeSumR() RETURNS integer AS $$
exp <- pg.spi.prepare("SELECT 1 as c, 2 as b")
sum <- 0

cursor_obj <- pg.spi.cursor_open('my_cursor', exp)

row <- pg.spi.cursor_fetch(cursor_obj, TRUE, as.integer(1));

sum <- sum + row$b + row$c

pg.spi.cursor_close(cursor_obj);

return(sum)
$$ LANGUAGE plr;
SELECT returnCompositeSumR() = integer '3';

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
CREATE OR REPLACE FUNCTION checkTypesR() RETURNS boolean AS $$
exp <- pg.spi.prepare("SELECT * from pldotnettypes")

cursor_obj <- pg.spi.cursor_open('my_cursor', exp)

row <- pg.spi.cursor_fetch(cursor_obj, TRUE, as.integer(1))

if(
    typeof(row$bcol) != typeof(TRUE)
    & typeof(row$i2col) != typeof(12)
    & typeof(row$i4col) != typeof(1231231)
    & typeof(row$i8col) != typeof(123123123123123)
    & typeof(row$f4col) != typeof(5.33)
    & typeof(row$f8col) != typeof(5.3345444444)
    & typeof(row$ncol) != typeof(5.3345444444)
    & typeof(row$vccol) != typeof('1231231')
) {

    return(FALSE)
}

pg.spi.cursor_close(cursor_obj);

return(TRUE)
$$ LANGUAGE plr;
SELECT checkTypesR() is true;


DROP TABLE IF EXISTS usersavings;
CREATE TABLE usersavings(ssnum int8, name varchar, sname varchar, balance float4);
INSERT INTO usersavings VALUES (123456789,'Homer','Simpson',2304.55);
INSERT INTO usersavings VALUES (987654321,'Charles Montgomery','Burns',3000000.65);
CREATE OR REPLACE FUNCTION getUsersWithBalanceR(searchbalance real) RETURNS varchar AS $$
res <- paste("User(s) found with ", searchbalance, " account balance", sep="");

exp <- pg.spi.prepare("SELECT * from usersavings")

cursor_obj <- pg.spi.cursor_open('my_cursor', exp)

user <- pg.spi.cursor_fetch(cursor_obj, TRUE, as.integer(1))

if(user$balance == searchbalance) {
    res <- paste(res, ", ", user$name, " ", user$sname, " (Social Security Number ", user$ssnum, ")", sep="")

    res <- paste(res, ".", sep="")
}

pg.spi.cursor_close(cursor_obj);

return(res)
$$ LANGUAGE plr;
SELECT getUsersWithBalanceR(2304.55) = varchar 'User(s) found with 2304.55 account balance, Homer Simpson (Social Security Number 123456789).';

CREATE OR REPLACE FUNCTION getUserDescriptionR(ssnum bigint) RETURNS varchar AS $$
res <- "No user found"

exp <- pg.spi.prepare(paste("SELECT * from usersavings WHERE ssnum=", ssnum, sep=""))

cursor_obj <- pg.spi.cursor_open('my_cursor', exp)

user <- pg.spi.cursor_fetch(cursor_obj, TRUE, as.integer(1))

res <- paste(user$name, " ", user$sname, ", Social security Number ", user$ssnum, ", has ", user$balance, " account balance.", sep="");

pg.spi.cursor_close(cursor_obj);

return(res);
$$ LANGUAGE plr;

SELECT getUserDescriptionR(123456789) = varchar 'Homer Simpson, Social security Number 123456789, has 2304.55 account balance.';
SELECT getUserDescriptionR(987654321) = varchar 'Charles Montgomery Burns, Social security Number 987654321, has 3e+06 account balance.';
