// PROBLEM
foreach(var p in context.Products) {
    p.Price *= 1.1m;
}
context.SaveChanges();








// LÖSUNG
await context.Products
    .Where(p => p.Category == "Electronics")
    .ExecuteUpdateAsync(p => p.SetProperty(x => x.Price, x => x.Price * 1.1m));

// Auch: .ExecuteDeleteAsync() für Bulk Delete
// Seit EF Core 7.0: Bulk-Operationen direkt in der Datenbank, 
// ohne vorherige Abfrage der Entities!