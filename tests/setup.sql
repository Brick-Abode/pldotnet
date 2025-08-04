DROP TABLE IF EXISTS automated_test_results;
CREATE TABLE automated_test_results(ID SERIAL PRIMARY KEY, FEATURE TEXT, TEST_NAME TEXT, RESULT boolean);

DROP TABLE IF EXISTS SPITEST;
CREATE TABLE SPITEST (
    BOOLCOL BOOLEAN,
    I2COL SMALLINT,
    I4COL INTEGER,
    I8COL BIGINT,
    F4COL FLOAT4,
    F8COL FLOAT8,
    POINTCOL POINT,
    LINECOL LINE,
    LSEGCOL LSEG,
    BOXCOL BOX,
    POLYGONCOL POLYGON,
    PATHCOL PATH,
    CIRCLECOL CIRCLE,
    DATECOL DATE,
    TIMECOL TIME,
    TIMETZCOL TIMETZ,
    TIMESTAMPCOL TIMESTAMP,
    TIMESTAMPTZCOL TIMESTAMP WITH TIME ZONE,
    INTERVALCOL INTERVAL,
    MACCOL MACADDR,
    MAC8COL MACADDR8,
    INETCOL INET,
    CIDRCOL CIDR,
    MONEYCOL MONEY,
    VARBITCOL BIT VARYING(64),
    BITCOL BIT(8),
    BYTEACOL BYTEA,
    TEXTCOL TEXT,
    CHARCOL CHAR(10),
    VARCHARCOL VARCHAR(10),
    XMLCOL XML,
    JSONCOL JSON,
    UUIDCOL UUID,
    I4RCOL INT4RANGE,
    I8RCOL INT8RANGE,
    TSRCOL TSRANGE,
    TSTZRCOL TSTZRANGE,
    DRCOL DATERANGE
);
INSERT INTO SPITEST(
    BOOLCOL,
    I2COL,
    I4COL,
    I8COL,
    F4COL,
    F8COL,
    POINTCOL,
    LINECOL,
    LSEGCOL,
    BOXCOL,
    POLYGONCOL,
    PATHCOL,
    CIRCLECOL,
    DATECOL,
    TIMECOL,
    TIMETZCOL,
    TIMESTAMPCOL,
    TIMESTAMPTZCOL,
    INTERVALCOL,
    MACCOL,
    MAC8COL,
    INETCOL,
    CIDRCOL,
    MONEYCOL,
    VARBITCOL,
    BITCOL,
    BYTEACOL,
    TEXTCOL,
    CHARCOL,
    VARCHARCOL,
    XMLCOL,
    JSONCOL,
    UUIDCOL,
    I4RCOL,
    I8RCOL,
    TSRCOL,
    TSTZRCOL,
    DRCOL
)
VALUES (
    TRUE,
    2023,
    327670,
    21474836470,
    10.20212,
    10.202120222023202,
    '(1.0,2.0)',
    '{1.0,2.0,3.0}',
    '((1.0,1.0),(2.0,2.0))',
    '((1.0,1.0),(2.0,2.0))',
    '((1.0,1.0),(2.0,2.0))',
    '( (1.0,1.0), (2.0,1.0), (2.0,2.0), (2.0,1.0) )',
    '<(1.0,1.0),0.5>',
    '1989-07-25',
    '12:00:01',
    '05:30-03:00',
    '1989-07-25 12:00:01',
    '1989-07-25 12:00:01 America/New_York',
    ('1999-03-24 19:18:17 America/Los_Angeles'::timestamp - '1989-07-25 12:00:01 America/New_York'::timestamp),
    'f6:30:00:00:00:00',
    'f6:30:00:ff:fe:00:00:00',
    '207.69.188.185/24',
    '207.69.188.185',
    '31415.92',
    '100111001',
    '10011001',
    '\x5468616e6b20796f7521',
    'hello',
    'good bye',
    'Bye Bye',
    '<?xml version="1.0" encoding="utf-8"?><title>Hello, World!</title>',
    '{"a":"Sunday", "b":"Monday", "c":"Tuesday"}',
    '123e4567-e89b-12d3-a456-426614174000',
    '(-2147483648,2147483644)',
    '[,9223372036854775804)',
    '[2010-01-01 14:30, 2010-01-01 15:30)',
    '["2013-10-01 07:00:00-03","2013-10-01 07:15:00-03")',
    '[2020-01-01,2021-01-01)'
);

DROP TABLE IF EXISTS SPINULLS;
CREATE TABLE SPINULLS (
    BOOLCOL BOOLEAN, -- basic type
    I4COL INTEGER, -- basic type
    F8COL FLOAT8, -- basic type
    DATECOL DATE, -- complex struct
    TEXTCOL TEXT, -- object
    MAC8COL MACADDR8 -- object
);
INSERT INTO SPINULLS(
    BOOLCOL,
    I4COL,
    F8COL,
    DATECOL,
    TEXTCOL,
    MAC8COL
)
VALUES (
    NULL::BOOLEAN,
    NULL::INTEGER,
    NULL::FLOAT8,
    '2023-02-01'::DATE,
    'Hello Terrible World!'::TEXT,
    'ab-01-2b-31-41-fa-ab-ac'::MACADDR8
);
INSERT INTO SPINULLS(
    BOOLCOL,
    I4COL,
    F8COL,
    DATECOL,
    TEXTCOL,
    MAC8COL
)
VALUES (
    'TRUE'::BOOLEAN,
    '2023'::INTEGER,
    '3.141592653'::FLOAT8,
    NULL::DATE,
    NULL::TEXT,
    NULL::MACADDR8
);

CREATE OR REPLACE FUNCTION numbers(count int4)
RETURNS SETOF int4 AS
$$
	if(count == null){ for(int i=0;;i++) { yield return i; } }
	else { for(int i=0;i<count;i++) { yield return i; } }
$$
LANGUAGE plcsharp;

CREATE OR REPLACE FUNCTION numbers(count int8)
RETURNS SETOF int8 AS
$$
	if(count == null){ for(long i=0;;i++) { yield return i; } }
	else { for(long i=0;i<count;i++) { yield return i; } }
$$
LANGUAGE plcsharp;

CREATE OR REPLACE FUNCTION numbers()
RETURNS SETOF int8 AS
$$
	for(long i=0;;i++){ yield return i;}
$$
LANGUAGE plcsharp;

CREATE OR REPLACE FUNCTION string_to_integer_array_plsql(strings text[])
RETURNS SETOF integer AS
$$
    DECLARE
      value text;
    BEGIN
      FOREACH value IN ARRAY strings
      LOOP
        IF value IS NULL THEN
          RETURN NEXT 0;
        ELSE
          RETURN NEXT value::integer;
        END IF;
      END LOOP;
      RETURN;
    END;
$$
LANGUAGE plpgsql;

CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE OR REPLACE FUNCTION ten_items(arg text)
RETURNS SETOF text AS
$$
        for(int i=1; i<=10; i++){ yield return $"{i} {arg}"; }
$$
LANGUAGE plcsharp STRICT;

-- Creating auxiliary table for trigger tests (C# and F#)
DROP TABLE IF EXISTS trigger_test_table;

CREATE TABLE trigger_test_table(
    id      INT,
    message TEXT
);
