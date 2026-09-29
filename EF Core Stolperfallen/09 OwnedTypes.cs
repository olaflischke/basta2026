public class Customer
{

    public string CustomerID { get; set; } = "";
    public string CompanyName { get; set; } = "";

    // STOLPERFALLE: Address als Property
    public Address ShippingAddress { get; set; } = null!;
}

public class Address
{
    public string Street { get; set; } = "";
    public string City { get; set; } = "";
    public string PostalCode { get; set; } = "";
    public string Country { get; set; } = "";
}

public class NorthwindContext : DbContext
{
    public DbSet<Customer> Customers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ═══════════════════════════════════════════════════════════════
        // FALL 1: DEFAULT - Table Sharing (UNERWARTET!)
        // ═══════════════════════════════════════════════════════════════
        /*
        modelBuilder.Entity<Customer>()
            .OwnsOne(c => c.ShippingAddress);  // Ohne ToTable()
            // oder gar keine Konfiguration: EF Core erkennt automatisch 
            // die Owned Type-Eigenschaft!
        */
        /*
        → MIGRATION erzeugt:
        ALTER TABLE Customers ADD
            ShippingAddress_Street NVARCHAR(60),
            ShippingAddress_City NVARCHAR(15),
            ShippingAddress_PostalCode NVARCHAR(10),
            ShippingAddress_Country NVARCHAR(15)
        */

        // ═══════════════════════════════════════════════════════════════
        // FALL 2: Separate Tabelle (explizit)
        // ═══════════════════════════════════════════════════════════════
        /*
        modelBuilder.Entity<Customer>()
            .OwnsOne(c => c.ShippingAddress, addr =>
            {
                addr.ToTable("CustomerAddresses");  // ← Separate Tabelle!
            });
        
        → CustomerAddresses(CustomerCustomerID, Street, City, PostalCode)
        */

        // ═══════════════════════════════════════════════════════════════
        // FALL 3: Separate Entity (BEST PRACTICE)
        // ═══════════════════════════════════════════════════════════════
        /*
        modelBuilder.Entity<Customer>()
            .HasOne<Address>("ShippingAddress")
            .WithOne()
            .HasForeignKey<Address>("CustomerID");
        */
    }
}


