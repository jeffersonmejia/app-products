# Actividad Code First, PostgreSQL y consultas LINQ

## Objetivo

Analizar los servicios, dependencias y componentes principales de una aplicacion ASP.NET Core MVC con Entity Framework Core, aplicando el enfoque Code First para crear una nueva entidad y utilizando consultas LINQ basicas para recuperar, filtrar, ordenar y limitar datos desde PostgreSQL.

## Componentes principales del proyecto

- `Program.cs`: configura los servicios de ASP.NET Core MVC, registra `AppDbContext` con PostgreSQL y aplica migraciones al iniciar.
- `Data/AppDbContext.cs`: representa la conexion de EF Core con la base de datos. Contiene los `DbSet` de `Productos` y `Categorias`.
- `Models/Producto.cs`: entidad inicial del taller.
- `Models/Categoria.cs`: nueva entidad creada para esta actividad con enfoque Code First.
- `Data/DbInitializer.cs`: aplica migraciones pendientes y carga datos iniciales de productos y categorias si las tablas estan vacias.
- `Controllers/CategoriasController.cs`: ejecuta consultas LINQ basicas sobre PostgreSQL.
- `Views/Categorias/Index.cshtml`: muestra los resultados de las consultas LINQ.
- `docker-compose.yml`: levanta PostgreSQL local con healthcheck, volumen persistente y secret para la clave.

## Dependencias utilizadas

El proyecto usa:

- ASP.NET Core MVC.
- Entity Framework Core.
- `Npgsql.EntityFrameworkCore.PostgreSQL` como proveedor de PostgreSQL.
- Docker Compose para levantar la base de datos local.

## Nueva tabla creada con Code First

La nueva entidad es `Categoria`:

```csharp
public class Categoria
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool Activa { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}
```

La entidad se registra en `AppDbContext`:

```csharp
public DbSet<Categoria> Categorias { get; set; }
```

La migracion que crea la tabla esta en:

```text
Migrations/20260603235000_AddCategorias.cs
```

## Carga de datos

La clase `DbInitializer` inserta productos y categorias iniciales cuando las tablas estan vacias:

```csharp
if (await context.Productos.AnyAsync())
{
    return;
}
```

Esto evita duplicar datos cada vez que se inicia la aplicacion.

## Consultas LINQ implementadas

Las consultas estan en `Controllers/CategoriasController.cs`.

Recuperar datos:

```csharp
await categorias.OrderBy(c => c.Id).ToListAsync();
```

Filtrar datos:

```csharp
await categorias.Where(c => c.Activa).ToListAsync();
```

Filtrar texto con PostgreSQL:

```csharp
await categorias
    .Where(c => EF.Functions.ILike(c.Nombre, $"%{busqueda}%")
        || EF.Functions.ILike(c.Descripcion, $"%{busqueda}%"))
    .ToListAsync();
```

Ordenar datos:

```csharp
await categorias.OrderBy(c => c.Nombre).ToListAsync();
```

Limitar resultados:

```csharp
await categorias
    .OrderByDescending(c => c.FechaCreacion)
    .Take(3)
    .ToListAsync();
```

## Pasos para replicar

1. Levantar PostgreSQL:

```bash
docker compose up -d postgres
```

2. Ejecutar la aplicacion:

```bash
dotnet run
```

Al iniciar, la aplicacion aplica las migraciones pendientes y carga los productos y categorias iniciales.

3. Abrir el apartado de la nueva tabla:

```text
http://localhost:5198/Categorias
```

Tambien se puede entrar desde el menu superior en `Nueva tabla: Categorias`.

## Verificacion esperada

En la ruta `/Categorias` se observan cuatro bloques:

- Todas las categorias recuperadas desde PostgreSQL.
- Categorias activas filtradas con `Where`.
- Categorias filtradas por texto usando `ILike`.
- Ultimas 3 categorias usando `OrderByDescending` y `Take`.

La tabla `Categorias` se crea desde el modelo C# mediante la migracion, manteniendo el enfoque Code First.
