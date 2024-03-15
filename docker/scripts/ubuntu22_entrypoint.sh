#!/bin/bash

service postgresql start

while true
do
    echo "Press <enter> for psql, 'q' to exit, or anything else for a shell"
    read j
    if [ "$j" == "q" ]
    then
        break
    elif [ "$j" == "" ]
    then
        sudo -u postgres psql
    else
        echo "Exit this shell to return to the top level"
        bash
    fi
done