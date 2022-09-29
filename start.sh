#!/bin/sh

deploy_dir_base="/var/www/EnsimagCafet-"

if [ "$1" != "dev" ] && [ "$1" != "prod" ]; then
	echo "Usage: sh publish.sh <dev/prod>"
	exit 1
fi

current_dir=$(pwd)
env_name=$1
deploy_dir="${deploy_dir_base}${env_name}/"

cd $deploy_dir && dotnet EnsimagCafet.Web.dll
cd $current_dir
