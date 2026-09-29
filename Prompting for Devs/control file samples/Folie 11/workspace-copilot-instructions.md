# copilot-instructions.md – Workspace-Ebene

Diese Datei liegt unter `.github\copilot-instructions.md`.
Sie gilt für das **gesamte Repository** und enthält Projektregeln,
die für alle Entwickler im Team verbindlich sind.

## Projekt: WebShop API

### Architektur

- .NET 9 Minimal API mit Clean Architecture
- Ordnerstruktur:
  - `src/Api/` – Endpoints und Middleware
  - `src/Domain/` – Entities und Value Objects
  - `src/Infrastructure/` – EF Core, externe Services
  - `tests/Unit/` – Unit Tests
  - `tests/Integration/` – Integrationstests

### Namenskonventionen

- Interfaces beginnen mit `I` (z. B. `IOrderRepository`)
- Async-Methoden enden auf `Async` (z. B. `GetByIdAsync`)
- Keine Abkürzungen in Bezeichnern (`customerId` statt `custId`)
- DTOs als Records, Suffix `Dto` (z. B. `OrderDto`)

### Verbotene Libraries

- Kein `Newtonsoft.Json` – stattdessen `System.Text.Json`
- Kein `AutoMapper` – manuelle Mappings oder Records
- Kein `Dapper` – ausschließlich EF Core

### Coding-Regeln

- C# 12
- Records statt classes für DTOs
- Keine statischen Klassen oder Methoden
- Alle public APIs mit XML-Dokumentationskommentaren
- Async/await durchgehend
- Fehlerbehandlung über ProblemDetails (RFC 9457)

### Beispiel: Minimal-Endpoint

```csharp
/// <summary>
/// Gibt eine Bestellung anhand der ID zurück.
/// </summary>
app.MapGet("/api/orders/{id:int}", async (
    int id,
    IOrderService orderService,
    CancellationToken ct) =>
{
    var order = await orderService.GetByIdAsync(id, ct);
    return order is null
        ? Results.NotFound()
        : Results.Ok(order);
})
.WithName("GetOrderById")
.WithOpenApi();
```

### Tests

- Jeder Endpoint braucht einen Integrationstest
- Unit Tests mit xUnit und NSubstitute
- AAA-Pattern: Arrange, Act, Assert
- Tests ausführen:
  ```
  dotnet test tests/Unit/
  dotnet test tests/Integration/
  ```

### Hinweis

Diese Datei gilt für alle Copilot-Interaktionen im Repo.
Für alle KI-Agenten im Projekt gilt zusätzlich `AGENTS.md` im Root-Verzeichnis.
