# Tajan Echosystem

This repository contains a set of small .NET 9 microservices used for local development and integration testing of the Tajan echosystem (API Gateway, Identity, Product (API + gRPC), Order, Notification, etc.).

This README documents how to set up the project locally (Windows), how to run the full stack with Docker Compose, useful troubleshooting steps, and tips to speed up Docker builds.

## Contents

- Tajan.ApiGateway - Ocelot gateway
- Tajan.ProductService.API - Product HTTP API
- Tajan.ProductService.gRPCApi - Product gRPC service
- Tajan.OrderService.API - Order HTTP API
- Tajan.Identity.API - Identity service
- Tajan.Notification.API - Notification service
- docker-compose.yml - compose stack for local development (SQL Server, RabbitMQ, Redis + services)

## Prerequisites

- Windows (these instructions assume Windows + PowerShell)
- Docker Desktop (with Compose v2)
- .NET SDK 9.0 installed (optional if you only use Docker)
- A few useful tools: curl or PowerShell's Invoke-RestMethod, SQL Server client (SSMS) if you want to inspect DBs

## Quick start (full stack)

1. Open a PowerShell terminal in the repository root (where `docker-compose.yml` lives):

2. Start the full stack with Docker Compose (this will build service images the first time):

```powershell
# build images in parallel (faster on machines with multiple cores)
docker compose build --parallel

# then start services
docker compose up -d
```

3. Confirm services are running:

```powershell
docker compose ps
```

4. Call a service via the API gateway (example: add a product):

Notes about ports used by the compose file (defaults in this workspace):

- API Gateway: http://localhost:5003 -> forwards to services
- Product HTTP API: mapped host 5001 -> container 8080
- Order API: mapped host 5002 -> container 8080
- Notification API: mapped host 5000 -> container 80
- Identity API: mapped host 5004 -> container 80
- SQL Server: host 6566 -> container 1433 (use `Server=localhost,6566` in tools)
- RabbitMQ management UI: http://localhost:15672
- Redis: host 6379

## Useful compose commands

- Start the full stack (build if needed):

```powershell
docker compose up -d --build
```

- Stop the stack:

```powershell
docker compose down
```

- Rebuild a single service (faster during development):

```powershell
docker compose build product-api
docker compose up -d --no-deps product-api
```

- View logs for a service:

```powershell
docker compose logs --tail 200 api-gateway
docker compose logs --tail 200 product-api
```

## How the services initialize the database (local dev)

Many services call `context.Database.EnsureCreated()` on startup for local convenience. That is handy for early development but has limitations:

- `EnsureCreated()` may throw if the database already exists or there are concurrent creation attempts. In this repo we've made startup resilient in several services by catching that exception and continuing so containers don't exit unexpectedly.
- For production or when you need schema migrations, prefer EF Core Migrations and `Database.Migrate()`.

## Common troubleshooting

1) 502 / Bad Gateway from API Gateway

- Symptom: Calling the gateway (e.g. `http://localhost:5003/ProductService/...`) returns 502.
- Cause: Ocelot cannot connect to the downstream service (e.g. `product-api:8080`) — the downstream container may have crashed or not be listening on the expected port.
- Quick checks:

```powershell
docker compose ps
docker compose logs --tail 200 api-gateway
docker compose logs --tail 200 product-api
```

- If the downstream service is crashing on startup look at its logs and search for DB/Redis/RabbitMQ connection errors.

2) product-api container crashes on startup with SQL errors

- Symptom: `SqlException` during `EnsureCreated()` (e.g. "Database 'Tajan_Product' already exists")
- Fix: We added a try/catch around `EnsureCreated()` so the service should stay up. If you still see problems:

	- Confirm SQL Server container is healthy: `docker compose ps` and check logs.
	- Ensure the connection string in `docker-compose.yml` points to `sqlserver` (the compose service) and not `localhost`.
	- If you manually created DBs with a different owner/permissions, consider dropping and recreating or use migrations.

