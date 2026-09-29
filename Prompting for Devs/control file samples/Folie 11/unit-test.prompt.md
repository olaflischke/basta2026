# unit-test.prompt.md – Wiederverwendbarer Prompt: Unit Test generieren

Diese Datei liegt unter `.github\prompts\unit-test.prompt.md`.
Sie ist ein wiederverwendbarer Prompt für die Generierung von Unit Tests.

---

Du bist ein erfahrener .NET-Entwickler mit Fokus auf Testqualität.

## Aufgabe

Generiere Unit Tests für die folgende C#-Klasse oder Methode.

## Rahmenbedingungen

- Framework: xUnit
- Mocking: NSubstitute
- Muster: AAA (Arrange, Act, Assert)
- Sprache: C# 12
- Alle Abhängigkeiten werden gemockt – kein echter Datenbankzugriff

## Testfälle, die immer enthalten sein müssen

1. Happy Path – der Normalfall funktioniert korrekt
2. Nicht gefunden – Rückgabe von `null` oder leerem Ergebnis
3. Ungültige Eingabe – `null`, leer oder Grenzwert
4. Fehlerfall – Exception wird korrekt ausgelöst

## Namenskonvention für Testmethoden

```
MethodenName_Szenario_ErwartetesErgebnis
```

Beispiele:
- `GetByIdAsync_ExistingOrder_ReturnsOrder`
- `GetByIdAsync_OrderNotFound_ReturnsNull`
- `ProcessOrder_NullOrder_ThrowsArgumentNullException`

## Beispiel-Output

```csharp
public class OrderServiceTests
{
    private readonly IOrderRepository _repository;
    private readonly OrderService _sut;

    public OrderServiceTests()
    {
        _repository = Substitute.For<IOrderRepository>();
        _sut = new OrderService(_repository);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingOrder_ReturnsOrder()
    {
        // Arrange
        var expected = new Order { Id = 1, CustomerName = "Max Mustermann" };
        _repository.GetByIdAsync(1, Arg.Any<CancellationToken>())
                   .Returns(expected);

        // Act
        var result = await _sut.GetByIdAsync(1, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_OrderNotFound_ReturnsNull()
    {
        // Arrange
        _repository.GetByIdAsync(99, Arg.Any<CancellationToken>())
                   .Returns((Order?)null);

        // Act
        var result = await _sut.GetByIdAsync(99, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ProcessOrder_NullOrder_ThrowsArgumentNullException()
    {
        // Arrange & Act
        var act = async () => await _sut.ProcessOrder(null!, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(act);
    }
}
```

## Hinweis

Dieser Prompt wird über `.github\prompts\unit-test.prompt.md` in Copilot Chat geladen.
Er kann für jede Service- oder Repository-Klasse im Projekt wiederverwendet werden.
