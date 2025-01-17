ARG COMMIT=""

FROM mcr.microsoft.com/dotnet/aspnet:5.0 as base
LABEL "Maintainer"="Nazmi Altun <nazmialtun@windowslive.com>"
LABEL Commit=${COMMIT}
RUN apt-get update && apt-get upgrade -y && apt-get install -y curl vim
RUN sed -i 's/SECLEVEL=2/SECLEVEL=1/g' /etc/ssl/openssl.cnf

FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
WORKDIR /src
COPY . .
RUN dotnet build "./src/TestOkur.Notification/TestOkur.Notification.csproj" -c Release -o /app

FROM build AS publish
WORKDIR /src
RUN dotnet publish "./src/TestOkur.Notification/TestOkur.Notification.csproj" --no-restore -c Release -o /app

FROM base as final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app .
ARG COMMIT
ENV COMMIT_SHA=${COMMIT}
ENTRYPOINT ["dotnet", "TestOkur.Notification.dll"]
