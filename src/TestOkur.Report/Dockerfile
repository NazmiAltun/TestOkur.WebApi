FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine as base
LABEL "Maintainer"="Nazmi Altun <nazmialtun@windowslive.com>"

FROM microsoft/dotnet:2.2-sdk-alpine AS build
WORKDIR /src
COPY ./*.sln ./
COPY src/*/*.csproj ./  
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done
COPY tests/*/*.csproj ./  
RUN for file in $(ls *.csproj); do mkdir -p tests/${file%.*}/ && mv $file tests/${file%.*}/; done
RUN dotnet restore
COPY . .
RUN dotnet build "./src/TestOkur.Report/TestOkur.Report.csproj" -c Release --no-restore -o /app

FROM build as integrationtest
WORKDIR /src/tests/TestOkur.Report.Integration.Tests

FROM build AS publish
WORKDIR /src
RUN dotnet publish "./src/TestOkur.Report/TestOkur.Report.csproj" --no-restore -c Release -o /app

FROM base as final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TestOkur.Report.dll"]
