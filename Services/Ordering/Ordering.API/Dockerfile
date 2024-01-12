FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
# EXPOSE 5044
EXPOSE 80
EXPOSE 443

# ENV ASPNETCORE_URLS=http://+:5044

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Services/Ordering/Ordering.API/Ordering.API.csproj", "Services/Ordering/Ordering.API/"]
COPY ["Services/Ordering/Ordering.Aplication/Ordering.Aplication.csproj", "Services/Ordering/Ordering.Aplication/"]
COPY ["Services/Ordering/Ordering.Core/Ordering.Core.csproj", "Services/Ordering/Ordering.Core/"]
COPY ["Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj", "Services/Ordering/Ordering.Infrastructure/"]
COPY ["Infrastructure/EventBus.Messages/EventBus.Messages.csproj", "Infrastructure/EventBus.Messages/"]

RUN dotnet restore "Services/Ordering/Ordering.API/Ordering.API.csproj"
COPY . .
WORKDIR "/src/Services/Ordering/Ordering.API"
RUN dotnet build "Ordering.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
# RUN dotnet publish "Ordering.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false
RUN dotnet publish "Ordering.API.csproj" -c $configuration -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]
