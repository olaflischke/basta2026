// PROBLEM: DbContext als Singleton registriert
// DbContext ist NICHT thread-safe und sollte eine kurze Lebensdauer haben

public class SingletonDIExample
{
    public static void ConfigureServices()
    {
        var services = new ServiceCollection();

        // DbContext als Singleton
        services.AddSingleton<AppDbContext>();
        
        // Andere Services
        services.AddTransient<ProductService>();
        services.AddTransient<OrderService>();

        var serviceProvider = services.BuildServiceProvider();

        // Beide Services bekommen die GLEICHE DbContext-Instanz
        var productService1 = serviceProvider.GetService<ProductService>();
        var productService2 = serviceProvider.GetService<ProductService>();
        
        // Problem bei parallelen Zugriffen
        var task1 = System.Threading.Tasks.Task.Run(() => productService1.GetProducts());
        var task2 = System.Threading.Tasks.Task.Run(() => productService2.GetProducts());
        
        System.Threading.Tasks.Task.WaitAll(task1, task2);
        // Race Conditions, Exceptions, inkonsistente Daten!
    }
}
#region Demo-Klassen
public class ProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public void GetProducts()
    {
        // Alle Services teilen sich den gleichen DbContext
        // Bei parallelen Aufrufen: Chaos!
        var products = _context.Products.ToList();
    }

    public void UpdateProduct(int id, string name)
    {
        var product = _context.Products.Find(id);
        product.Name = name;
        _context.SaveChanges();
        // ChangeTracker wird niemals geleert, wächst immer weiter
    }
}

public class OrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context; // Gleiche Instanz wie ProductService!
    }

    public void GetOrders()
    {
        var orders = _context.Orders.ToList();
    }
}

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=.;Database=Demo;Trusted_Connection=True;");
    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal Total { get; set; }
}
#endregion







// LÖSUNG: Richtige Lifetime für DbContext
public class CorrectDIExample
{
    public static void ConfigureServices()
    {
        var services = new ServiceCollection();

        // DbContext mit Scoped Lifetime
        // In ASP.NET Core: Ein DbContext pro HTTP-Request
        // In Desktop-Apps: Ein DbContext pro Unit of Work
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer("Server=.;Database=Demo;Trusted_Connection=True;"),
            ServiceLifetime.Scoped); // Standard bei AddDbContext

        // Oder explizit:
        // services.AddScoped<AppDbContext>();

        services.AddTransient<ProductService>();
    }
}

// PROBLEME beim Singleton-DbContext:
// 1. Nicht thread-safe → Race Conditions
// 2. ChangeTracker wächst unbegrenzt → Memory Leak
// 3. Alte Daten im Cache → Stale Data
// 4. Connection-Pool-Probleme
// 5. Transaktions-Management unmöglich
