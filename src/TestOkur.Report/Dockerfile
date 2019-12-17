FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as base
LABEL "Maintainer"="Nazmi Altun <nazmialtun@windowslive.com>"

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY . .
RUN dotnet build "./src/TestOkur.Report/TestOkur.Report.csproj" -c Release  -o /app
RUN dotnet tool install --global dotnet-trace
RUN dotnet tool install --global dotnet-dump
RUN dotnet tool install --global dotnet-counters
RUN dotnet tool install --global dotnet-sos
ENV PATH="/root/.dotnet/tools:${PATH}"

FROM build as unittest
WORKDIR /src/tests/TestOkur.Report.Unit.Tests

FROM build as integrationtest
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.5.0/wait /wait
RUN chmod +x /wait
WORKDIR /src/tests/TestOkur.Report.Integration.Tests

FROM build AS publish
WORKDIR /src
RUN dotnet publish "./src/TestOkur.Report/TestOkur.Report.csproj" --no-restore -c Release -o /app

#FROM golang:latest as minifier
#RUN go get -v github.com/tdewolff/minify/cmd/minify
#WORKDIR /minify
#COPY /src/TestOkur.Report/wwwroot/Templates .
#RUN minify -o out/ .

FROM base as final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app .
#COPY --from=minifier /minify/out wwwroot/Templates/
COPY --from=build /root/.dotnet/tools/ /opt/bin
ENV PATH="/opt/bin:${PATH}"
ENTRYPOINT ["dotnet", "TestOkur.Report.dll"]
