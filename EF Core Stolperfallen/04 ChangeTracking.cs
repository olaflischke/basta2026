// Aufgabe: Produkte für eine Übersicht laden.

//  PROBLEM
var products = context.Products.ToList(); 






//  LÖSUNG
var products = context.Products
    .AsNoTracking()  // Tracking = false
    .ToList();
