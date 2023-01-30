#!/bin/bash
make clean
make
make pldotnet-install

runuser -u postgres -- psql -c "DO \$\$ BEGIN CREATE ROLE root superuser createdb login createrole replication bypassrls; EXCEPTION WHEN duplicate_object THEN RAISE NOTICE '%, skipping', SQLERRM USING ERRCODE = SQLSTATE; END \$\$;"
runuser -u postgres -- psql -c "DROP EXTENSION IF EXISTS pldotnet CASCADE;"
runuser -u postgres -- psql -c "CREATE EXTENSION pldotnet;"