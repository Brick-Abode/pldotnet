CREATE TYPE Person AS (
    name            text,
    age             integer,
    weight          double precision,
    height          real,
    salary          double precision,
    married         boolean
);

CREATE OR REPLACE FUNCTION helloPersonAgeJava(Person)
  RETURNS INTEGER
  AS 'com.example.proj.TestComposites.helloPersonAgeJava'
  IMMUTABLE LANGUAGE java;

SELECT helloPersonAgeJava(('John Smith', 38, 85.5, 1.71, 999.999, true)) = integer '38';

CREATE OR REPLACE FUNCTION helloPersonJava(Person)
  RETURNS Person
  AS 'com.example.proj.TestComposites.helloPersonJava'
  IMMUTABLE LANGUAGE java;

SELECT helloPersonJava(('John Smith', 38, 85.5, 1.71, 999.999, true));
