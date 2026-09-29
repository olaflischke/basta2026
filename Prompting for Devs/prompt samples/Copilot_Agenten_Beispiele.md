# Copilot Agenten in Visual Studio 2026 – Beispiele

## Agent: Test

**Szenario:** Du hast eine neue Methode `CalculateDiscount` in einer C#-Klasse geschrieben und willst schnell Unit Tests dafür erstellen.

**Vorgehen:**

1. Rechtsklick auf die Methode im Editor
2. Copilot-Menü > **"Generate Tests with Test Agent"** auswählen
3. Der Test-Agent analysiert die Methode und schlägt automatisch Tests vor:

```csharp
// Methode (Produktionscode)
public decimal CalculateDiscount(decimal price, int customerLevel)
{
    if (customerLevel >= 5) return price * 0.80m;
    if (customerLevel >= 2) return price * 0.90m;
    return price;
}
```

```csharp
// Vom Test-Agenten generierte Unit Tests (xUnit)
public class DiscountServiceTests
{
    [Fact]
    public void CalculateDiscount_PremiumCustomer_Returns20PercentDiscount()
    {
        var service = new DiscountService();
        var result = service.CalculateDiscount(100m, 5);
        Assert.Equal(80m, result);
    }

    [Fact]
    public void CalculateDiscount_StandardCustomer_Returns10PercentDiscount()
    {
        var service = new DiscountService();
        var result = service.CalculateDiscount(100m, 2);
        Assert.Equal(90m, result);
    }

    [Fact]
    public void CalculateDiscount_NewCustomer_ReturnsFullPrice()
    {
        var service = new DiscountService();
        var result = service.CalculateDiscount(100m, 0);
        Assert.Equal(100m, result);
    }
}
```

4. Der Agent führt die Tests direkt aus und zeigt das Ergebnis im Copilot-Chat-Fenster an.
5. Fehlgeschlagene Tests werden erklärt und Korrekturvorschläge gemacht.

**Kernaussage:** Der Test-Agent spart das manuelle Schreiben von Testgerüsten und deckt automatisch typische Edge Cases ab.

---

## Agent: Profiler

**Szenario:** Eine API-Methode `GetOrdersByCustomer` ist in der Produktion langsam. Du willst wissen, wo der Engpass liegt.

**Vorgehen:**

1. Copilot Chat öffnen, Profiler-Agent aktivieren: `@profiler`
2. Prompt eingeben: *"Analysiere die Performance von GetOrdersByCustomer"*
3. Der Profiler-Agent startet eine Profiling-Session und wertet die Ergebnisse aus:

```csharp
// Problematischer Code – wird vom Agenten analysiert
public List<Order> GetOrdersByCustomer(int customerId)
{
    var orders = _dbContext.Orders.ToList(); // Lädt ALLE Bestellungen in den Speicher
    return orders.Where(o => o.CustomerId == customerId).ToList();
}
```

4. Der Agent meldet im Chat:

> **Engpass gefunden:** `ToList()` wird vor dem Filter aufgerufen. Es werden alle Datensätze aus der Datenbank geladen (aktuell: 48.000 Einträge). Die WHERE-Bedingung wird im Speicher ausgeführt, nicht in der Datenbank.
>
> **Empfehlung:** Filter direkt in die LINQ-Abfrage verlagern, damit SQL die Filterung übernimmt.

5. Optimierungsvorschlag vom Agenten:

```csharp
// Optimierter Code – vom Profiler-Agenten vorgeschlagen
public List<Order> GetOrdersByCustomer(int customerId)
{
    return _dbContext.Orders
        .Where(o => o.CustomerId == customerId) // Filter läuft in SQL
        .ToList();
}
```

6. Der Agent zeigt den Vorher-/Nachher-Vergleich der Laufzeit direkt im Chat an.

**Kernaussage:** Der Profiler-Agent verbindet Laufzeitanalyse und Optimierungsvorschlag in einem Schritt — ohne manuelles Interpretieren von Profiling-Daten.

---

*Diese Beispiele lassen sich direkt als Folgefolie oder Demo-Abschnitt im Seminar einsetzen.*
