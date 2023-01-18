FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 82

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["authz.csproj", "./"]
RUN dotnet restore "./authz.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "authz.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "authz.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "authz.dll"]