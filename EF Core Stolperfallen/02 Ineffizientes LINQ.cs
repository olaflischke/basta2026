// Aufgabe: Liste aller Orders von Kunden aus Deutschland.

// PROBLEM: Frühes ToList() und Contains()
var germanCustomers = context.Customers
    .Where(c => c.Country == "Germany")
    .Select(c => c.CustomerID)
    .ToList();  // -> alle dt. IDs im Memory, auch die von Kunden ohne Order!

var orders = context.Orders
    // Client-seitig: Contains ist C#-Methode, kein SQL-Operator!
    .Where(o => germanCustomers.Contains(o.CustomerID))
    .ToList();  // Lädt ALLE Orders aus der ganzen Welt!













// LÖSUNG
var orders = context.Orders
    .Where(o => o.Customer.Country == "Germany")  // 1 Query!
    .Include(o => o.Customer)
    .ToList();
