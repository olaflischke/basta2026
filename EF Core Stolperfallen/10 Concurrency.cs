// PROBLEM - Kein Concurrency-Schutz
public class Order 
{
    public int OrderID { get; set; }
    public decimal Freight { get; set; }
    // Last Save wins → User A & B überschreiben sich!
}

// LÖSUNG - RowVersion Token
public class Order 
{
    public int OrderID { get; set; }
    public decimal Freight { get; set; }
    [Timestamp]  // Automatische Konfiguration für SQL-Server!
    public byte[] RowVersion { get; set; } = null!;
}

/*
// Exception-Handling:
try {
    context.SaveChanges();
} catch (DbUpdateConcurrencyException) {
    // Konflikt! Reload + Merge
    var entry = ex.Entries.Single();
    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
}
*/

// Postgres:
public class Order 
{
    public int OrderID { get; set; }
    public decimal Freight { get; set; }
    
    // xmin (automatisch von PG aktualisiert)
    public uint Xmin { get; set; }  // Mappt zu PostgreSQL xmin!
}

// OnModelCreating()
modelBuilder.Entity<Order>()
    .Property(o => o.Xmin)
    .IsConcurrencyToken()
    .HasColumnName("xmin");  // System-Column!

// MySql:
public class Order 
{
    public int OrderID { get; set; }
    public decimal Freight { get; set; }
    
    // BIGINT AUTO_INCREMENT oder Timestamp
    public long RowVersion { get; set; }  // App-managed oder DB-Timestamp
}

// OnModelCreating()
modelBuilder.Entity<Order>()
    .Property(o => o.RowVersion)
    .IsConcurrencyToken()
    .ValueGeneratedOnAddOrUpdate();  // MySQL TIMESTAMP

// Oracle:
public class Order 
{
    public int OrderID { get; set; }
    public decimal Freight { get; set; }
    
    // RAW(8) oder VARCHAR2 Timestamp
    public byte[] RowVersion { get; set; } = null!;
}

// OnModelCreating()
modelBuilder.Entity<Order>()
    .Property(o => o.RowVersion)
    .IsConcurrencyToken()
    .HasColumnType("RAW(16)");  // Oracle-spezifisch
    

// Universeller Ansatz: Manuelles Token
public class Order 
{
    public int OrderID { get; set; }
    public decimal Freight { get; set; }
    public Guid ConcurrencyToken { get; set; } = Guid.NewGuid();
}

// Vor SaveChanges()
foreach(var entry in context.ChangeTracker.Entries()) {
    if(entry.Entity is Order order)
        order.ConcurrencyToken = Guid.NewGuid();  // Neu generieren!
}
