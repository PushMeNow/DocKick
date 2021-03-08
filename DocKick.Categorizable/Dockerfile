FROM mcr.microsoft.com/dotnet/sdk:5.0.200 AS build
WORKDIR /app

COPY *.sln .
COPY DocKick.Categorizable/*.csproj ./DocKick.Categorizable/
COPY DocKick.Data/*.csproj ./DocKick.Data/
COPY DocKick.Dtos/*.csproj ./DocKick.Dtos/
COPY DocKick.Entities/*.csproj ./DocKick.Entities/
COPY DocKick.Exceptions/*.csproj ./DocKick.Exceptions/
COPY DocKick.Helpers/*.csproj ./DocKick.Helpers/
COPY DocKick.Mapper/*.csproj ./DocKick.Mapper/
COPY DocKick.Services/*.csproj ./DocKick.Services/
COPY DocKick.Validation/*.csproj ./DocKick.Validation/

RUN dotnet restore

COPY DocKick.Categorizable/. ./DocKick.Categorizable/
COPY DocKick.Data/. ./DocKick.Data/
COPY DocKick.Dtos/. ./DocKick.Dtos/
COPY DocKick.Entities/. ./DocKick.Entities/
COPY DocKick.Exceptions/. ./DocKick.Exceptions/
COPY DocKick.Helpers/. ./DocKick.Helpers/
COPY DocKick.Mapper/. ./DocKick.Mapper/
COPY DocKick.Services/. ./DocKick.Services/
COPY DocKick.Validation/. ./DocKick.Validation/

WORKDIR /app/DocKick.Authentication
RUN dotnet publish -c Docker -o out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:5.0.3 AS runtime
WORKDIR /app

COPY --from=build /app/DocKick.Categorizable/out ./
ENTRYPOINT ["dotnet", "DocKick.Categorizable.dll"]