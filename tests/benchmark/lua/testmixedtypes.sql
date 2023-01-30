/* Not supported */

CREATE OR REPLACE FUNCTION ageTestLua(name varchar, age integer, lname varchar) RETURNS varchar AS $$
if age < 18 then
    return "Hey " .. name .. " " .. lname .. "! Dude you are still a kid."
elseif age >= 18 and age < 40 then
    return "Hey " .. name .. " " .. lname .. "! You are in the mood!"
else
    return "Hey " .. name .. " " .. lname .. "! You are getting experienced!"
end
$$ LANGUAGE pllua;

SELECT ageTestLua('Billy', 10, 'The KID') =  varchar 'Hey Billy The KID! Dude you are still a kid.';
SELECT ageTestLua('John', 33, 'Smith') =  varchar 'Hey John Smith! You are in the mood!';
SELECT ageTestLua('Robson', 41, 'Cruzoe') =  varchar 'Hey Robson Cruzoe! You are getting experienced!';
