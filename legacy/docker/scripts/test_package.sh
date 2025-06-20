#!/bin/bash
make installcheck
DISTRO=$(lsb_release -i | grep -Po '(?<=ID:\t)\w+')
PG_VER=$(lsb_release -a | grep -Po '(?<=Release:\t)[\.\d]+')
OUT_DIR="${DISTRO}_${PG_VER}"
TEST_RESULTS_DIR=/app/pldotnet/installation_test_results/deb/${OUT_DIR}

rm -rf ${TEST_RESULTS_DIR}/* || true
if [ -f /app/pldotnet/regression.diffs ]; then
    echo "Tests for this version did not achieve 100%. Storing results in ${TEST_RESULTS_DIR}/"
    mkdir -p ${TEST_RESULTS_DIR}
    cp /app/pldotnet/regression.diffs ${TEST_RESULTS_DIR}
    cp /app/pldotnet/regression.out ${TEST_RESULTS_DIR}
fi
