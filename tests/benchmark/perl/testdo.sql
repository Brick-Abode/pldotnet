do $$
   $sumResult = 3 + 7;
   elog(INFO, "Compiled $sumResult");
$$ language plperl;

SELECT 1;
