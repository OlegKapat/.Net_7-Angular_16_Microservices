docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
dotnet new classlib -o MyLibrary
dotnet new webapi -o MyWebApi
dotnet add reference ..\Catalog.Application\Catalog.Application.csproj    
dotnet ef migrations add initial-create   
dotnet ef database update
when migrate DB: dotnet ef database update --connection "Server=DESKTOP-BFF5P6I\SQLEXPRESS;Database=OrderDb;
                 User Id=bombastik;Password=1q2q3q4q;TrustServerCertificate=True;MultipleActiveResultSets=true"
dotnet new -i identityserver4.templates
dotnet new is4inmem -n EShopping.Identity