// PROBLEM: DbContext wird im MainWindow instanziiert und lebt zu lange
// Der DbContext sollte eine kurze Lebensdauer haben (per Request/Operation)

public class MainWindow : Window
{
    // Backing Field für DbContext
    private readonly AppDbContext _context;

    public MainWindow()
    {
        InitializeComponent();
        
        // DbContext wird im Konstruktor erstellt und bleibt für die 
        // gesamte Window-Lebensdauer bestehen
        _context = new AppDbContext();
    }

    private void OpenChildWindow_Click(object sender, RoutedEventArgs e)
    {
        // Der gleiche DbContext wird an das ChildWindow weitergegeben
        var childWindow = new ChildWindow(_context);
        childWindow.Show();
    }
}

public class ChildWindow : Window
{
    private readonly AppDbContext _context;

    // ChildWindow erhält DbContext über Konstruktor
    public ChildWindow(AppDbContext context)
    {
        InitializeComponent();
        _context = context;
    }

    private void LoadData()
    {
        // Beide Fenster arbeiten mit dem gleichen DbContext
        // Dies kann zu Problemen führen:
        // - ChangeTracker wird überladen
        // - Alte Daten im Cache
        // - Keine klare Transaktion-Grenze
        var data = _context.Products.ToList();
    }
}

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

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








// LÖSUNG: DbContext pro Operation/Dialog
// - Erstelle einen neuen DbContext für jede Operation
// - Verwende using-Block oder explizites Dispose
// - Bei WPF: DbContext in ViewModel mit kurzer Lebensdauer
