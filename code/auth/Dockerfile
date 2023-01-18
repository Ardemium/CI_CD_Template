FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["auth.csproj", "./"]
RUN dotnet restore "./auth.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "auth.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "auth.dll"]