3) Redis timeouts in product services

- Symptom: StackExchange.Redis unable to connect to `localhost:6379` or timeouts.
- Fix: Make sure `redis` service is present in `docker-compose.yml` and the product services are configured to use host `redis` and port `6379`. The included compose file already sets these env vars for product services.

4) RabbitMQ / MassTransit connection retries

- Symptom: MassTransit logs show broker unreachable messages.
- Fix: Ensure `rabbitmq` service is Up (management UI on http://localhost:15672). Services depending on RabbitMQ may retry for a while before succeeding.

## Speeding up Docker builds

If Docker builds are slow for you, the usual causes are missing `.dockerignore` or Dockerfiles that `COPY . .` too early. Here are practical tips:

- Add a `.dockerignore` (exclude `bin/`, `obj/`, `.git`, `.vscode`) to avoid shipping large build artifacts into the context.
- Use the cache-friendly pattern in .NET Dockerfiles: copy only the `.csproj` files and run `dotnet restore` before copying full sources. This preserves the package-restore layer across source changes.
- Build only the services you changed during development: `docker compose build product-api` and `docker compose up -d --no-deps product-api`.
- Use `docker compose build --parallel` to leverage multiple cores.
- Advanced: use BuildKit + `--cache-to` / `--cache-from` to persist cache across machines or CI.

## Development notes and conventions

- gRPC endpoints: `Tajan.ProductService.gRPCApi` exposes product data for internal service-to-service calls. `PRODUCT_GRPC_URL` environment variable is used by Order service to call the product gRPC endpoint (set in `docker-compose.yml`).
- Configuration keys used in compose:
	- `ConnectionStrings__Tajan_Product_Db` / `ConnectionStrings__CoreDbContext` — SQL Server connection strings
	- `UseInMemoryDatabase` / `UseInMemoryDataBase` — flags used in various services to decide between InMemory/SQL
	- `Redis__MasterNode__Host` / `Redis__MasterNode__Port` — Redis config
	- `PRODUCT_GRPC_URL` — the gRPC address used by Order service

## Running unit/integration tests

There are test projects in `tests/`. Run them with the .NET CLI:

```powershell
dotnet test "C:\Parham\Projects\TajanEchosystem\tests\OrderService.Unit" -c Debug
dotnet test "C:\Parham\Projects\TajanEchosystem\tests\OrderService.Integration" -c Debug
```

Integration tests may depend on in-memory stubs (see test startup) or a running local compose stack depending on the test design.

## Health checks and debugging tips

- Add `docker compose logs --follow <service>` when debugging long-running startup issues.
- Use `docker exec -it <container> /bin/bash` (or powershell) to inspect container filesystem and run `dotnet` commands inside the image.
- For SQL Server, connect with SSMS to `localhost,6566` (username: `sa`, password: `YourStrong!Passw0rd`) to inspect databases and tables.

## Recommended next improvements

- Replace `EnsureCreated()` with EF Core Migrations (`Database.Migrate()`) and migrate the DB schema under CI control.
- Add a proper `.dockerignore` and refactor Dockerfiles to copy only `.csproj` then `dotnet restore` (improves cache hits).
- Add health checks for dependent services (SQL/Redis/RabbitMQ) and implement a startup retry/backoff so containers don't fail when a dependency is temporarily unavailable.

## Contributing

If you want to add features or fix issues:

1. Create a feature branch from `1-basket-crud-order-read-tests` or main branch used by the team.
2. Add tests where appropriate (unit + integration).
3. Run `dotnet build` and tests locally, then run `docker compose build <service>` if you changed Dockerfiles.

## Contact / Notes

If you want me to optimize the Dockerfiles and add `.dockerignore`, I can make those edits and rebuild the product images for you. I can also add a short troubleshooting checklist per-service if you'd like.

---

This README was generated/updated to help bootstrap local development and debugging. If anything in your environment differs (custom ports, different SA password, etc.) update `docker-compose.yml` accordingly and restart the stack.
