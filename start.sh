#!/bin/bash

PROJECT_NAME="ProductsApp"

dotnet new mvc -n "$PROJECT_NAME"

cd "$PROJECT_NAME" || exit 1

dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

dotnet tool install --global dotnet-ef

dotnet restore

dotnet build

dotnet ef migrations add InitialCreate

dotnet ef database update

dotnet run