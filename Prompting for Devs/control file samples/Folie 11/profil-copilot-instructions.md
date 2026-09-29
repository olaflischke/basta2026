# copilot-instructions.md – Profil-Ebene

Diese Datei liegt unter `C:\Users\<Name>\copilot-instructions.md`.
Sie gilt für **alle Projekte** auf diesem Rechner und enthält
persönliche Präferenzen des Entwicklers.

## Sprache und Stil

- Antworten im Copilot Chat immer auf Deutsch
- Kommentare im Code auf Deutsch
- XML-Dokumentationskommentare auf Deutsch

## Bevorzugte Patterns

- Clean Architecture als Standard-Strukturierung
- Repository Pattern für Datenzugriff
- Guard Clauses statt verschachtelter if-Blöcke
- Keine Magic Numbers – immer benannte Konstanten

## C#-Präferenzen

- C# 12 als Sprachversion
- Records statt classes für DTOs
- Keine statischen Klassen oder Methoden
- Async/await durchgehend – keine blockierenden Aufrufe (`Task.Result`, `.Wait()`)
- Nullable Reference Types aktiviert – kein `!`-Operator ohne Begründung

## Beispiel: Bevorzugter Methodenkopf

```csharp
/// <summary>
/// Gibt eine Bestellung anhand der ID zurück.
/// </summary>
/// <param name="id">Die ID der Bestellung.</param>
/// <param name="ct">Abbruch-Token für asynchrone Operationen.</param>
/// <returns>Die Bestellung oder null, wenn nicht gefunden.</returns>
public async Task<Order?> GetByIdAsync(int id, CancellationToken ct)
{
    ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
    return await _repository.GetByIdAsync(id, ct);
}
```

## Verboten

- Kein `var` bei nicht offensichtlichem Typ
- Keine `Newtonsoft.Json` – stattdessen `System.Text.Json`
- Kein `AutoMapper` – manuelle Mappings oder Records

## Hinweis

Diese Datei gilt persönlich und projektübergreifend.
Projektspezifische Regeln stehen in `.github\copilot-instructions.md` im jeweiligen Repository.
