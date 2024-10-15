# Makefile for PL/.NET

UNAME = $(shell uname)
PYTHON ?= python3
SED ?= sed
DBUSER ?= postgres

# General
# Get installed dotnet host host
DOTNET_VER = $(shell dotnet --info | grep 'Host' -A 3 | $(SED) -n 's/Version: \(.*\)/\1/p' | xargs)

PLDOTNET_ENGINE_DIR = -DPLDOTNET_ENGINE_DIR=$(PLDOTNET_ENGINE_ROOT)/PlDotNET
PLDOTNET_TEMPLATE_DIR = $(PLDOTNET_ENGINE_ROOT)/PlDotNET/Templates

# Linux support
ifeq ($(UNAME), Linux)
	DOTNET_HOSTDIR ?= $(shell dpkg -L dotnet-apphost-pack-6.0 | grep hostfxr.h | head -1 | xargs dirname)
	DOTNET_LIBDIR  ?= $(shell dpkg -L dotnet-apphost-pack-6.0 | grep hostfxr.h | head -1 | xargs dirname)
	DOTNET_HOSTLIB ?= -L$(DOTNET_LIBDIR) -lnethost -Wl,-rpath $(DOTNET_LIBDIR)
	PLDOTNET_ENGINE_ROOT ?= /var/lib
	PG_CONFIG ?= pg_config
	PKG_LIBDIR = $(shell $(PG_CONFIG) --pkglibdir)
	PG_INCDIR = $(shell $(PG_CONFIG) --includedir-server )
endif

# OSX support
ifeq ($(UNAME), Darwin)
	SED=gsed
	DBUSER=$(USER)
	DOTNET_HOSTDIR ?= $(shell find /usr/local/share/dotnet -name hostfxr.h | head -1 | xargs dirname)
	DOTNET_LIBDIR  ?= $(shell find /usr/local/share/dotnet -name hostfxr.h | head -1 | xargs dirname)
	DOTNET_HOSTLIB ?= -L$(DOTNET_LIBDIR) -Wl,-rpath,$(DOTNET_LIBDIR) -lglib-2.0 -lnethost
	PLDOTNET_ENGINE_ROOT ?= /tmp/pldotnet

	PG_CONFIG ?= pg_config
	PKG_LIBDIR = $(shell $(PG_CONFIG) --pkglibdir)
	PG_INCDIR = $(shell $(PG_CONFIG) --includedir-server )
endif

GLIB_INC := `pkg-config --cflags --libs glib-2.0`

ifeq ("$(shell echo $(USE_DOTNETBUILD) | tr A-Z a-z)", "true")
	DEFINE_DOTNET_BUILD := -DUSE_DOTNETBUILD
else
	BUILD_PLDOTNET_PROJECT := dotnet build $(PLDOTNET_ENGINE_ROOT)/PlDotNET -c Release
endif

PG_VER = $(shell pg_config --version | grep -Po '(?<=SQL )[0-9]+')
PG_10_OR_12PLUS = $(shell if [ ${PG_VER}  -lt "12" ]; then echo '10';  else echo '12plus'; fi)

MODULE_big = pldotnet
EXTENSION = pldotnet
DATA = pldotnet--0.9.sql

OBJS = src/pldotnet_hostfxr.o src/pldotnet.o src/pldotnet_conversions.o src/pldotnet_main.o src/pldotnet_spi.o

PG_CPPFLAGS = -I$(DOTNET_HOSTDIR) -I$(PG_INCDIR) $(GLIB_INC) \
			  -Iinc -DLINUX $(DEFINE_DOTNET_BUILD) $(PLDOTNET_ENGINE_DIR) \
			  -DPKG_LIBDIR=$(PKG_LIBDIR)
PGXS = $(shell $(PG_CONFIG) --pgxs)

ifeq ($(UNAME), Darwin)
	PG_CPPFLAGS = -isystem $(DOTNET_HOSTDIR) -isystem $(PG_INCDIR) $(GLIB_INC) \
			  -Iinc -DLINUX $(DEFINE_DOTNET_BUILD) $(PLDOTNET_ENGINE_DIR) \
			  -DPKG_LIBDIR=$(PKG_LIBDIR)
endif

SHLIB_LINK = $(DOTNET_HOSTLIB) $(GLIB_INC)

CURRENT_DIR = $(shell pwd)

include $(PGXS)

CP_CHOWN = cp -R dotnet_src $(PLDOTNET_ENGINE_ROOT)/PlDotNET
ifeq ($(UNAME), Linux)
	CP_CHOWN += && chown -R postgres $(PLDOTNET_ENGINE_ROOT)/PlDotNET
