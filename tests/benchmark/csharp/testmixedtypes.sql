CREATE OR REPLACE FUNCTION ageTest(name varchar, age integer, lname varchar) RETURNS varchar AS $$
FormattableString res;
if (age < 18)
    return "Hey "+name+" "+lname+"! Dude you are still a kid.";
else if (age >= 18 && age < 40)
    return "Hey "+name+" "+lname+"! You are in the mood!";
else
    return "Hey "+name+" "+lname+"! You are getting experienced!";
$$ LANGUAGE plcsharp;

SELECT ageTest('Billy', 10, 'The KID') = varchar 'Hey Billy The KID! Dude you are still a kid.';
SELECT ageTest('John', 33, 'Smith') =  varchar 'Hey John Smith! You are in the mood!';
SELECT ageTest('Robson', 41, 'Cruzoe') =  varchar 'Hey Robson Cruzoe! You are getting experienced!';

