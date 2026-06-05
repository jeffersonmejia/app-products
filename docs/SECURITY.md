# Application Security Documentation

## Table of Contents

1. [Overview](#1-overview)
2. [Implemented Controls](#2-implemented-controls)
3. [Database Security](#3-database-security)
4. [Input Validation](#4-input-validation)
5. [Configuration Risks](#5-configuration-risks)
6. [Recommended Improvements](#6-recommended-improvements)

## 1. Overview

This document describes security controls and risks for the ASP.NET Core MVC application. The project is intended for local academic use, but it already includes several basic protections through ASP.NET Core, Entity Framework Core, and Docker Compose.

## 2. Implemented Controls

1. Anti-forgery validation is enabled on product POST actions with `[ValidateAntiForgeryToken]`.
2. Model validation uses data annotations such as `[Required]`, `[StringLength]`, and `[Range]`.
3. Entity Framework Core parameterizes database queries generated from LINQ expressions.
4. Production exception handling is configured through `UseExceptionHandler("/Home/Error")`.
5. HSTS is enabled outside the development environment.
6. HTTPS redirection is enabled outside the development environment.
7. Static file serving is limited to configured application static files.

## 3. Database Security

1. PostgreSQL runs in Docker with a local-only port binding: `127.0.0.1:5432`.
2. The Docker Compose file uses a password secret file instead of placing the PostgreSQL password directly in the compose environment block.
3. The PostgreSQL container drops Linux capabilities with `cap_drop: ALL`.
4. The container uses `no-new-privileges:true`.
5. The container filesystem is read-only, with temporary writable mounts for PostgreSQL runtime paths.
6. PostgreSQL uses SCRAM authentication arguments for host and local authentication.

## 4. Input Validation

1. Product names and descriptions are required.
2. Product names are limited to 100 characters.
3. Product descriptions are limited to 250 characters.
4. Product prices must be greater than zero and no higher than `9999.99`.
5. Product stock cannot be negative and cannot exceed `10000`.
6. Category names and descriptions are required.
7. Category names are limited to 80 characters.
8. Category descriptions are limited to 200 characters.
9. Category discount percentages must be between `0` and `100`.

## 5. Configuration Risks

1. `appsettings.json` currently contains a database password in the connection string.
2. The application does not include authentication or authorization rules for product changes.
3. Product and category create, update, and delete actions are publicly reachable in the current MVC setup.
4. `AllowedHosts` is set to `*`, which is acceptable for local development but should be restricted for production.
5. Automatic migrations on startup are convenient for development, but production deployments should review migration execution policies.

## 6. Recommended Improvements

1. Move the application database password out of `appsettings.json` and into user secrets, environment variables, or a managed secret provider.
2. Add authentication before allowing product create, edit, and delete actions.
3. Add authorization policies for administrative actions.
4. Restrict `AllowedHosts` to the real host names used in deployment.
5. Enable HTTPS in every deployed environment.
6. Review migration execution before production release.
7. Add logging for failed write operations and unexpected database errors.
8. Keep Docker secrets outside version control.
