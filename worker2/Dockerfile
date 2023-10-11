
FROM mcr.microsoft.com/dotnet/sdk:7.0

WORKDIR /app

COPY . .

RUN dotnet restore

ENTRYPOINT ["dotnet","run"]
