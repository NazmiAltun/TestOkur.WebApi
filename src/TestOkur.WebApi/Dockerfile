FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 as base
LABEL "Maintainer"="Nazmi Altun <nazmialtun@windowslive.com>"

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ./*.sln ./
COPY src/*/*.csproj ./  
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done
COPY tests/*/*.csproj ./  
RUN for file in $(ls *.csproj); do mkdir -p tests/${file%.*}/ && mv $file tests/${file%.*}/; done
RUN dotnet restore
COPY . .
RUN dotnet build "./src/TestOkur.WebApi/TestOkur.WebApi.csproj" -c Release --no-restore -o /app

FROM build as domaintest
WORKDIR /src/tests/TestOkur.Domain.Unit.Tests

FROM build as unittest
WORKDIR /src/tests/TestOkur.WebApi.Unit.Tests

FROM build as integrationtest
RUN apt-get update && apt-get install fonts-dejavu-core && apt-get install -y libgdiplus && rm -rf /var/lib/apt/lists/* && ln -s /lib/x86_64-linux-gnu/libdl.so.2 /lib/x86_64-linux-gnu/libdl.so && ln -s /usr/lib/libgdiplus.so /lib/x86_64-linux-gnu/libgdiplus.so
WORKDIR /src/tests/TestOkur.WebApi.Integration.Tests

FROM build AS publish
WORKDIR /src
RUN dotnet publish "./src/TestOkur.WebApi/TestOkur.WebApi.csproj" --no-restore -c Release -o /app

FROM base as final
WORKDIR /app
RUN apt-get update && apt-get install fonts-dejavu-core && apt-get install -y libgdiplus && rm -rf /var/lib/apt/lists/* && ln -s /lib/x86_64-linux-gnu/libdl.so.2 /lib/x86_64-linux-gnu/libdl.so && ln -s /usr/lib/libgdiplus.so /lib/x86_64-linux-gnu/libgdiplus.so
EXPOSE 80
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TestOkur.WebApi.dll"]
