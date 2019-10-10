FROM mcr.microsoft.com/dotnet/core/sdk:3.0 
RUN dotnet tool install -g dotnet-reportgenerator-globaltool
RUN mkdir /reports
VOLUME /reports/
WORKDIR /src
COPY . .
ENTRYPOINT /root/.dotnet/tools/reportgenerator \
      "-reports:/reports/*test.xml" \
      "-targetdir:/reports" \
      -reporttypes:Cobertura