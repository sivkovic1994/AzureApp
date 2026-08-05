# Order Management System

A .NET 8 order management API, built as a hands-on project to learn and demonstrate Azure cloud development. It covers Clean Architecture, CQRS with MediatR, and domain-driven design, with the goal of deploying a non-trivial, production-shaped system to Azure.

This project is a work in progress — see [Roadmap](#roadmap) below for current status.

## Why this project exists

This is a deliberate effort to build real, hands-on Azure experience on a non-trivial system rather than isolated tutorials — covering deployment, configuration, secrets management, monitoring, and CI/CD on Azure, on top of solid application architecture.

## Architecture

The solution follows Clean Architecture, with dependencies pointing inward:

```
OrderManagement.Api            -> HTTP layer: controllers, DI wiring, Swagger
OrderManagement.Infrastructure -> EF Core, repositories, Azure service integrations
OrderManagement.Application    -> CQRS commands/queries (MediatR), DTOs, validation
OrderManagement.Domain         -> Entities, value objects, domain events (no external dependencies)
```

- **Domain** — `Order`, `Product`, `Customer` as rich entities with encapsulated business rules (e.g. an order can't be confirmed if empty, stock can't go negative). `Order` is the aggregate root; all mutations go through it.
- **Application** — CQRS with [MediatR](https://github.com/jbogard/MediatR): one command/query per use case, each with its own handler and [FluentValidation](https://docs.fluentvalidation.net/) validator wired in through a MediatR pipeline behavior. Domain events (e.g. `OrderConfirmedEvent`) are dispatched as MediatR notifications and handled independently of the command that raised them.
- **Infrastructure** — EF Core persistence and Azure service integrations (planned: Azure SQL Database, Blob Storage, Key Vault).
- **Api** — thin controllers that only translate HTTP requests into MediatR commands/queries.

## Tech stack

- .NET 8 / ASP.NET Core Web API
- MediatR (CQRS)
- FluentValidation
- Entity Framework Core
- Azure App Service, Azure SQL Database, Key Vault, Application Insights, Blob Storage (deployment target)
- GitHub Actions (CI/CD, planned)

## Roadmap

- [x] Domain layer — entities, value objects, domain events
- [x] Application layer — CQRS commands/queries, validation, event handlers
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
