CREATE OR REPLACE FUNCTION ageTestPg(name varchar, age integer, lname varchar) RETURNS varchar AS $$
DECLARE
    res varchar;
BEGIN
    IF (age < 18) THEN
        RETURN concat('Hey ', name, ' ', lname, '! Dude you are still a kid.');
    ELSIF age >= 18 AND age < 40 THEN
        RETURN concat('Hey ',name, ' ', lname, '! You are in the mood!');
    ELSE
        RETURN concat('Hey ', name, ' ', lname, '! You are getting experienced!');
    END IF;
END
$$ LANGUAGE plpgsql;

SELECT ageTestPg('Billy', 10, 'The KID') = varchar 'Hey Billy The KID! Dude you are still a kid.';
SELECT ageTestPg('John', 33, 'Smith') =  varchar 'Hey John Smith! You are in the mood!';
SELECT ageTestPg('Robson', 41, 'Cruzoe') =  varchar 'Hey Robson Cruzoe! You are getting experienced!';
