do $$
DECLARE
	sumResult int;
BEGIN
	sumResult := 3 + 7;
	raise notice 'Compiled Sum: %', sumResult;
END
    /* Check if there is a 'Compiled Sum: 10' in PostgreSQL logs */
$$ language plpgsql;

SELECT 1;
