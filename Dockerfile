

FROM microsoft/dotnet:2.0-runtime-nanoserver-1709 AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY DatingApp.API/DatingApp.API.csproj DatingApp.API/DatingApp.API/
RUN dotnet restore DatingApp.API/DatingApp.API/DatingApp.API/DatingApp.API.csproj
WORKDIR /src/DatingApp.API/DatingApp.API
COPY . .
RUN dotnet build DatingApp.API/DatingApp.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish DatingApp.API/DatingApp.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DatingApp.API/DatingApp.API.dll"]
