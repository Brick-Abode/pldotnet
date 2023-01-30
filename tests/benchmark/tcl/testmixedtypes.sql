CREATE OR REPLACE FUNCTION ageTestTcl(name varchar, age integer, lname varchar) RETURNS varchar AS $$
if {$2 < 18} {
    return [concat Hey $1 $3! Dude you are still a kid.];
} elseif {$2 >= 18 && $2 < 40} {
    return [concat Hey $1 $3! You are in the mood!];
} else {
    return [concat Hey $1 $3! You are getting experienced!];
}
$$ LANGUAGE pltcl;

SELECT ageTestTcl('Billy', 10, 'The KID') = varchar 'Hey Billy The KID! Dude you are still a kid.';
SELECT ageTestTcl('John', 33, 'Smith') =  varchar 'Hey John Smith! You are in the mood!';
SELECT ageTestTcl('Robson', 41, 'Cruzoe') =  varchar 'Hey Robson Cruzoe! You are getting experienced!';
