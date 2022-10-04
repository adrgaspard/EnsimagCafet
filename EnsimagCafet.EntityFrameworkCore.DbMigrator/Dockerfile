FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EnsimagCafet.EntityFrameworkCore.DbMigrator/EnsimagCafet.EntityFrameworkCore.DbMigrator.csproj", "EnsimagCafet.EntityFrameworkCore.DbMigrator/"]
COPY ["EnsimagCafet.Domain.Shared/EnsimagCafet.Domain.Shared.csproj", "EnsimagCafet.Domain.Shared/"]
COPY ["EnsimagCafet.EntityFrameworkCore/EnsimagCafet.EntityFrameworkCore.csproj", "EnsimagCafet.EntityFrameworkCore/"]
COPY ["EnsimagCafet.Domain/EnsimagCafet.Domain.csproj", "EnsimagCafet.Domain/"]
COPY ["APITools.Domain.Contracts/APITools.Domain.Contracts.csproj", "APITools.Domain.Contracts/"]
COPY ["APITools.CommonTools/APITools.CommonTools.csproj", "APITools.CommonTools/"]
RUN dotnet restore "EnsimagCafet.EntityFrameworkCore.DbMigrator/EnsimagCafet.EntityFrameworkCore.DbMigrator.csproj"
COPY . .
WORKDIR "/src/EnsimagCafet.EntityFrameworkCore.DbMigrator"
RUN dotnet build "EnsimagCafet.EntityFrameworkCore.DbMigrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EnsimagCafet.EntityFrameworkCore.DbMigrator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnsimagCafet.EntityFrameworkCore.DbMigrator.dll"]