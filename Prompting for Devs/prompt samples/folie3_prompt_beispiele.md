# Folie 3 – Praktische Prompt-Beispiele: Die drei Modi

*Die drei Copilot-Modi in VS Code – Voraussetzungen und Live-Prompts*

---

## Voraussetzungen (gelten für alle Beispiele)

- VS Code mit installierter **GitHub Copilot**-Extension (Version mit Chat-Support)
- Aktive Copilot-Lizenz (Individual, Business oder Enterprise)
- Ein geöffnetes **.NET-Projekt** im Workspace (mindestens eine `.cs`-Datei)
- Den **Copilot Chat** geöffnet (`Ctrl+Alt+I`)
- Den jeweiligen **Modus** oben im Chat-Fenster ausgewählt (Dropdown neben dem Eingabefeld)

---

## Modus: Ask

**Voraussetzung:** Eine C#-Datei ist geöffnet, z. B. eine Klasse `OrderService.cs`.

```
Erkläre mir, was diese Klasse macht und welche potenziellen Probleme du siehst.
```

Copilot analysiert die aktive Datei und gibt eine strukturierte Erklärung zurück – ohne etwas zu verändern.

**Zweites Beispiel** – ohne geöffnete Datei, rein konzeptionell:

```
Was ist der Unterschied zwischen Scoped, Transient und Singleton
in der .NET Dependency Injection?
```

---

## Modus: Plan

**Voraussetzung:** Eine User Story oder Aufgabe ist klar definiert. Keine spezielle Datei nötig, aber hilfreich ist eine geöffnete Schnittstellendatei oder ein leeres Projektgerüst.

```
Ich möchte einen CustomerService implementieren, der:
- Customers validiert
- Sie in einer SQL-Datenbank speichert (EF Core)
- Bei Fehler eine CustomerException wirft
- Vollständig async ist

Erstelle zunächst nur einen Plan. Noch keinen Code.
```

Copilot listet die geplanten Schritte auf – zum Beispiel:

1. Interface `ICustomerService` mit Methode `AddCustomerAsync` definieren
2. Klasse `CustomerService` anlegen und Interface implementieren
3. Validierungslogik in `AddCustomerAsync` einbauen
4. EF Core `DbContext` einbinden und `SaveChangesAsync` aufrufen
5. `CustomerException` bei Validierungsfehlern werfen
6. Services in `Program.cs` registrieren

Du prüfst, ob der Plan stimmt – und nimmst ihn direkt mit in den Agent-Modus.

---

## Modus: Agent

**Voraussetzung:** Ein vollständiges .NET-Projektgerüst ist vorhanden (`dotnet new webapi` reicht). Das Terminal muss von VS Code aus erreichbar sein.

> **Wichtig:** Der Plan aus dem vorherigen Schritt ist bestätigt. Jetzt wird er ausgeführt.

```
Setze den folgenden Plan um:

1. Interface ICustomerService mit Methode AddCustomerAsync definieren
2. Klasse CustomerService anlegen und Interface implementieren
3. Validierungslogik in AddCustomerAsync einbauen
4. EF Core DbContext einbinden und SaveChangesAsync aufrufen
5. CustomerException bei Validierungsfehlern werfen
6. Services in Program.cs registrieren

Lege alle nötigen Dateien an und stelle sicher, dass das Projekt danach kompiliert.
```

Copilot legt Dateien an, bearbeitet `Program.cs` und führt `dotnet build` aus, um zu prüfen, ob alles kompiliert.

---

## Hinweis für die Live-Demo

Der **Plan → Agent**-Übergang ist der stärkste Moment für das Publikum. Zeige erst den Plan-Prompt, lass das Publikum den Plan lesen und nicken – dann kopiere den Plan direkt in den Agent-Modus und führe ihn aus. Der Kontrast zwischen „Copilot plant" und „Copilot handelt" ist sofort verständlich. Besonders wirkungsvoll: Der Agent-Prompt ist kein neuer Gedanke – er ist exakt das, was Copilot selbst im Plan-Schritt vorgeschlagen hat.
