CREATE OR REPLACE FUNCTION plbench(query text, n int) returns float as $$
DECLARE
    t0 timestamp with time zone;
    e float;
BEGIN
    execute query;
    t0 := clock_timestamp();
    for i in 1 .. n loop
        execute query;
    end loop;
    e := extract(microseconds from (clock_timestamp() - t0));
    return e / 1000000;
END;
$$ language plpgsql;
