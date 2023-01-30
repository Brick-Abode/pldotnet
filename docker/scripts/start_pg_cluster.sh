#!/bin/bash

PGVERSION=$(pg_config --version | grep -Po '(?<=SQL )[0-9]+')

# if cluster is not populated, copy from backup created in the building process
if [[ ! $(ls ${PGDATA}) ]]; then
    rsync -a /var/lib/postgresql/pgnet_backup_cluster/* ${PGDATA}
fi

# fixing permissions/ownership
chown postgres -R ${PGDATA}
chmod 700 -R ${PGDATA}

# starting cluster
/usr/bin/pg_ctlcluster \
${PGVERSION} pgnet \
start \
-- -l /var/log/postgresql/postgresql-main.log \
-D /etc/postgresql/${PGVERSION}/pgnet/ \
-s

# Setting postgres user password to POSTGRES_PASSWORD...
runuser -u postgres -- psql -c "ALTER USER ${POSTGRES_USER:-postgres} WITH PASSWORD '${POSTGRES_PASSWORD:-postgres}';"
/usr/bin/pg_ctlcluster \
${PGVERSION} pgnet \
restart -- -l /var/log/postgresql/postgresql-main.log \
-D /etc/postgresql/${PGVERSION}/pgnet/ \
-s

exec "$@"