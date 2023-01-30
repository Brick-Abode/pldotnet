do $$
	var sumResult = 3 + 7;
	plv8.elog(INFO, "Compiled Sum: "+sumResult);
    /* Check if there is a 'Compiled Sum: 10' in PostgreSQL logs */
$$ language plv8;

SELECT 1;
