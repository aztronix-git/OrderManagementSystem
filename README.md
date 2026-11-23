# Order Management + Inventory Management API (.NET 10)

Starter solution built with Clean Architecture, EF Core and SQL Server LocalDB.

## Projects
- `OrderManagement.Core`: domain entities
- `OrderManagement.Application`: DTOs and contracts
- `OrderManagement.Infrastructure`: EF Core, repositories and business service implementations
- `OrderManagement.API`: REST controllers and dependency injection

## Open in Visual Studio 2026 Community
1. Install the **ASP.NET and web development** workload and SQL Server Express LocalDB (or use an existing SQL Server instance).
2. Open `OrderManagementSystem.slnx` (or `OrderManagement.API/OrderManagement.API.csproj`) in Visual Studio.
3. Set `OrderManagement.API` as the startup project.
4. Open **Tools > NuGet Package Manager > Package Manager Console**.
5. Set Default project to `OrderManagement.Infrastructure`, then run: `Add-Migration InitialCreate -StartupProject OrderManagement.API`.
6. Run `Update-Database -StartupProject OrderManagement.API`.
7. Press F5. Swagger opens in the browser.

Alternatively, from a terminal in the solution folder:
```powershell
dotnet restore
dotnet tool install --global dotnet-ef --version 10.0.0
dotnet ef migrations add InitialCreate --project OrderManagement.Infrastructure --startup-project OrderManagement.API
dotnet ef database update --project OrderManagement.Infrastructure --startup-project OrderManagement.API
dotnet run --project OrderManagement.API
```

## API endpoints
- `GET/POST/PUT/DELETE /api/customers`
- `GET/POST/PUT/DELETE /api/products`
- `GET/POST /api/orders`, `GET /api/orders/{id}`, `POST /api/orders/{id}/cancel`
- `GET/POST/PUT/DELETE /api/suppliers`
- `GET /api/inventory`, `GET /api/inventory/low-stock`
- `POST /api/inventory/receive`, `POST /api/inventory/issue`, `GET /api/inventory/movements`
- `GET/POST /api/payments`

## Example request bodies
Create customer: `{ "name":"Asha", "email":"asha@example.com", "phone":"9876543210" }`

Create product: `{ "name":"Keyboard", "sku":"KEY-001", "price":1200, "reorderLevel":5 }`

Receive stock: `{ "productId":1, "quantity":25, "reference":"PO-1001" }`

Create order: `{ "customerId":1, "items":[{ "productId":1, "quantity":2 }] }`

Create payment: `{ "orderId":1, "amount":2400, "method":"UPI" }`

## Important notes
- Create a customer and product first, then receive stock, then create an order.
- Order creation deducts available stock in a database transaction. Order cancellation restores stock and records an IN movement.
- This is a learning starter, not production-ready: add authentication/authorization, DTO validation, pagination, structured exception handling, concurrency tokens/row-version handling, idempotency, audit logging, and payment-gateway integration before production. Payment endpoint here records a simulated payment; it does not charge a real card or UPI account.
- For production-grade stock concurrency, use SQL transactions with suitable isolation/row-version checks and consider a reservation/fulfilment workflow.
