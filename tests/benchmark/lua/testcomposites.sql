CREATE TYPE Person AS (
    name            text,
    age             integer,
    weight          double precision,
    height          real,
    salary          double precision,
    married         boolean
);

CREATE OR REPLACE FUNCTION helloPersonAgeLua(per Person) RETURNS integer AS $$
return per.age;
$$ LANGUAGE pllua;
SELECT helloPersonAgeLua(('Jerry Smith', 38, 85.5, 1.71, 999.999, true)) = integer '38';

CREATE OR REPLACE FUNCTION helloPersonLua(p Person) RETURNS Person AS $$
p.age = p.age + 1;
return p;
$$ LANGUAGE pllua;

SELECT helloPersonLua(('Jerry Smith', 38, 85.5, 1.71, 999.999, true));
