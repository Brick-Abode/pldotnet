#!/bin/bash

PGVERSION=$(pg_config --version | grep -Po '(?<=SQL )[0-9]+')
PGDATA=/var/lib/postgresql/unit_tests

(/usr/bin/pg_createcluster -d ${PGDATA} --port 5432 -l /var/log/postgresql/postgresql-main.log ${PGVERSION} main -- --auth-local=trust --no-clean) || true
(/usr/bin/pg_ctlcluster ${PGVERSION} main start -- -l /var/log/postgresql/postgresql-main.log) || true
service postgresql start

exec "$@"
