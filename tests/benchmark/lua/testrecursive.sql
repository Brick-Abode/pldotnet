/* Not supported */

CREATE OR REPLACE FUNCTION fibbbLua(n integer) RETURNS integer AS $$
function fibonacci(n)
    if n <= 1 then
        return n
    else
        return fibonacci(n-1) + fibonacci(n-2)
    end
end
return fibonacci(n)
$$ LANGUAGE pllua;
SELECT fibbbLua(30) = integer '832040';

CREATE OR REPLACE FUNCTION factLua(n integer) RETURNS integer AS $$
function factorial(n)
    if n <= 1 then
        return 1
    else
        return n * factorial(n-1)
    end
end
return factorial(n)
$$ LANGUAGE pllua;
SELECT factLua(5) = integer '120';

