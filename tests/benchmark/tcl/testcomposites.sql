CREATE TYPE Person AS (
    name            text,
    age             integer,
    weight          double precision,
    height          real,
    salary          double precision,
    married         boolean
);

CREATE OR REPLACE FUNCTION helloPersonAgeTcl(per Person) RETURNS integer AS $$
return $1(age);
$$ LANGUAGE pltcl;
SELECT helloPersonAgeTcl(('John Smith', 38, 85.5, 1.71, 999.999, true)) = integer '38';

CREATE OR REPLACE FUNCTION helloPersonTcl(p Person) RETURNS Person AS $$
set n 1;
set 1(age) [expr {$1(age) + $n}]
return [array get 1]
$$ LANGUAGE pltcl;

SELECT helloPersonTcl(('John Smith', 38, 85.5, 1.71, 999.999, true));
