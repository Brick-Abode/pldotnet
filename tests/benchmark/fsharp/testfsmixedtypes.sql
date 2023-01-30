CREATE OR REPLACE FUNCTION ageTestFSharp(name varchar, age integer, lname varchar) RETURNS varchar AS $$
let _age = age.Value
if (_age < 18) then
    "Hey "+name+" "+lname+"! Dude you are still a kid."
elif (_age >= 18 && _age < 40) then
    "Hey "+name+" "+lname+"! You are in the mood!"
else
    "Hey "+name+" "+lname+"! You are getting experienced!"
$$ LANGUAGE plfsharp;
SELECT ageTestFSharp('Billy', 10, 'The KID') = varchar 'Hey Billy The KID! Dude you are still a kid.';
SELECT ageTestFSharp('John', 33, 'Smith') =  varchar 'Hey John Smith! You are in the mood!';
SELECT ageTestFSharp('Robson', 41, 'Cruzoe') =  varchar 'Hey Robson Cruzoe! You are getting experienced!';

