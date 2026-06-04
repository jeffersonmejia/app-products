dotnet --version

dotnet workload update

dotnet tool update --global dotnet-ef
dotnet tool update --global dotnet-aspnet-codegenerator

dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 10.0.1
dotnet add package Microsoft.EntityFrameworkCore.Design --version 10.0.2
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 10.0.2
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --version 10.0.2
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 10.0.2

dotnet tool install -g dotnet-ef --version 10.0.2
dotnet tool install -g dotnet-aspnet-codegenerator --version 10.0.2

dotnet restore

dotnet ef --version
dotnet aspnet-codegenerator --help

dotnet build