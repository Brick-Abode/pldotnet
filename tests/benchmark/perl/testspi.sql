CREATE OR REPLACE FUNCTION returnCompositeSumPerl() RETURNS integer AS $$
my $rv = spi_exec_query("SELECT 1 as c, 2 as b", 1);
my $nrows = $rv->{processed};
my $sum = 0;
foreach my $rn (0 .. $nrows - 1) {
   $sum = $sum + $rv->{rows}[$rn]->{b} + $rv->{rows}[$rn]->{c};
}
return $sum;
$$ LANGUAGE plperl;

SELECT returnCompositeSumPerl() = integer '3';

DROP TABLE IF EXISTS usersavings;
CREATE TABLE usersavings(ssnum int8, name varchar, sname varchar, balance float4);

INSERT INTO usersavings VALUES (123456789,'Homer','Simpson',2304.55);

INSERT INTO usersavings VALUES (987654321,'Charles Montgomery','Burns',3000000.65);

CREATE OR REPLACE FUNCTION getUsersWithBalancePerl(searchbalance real) RETURNS varchar AS $$
my $rv = spi_exec_query("SELECT * from usersavings", 1);
my $nrows = $rv->{processed};

my ($searchbalance, ) = @_;

my $res = "User(s) found with " . $searchbalance . " account balance";

foreach my $rn (0 .. $nrows - 1) {
        $res = "$res, $rv->{rows}[$rn]->{name} $rv->{rows}[$rn]->{sname} (Social Security Number $rv->{rows}[$rn]->{ssnum})";
}
return "$res.";
$$ LANGUAGE plperl;

SELECT getUsersWithBalancePerl(2304.55) = varchar 'User(s) found with 2304.55 account balance, Homer Simpson (Social Security Number 123456789).';

CREATE OR REPLACE FUNCTION getUserDescriptionPerl(ssnum bigint) RETURNS varchar AS $$
my ($ssnum, ) = @_;
my $rv = spi_exec_query("SELECT * from usersavings", 2);
my $nrows = $rv->{processed};

my $res = "No user found";

foreach my $rn (0 .. $nrows - 1) {
   if($rv->{rows}[$rn]->{ssnum} == $ssnum) {
        $res = "$rv->{rows}[$rn]->{name} $rv->{rows}[$rn]->{sname}, Social security Number $rv->{rows}[$rn]->{ssnum}, has $rv->{rows}[$rn]->{balance} account balance."
   }
}
return $res
$$ LANGUAGE plperl;

SELECT getUserDescriptionPerl(123456789) = varchar 'Homer Simpson, Social security Number 123456789, has 2304.55 account balance.';

SELECT getUserDescriptionPerl(987654321) = varchar 'Charles Montgomery Burns, Social security Number 987654321, has 3e+06 account balance.';
