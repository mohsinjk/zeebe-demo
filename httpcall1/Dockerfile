
FROM mcr.microsoft.com/dotnet/sdk:7.0

WORKDIR /app

COPY . .

RUN dotnet restore --configfile ./nuget.config

ENTRYPOINT ["dotnet","run"]
