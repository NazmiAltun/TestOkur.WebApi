FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 as base
LABEL "Maintainer"="Nazmi Altun <nazmialtun@windowslive.com>"

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY ./*.sln ./
COPY src/*/*.csproj ./  
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done
COPY tests/*/*.csproj ./  
RUN for file in $(ls *.csproj); do mkdir -p tests/${file%.*}/ && mv $file tests/${file%.*}/; done
RUN dotnet restore
COPY . .
RUN dotnet build "./src/TestOkur.Notification/TestOkur.Notification.csproj" -c Release --no-restore -o /app

FROM build as unittest
WORKDIR /src/tests/TestOkur.Notification.Unit.Tests

FROM build as integrationtest
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.5.0/wait /wait
RUN chmod +x /wait
WORKDIR /src/tests/TestOkur.Notification.Integration.Tests

FROM build AS publish
WORKDIR /src
RUN dotnet publish "./src/TestOkur.Notification/TestOkur.Notification.csproj" --no-restore -c Release -o /app

FROM base as final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TestOkur.Notification.dll"]
