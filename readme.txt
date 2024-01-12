docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
dotnet new classlib -o MyLibrary
dotnet new webapi -o MyWebApi
dotnet add reference ..\Catalog.Application\Catalog.Application.csproj    
dotnet ef migrations add initial-create   
dotnet ef database update