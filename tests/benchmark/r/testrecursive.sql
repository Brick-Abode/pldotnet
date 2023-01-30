CREATE OR REPLACE FUNCTION fibbbR(n INT)
RETURNS INT AS $$
fibonacci <- function(n) {
    if (n <= 1) {
        return (n)
    } else {
        return (fibonacci(n-1) + fibonacci(n-2))
    }
}
return (fibonacci(n))
$$ LANGUAGE plr;
SELECT fibbbR(30) = integer '832040';

CREATE OR REPLACE FUNCTION factR(n integer) RETURNS integer AS $$
factorial <- function(n) {
    if (n <= 1) {
        return (1)
    } else {
        return (n * factorial(n-1))
    }
}
return (factorial(n))
$$ LANGUAGE plr;
SELECT factR(5) = integer '120';