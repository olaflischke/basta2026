# AGENTS.md – Gemeinsame Wissensbasis für alle KI-Agenten

Dieser offene Standard wird von mehreren KI-Agenten verstanden:
GitHub Copilot, Claude Code, Codex und anderen.

## Projekt: WebShop API

### Architektur

- .NET 9 Minimal API mit Clean Architecture
- `src/Api/` – Endpoints und Middleware
- `src/Domain/` – Entities und Value Objects
- `src/Infrastructure/` – EF Core, externe Services

### Technologie-Stack

- Sprache: C# 12
- Framework: .NET 9
- ORM: Entity Framework Core 9
- Tests: xUnit + NSubstitute

### Coding-Konventionen

- Records statt classes für DTOs
- Keine statischen Klassen oder Methoden
- Alle public APIs mit XML-Dokumentationskommentaren
- Guard Clauses statt verschachtelter if-Blöcke
- Async/await durchgehend – keine blockierenden Aufrufe

### Teststrategie

- Jeder Endpoint braucht mindestens einen Integrationstest
- Unit Tests für Domain-Logik und Services
- Tests ausführen:
  ```
  dotnet test tests/Unit/
  dotnet test tests/Integration/
  ```

### Fehlerbehandlung

- Fehlerbehandlung über ProblemDetails (RFC 9457)
- Kein Stack-Trace nach außen
- Logging bei Fehlern mit strukturiertem Log (Serilog)

### Hinweis für Agenten

Diese Datei gilt als gemeinsame Wissensbasis.
Für Copilot-spezifische Regeln zusätzlich `.github/copilot-instructions.md` beachten.
