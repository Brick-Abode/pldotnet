# Makefile for PL/.NET

# General
# Get installed dotnet host host
DOTNET_VER = $(shell dotnet --info | grep 'Host' -A 3 | sed -n 's/Version: \(.*\)/\1/p' | xargs)
DOTNET_HOSTDIR ?= $(shell dpkg -L dotnet-apphost-pack-6.0 | grep hostfxr.h | head -1 | xargs dirname)
DOTNET_LIBDIR  ?= $(shell dpkg -L dotnet-apphost-pack-6.0 | grep hostfxr.h | head -1 | xargs dirname)
DOTNET_HOSTLIB ?= -L$(DOTNET_LIBDIR) -lnethost -Wl,-rpath $(DOTNET_LIBDIR)
GLIB_INC := `pkg-config --cflags --libs glib-2.0`
PLDOTNET_ENGINE_ROOT ?= /var/lib
PLDOTNET_ENGINE_DIR := -D PLDOTNET_ENGINE_DIR=$(PLDOTNET_ENGINE_ROOT)/PlDotNET
PLDOTNET_TEMPLATE_DIR =$(PLDOTNET_ENGINE_ROOT)/PlDotNET/Templates

ifeq ("$(shell echo $(USE_DOTNETBUILD) | tr A-Z a-z)", "true")
	DEFINE_DOTNET_BUILD := -D USE_DOTNETBUILD
else
	BUILD_PLDOTNET_PROJECT := dotnet build $(PLDOTNET_ENGINE_ROOT)/PlDotNET -c Release
endif

PG_CONFIG ?= pg_config
PKG_LIBDIR := $(shell $(PG_CONFIG) --pkglibdir)
PG_VER = $(shell pg_config --version | grep -Po '(?<=SQL )[0-9]+')
PG_10_OR_12PLUS = $(shell if [ ${PG_VER}  -lt "12" ]; then echo '10';  else echo '12plus'; fi)

MODULE_big = pldotnet
EXTENSION = pldotnet
DATA = pldotnet--0.9.sql

OBJS = src/pldotnet_hostfxr.o src/pldotnet.o src/pldotnet_conversions.o src/pldotnet_main.o

PG_CPPFLAGS = -I$(DOTNET_HOSTDIR) \
			  -Iinc -D LINUX $(DEFINE_DOTNET_BUILD) $(PLDOTNET_ENGINE_DIR) \
			  $(GLIB_INC) -D PKG_LIBDIR=$(PKG_LIBDIR)

SHLIB_LINK = $(DOTNET_HOSTLIB) $(GLIB_INC)

PGXS := $(shell $(PG_CONFIG) --pgxs)

CURRENT_DIR = $(shell pwd)

include $(PGXS)

pldotnet-install: pldotnet-uninstall install
	echo $(DOTNET_LIBDIR) > /etc/ld.so.conf.d/nethost_pldotnet.conf && ldconfig
	cp -R dotnet_src $(PLDOTNET_ENGINE_ROOT)/PlDotNET && chown -R postgres $(PLDOTNET_ENGINE_ROOT)/PlDotNET
	sed -i 's/@PKG_LIBDIR/$(shell echo $(PKG_LIBDIR) | sed 's/\//\\\//g')/' $(PLDOTNET_ENGINE_ROOT)/PlDotNET/Engine.cs
	sed -i 's/@PKG_LIBDIR/$(shell echo $(PKG_LIBDIR) | sed 's/\//\\\//g')/' $(PLDOTNET_ENGINE_ROOT)/PlDotNET/TypeHandlers/*.cs
	sed -i 's/@PKG_LIBDIR/$(shell echo $(PKG_LIBDIR) | sed 's/\//\\\//g')/' $(PLDOTNET_ENGINE_ROOT)/PlDotNET/FSharp/FSharpCompiler.fs
	sed -i 's/@PLDOTNET_TEMPLATE_DIR/$(shell echo $(PLDOTNET_TEMPLATE_DIR) | sed 's/\//\\\//g')/' $(PLDOTNET_ENGINE_ROOT)/PlDotNET/CodeGenerator.cs
	$(BUILD_PLDOTNET_PROJECT)

pldotnet-uninstall: uninstall
	rm -rf $(PLDOTNET_ENGINE_ROOT)/PlDotNET

pldotnet-install-dpkg:
	make documentation
	rm -f debian/packages/postgresql-*-pldotnet_*.deb
	-sudo -u postgres pg_createcluster $(PG_VER) default
	service postgresql start
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
	make clean && make && make pldotnet-install
	sudo -u postgres psql

build-package:
	docker-compose up pldotnet-build | tee package-build-log.txt

build-package-bash:
	make build-package
	docker-compose run --rm pldotnet-build bash

build-package-arm:
	docker-compose up pldotnet-build-arm | tee package-build-arm-log.txt

build-package-arm-bash:
	make build-package-arm
	docker-compose run --rm pldotnet-build-arm bash

