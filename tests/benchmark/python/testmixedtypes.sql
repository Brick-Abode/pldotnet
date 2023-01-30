CREATE OR REPLACE FUNCTION ageTestPython(name varchar, age integer, lname varchar) RETURNS varchar AS $$
res = ""
if (age < 18):
    return "Hey "+name+" "+lname+"! Dude you are still a kid."
elif (age >= 18 and age < 40):
    return "Hey "+name+" "+lname+"! You are in the mood!"
else:
    return "Hey "+name+" "+lname+"! You are getting experienced!"
$$ LANGUAGE plpython3u;

SELECT ageTestPython('Billy', 10, 'The KID') = varchar 'Hey Billy The KID! Dude you are still a kid.';
SELECT ageTestPython('John', 33, 'Smith') =  varchar 'Hey John Smith! You are in the mood!';
SELECT ageTestPython('Robson', 41, 'Cruzoe') =  varchar 'Hey Robson Cruzoe! You are getting experienced!';
