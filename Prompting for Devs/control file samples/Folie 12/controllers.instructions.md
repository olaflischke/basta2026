---
applyTo: "src/Api/Controllers/**/*.cs"
---

# Dateispezifische Anweisungen: Controller-Klassen

Diese Regeln gelten nur für Dateien unter `src/Api/Controllers/`.

## Struktur

- Jeder Controller erbt von `ControllerBase`
- Kein Geschäftslogik-Code direkt im Controller
- Aufrufe gehen immer über einen Service oder MediatR-Handler

## Beispiel

```csharp
/// <summary>
/// Verarbeitet HTTP-Anfragen für Bestellungen.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Gibt eine Bestellung anhand der ID zurück.
    /// </summary>
    /// <param name="id">Die ID der Bestellung.</param>
    /// <param name="ct">Abbruch-Token.</param>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken ct)
    {
        var order = await _orderService.GetByIdAsync(id, ct);
        return order is null ? NotFound() : Ok(order);
    }
}
```

## Fehlerbehandlung

- Rückgabe über `IActionResult` oder `Results<T1, T2>`
- Fehler als ProblemDetails (RFC 9457) zurückgeben
- Kein try/catch im Controller – Middleware übernimmt das

## Verboten

- Kein direkter Datenbankzugriff im Controller
- Keine `HttpContext`-Manipulation außerhalb von Middleware
