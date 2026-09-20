# Store Management API

A store management REST API built with **ASP.NET Core 8**, **Entity Framework Core** and **SQL Server**, organised with Clean Architecture. Customers register, browse products and place orders with JWT authentication.

## Architecture

```
Store/
  Store.Domain           Entities (BaseEntity, Customer, Category, Product, Order, OrderItem) and enums
  Store.Application      Contracts, DTOs, AutoMapper profile, feature services, exceptions,
                         response wrapper, exception middleware, model-state filter
  Store.Infrastructure   JWT token service and PBKDF2 password hasher
  Store.Persistence      DbContext, entity configurations, seed data, repositories
  Store.Web.Api          Controllers, Swagger, authentication and the composition root
```

The layout follows the same conventions as a layered portal project: services live in `Application/Features`, every layer exposes a `Configure...Services` extension, and every response goes through a common `Response<T>` wrapper.

## Features

- Customer registration and login with JWT
- Product catalogue with search, category, price filters and pagination
- Order placement with stock validation and price snapshots in a single transaction
- Soft delete through `IsActive`, automatic `CreatedAt` and `UpdatedAt`
- Consistent error responses through `ExceptionMiddleware`

## Run locally

```bash
cd Store/Store.Web.Api
dotnet user-secrets set "JwtSettings:Key" "<random string, at least 32 characters>"
dotnet ef database update --project ../Store.Persistence
dotnet run
```

Swagger UI is served at `/docs` in the Development environment. The default connection string uses SQL Server LocalDB; override `ConnectionStrings:DefaultConnection` to use another server.

## Main endpoints

| Method | Route | Auth |
| --- | --- | --- |
| POST | `/api/auth/register`, `/api/auth/login` | none |
| GET | `/api/products`, `/api/products/{id}`, `/api/categories` | none |
| POST / PUT / DELETE | `/api/products`, `/api/categories` | JWT |
| GET / PUT / DELETE | `/api/customers/me` | JWT |
| GET / POST | `/api/orders` | JWT |
| GET | `/api/orders/{id}` | JWT |
| PUT | `/api/orders/{id}/status` | JWT |
