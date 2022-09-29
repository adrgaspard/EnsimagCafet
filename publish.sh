#!/bin/sh

deploy_dir_base="/var/www/EnsimagCafet-"

if [ "$1" != "dev" ] && [ "$1" != "prod" ]; then
	echo "Usage: sh publish.sh <dev/prod>"
	exit 1
fi

current_dir=$(pwd)
env_name=$1
deploy_dir="${deploy_dir_base}${env_name}/"

dotnet publish EnsimagCafet.Web/EnsimagCafet.Web.csproj -c Release \
&& sudo cp -a EnsimagCafet.Web/bin/Release/net6.0/publish/* $deploy_dir \
&& cd $deploy_dir \
&& sudo chmod 755 * \
&& sudo chown -R $USER:$USER *
cd $current_dir
