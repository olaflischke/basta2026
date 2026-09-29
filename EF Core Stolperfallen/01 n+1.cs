// PROBLEM - N+1 in Schleife (ohne Proxies!)
var orders = context.Orders.ToList();  // 1 Query

foreach (var order in orders)
{
    var customerName = context.Customers  // N Queries!
        .Where(c => c.CustomerID == order.CustomerID)
        .Select(c => c.CompanyName)
        .FirstOrDefault();
    Console.WriteLine($"Order {order.OrderID}: {customerName}");
}







// LÖSUNG 1: 1 Query!
var orders = context.Orders
    .Include(o => o.Customer)     // JOIN Customer
    .ToList();

/* 
SELECT [o].[OrderID], [o].[CustomerID], [o].[EmployeeID], 
       [o].[Freight], [c].[CustomerID], [c].[CompanyName], 
       [c].[ContactName]
FROM [Orders] AS [o]
LEFT OUTER JOIN [Customers] AS [c] ON [o].[CustomerID] = [c].[CustomerID]
*/

foreach (var order in orders)
{
    Console.WriteLine($"Order {order.OrderID}: {order.Customer.CompanyName}");
}

// LÖSUNG 2: PROJEKTION mit .Select() (BESTE Performance!)
var orderDtos = context.Orders
    .Select(o => new    // 1 Query, lädt nur benötigte Daten!
    {           
        OrderID = o.OrderID,
        CustomerName = o.Customer.CompanyName,
        TotalItems = o.OrderDetails.Count(),
        Freight = o.Freight
    })
    .ToList();

/*
SELECT o.OrderID, c.CompanyName, COUNT(od.OrderID), o.Freight
FROM Orders o
LEFT JOIN Customers c ON o.CustomerID = c.CustomerID
LEFT JOIN OrderDetails od ON o.OrderID = od.OrderID
GROUP BY o.OrderID, c.CompanyName, o.Freight
*/

foreach (var dto in orderDtos)
{
    Console.WriteLine($"{dto.CustomerName}: Order {dto.OrderID} ({dto.TotalItems} Items)");
}