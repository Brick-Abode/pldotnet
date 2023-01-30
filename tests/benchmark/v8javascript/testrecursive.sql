
CREATE OR REPLACE FUNCTION fibbbV8(n integer) RETURNS integer AS $$
    function fibonacci(n) {
        if (n <= 1) {
            return n;
        } else {
            return fibonacci(n-1) + fibonacci(n-2);
        }
    }
    return fibonacci(n);
$$ LANGUAGE plv8;
SELECT fibbbV8(30) = integer '832040';

CREATE OR REPLACE FUNCTION factV8(n integer) RETURNS integer AS $$
    function factorial(n) {
        if (n <= 1) {
            return 1;
        } else {
            return n * factorial(n-1);
        }
    }
    return factorial(n);
$$ LANGUAGE plv8;
SELECT factV8(5) = integer '120';

-- CREATE OR REPLACE FUNCTION naturalV8(n numeric) RETURNS numeric AS $$
--     const naturalV8 = plv8.find_function('naturalV8')
--     if (n < 0)
--         return 0;
--     else if (n == 1)
-- 	return 1;
--     else
--     	return naturalV8(n - 1);
-- $$ LANGUAGE plv8;
-- SELECT naturalV8(10) =  numeric '1';
-- SELECT naturalV8(10.5) = numeric '0';
