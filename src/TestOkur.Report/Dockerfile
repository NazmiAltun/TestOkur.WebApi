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
RUN dotnet build "./src/TestOkur.Report/TestOkur.Report.csproj" -c Release --no-restore -o /app

FROM build as unittest
WORKDIR /src/tests/TestOkur.Report.Unit.Tests

FROM build as integrationtest
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.5.0/wait /wait
RUN chmod +x /wait
WORKDIR /src/tests/TestOkur.Report.Integration.Tests

FROM build AS publish
WORKDIR /src
RUN dotnet publish "./src/TestOkur.Report/TestOkur.Report.csproj" --no-restore -c Release -o /app

FROM golang:latest as minifier
RUN go get -v github.com/tdewolff/minify/cmd/minify
WORKDIR /minify
COPY /src/TestOkur.Report/wwwroot/Templates .
RUN minify -o out/ .

FROM base as final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app .
COPY --from=minifier /minify/out wwwroot/Templates/
ENTRYPOINT ["dotnet", "TestOkur.Report.dll"]
