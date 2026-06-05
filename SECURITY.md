# Security Policy

## Table of Contents

1. [Scope](#1-scope)
2. [Supported Version](#2-supported-version)
3. [Reporting Security Issues](#3-reporting-security-issues)
4. [Sensitive Data Handling](#4-sensitive-data-handling)
5. [Security Documentation](#5-security-documentation)

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
