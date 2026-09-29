//  PROBLEM - Single Query mit mehreren Collections
var customers = context.Customers
    .Include(c => c.Orders)              // Collection 1
    .ThenInclude(o => o.OrderDetails)    // Collection 2 (pro Order!)
    .ToList();

// SQL: Riesiger JOIN mit Duplikaten!
// 1 Customer + 5 Orders * 3 OrderItems = 15 Zeilen pro Kunde,
// alle Kundendaten 15mal!








//  LÖSUNG
var customers = context.Customers
    .Include(c => c.Orders)              // ← Collection 1
    .ThenInclude(o => o.OrderDetails)    // ← Collection 2  
    .AsSplitQuery()                      // Separate Queries!
    .ToList();

/*
Single Query:
SELECT c.*, o.*, od.*, p.* FROM Customers c
LEFT JOIN Orders o → LEFT JOIN OrderDetails od → ...

Split Query (3 Queries):
1. SELECT * FROM Customers
2. SELECT * FROM Orders WHERE CustomerID IN (...)
3. SELECT * FROM OrderDetails od 
   JOIN Orders o ON od.OrderID=o.OrderID 
   WHERE o.CustomerID IN (...)
*/