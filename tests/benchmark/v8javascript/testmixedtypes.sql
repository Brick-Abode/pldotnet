CREATE OR REPLACE FUNCTION ageTestV8(name varchar, age integer, lname varchar) RETURNS varchar AS $$
var res;
if (age < 18)
    return "Hey "+name+" "+lname+"! Dude you are still a kid.";
else if (age >= 18 && age < 40)
    return "Hey "+name+" "+lname+"! You are in the mood!";
else
    return "Hey "+name+" "+lname+"! You are getting experienced!";
$$ LANGUAGE plv8;

SELECT ageTestV8('Billy', 10, 'The KID') = varchar 'Hey Billy The KID! Dude you are still a kid.';
SELECT ageTestV8('John', 33, 'Smith') =  varchar 'Hey John Smith! You are in the mood!';
SELECT ageTestV8('Robson', 41, 'Cruzoe') =  varchar 'Hey Robson Cruzoe! You are getting experienced!';

