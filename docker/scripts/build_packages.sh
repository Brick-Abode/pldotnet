#!/bin/bash
runuser -u postgres -- initdb
runuser -u postgres -- pg_ctl start
runuser -u postgres -- psql -c "DO \$\$ BEGIN CREATE ROLE root superuser createdb login createrole replication bypassrls; EXCEPTION WHEN duplicate_object THEN RAISE NOTICE '%, skipping', SQLERRM USING ERRCODE = SQLSTATE; END \$\$;"
make pldotnet-install-dpkg