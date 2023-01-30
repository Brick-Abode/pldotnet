CREATE TYPE Person AS (
    name            text,
    age             integer,
    weight          double precision,
    height          real,
    salary          double precision,
    married         boolean
);

CREATE OR REPLACE FUNCTION helloPersonAgeR(per Person) RETURNS integer AS $$
return(per$age)
$$ LANGUAGE plr;
SELECT helloPersonAgeR(('John Smith', 38, 85.5, 1.71, 999.999, true)) = integer '38';

CREATE OR REPLACE FUNCTION helloPersonR(p Person) RETURNS Person AS $$
n <- 1;
p$age <- p$age+n;
return(p)
$$ LANGUAGE plr;
SELECT helloPersonR(('John Smith', 38, 85.5, 1.71, 999.999, true));
