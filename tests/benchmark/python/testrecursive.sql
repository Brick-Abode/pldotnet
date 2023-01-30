CREATE OR REPLACE FUNCTION fibbbPython(n integer) RETURNS integer AS $$
def fibonacci(n):
   if n <= 1:
      return n
   else:
      return fibonacci(n-1) + fibonacci(n-2)
return fibonacci(n)
$$ LANGUAGE plpython3u;
SELECT fibbbPython(30) = integer '832040';

CREATE OR REPLACE FUNCTION factPython(n integer) RETURNS integer AS $$
def factorial(n):
   if n <= 1:
      return 1
   else:
      return n * factorial(n-1)
return factorial(n)
$$ LANGUAGE plpython3u;
SELECT factPython(5) = integer '120';

-- CREATE OR REPLACE FUNCTION naturalPython(n numeric) RETURNS numeric AS $$
-- plpy.notice(n)
-- if (n < 0):
--     return 0
-- elif (n == 1):
--     return 1
-- else:
--     return plpy.execute("SELECT naturalPython(%f) as n" % (n-1))[0]["n"]
-- $$ LANGUAGE plpython3u;

-- SELECT naturalPython(10) =  numeric '1';

-- SELECT naturalPython(10.5) = numeric '0';
