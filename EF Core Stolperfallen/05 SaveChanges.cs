// Aufgabe: Erhöhe alle Preise um 10%.

// PROBLEM
foreach (var product in products)
{
    product.Price *= 1.1m;
    context.SaveChanges();  // N Roundtrips!
}







// LÖSUNG
foreach (var product in products)
{
    product.Price *= 1.1m;
}
await context.SaveChangesAsync();  // 1 Roundtrip!
