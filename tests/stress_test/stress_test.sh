#!/bin/sh

TIMES_EXECUTED=0

seq 1 5 | while read pow;
do
    start=$(date +%s);
    iterations=$((10**$pow));

    seq 1 $iterations |
    while read i; do
        TIMES_EXECUTED=$((TIMES_EXECUTED+1));
        echo $TIMES_EXECUTED > automated_test_results/times_executed.out;
        cat tests/stress_test/create_call_delete_function.sql
    done | (sudo -u postgres psql 2>&1) | tee -a automated_test_results/stress_test.out;

    echo "Stress test ($iterations) took $(($(date +%s)-start)) seconds. Times executed: $(cat automated_test_results/times_executed.out)" 2>&1 | tee -a automated_test_results/automated_test_results.out | tee -a automated_test_results/stress_test.out;

    fails=`sudo -u postgres psql -AXqtc "SELECT COUNT(*) AS FAIL_COUNT FROM automated_test_results WHERE result = 'f'"`

    if (( $fails == 0 )); then
        echo "Stress test successfully completed!" 2>&1 | tee -a automated_test_results/stress_test.out;
    else
        echo "Stress test failed!" 2>&1 | tee -a automated_test_results/stress_test.out;
        exit;
    fi

    sleep 2;
done;
