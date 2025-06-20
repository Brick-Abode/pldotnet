#!/bin/bash

PSQL_CMD="psql"
if [ "$(whoami)" != "postgres" ]; then
    PSQL_CMD="sudo -u postgres psql"
    service postgresql start
fi

echo "DROP EXTENSION IF EXISTS pldotnet CASCADE; CREATE EXTENSION pldotnet;" | $PSQL_CMD
