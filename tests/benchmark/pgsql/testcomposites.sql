CREATE TYPE Person AS (
    name            text,
    age             integer,
    weight          double precision,
    height          real,
    salary          double precision,
    married         boolean
);

CREATE OR REPLACE FUNCTION helloPersonAgePg(per Person) RETURNS integer AS $$
DECLARE
BEGIN
    RETURN per.age;
end
$$ LANGUAGE plpgsql;
SELECT helloPersonAgePg(('John Smith', 38, 85.5, 1.71, 999.999, true)) = integer '38';

CREATE OR REPLACE FUNCTION helloPersonPg(p Person) RETURNS Person AS $$
DECLARE
    n int;
BEGIN
    n := 1;
    p.age := p.age+ n;
    RETURN p;
END
$$ LANGUAGE plpgsql;
SELECT helloPersonPg(('John Smith', 38, 85.5, 1.71, 999.999, true));
