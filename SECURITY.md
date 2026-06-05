# Security Policy

## Table of Contents

1. [Scope](#1-scope)
2. [Supported Version](#2-supported-version)
3. [Reporting Security Issues](#3-reporting-security-issues)
4. [Sensitive Data Handling](#4-sensitive-data-handling)
5. [Security Documentation](#5-security-documentation)
6. [Current Security Controls](#6-current-security-controls)
7. [Future Work](#7-future-work)

## 1. Scope

This repository is an academic ASP.NET Core MVC project that uses PostgreSQL and Entity Framework Core. The security policy applies to application code, database configuration, Docker Compose configuration, and project documentation.

## 2. Supported Version

1. The current repository state is the supported version.
2. Older classroom or experimental revisions are not maintained unless they are restored in the active branch.

## 3. Reporting Security Issues

1. Do not publish exploitable details in a public issue.
2. Share the affected file, route, or configuration item with the project maintainer.
3. Include clear reproduction steps when possible.
4. Include the expected risk, such as unauthorized data modification, exposed credentials, or broken validation.

## 4. Sensitive Data Handling

1. Do not commit real production passwords, tokens, private keys, or personal credentials.
2. Use environment variables, .NET user secrets, Docker secrets, or a managed secret provider for sensitive values.
3. Rotate any credential that was accidentally committed.
4. Keep local secret files outside public distribution.

## 5. Security Documentation

1. Application security details are documented in [docs/SECURITY.md](docs/SECURITY.md).
2. Architecture details are documented in [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md).
3. MVC route and endpoint details are documented in [docs/API.md](docs/API.md).

## 6. Current Security Controls

1. Product and category POST actions use anti-forgery validation.
2. Entity Framework Core LINQ queries are used instead of raw SQL, reducing SQL injection risk through query parameterization.
3. Product creation and editing reject another product with exactly the same `Nombre`.
4. Category creation and editing reject another category with exactly the same `Nombre`.
5. Product forms validate that the selected `CategoriaId` exists before saving.
6. Product and category text input is trimmed before saving to avoid duplicated values caused only by leading or trailing spaces.
7. The category search input is trimmed and limited before being used in the LINQ query.
8. Basic HTTP security headers are added in `Program.cs`: `X-Content-Type-Options`, `X-Frame-Options`, `Referrer-Policy`, and `Permissions-Policy`.

## 7. Future Work

1. Add authentication so only authorized users can create, edit, or delete products and categories.
2. Add authorization roles or policies for administrative actions.
3. Move the database password out of `appsettings.json` and into user secrets, environment variables, Docker secrets, or a managed secret provider.
4. Add database-level unique indexes for product and category names to protect against race conditions between simultaneous requests.
5. Add centralized exception logging for failed writes, validation failures, and unexpected database errors.
6. Add rate limiting to reduce repeated automated form submissions.
7. Add a Content Security Policy after replacing the current CDN and inline Tailwind configuration with local or hashed assets.
8. Restrict `AllowedHosts` for any deployment outside local development.
9. Review whether automatic migrations should run on startup in production environments.
