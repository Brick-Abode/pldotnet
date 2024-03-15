# Makefile for samples in PL/.NET

UNAME = $(shell uname)
SED ?= sed
DBUSER ?= postgres

# General
# Get installed dotnet host host
DOTNET_VER = $(shell dotnet --info | grep 'Host' -A 3 | $(SED) -n 's/Version: \(.*\)/\1/p' | xargs)

CURRENT_DIR = $(shell pwd)

pre-tests-script:
	mkdir -p automated_test_results
	find automated_test_results -mindepth 1 -delete
	echo 'DROP TABLE IF EXISTS automated_test_results;CREATE TABLE automated_test_results(ID SERIAL PRIMARY KEY, FEATURE TEXT, TEST_NAME TEXT, RESULT boolean);' | (sudo -u $(DBUSER)  psql)

post-tests-script:
	cd $(CURRENT_DIR)/csharp/DotNetTestProject/ && rm -rf bin obj
	cd $(CURRENT_DIR)/fsharp/DotNetTestProject/ && rm -rf bin obj
	cd $(CURRENT_DIR)/
	echo 'SELECT FEATURE, TEST_NAME, RESULT from automated_test_results;' | (sudo -u $(DBUSER)  psql 2>&1) | tee automated_test_results/automated_test_results.out
	echo 'SELECT RESULT, COUNT(1) FROM automated_test_results GROUP BY RESULT;' | (sudo -u $(DBUSER)  psql)

# xUnit test directory
XUNIT_TEST_DIR := $(CURRENT_DIR)/xUnit
# Command to run xUnit tests
RUN_XUNIT_TESTS = cd $(XUNIT_TEST_DIR) && dotnet test

pldotnet-tests:
	make pre-tests-script
	$(RUN_XUNIT_TESTS)

csharp-tests:
	make pre-tests-script
	$(RUN_XUNIT_TESTS) --filter Language=CSharp

fsharp-tests:
	make pre-tests-script
	$(RUN_XUNIT_TESTS) --filter Language=FSharp

csharp-tests-cats:
	for sqlfile in csharp/*.sql; do \
		echo "Running $$sqlfile"; \
		cat $$sqlfile | (sudo -u postgres psql 2>&1) | tee automated_test_results/`basename $$sqlfile .sql`.out; \
	done

fsharp-tests-cats:
	for sqlfile in fsharp/*.sql; do \
		echo "Running $$sqlfile"; \
		cat $$sqlfile | (sudo -u postgres psql 2>&1) | tee automated_test_results/`basename $$sqlfile .sql`.out; \
	done

pldotnet-tests-sql:
	make pre-tests-script
	make csharp-tests-cats
	make fsharp-tests-cats
	make post-tests-script

csharp-tests-sql:
	make pre-tests-script
	make csharp-tests-cats
	make post-tests-script

fsharp-tests-sql:
	make pre-tests-script
	make fsharp-tests-cats
	make post-tests-script

stress-test:
	mkdir -p automated_test_results
	find automated_test_results -mindepth 1 -delete
	echo 'DROP TABLE automated_test_results;CREATE TABLE automated_test_results(ID SERIAL PRIMARY KEY, FEATURE TEXT, TEST_NAME TEXT, RESULT boolean);' | (sudo -u $(DBUSER) psql)
	sudo bash stress_test/stress_test.sh

benchmark-tests:
	mkdir -p automated_test_results
	cat benchmark/python/init-extension.sql | (sudo -u $(DBUSER) psql)
	cat benchmark/java/init-extension.sql | (sudo -u $(DBUSER) psql)
	cat benchmark/perl/init-extension.sql | (sudo -u $(DBUSER) psql)
	cat benchmark/lua/init-extension.sql | (sudo -u $(DBUSER) psql)
	cat benchmark/tcl/init-extension.sql | (sudo -u $(DBUSER) psql)
	cat benchmark/r/init-extension.sql | (sudo -u $(DBUSER) psql)
	cat benchmark/v8javascript/init-extension.sql | (sudo -u $(DBUSER) psql)
	cd $(CURRENT_DIR)/benchmark/java/test-suite && mvn install && cd $(CURRENT_DIR)/
	echo "select sqlj.install_jar('file:$(CURRENT_DIR)/benchmark/java/test-suite/target/test-suite-1.0.0.jar', 'testsuite', true);" | (sudo -u $(DBUSER) psql)
	echo "select sqlj.set_classpath('public', 'testsuite');" | (sudo -u $(DBUSER) psql)
	bash benchmark/benchmark.sh

spi-tests:
	make pre-tests-script
	cat csharp/testspi.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testspi.out
	cat fsharp/testfsspi.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testfsspi.out
	make post-tests-script

npgsql-tests:
	bash npgsql/run_tests.sh
	python3 npgsql/process_npgsql_results.py

npgsql-working-tests:
	bash npgsql/run_working_tests.sh
	python3 npgsql/process_npgsql_results.py