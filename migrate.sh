#!/bin/sh

dotnet build EnsimagCafet.EntityFrameworkCore.DbMigrator/EnsimagCafet.EntityFrameworkCore.DbMigrator.csproj -c Release \
&& dotnet ef database update --project EnsimagCafet.EntityFrameworkCore.DbMigrator/EnsimagCafet.EntityFrameworkCore.DbMigrator.csproj \
&& dotnet run --project EnsimagCafet.EntityFrameworkCore.DbMigrator/EnsimagCafet.EntityFrameworkCore.DbMigrator.csproj -c Release
