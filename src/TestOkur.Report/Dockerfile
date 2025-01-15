ARG COMMIT=""

FROM mcr.microsoft.com/dotnet/aspnet:5.0 as base
LABEL "Maintainer"="Nazmi Altun <nazmialtun@windowslive.com>"
LABEL Commit=${COMMIT}

FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
WORKDIR /src
COPY . .
RUN dotnet build "./src/TestOkur.Report/TestOkur.Report.csproj" -c Release  -o /app

FROM build AS publish
WORKDIR /src
RUN dotnet publish "./src/TestOkur.Report/TestOkur.Report.csproj" --no-restore -c Release -o /app

FROM golang:latest as minifier
RUN go install github.com/tdewolff/minify/v2@latest
WORKDIR /minify
COPY /src/TestOkur.Report/wwwroot/Templates .
RUN minify -o out/ .

FROM base as final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app .
COPY --from=minifier /minify/out wwwroot/Templates/
ARG COMMIT
ENV COMMIT_SHA=${COMMIT}
ENTRYPOINT ["dotnet", "TestOkur.Report.dll"]
