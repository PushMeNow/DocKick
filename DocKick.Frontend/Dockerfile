FROM mcr.microsoft.com/dotnet/sdk:5.0.200 AS build

### Insall NodeJS
RUN apt-get install --yes curl
RUN curl --silent --location https://deb.nodesource.com/setup_15.x | bash -
RUN apt-get install -y nodejs
###

WORKDIR /app

COPY *.sln .
COPY DocKick.Frontend/*.csproj ./DocKick.Frontend/

RUN dotnet restore

COPY DocKick.Frontend/. ./DocKick.Frontend/

#WORKDIR /app/DocKick.Frontend/wwwroot
#RUN npm install
#RUN npm run build

WORKDIR /app/DocKick.Frontend/
RUN dotnet publish -c Release -o out/ --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:5.0.3 AS runtime
WORKDIR /app

COPY --from=build /app/DocKick.Frontend/out ./
ENTRYPOINT ["dotnet", "DocKick.Frontend.dll"]