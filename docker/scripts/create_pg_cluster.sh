#!/bin/bash

PGVERSION=$(pg_config --version | grep -Po '(?<=SQL )[0-9]+')
# PGDATA=/var/lib/postgresql/data

PG_HBA_FILE="/etc/postgresql/${PGVERSION}/pgnet/pg_hba.conf"
PG_CONFIG_FILE="/etc/postgresql/${PGVERSION}/pgnet/postgresql.conf"

PG_HBA_LINE="host  all  all 0.0.0.0/0 md5"
PG_CONFIG_LINE="listen_addresses = '*'"

# dropping previous cluster
echo "/usr/bin/pg_dropcluster --stop ${PGVERSION} main || true"
/usr/bin/pg_dropcluster --stop ${PGVERSION} main || true

# creating new cluster
echo "/usr/bin/pg_createcluster -d ${PGDATA} --port 5432 -l /var/log/postgresql/postgresql-main.log ${PGVERSION} pgnet -- --no-clean || true"
/usr/bin/pg_createcluster -d ${PGDATA} --port 5432 -l /var/log/postgresql/postgresql-main.log ${PGVERSION} pgnet -- --no-clean || true

# setting access
sed -i "1s;^;${PG_HBA_LINE}\n;" "${PG_HBA_FILE}"
sed -i "1s;^;${PG_CONFIG_LINE}\n;" "${PG_CONFIG_FILE}"

# copying conf files to data dir
echo "rsync -a /etc/postgresql/${PGVERSION}/pgnet/*.conf ${PGDATA}/"
rsync -a /etc/postgresql/${PGVERSION}/pgnet/*.conf ${PGDATA}/

# fixing ownership (must be done at runtime)
echo "chown postgres -R ${PGDATA}"
chown postgres -R ${PGDATA}

# starting new cluster
/usr/bin/pg_ctlcluster \
${PGVERSION} pgnet \
start \
-- -l /var/log/postgresql/postgresql-main.log \
-D /etc/postgresql/${PGVERSION}/pgnet/ \
-s

# backing up cluster for when the container starts
rsync -a ${PGDATA}/* /var/lib/postgresql/pgnet_backup_cluster

# removing unused pid file (necessary to avoid delays in the next start)
rm /var/lib/postgresql/pgnet_backup_cluster/*.pid