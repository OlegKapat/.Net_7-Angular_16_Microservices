FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
#EXPOSE 5044
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:5044

FROM mcr.microsoft.com/dotnet/sdk:7.0  AS build
ARG configuration=Release
WORKDIR /src
COPY ["ApiGateways/Ocelot.ApiGateways/Ocelot.ApiGateways.csproj", "ApiGateways/Ocelot.ApiGateways/"]
RUN dotnet restore "ApiGateways/Ocelot.ApiGateways/Ocelot.ApiGateways.csproj"
COPY . .
WORKDIR "/src/ApiGateways/Ocelot.ApiGateways"
RUN dotnet build "Ocelot.ApiGateways.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
# RUN dotnet publish "Ocelot.ApiGateways.csproj" -c $configuration -o /app/publish /p:UseAppHost=false
RUN dotnet publish "Ocelot.ApiGateways.csproj" -c $configuration -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ocelot.ApiGateways.dll"]
