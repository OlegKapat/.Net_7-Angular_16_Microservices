FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5011

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Infrastructure/EShopping.Identity/EShopping.Identity.csproj", "Infrastructure/EShopping.Identity/"]
RUN dotnet restore "Infrastructure/EShopping.Identity/EShopping.Identity.csproj"
COPY . .
WORKDIR "/src/Infrastructure/EShopping.Identity"
RUN dotnet build "EShopping.Identity.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
# RUN dotnet publish "EShopping.Identity.csproj" -c $configuration -o /app/publish /p:UseAppHost=false
RUN dotnet publish "EShopping.Identity.csproj" -c $configuration -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShopping.Identity.dll"]
