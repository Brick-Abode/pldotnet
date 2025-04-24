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
	DOTNET_HOSTDIR ?= $(shell dpkg -L dotnet-apphost-pack-9.0 | grep hostfxr.h | head -1 | xargs dirname)
	DOTNET_LIBDIR  ?= $(shell dpkg -L dotnet-apphost-pack-9.0 | grep hostfxr.h | head -1 | xargs dirname)
	DOTNET_HOSTLIB ?= -L$(DOTNET_LIBDIR) -lnethost -Wl,-rpath $(DOTNET_LIBDIR)
	PLDOTNET_ENGINE_ROOT ?= /var/lib
	PG_CONFIG = pg_config
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
	PLDOTNET_ENGINE_ROOT = $(HOME)/dev/pldotnet/tmp/dotnet_install
	PG_CONFIG = $(HOME)/postgresql-15/bin/pg_config
	PKG_LIBDIR = $(HOME)/postgresql-15/lib/
	PG_INCDIR = $(HOME)/postgresql-15/include/server
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
	PGXS = $(HOME)/postgresql-15/lib/pgxs/src/makefiles/pgxs.mk
endif

SHLIB_LINK = $(DOTNET_HOSTLIB) $(GLIB_INC)

CURRENT_DIR = $(shell pwd)

include $(PGXS)

CP_CHOWN = cp -R dotnet_src $(PLDOTNET_ENGINE_ROOT)/PlDotNET
ifeq ($(UNAME), Linux)
	CP_CHOWN += && chown -R postgres $(PLDOTNET_ENGINE_ROOT)/PlDotNET
endif

SHELL := /bin/bash

#########
# BUILD #
#########

.PHONY: build-clean
build-clean:
	rm -rf ../postgresql-*-pldotnet*deb ../pldotnet_*.build ../pldotnet_*.changes ../pldotnet_*.buildinfo
	rm -rf build-*
	rm -rf debian/.debhelper debian/postgresql-*-pldotnet* debian/control debian/debhelper-build-stamp debian/files

.PHONY: build-local
build-local:
	rm -f debian/packages/postgresql-*-pldotnet_*.deb
	pg_buildext updatecontrol
	debuild -b -uc -us --lintian-opts --suppress-tags=initial-upload-closes-no-bugs,custom-library-search-path --profile debian
	mkdir -p debian/packages
	cp ../postgresql-*-pldotnet_*.deb debian/packages/
	$(MAKE) build-clean

.PHONY: build-docker
build-docker:
	@echo "[INFO] Loading environment from .env"
	@set -a && \
	. ./.env && \
	set +a && \
	echo "[INFO] Building with Docker buildx. .NET $$DOTNET_VERSION / PostgreSQL $$POSTGRES_VERSION" && \
	docker buildx build \
	  --target artifacts \
	  --output type=local,dest=./debian/packages \
	  --build-arg DOTNET_VERSION=$$DOTNET_VERSION \
	  --build-arg POSTGRES_VERSION=$$POSTGRES_VERSION \
	  --build-arg POSTGRES_PORT=$$POSTGRES_PORT \
	  --build-arg POSTGRES_PASSWORD=$$POSTGRES_PASSWORD \
	  .

########
# TEST #
########

# xUnit test directory
XUNIT_TEST_DIR := $(CURRENT_DIR)/tests/xUnit
# Command to run xUnit tests
RUN_XUNIT_TESTS = cd $(XUNIT_TEST_DIR) && dotnet test

.PHONY: pre-tests-script
pre-tests-script:
	dotnet build $(CURRENT_DIR)/tests/csharp/DotNetTestProject -c Release
	dotnet build $(CURRENT_DIR)/tests/fsharp/DotNetTestProject -c Release
	mkdir -p automated_test_results
	find automated_test_results -mindepth 1 -delete
	runuser -u $(DBUSER) -- psql -c 'DROP TABLE IF EXISTS automated_test_results;CREATE TABLE automated_test_results(ID SERIAL PRIMARY KEY, FEATURE TEXT, TEST_NAME TEXT, RESULT boolean);'

.PHONY: test-local
test-local:
	$(MAKE) pre-tests-script
	$(RUN_XUNIT_TESTS)

.PHONY: test-docker
test-docker:
	docker exec -it pldotnet-runtime mkdir -p /app/pldotnet
	docker cp Makefile pldotnet-runtime:/app/pldotnet/Makefile
	docker cp tests pldotnet-runtime:/app/pldotnet/tests
	docker cp src pldotnet-runtime:/app/pldotnet/src
	docker cp dotnet_src pldotnet-runtime:/app/pldotnet/dotnet_src
	docker cp pldotnet--0.9.sql pldotnet-runtime:/app/pldotnet
	docker exec -w "/app/pldotnet" -it pldotnet-runtime make test-local