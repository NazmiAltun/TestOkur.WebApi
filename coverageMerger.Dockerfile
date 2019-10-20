FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine
RUN dotnet tool install -g dotnet-reportgenerator-globaltool
RUN mkdir /reports
VOLUME /reports/
WORKDIR /src
COPY . .
ENTRYPOINT /root/.dotnet/tools/reportgenerator \
      "-reports:/reports/*test.opencover.xml" \
      "-targetdir:/reports" \
      -reporttypes:Cobertura