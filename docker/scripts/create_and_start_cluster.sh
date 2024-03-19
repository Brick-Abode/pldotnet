PGVERSION=$(pg_config --version | grep -Po '(?<=SQL )[0-9]+')

# Starting PG server
pg_dropcluster --stop ${PGVERSION} main || true
pg_createcluster -d ${PGDATA} --port 5432 -l ${PGLOG} ${PGVERSION} main -- --auth-local=trust --no-clean || true
pg_ctlcluster ${PGVERSION} main start -- -l ${PGLOG} || true

exec "$@"
