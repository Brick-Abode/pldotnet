CREATE OR REPLACE FUNCTION fibbbPerl(n integer) RETURNS integer AS $$
sub fibonacci
{
my $x = $_[0];
if ($x <= 1)
{
    return $x;
}
else
{
    return fibonacci($x - 1) + fibonacci($x - 2);
}
}
return fibonacci($_[0]);
$$ LANGUAGE plperl;
SELECT fibbbPerl(30) = integer '832040';

CREATE OR REPLACE FUNCTION factPerl(n integer) RETURNS integer AS $$
sub fact
{
my $x = $_[0];
if ($x <= 1)
{
    return 1;
}
else
{
    return $x * fact($x - 1);
}
}
return fact($_[0]);
$$ LANGUAGE plperl;
SELECT factPerl(5) = integer '120';

-- CREATE OR REPLACE FUNCTION naturalPerl(n numeric) RETURNS numeric AS $$
-- plpy.notice(n)
-- if (n < 0):
--     return 0
-- elif (n == 1):
--     return 1
-- else:
--     return plpy.execute("SELECT naturalPerl(%f) as n" % (n-1))[0]["n"]
-- $$ LANGUAGE plperl;
-- SELECT naturalPerl(10) =  numeric '1';
-- SELECT naturalPerl(10.5) = numeric '0';
