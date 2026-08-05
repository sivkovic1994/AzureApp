# Order Management System

A .NET 8 order management API, built as a hands-on project to learn and demonstrate Azure cloud development. It covers Clean Architecture and domain-driven design, with the goal of deploying a non-trivial, production-shaped system to Azure.

This project is a work in progress — see [Roadmap](#roadmap) below for current status.

## Why this project exists

This is a deliberate effort to build real, hands-on Azure experience on a non-trivial system rather than isolated tutorials — covering deployment, configuration, secrets management, monitoring, and CI/CD on Azure, on top of solid application architecture.

## Architecture

The solution follows Clean Architecture, with dependencies pointing inward:

```
OrderManagement.Api            -> HTTP layer: controllers, DI wiring, Swagger
OrderManagement.Infrastructure -> EF Core, repositories, Azure service integrations
OrderManagement.Application    -> Application services, DTOs, validation
OrderManagement.Domain         -> Entities, value objects, domain events (no external dependencies)
```

- **Domain** — `Order`, `Product`, `Customer` as rich entities with encapsulated business rules (e.g. an order can't be confirmed if empty, stock can't go negative). `Order` is the aggregate root; all mutations go through it.
- **Application** — one service per feature (`OrderService`, `ProductService`, `CustomerService`) with [FluentValidation](https://docs.fluentvalidation.net/) validating requests before any domain logic runs. Domain events (e.g. `OrderConfirmedEvent`) are raised by entities and picked up by the service right after the operation that triggered them.
- **Infrastructure** — EF Core persistence and Azure service integrations (planned: Azure SQL Database, Blob Storage, Key Vault).
- **Api** — thin controllers that translate HTTP requests into Application service calls.

## Tech stack

- .NET 8 / ASP.NET Core Web API
- FluentValidation
- Entity Framework Core
- Azure App Service, Azure SQL Database, Key Vault, Application Insights, Blob Storage (deployment target)
- GitHub Actions (CI/CD, planned)

## Roadmap

- [x] Domain layer — entities, value objects, domain events
- [x] Application layer — services, validation, domain event handling
- [ ] Infrastructure layer — EF Core DbContext, repositories, Azure Blob Storage
- [ ] Api layer — controllers, Swagger, DI wiring
- [ ] Local run + verification
- [ ] Azure deployment — App Service, Azure SQL, Key Vault, Application Insights
- [ ] CI/CD pipeline via GitHub Actions

## Running locally

Once the Api layer is complete:

```bash
dotnet restore
dotnet build
dotnet run --project src/OrderManagement.Api
```

Details on required configuration (connection strings, etc.) will be added here once the Infrastructure layer lands.

## License

Personal learning project — no license restrictions on viewing or referencing the code.
