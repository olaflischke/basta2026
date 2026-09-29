# copilot-instructions.md – Copilot-spezifische Projektregeln

Diese Datei gilt für alle Copilot-Interaktionen im Repository:
Chat, Agent Mode und Coding Agent.

## Projekt: WebShop API

### Allgemeine Regeln

- Schreibe C# 12
- Nutze records statt classes für DTOs
- Keine statischen Klassen oder Methoden
- Alle public APIs mit XML-Dokumentationskommentaren
- Async/await durchgehend

### Architektur

- .NET 9 Minimal API mit Clean Architecture
- Ordnerstruktur:
  - `src/Api/` – Endpoints und Middleware
  - `src/Domain/` – Entities und Value Objects
  - `src/Infrastructure/` – EF Core, externe Services

### Namenskonventionen

- Interfaces beginnen mit `I` (z. B. `IOrderRepository`)
- Async-Methoden enden auf `Async` (z. B. `GetByIdAsync`)
- Keine Abkürzungen in Bezeichnern (z. B. `customerId` statt `custId`)

### Verbotene Libraries

- Keine `Newtonsoft.Json` – stattdessen `System.Text.Json`
- Kein `AutoMapper` – manuelle Mappings oder Records

### Fehlerbehandlung

- Fehlerbehandlung über ProblemDetails (RFC 9457)
- Kein Stack-Trace nach außen geben
- Custom Exceptions enden auf `Exception` (z. B. `OrderNotFoundException`)

### Tests

- Jeder Endpoint braucht einen Integrationstest
- Unit Tests mit xUnit und NSubstitute
- AAA-Pattern: Arrange, Act, Assert

### Hinweis

Diese Datei gilt nur für Copilot.
Für alle KI-Agenten im Projekt gilt zusätzlich `AGENTS.md` im Root-Verzeichnis.
