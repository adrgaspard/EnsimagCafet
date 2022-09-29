#!/bin/sh

current_dir=$(pwd)

cd EnsimagCafet.EntityFrameworkCore.DbMigrator \
&& dotnet build EnsimagCafet.EntityFrameworkCore.DbMigrator.csproj -c Release \
&& dotnet ef database update --project EnsimagCafet.EntityFrameworkCore.DbMigrator.csproj \
&& dotnet run --project EnsimagCafet.EntityFrameworkCore.DbMigrator.csproj -c Release
cd $current_dir