endif

pldotnet-install: pldotnet-uninstall install
	$(CP_CHOWN)
	$(SED) -i 's/@PKG_LIBDIR/$(shell echo $(PKG_LIBDIR) | $(SED) 's/\//\\\//g')/' $(PLDOTNET_ENGINE_ROOT)/PlDotNET/*.cs
	$(SED) -i 's/@PKG_LIBDIR/$(shell echo $(PKG_LIBDIR) | $(SED) 's/\//\\\//g')/' $(PLDOTNET_ENGINE_ROOT)/PlDotNET/Common/*.cs
	$(SED) -i 's/@PKG_LIBDIR/$(shell echo $(PKG_LIBDIR) | $(SED) 's/\//\\\//g')/' $(PLDOTNET_ENGINE_ROOT)/PlDotNET/TypeHandlers/*.cs
	$(SED) -i 's/@PKG_LIBDIR/$(shell echo $(PKG_LIBDIR) | $(SED) 's/\//\\\//g')/' $(PLDOTNET_ENGINE_ROOT)/PlDotNET/npgsql/src/Npgsql/PlDotNET/*.cs
	$(SED) -i 's/@PLDOTNET_TEMPLATE_DIR/$(shell echo $(PLDOTNET_TEMPLATE_DIR) | $(SED) 's/\//\\\//g')/' $(PLDOTNET_ENGINE_ROOT)/PlDotNET/CodeGenerator.cs
	$(SED) -i 's/@PLDOTNET_TEMPLATE_DIR/$(shell echo $(PLDOTNET_TEMPLATE_DIR) | $(SED) 's/\//\\\//g')/' $(PLDOTNET_ENGINE_ROOT)/PlDotNET/Engine.cs
	$(BUILD_PLDOTNET_PROJECT)

pldotnet-uninstall: uninstall
	rm -rf $(PLDOTNET_ENGINE_ROOT)/PlDotNET

pldotnet-build-debian-packages:
	$(MAKE) documentation
	rm -f debian/packages/postgresql-*-pldotnet_*.deb
	pg_buildext updatecontrol
	debuild -b -uc -us --lintian-opts --suppress-tags=initial-upload-closes-no-bugs,custom-library-search-path --profile debian
	mkdir -p debian/packages
	cp ../postgresql-*-pldotnet_*.deb debian/packages/
	rm -rf ../postgresql-*-pldotnet_*.deb

