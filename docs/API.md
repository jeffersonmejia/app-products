# API Documentation

## Table of Contents

1. [Overview](#1-overview)
2. [Routing](#2-routing)
3. [Product Endpoints](#3-product-endpoints)
4. [Category Endpoints](#4-category-endpoints)
5. [Models](#5-models)
6. [Validation](#6-validation)
7. [Response Format](#7-response-format)

## 1. Overview

This project exposes server-rendered MVC routes instead of a JSON REST API. Each endpoint returns an HTML view, redirects after successful form submissions, or returns a standard MVC error response such as `NotFound()`.

## 2. Routing

1. Default route pattern:

```text
{controller=Productos}/{action=Index}/{id?}
```

2. Default entry point:

```text
/Productos/Index
```

3. Optional `id` route values are used by details, edit, and delete operations.

## 3. Product Endpoints

1. `GET /Productos`
   - Displays a paginated product list.
   - Query parameter: `page`.
   - Default page: `1`.
   - Page size: `5`.

2. `GET /Productos/Details/{id}`
   - Displays one product by identifier.
   - Returns `NotFound()` when the id is missing or the product does not exist.

3. `GET /Productos/Create`
   - Displays the product creation form.

4. `POST /Productos/Create`
   - Creates a product.
   - Uses anti-forgery validation.
   - Binds `Id`, `Nombre`, `Descripcion`, `Precio`, and `Stock`.
   - Sets `FechaRegistro` with the current UTC time before saving.
   - Redirects to the product index after success.

5. `GET /Productos/Edit/{id}`
   - Displays the edit form for an existing product.
   - Returns `NotFound()` when the id is missing or the product does not exist.

6. `POST /Productos/Edit/{id}`
   - Updates an existing product.
   - Uses anti-forgery validation.
   - Verifies that the route id matches the submitted product id.
   - Handles EF Core concurrency errors and returns `NotFound()` when the product no longer exists.
   - Redirects to the product index after success.

7. `GET /Productos/Delete/{id}`
   - Displays a delete confirmation page.
   - Returns `NotFound()` when the id is missing or the product does not exist.

8. `POST /Productos/Delete/{id}`
   - Deletes the selected product.
   - Uses anti-forgery validation.
   - Redirects to the product index after success.

## 4. Category Endpoints

1. `GET /Categorias`
   - Displays LINQ query results for categories.
   - Optional query parameter: `busqueda`.

2. Category data shown by the page:
   - All categories ordered by id.
   - Active categories ordered by name.
   - Categories filtered by `busqueda` using PostgreSQL case-insensitive matching.
   - The three most recent categories ordered by creation date.

## 5. Models

1. `Producto`
   - `Id`: integer primary identifier.
   - `Nombre`: required string, maximum length 100.
   - `Descripcion`: required string, maximum length 250.
   - `Precio`: decimal value from `0.01` to `9999.99`.
   - `Stock`: integer value from `0` to `10000`.
   - `CategoriaId`: optional foreign key to `Categoria`.
   - `FechaRegistro`: UTC registration date.

2. `Categoria`
   - `Id`: integer primary identifier.
   - `Nombre`: required string, maximum length 80.
   - `Descripcion`: required string, maximum length 200.
   - `DescuentoPorcentaje`: decimal discount percentage from `0` to `100`.
   - `Activa`: boolean status.
   - `FechaCreacion`: UTC creation date.

3. `PagedResult<T>`
   - Contains paginated items, current page, page size, total item count, and pagination metadata.

4. `ConsultasCategoriasViewModel`
   - Contains the category query results displayed by the category page.

## 6. Validation

1. MVC model validation is driven by data annotations in the model classes.
2. Product create and edit forms validate required fields, string lengths, price range, and stock range.
3. Category fields are validated by model annotations and EF Core model configuration.
4. POST actions use `[ValidateAntiForgeryToken]` to reduce CSRF risk.

## 7. Response Format

1. Successful GET requests return HTML pages rendered by Razor views.
2. Successful POST requests redirect to the related index page.
3. Missing entities return MVC `NotFound()` responses.
4. Invalid form submissions redisplay the same view with validation messages.
