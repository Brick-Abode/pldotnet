CREATE OR REPLACE FUNCTION ageTestR(name varchar, age integer, lname varchar) RETURNS varchar AS $$
res <- ''
if (age < 18)
    return(paste('Hey ', name, ' ', lname, '! Dude you are still a kid.', sep=''))
else if (age >= 18 && age < 40)
    return(paste('Hey ', name, ' ', lname, '! You are in the mood!', sep=''))
else
    return(paste('Hey ', name, ' ', lname, '! You are getting experienced!', sep=''))
$$ LANGUAGE plr;

SELECT ageTestR('Billy', 10, 'The KID') = varchar 'Hey Billy The KID! Dude you are still a kid.';
SELECT ageTestR('John', 33, 'Smith') =  varchar 'Hey John Smith! You are in the mood!';
SELECT ageTestR('Robson', 41, 'Cruzoe') =  varchar 'Hey Robson Cruzoe! You are getting experienced!';