cpplint:
	cpplint --filter=-readability/casting,-build/include_subdir,-runtime/int,-runtime/printf,-build/header_guard src/*.c src/*.h

documentation:
	doxygen docs/Doxyfile

clean-docker:
	# These might fail if there are no containers and/or images
	# first you remove the containers
	-docker ps -a|grep -v CREATED|awk '{print $$1}'| xargs docker rm
	# second, you remove the images
	-docker images|grep -v CREATED|awk '{print $$3}'| xargs docker rmi
	rm -rf postgres-data

pldotnet-ubuntu:
	docker-compose run --rm pldotnet-ubuntu22 bash

pldotnet-postgres:
	$(MAKE) clean
	$(MAKE)
	$(MAKE) pldotnet-install
	sudo -u $(DBUSER) psql

build-package:
	docker-compose up pldotnet-build | tee package-build-log.txt

build-package-bash:
	$(MAKE) build-package
	docker-compose run --rm pldotnet-build bash

build-package-arm:
	docker-compose up pldotnet-build-arm | tee package-build-arm-log.txt

build-package-arm-bash:
	$(MAKE) build-package-arm
	docker-compose run --rm pldotnet-build-arm bash

pre-tests-script:
	dotnet build $(CURRENT_DIR)/tests/csharp/DotNetTestProject -c Release
	dotnet build $(CURRENT_DIR)/tests/fsharp/DotNetTestProject -c Release
	mkdir -p automated_test_results
	find automated_test_results -mindepth 1 -delete
	echo 'DROP TABLE IF EXISTS automated_test_results;CREATE TABLE automated_test_results(ID SERIAL PRIMARY KEY, FEATURE TEXT, TEST_NAME TEXT, RESULT boolean);' | (sudo -u $(DBUSER)  psql)

post-tests-script:
	cd $(CURRENT_DIR)/tests/csharp/DotNetTestProject/ && rm -rf bin obj
	cd $(CURRENT_DIR)/tests/fsharp/DotNetTestProject/ && rm -rf bin obj
	cd $(CURRENT_DIR)/
	echo 'SELECT FEATURE, TEST_NAME, RESULT from automated_test_results;' | (sudo -u $(DBUSER)  psql 2>&1) | tee automated_test_results/automated_test_results.out
	echo 'SELECT RESULT, COUNT(1) FROM automated_test_results GROUP BY RESULT;' | (sudo -u $(DBUSER)  psql)


# xUnit test directory
XUNIT_TEST_DIR := $(CURRENT_DIR)/tests/xUnit
# Command to run xUnit tests
RUN_XUNIT_TESTS = cd $(XUNIT_TEST_DIR) && dotnet test

pldotnet-tests:
	$(MAKE) pre-tests-script
	$(RUN_XUNIT_TESTS)

csharp-tests:
	$(MAKE) pre-tests-script
	$(RUN_XUNIT_TESTS) --filter Language=CSharp

fsharp-tests:
	$(MAKE) pre-tests-script
	$(RUN_XUNIT_TESTS) --filter Language=FSharp

csharp-tests-cats:
	for sqlfile in tests/csharp/*.sql; do \
		echo "Running $$sqlfile"; \
		cat $$sqlfile | (sudo -u postgres psql 2>&1) | tee automated_test_results/`basename $$sqlfile .sql`.out; \
	done

fsharp-tests-cats:
	for sqlfile in tests/fsharp/*.sql; do \
		echo "Running $$sqlfile"; \
		cat $$sqlfile | (sudo -u postgres psql 2>&1) | tee automated_test_results/`basename $$sqlfile .sql`.out; \
	done

pldotnet-tests-sql:
	$(MAKE) pre-tests-script
	$(MAKE) csharp-tests-cats
	$(MAKE) fsharp-tests-cats
	$(MAKE) post-tests-script

csharp-tests-sql:
	$(MAKE) pre-tests-script
	$(MAKE) csharp-tests-cats
	$(MAKE) post-tests-script

fsharp-tests-sql:
	$(MAKE) pre-tests-script
	$(MAKE) fsharp-tests-cats
	$(MAKE) post-tests-script

stress-test:
	mkdir -p automated_test_results
	find automated_test_results -mindepth 1 -delete
	echo 'DROP TABLE automated_test_results;CREATE TABLE automated_test_results(ID SERIAL PRIMARY KEY, FEATURE TEXT, TEST_NAME TEXT, RESULT boolean);' | (sudo -u $(DBUSER) psql)
	sudo bash tests/stress_test/stress_test.sh

benchmark-tests:
	mkdir -p automated_test_results
	cat tests/benchmark/python/init-extension.sql | (sudo -u $(DBUSER) psql)
	echo "ALTER DATABASE postgres SET pljava.libjvm_location TO '$$(sudo find /usr -name libjvm.so | head -n 1)';" | sudo -u $(DBUSER) psql
	cat tests/benchmark/java/init-extension.sql | (sudo -u $(DBUSER) psql)
	cat tests/benchmark/perl/init-extension.sql | (sudo -u $(DBUSER) psql)
	cat tests/benchmark/lua/init-extension.sql | (sudo -u $(DBUSER) psql)
	cat tests/benchmark/tcl/init-extension.sql | (sudo -u $(DBUSER) psql)
	cat tests/benchmark/r/init-extension.sql | (sudo -u $(DBUSER) psql)
	cat tests/benchmark/v8javascript/init-extension.sql | (sudo -u $(DBUSER) psql)
	cd $(CURRENT_DIR)/tests/benchmark/java/test-suite && mvn install && cd $(CURRENT_DIR)/
	echo "select sqlj.install_jar('file:$(CURRENT_DIR)/tests/benchmark/java/test-suite/target/test-suite-1.0.0.jar', 'testsuite', true);" | (sudo -u $(DBUSER) psql)
	echo "select sqlj.set_classpath('public', 'testsuite');" | (sudo -u $(DBUSER) psql)
	bash tests/benchmark/benchmark.sh

spi-tests:
	$(MAKE) pre-tests-script
	cat tests/csharp/testspi.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testspi.out
	cat tests/fsharp/testfsspi.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testfsspi.out
	$(MAKE) post-tests-script

npgsql-tests:
	bash tests/npgsql/run_tests.sh
	$(PYTHON) tests/npgsql/process_npgsql_results.py

npgsql-working-tests:
	bash tests/npgsql/run_working_tests.sh
	$(PYTHON) tests/npgsql/process_npgsql_results.py
