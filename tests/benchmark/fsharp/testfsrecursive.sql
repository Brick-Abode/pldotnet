CREATE OR REPLACE FUNCTION fibbbFSharp(m integer) RETURNS integer AS $$
let rec fib (n: int) (a: int) (b: int) =
    if n = 0 then a else fib (n-1) b (a+b)
match m.HasValue with
| true -> Nullable(fib m.Value 0 1)
| _ -> System.Nullable()
$$ LANGUAGE plfsharp;
SELECT fibbbFSharp(30) = integer '832040';

CREATE OR REPLACE FUNCTION factFSharp(m integer) RETURNS integer AS $$
let rec factorial acc n =
    if n = 0 then acc else factorial (acc * n) (n-1)

match m.HasValue with
| true -> Nullable(factorial 1 m.Value)
| _ -> System.Nullable()
$$ LANGUAGE plfsharp;
SELECT factFSharp(5) = integer '120';
