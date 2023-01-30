CREATE TYPE Person AS (
    name            text,
    age             integer,
    weight          double precision,
    height          real,
    salary          double precision,
    married         boolean
);

CREATE OR REPLACE FUNCTION helloPersonAgeV8(per Person) RETURNS integer AS $$
    var res = "Hello M(r)(s). "+per.name+"! Your age "+per.age+", "+per.weight+", "+per.height+", "+per.salary+". "+per.married;
    return per.age;
$$ LANGUAGE plv8;
SELECT helloPersonAgeV8(('John Smith', 38, 85.5, 1.71, 999.999, true)) = integer '38';

CREATE OR REPLACE FUNCTION helloPersonV8(p Person) RETURNS Person AS $$
    var n = 1;
    p.age += n;
    var res = "Hello M(r)(s). "+p.name+"! Your age "+p.age+", "+p.weight+", "+p.height+", "+p.salary+". "+p.married;
    return p;
$$ LANGUAGE plv8;
SELECT helloPersonV8(('John Smith', 38, 85.5, 1.71, 999.999, true));
