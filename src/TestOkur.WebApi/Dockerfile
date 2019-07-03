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
RUN dotnet build "./src/TestOkur.WebApi/TestOkur.WebApi.csproj" -c Release --no-restore -o /app


FROM build as domaintest
WORKDIR /src/tests/TestOkur.Domain.Unit.Tests

FROM build as unittest
WORKDIR /src/tests/TestOkur.WebApi.Unit.Tests

FROM build as integrationtest
RUN apk add libgdiplus bash ttf-ubuntu-font-family --repository http://dl-3.alpinelinux.org/alpine/edge/testing/ && \
    ln /usr/lib/libgdiplus.so.0 /usr/lib/libgdiplus.so && \
	rm -rf /var/cache/apk/* && \
	fc-cache -f
WORKDIR /src/tests/TestOkur.WebApi.Integration.Tests

FROM build AS publish
WORKDIR /src
RUN dotnet publish "./src/TestOkur.WebApi/TestOkur.WebApi.csproj" --no-restore -c Release -o /app

FROM base as final
WORKDIR /app
RUN apk add libgdiplus bash ttf-ubuntu-font-family --repository http://dl-3.alpinelinux.org/alpine/edge/testing/ && \
    ln /usr/lib/libgdiplus.so.0 /usr/lib/libgdiplus.so && \
	rm -rf /var/cache/apk/* && \
	fc-cache -f
EXPOSE 80
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TestOkur.WebApi.dll"]