pre-tests-script:
	dotnet build $(CURRENT_DIR)/tests/csharp/DotNetTestProject -c Release
	dotnet build $(CURRENT_DIR)/tests/fsharp/DotNetTestProject -c Release
	rm -rf automated_test_results
	mkdir -p automated_test_results
	echo 'DROP TABLE IF EXISTS automated_test_results;CREATE TABLE automated_test_results(FEATURE TEXT, TEST_NAME TEXT, RESULT boolean);' | (sudo -u postgres  psql)

post-tests-script:
	cd $(CURRENT_DIR)/tests/csharp/DotNetTestProject/ && rm -rf bin obj
	cd $(CURRENT_DIR)/tests/fsharp/DotNetTestProject/ && rm -rf bin obj
	cd $(CURRENT_DIR)/
	echo 'SELECT FEATURE, TEST_NAME, RESULT from automated_test_results;' | (sudo -u postgres  psql 2>&1) | tee automated_test_results/automated_test_results.out
	echo 'SELECT RESULT, COUNT(1) FROM automated_test_results GROUP BY RESULT;' | (sudo -u postgres  psql)

csharp-tests-cats:
	cat tests/csharp/testbit.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testbit.out
	cat tests/csharp/testbool.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testbool.out
	cat tests/csharp/testbytea.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testbytea.out
	cat tests/csharp/testdatetime.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testdatetime.out
	cat tests/csharp/testdll.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testdll.out
	cat tests/csharp/testfloats.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testfloats.out
	cat tests/csharp/testgeometric.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testgeometric.out
	cat tests/csharp/testintegers.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testintegers.out
	cat tests/csharp/testjson.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testjson.out
	cat tests/csharp/testmoney.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testmoney.out
	cat tests/csharp/testnetwork.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testnetwork.out
	cat tests/csharp/testrange.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testrange.out
	cat tests/csharp/teststring.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/teststring.out
	cat tests/csharp/testuuid.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testuuid.out
	cat tests/csharp/testdo.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testdo.out
	cat tests/csharp/testprocedure.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testprocedure.out
	cat tests/csharp/testcreate.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testcreate.out
	cat tests/csharp/testcall.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testcall.out

fsharp-tests-cats:
	cat tests/fsharp/testfsbit.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testfsbit.out
	cat tests/fsharp/testfsbool.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testfsbool.out
	cat tests/fsharp/testfsbytea.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testfsbytea.out
	cat tests/fsharp/testfsdate.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testfsdate.out
	cat tests/fsharp/testfsdatetime.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testfsdatetime.out
	cat tests/fsharp/testfsdo.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testfsdo.out
	cat tests/fsharp/testfsprocedure.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testfsprocedure.out
	cat tests/fsharp/testfsdll.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testfsdll.out
	cat tests/fsharp/testfsfloats.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testfsfloats.out
	cat tests/fsharp/testfsgeometric.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testfsgeometric.out
	cat tests/fsharp/testfsintegers.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testfsintegers.out
	cat tests/fsharp/testfsjson.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testfsjson.out
	cat tests/fsharp/testfsnetwork.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testfsnetwork.out
	cat tests/fsharp/testfsrange.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testfsrange.out
	cat tests/fsharp/testfsstring.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testfsstring.out
	cat tests/fsharp/testfsuuid.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testfsuuid.out
	cat tests/fsharp/testfscreate.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testfscreate.out
	cat tests/fsharp/testfscall.sql | (sudo -u postgres  psql 2>&1) | tee automated_test_results/testfscall.out

pldotnet-tests:
	make pre-tests-script
	make fsharp-tests-cats
	make csharp-tests-cats
	make post-tests-script

csharp-tests:
	make pre-tests-script
	make csharp-tests-cats
	make post-tests-script

fsharp-tests:
	make pre-tests-script
	make fsharp-tests-cats
	make post-tests-script

stress-test:
	rm -rf automated_test_results
	mkdir -p automated_test_results
	echo 'DROP TABLE automated_test_results;CREATE TABLE automated_test_results(FEATURE TEXT, TEST_NAME TEXT, RESULT boolean);' | (sudo -u postgres psql)
	sudo bash tests/stress_test/stress_test.sh

benchmark-tests:
	mkdir -p automated_test_results
	cat tests/benchmark/python/init-extension.sql | (sudo -u postgres psql)
	cat tests/benchmark/java/init-extension.sql | (sudo -u postgres psql)
	cat tests/benchmark/perl/init-extension.sql | (sudo -u postgres psql)
	cat tests/benchmark/lua/init-extension.sql | (sudo -u postgres psql)
	cat tests/benchmark/tcl/init-extension.sql | (sudo -u postgres psql)
	cat tests/benchmark/r/init-extension.sql | (sudo -u postgres psql)
	cat tests/benchmark/v8javascript/init-extension.sql | (sudo -u postgres psql)
	cd $(CURRENT_DIR)/tests/benchmark/java/test-suite && mvn install && cd $(CURRENT_DIR)/
	echo "select sqlj.install_jar('file:$(CURRENT_DIR)/tests/benchmark/java/test-suite/target/test-suite-1.0.0.jar', 'testsuite', true);" | (sudo -u postgres psql)
	echo "select sqlj.set_classpath('public', 'testsuite');" | (sudo -u postgres psql)
	bash tests/benchmark/benchmark.sh $(NRUNS)
