CREATE OR REPLACE FUNCTION fibbbTcl(n integer) RETURNS integer AS $$
proc fibonacci {n} {
    if {$n <= 1} {
        return $n
    } else {
        return [expr {[fibonacci [expr {$n-1}]] + [fibonacci [expr {$n-2}]]}]
    }
}
return [fibonacci $1]
$$ LANGUAGE pltcl;
SELECT fibbbTcl(30) = integer '832040';

CREATE OR REPLACE FUNCTION factTcl(n integer) RETURNS integer AS $$
proc factorial {n} {
    if {$n <= 1} {
        return 1
    } else {
        return [expr {$n * [factorial [expr {$n-1}]]}]
    }
}
return [factorial $1]
$$ LANGUAGE pltcl;
SELECT factTcl(5) = integer '120';

-- CREATE OR REPLACE FUNCTION naturalTcl(n numeric) RETURNS numeric AS $$
--     if {$1 < 0} {
--         return 0;
--     } elseif {$1 == 1} {
-- 	    return 1;
--     } else {
--     	return [naturalTcl [expr $1 - 1]];
--     }
-- $$ LANGUAGE pltcl;
-- SELECT naturalTcl(10) =  numeric '1';
-- SELECT naturalTcl(10.5) = numeric '0';
