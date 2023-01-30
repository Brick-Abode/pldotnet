CREATE TYPE Person AS (
    name            text,
    age             integer,
    weight          double precision,
    height          real,
    salary          double precision,
    married         boolean
);

CREATE OR REPLACE FUNCTION helloPersonAgePython(per Person) RETURNS integer AS $$
return per['age']
$$ LANGUAGE plpython3u;
SELECT helloPersonAgePython(('John Smith', 38, 85.5, 1.71, 999.999, true)) = integer '38';

CREATE OR REPLACE FUNCTION helloPersonPython(p Person) RETURNS Person AS $$
p["age"] += 1
return p
$$ LANGUAGE plpython3u;
SELECT helloPersonPython(('John Smith', 38, 85.5, 1.71, 999.999, true));
