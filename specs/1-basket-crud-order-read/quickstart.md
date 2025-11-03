# Quickstart: Basket CRUD + Order Read Query

**Purpose**: Run the basket endpoints and contract tests locally.

Prerequisites

- .NET SDK (matching repository requirements)
- Docker (if you prefer containerized dependencies: DB, product/identity stubs)

Run locally (development)

1. Start local dependencies (database, identity, product catalog). Use Docker compose if available:

```powershell
# Example (if there is a docker-compose for local infra):
docker-compose up -d
```

dotnet build
dotnet run --configuration Development
2. Build and run the Order service (basket functionality is implemented inside `Tajan.OrderService`):

```powershell
# Example path - adjust to actual project in this repository
cd src/Tajan.OrderService.API
dotnet build -f net9.0
dotnet run --configuration Development --framework net9.0
```

3. Run contract tests (Product/Identity mocks):

```powershell
cd tests/contract
dotnet test
```

Notes

- If the repository uses a single service project for baskets, adjust project path accordingly.
- For CI, ensure contract tests run against lightweight stubs for Product and Identity services.
