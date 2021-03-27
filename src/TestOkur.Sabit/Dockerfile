ARG COMMIT=""

FROM mcr.microsoft.com/dotnet/aspnet:5.0 as base
LABEL "Maintainer"="Nazmi Altun <nazmialtun@windowslive.com>"
LABEL Commit=${COMMIT}

FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
WORKDIR /src
COPY . .
RUN dotnet build "./src/TestOkur.Sabit/TestOkur.Sabit.csproj" -c Release -o /app

FROM build AS publish
WORKDIR /src
RUN dotnet publish "./src/TestOkur.Sabit/TestOkur.Sabit.csproj" --no-restore -c Release -o /app

FROM base as final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app .
ARG COMMIT
ENV COMMIT_SHA=${COMMIT}
ENTRYPOINT ["dotnet", "TestOkur.Sabit.dll"]
