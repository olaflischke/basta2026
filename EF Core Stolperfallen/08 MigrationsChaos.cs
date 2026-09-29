/*
# PROBLEM - Parallel Development
Dev A: dotnet ef migrations add AddOrderNotes
Dev B: dotnet ef migrations add AddCustomerPhone


# Merge-Konflikt! 
*/

// DEV A:
// Add to Order.cs
public string? InternalNotes { get; set; }  


// DEV B:
// Add to Customer.cs  
public string? Phone { get; set; }








/*
# LÖSUNG - Team-Konventionen
git checkout main
dotnet ef migrations remove --force  # Beide entfernen
dotnet ef migrations add AllChanges  # Gemeinsam erstellen
dotnet ef database update
*/

/*
Standard-Prozess bei Migrationen im Team:
# 1. Konflikt erkannt → ABBRUCH
git checkout --ours/abort

# 2. Migrationen entfernen
dotnet ef migrations remove --force  # Dev A
dotnet ef migrations remove --force  # Dev B  

# 3. Merge Model-Änderungen
git merge dev-branch  # Nur Entities mergen!

# 4. NEUE kombinierte Migration
dotnet ef migrations add AddCustomerPhoneAndOrderNotes

# 5. Commit & Push
*/
