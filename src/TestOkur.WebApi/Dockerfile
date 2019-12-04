FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as base
LABEL "Maintainer"="Nazmi Altun <nazmialtun@windowslive.com>"

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY . .
RUN dotnet build "./src/TestOkur.WebApi/TestOkur.WebApi.csproj" -c Release -o /app
RUN dotnet tool install --global dotnet-trace
RUN dotnet tool install --global dotnet-dump
RUN dotnet tool install --global dotnet-counters
RUN dotnet tool install --global dotnet-sos
ENV PATH="/root/.dotnet/tools:${PATH}"

FROM build as domaintest
WORKDIR /src/tests/TestOkur.Domain.Unit.Tests

FROM build as unittest
WORKDIR /src/tests/TestOkur.WebApi.Unit.Tests

FROM build as integrationtest
ARG install_drawing_packages
RUN if [ "$install_drawing_packages" = "True" ]; then \
    apt-get update && \
	apt-get install fonts-dejavu-core && \
	apt-get install -y libgdiplus && \
	rm -rf /var/lib/apt/lists/* && \
	ln -s /lib/x86_64-linux-gnu/libdl.so.2 /lib/x86_64-linux-gnu/libdl.so && \
	ln -s /usr/lib/libgdiplus.so /lib/x86_64-linux-gnu/libgdiplus.so; \
  else \
    echo "Drawing packages won't be installed"; \
  fi
WORKDIR /src/tests/TestOkur.WebApi.Integration.Tests

FROM build AS publish
WORKDIR /src
RUN dotnet publish "./src/TestOkur.WebApi/TestOkur.WebApi.csproj" --no-restore -c Release -o /app

FROM base as final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app .
COPY --from=build /root/.dotnet/tools/ /opt/bin
ENV PATH="/opt/bin:${PATH}"
ENTRYPOINT ["dotnet", "TestOkur.WebApi.dll"]
