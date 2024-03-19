#!/bin/bash

TIMESTAMP=`date +%Y-%m-%d_%H-%M-%S`
FILENAME=automated_test_results/$TIMESTAMP.csv
NRUNS=${1:-1000}

touch $FILENAME
sudo chmod 777 $FILENAME

echo "Each test will run $NRUNS times."

# Create header
echo "Test Case;Category;plcsharp;plfsharp;plv8;plpython;plpgsql;pljava;plperl;pllua;pltcl;plr" >> $FILENAME

# Create the content
for file in tests/benchmark/bench-*.sql
do
    echo $file
    sed "s/NRUNS/$NRUNS/g" $file | (sudo -u postgres psql) | grep "|" | grep -v "column" | sed "s/|/;/g" | sed s/'\s'//g  >> $FILENAME
done

sed -i "s/;/\t/g" $FILENAME
cp $FILENAME "docs/img/pldotnet-performance-data-${TIMESTAMP:0:10}.csv"
