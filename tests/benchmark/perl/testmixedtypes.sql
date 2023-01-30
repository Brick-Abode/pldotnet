CREATE OR REPLACE FUNCTION ageTestPerl(name varchar, age integer, lname varchar) RETURNS varchar AS $$
my ($name, $age, $lname) = @_;
if($age < 18){
    return "Hey $name $lname! Dude you are still a kid.";
}elsif($age >= 18 and $age < 40){
    return "Hey $name $lname! You are in the mood!";
}else{
    return "Hey $name $lname! You are getting experienced!";
}
$$ LANGUAGE plperl;

SELECT ageTestPerl('Billy', 10, 'The KID') =  varchar 'Hey Billy The KID! Dude you are still a kid.';
SELECT ageTestPerl('John', 33, 'Smith') =  varchar 'Hey John Smith! You are in the mood!';
SELECT ageTestPerl('Robson', 41, 'Cruzoe') =  varchar 'Hey Robson Cruzoe! You are getting experienced!';
