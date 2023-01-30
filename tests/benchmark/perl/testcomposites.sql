CREATE TYPE Person AS (
    name            text,
    age             integer,
    weight          double precision,
    height          real,
    salary          double precision,
    married         boolean
);

CREATE OR REPLACE FUNCTION helloPersonAgePerl(per Person) RETURNS integer AS $$
my ($person) = @_;
return $person->{age};
$$ LANGUAGE plperl;
SELECT helloPersonAgePerl(('John Smith', 38, 85.5, 1.71, 999.999, true)) = integer '38';

CREATE OR REPLACE FUNCTION helloPersonPerl(p Person) RETURNS Person AS $$
my ($person) = @_;
$person->{age} = $person->{age} + 1;
return $person;
$$ LANGUAGE plperl;

SELECT helloPersonPerl(('John Smith', 38, 85.5, 1.71, 999.999, true));
