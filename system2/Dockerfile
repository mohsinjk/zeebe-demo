FROM mcr.microsoft.com/dotnet/core/sdk:3.1

WORKDIR /app

COPY . .

RUN dotnet restore --configfile ./nuget.config

ENTRYPOINT ["dotnet","run"]

