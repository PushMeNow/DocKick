FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

COPY *.sln .
COPY DocKick.Authentication/*.csproj ./DocKick.Authentication/
COPY DocKick.Data/*.csproj ./DocKick.Data/
COPY DocKick.DataTransferModels/*.csproj ./DocKick.DataTransferModels/
COPY DocKick.Entities/*.csproj ./DocKick.Entities/
COPY DocKick.Exceptions/*.csproj ./DocKick.Exceptions/
COPY DocKick.Extensions/*.csproj ./DocKick.Extensions/
COPY DocKick.Mapper/*.csproj ./DocKick.Mapper/
COPY DocKick.Services/*.csproj ./DocKick.Services/

RUN dotnet restore

COPY DocKick.Authentication/. ./DocKick.Authentication/
COPY DocKick.Data/. ./DocKick.Data/
COPY DocKick.DataTransferModels/. ./DocKick.DataTransferModels/
COPY DocKick.Entities/. ./DocKick.Entities/
COPY DocKick.Exceptions/. ./DocKick.Exceptions/
COPY DocKick.Extensions/. ./DocKick.Extensions/
COPY DocKick.Mapper/. ./DocKick.Mapper/
COPY DocKick.Services/. ./DocKick.Services/

WORKDIR /app/DocKick.Authentication
RUN dotnet publish -c Docker -o out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app

COPY --from=build /app/DocKick.Authentication/out ./
ENTRYPOINT ["dotnet", "DocKick.Authentication.dll"]