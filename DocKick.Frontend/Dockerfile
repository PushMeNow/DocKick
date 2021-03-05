FROM mcr.microsoft.com/dotnet/sdk:5.0.200 AS build

RUN curl --silent --location https://deb.nodesource.com/setup_10.x | bash -
RUN apt-get install --yes nodejs

WORKDIR /app

COPY *.sln .
COPY DocKick.Frontend/*.csproj ./DocKick.Frontend/

RUN dotnet restore

COPY DocKick.Frontend/. ./DocKick.Frontend/

WORKDIR /app/DocKick.Frontend
RUN dotnet publish -c Debug -o out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:5.0.3 AS runtime
WORKDIR /app

COPY --from=build /app/DocKick.Frontend/out ./
ENTRYPOINT ["dotnet", "DocKick.Frontend.dll"]