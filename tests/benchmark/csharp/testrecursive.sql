CREATE OR REPLACE FUNCTION fibbb(n integer) RETURNS integer AS $$
    if (n <= 1)
    {
        return n;
    }
    return fibbb(n-1) + fibbb(n-2);
$$ LANGUAGE plcsharp;
SELECT fibbb(30) = integer '832040';

CREATE OR REPLACE FUNCTION fact(n integer) RETURNS integer AS $$
    if (n <= 1)
        return 1;
    else
    	return n*fact(n.GetValueOrDefault()-1);
$$ LANGUAGE plcsharp;
SELECT fact(5) = integer '120';
