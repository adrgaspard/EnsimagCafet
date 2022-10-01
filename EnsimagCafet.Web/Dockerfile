FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EnsimagCafet.Web/EnsimagCafet.Web.csproj", "EnsimagCafet.Web/"]
COPY ["EnsimagCafet.Domain.Shared/EnsimagCafet.Domain.Shared.csproj", "EnsimagCafet.Domain.Shared/"]
COPY ["EnsimagCafet.EntityFrameworkCore/EnsimagCafet.EntityFrameworkCore.csproj", "EnsimagCafet.EntityFrameworkCore/"]
COPY ["EnsimagCafet.Domain/EnsimagCafet.Domain.csproj", "EnsimagCafet.Domain/"]
COPY ["APITools.Domain.Contracts/APITools.Domain.Contracts.csproj", "APITools.Domain.Contracts/"]
COPY ["APITools.CommonTools/APITools.CommonTools.csproj", "APITools.CommonTools/"]
COPY ["EnsimagCafet.MailKit/EnsimagCafet.MailKit.csproj", "EnsimagCafet.MailKit/"]
RUN dotnet restore "EnsimagCafet.Web/EnsimagCafet.Web.csproj"
COPY . .
WORKDIR "/src/EnsimagCafet.Web"
RUN dotnet build "EnsimagCafet.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EnsimagCafet.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnsimagCafet.Web.dll"]