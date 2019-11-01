FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 as base
LABEL "Maintainer"="Nazmi Altun <nazmialtun@windowslive.com>"

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY . .
RUN dotnet build "./src/TestOkur.Sabit/TestOkur.Sabit.csproj" -c Release -o /app

FROM build as integrationtest
WORKDIR /src/tests/TestOkur.Sabit.Integration.Tests

FROM build AS publish
WORKDIR /src
RUN dotnet publish "./src/TestOkur.Sabit/TestOkur.Sabit.csproj" --no-restore -c Release -o /app

FROM base as final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TestOkur.Sabit.dll"]
