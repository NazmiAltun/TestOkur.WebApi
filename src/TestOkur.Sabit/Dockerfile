ARG COMMIT=""

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as base
LABEL "Maintainer"="Nazmi Altun <nazmialtun@windowslive.com>"
LABEL Commit=${COMMIT}

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY . .
RUN dotnet build "./src/TestOkur.Sabit/TestOkur.Sabit.csproj" -c Release -o /app
RUN dotnet tool install --global dotnet-trace
RUN dotnet tool install --global dotnet-dump
RUN dotnet tool install --global dotnet-counters
RUN dotnet tool install --global dotnet-sos
ENV PATH="/root/.dotnet/tools:${PATH}"

FROM build as integrationtest
WORKDIR /src/tests/TestOkur.Sabit.Integration.Tests

FROM build AS publish
WORKDIR /src
RUN dotnet publish "./src/TestOkur.Sabit/TestOkur.Sabit.csproj" --no-restore -c Release -o /app

FROM base as final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app .
COPY --from=build /root/.dotnet/tools/ /opt/bin
ENV PATH="/opt/bin:${PATH}"
ENV COMMIT_SHA=${COMMIT}
RUN echo ${COMMIT}
ENTRYPOINT ["dotnet", "TestOkur.Sabit.dll"]
