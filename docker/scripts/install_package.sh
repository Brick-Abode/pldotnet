#!/bin/bash
echo "Waiting for the deb package to be ready..."
DEB_FILE=/app/pldotnet/debian/packages/postgresql-$(pg_config --version | grep -Po '(?<=SQL )[0-9]+')-*_amd64.deb
until [ -f ${DEB_FILE} ]
do
     sleep 2
done
echo "Deb package found! Starting installation..."
dpkg -i ${DEB_FILE}
