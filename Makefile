SHELL := /bin/bash

BASE_PATH := $(dir $(abspath $(lastword $(MAKEFILE_LIST))))
WEB_PATH := $(BASE_PATH)/src/Orbital.Web

build: setup
	cd $(BASE_PATH) && dotnet build

setup:
	cd $(BASE_PATH) && dotnet restore

start:
	cd $(WEB_PATH) && ASPNETCORE_ENVIRONMENT=Development dotnet watch run

migrations-add:
ifndef NAME
	$(error NAME is undefined)
endif
	cd $(WEB_PATH) && ASPNETCORE_ENVIRONMENT=Migations dotnet ef migrations add -c Orbital.Data.OrbitalContext -p ../Orbital.Data/Orbital.Data.csproj "$(NAME)"

migrations-up:
	cd $(WEB_PATH) && ASPNETCORE_ENVIRONMENT=Migations dotnet ef database update -c Orbital.Data.OrbitalContext -p ../Orbital.Data/Orbital.Data.csproj

package:
	echo "TODO"