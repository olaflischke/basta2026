using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DemoShop.Application.Orders;

/// <summary>
/// Verarbeitet eingehende Bestellungen und persistiert sie in der Datenbank.
/// </summary>
public class OrderService
{
    private readonly AppDbContext _db;
    private readonly ILogger<OrderService> _logger;

    public OrderService(AppDbContext db, ILogger<OrderService> logger)
    {
        _db     = db     ?? throw new ArgumentNullException(nameof(db));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Legt eine neue Bestellung an.
    /// </summary>
    /// <param name="order">Die zu speichernde Bestellung. Darf nicht null sein.</param>
    /// <param name="ct">Abbruch-Token fuer asynchrone Operationen.</param>
    /// <returns>Die gespeicherte Bestellung mit vergebener Id.</returns>
    /// <exception cref="ArgumentNullException">Wenn <paramref name="order"/> null ist.</exception>
    /// <exception cref="OrderException">Wenn die Bestellung ungueltig ist.</exception>
    public async Task<Order> CreateAsync(Order order, CancellationToken ct = default)
    {
        if (order is null)
            throw new ArgumentNullException(nameof(order));

        if (order.Items is null || order.Items.Count == 0)
            throw new OrderException("Eine Bestellung muss mindestens einen Artikel enthalten.");

        if (order.CustomerId <= 0)
            throw new OrderException("Eine gueltige Kunden-Id ist erforderlich.");

        try
        {
            order.CreatedAt = DateTime.UtcNow;
            order.Status    = OrderStatus.Pending;

            _db.Orders.Add(order);
            await _db.SaveChangesAsync(ct);

            _logger.LogInformation(
                "Bestellung {OrderId} fuer Kunde {CustomerId} angelegt.",
                order.Id, order.CustomerId);

            return order;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Fehler beim Speichern der Bestellung fuer Kunde {CustomerId}.", order.CustomerId);
            throw new OrderException("Die Bestellung konnte nicht gespeichert werden.", ex);
        }
    }

    /// <summary>
    /// Gibt eine Bestellung anhand ihrer Id zurueck.
    /// </summary>
    /// <param name="orderId">Die Id der gesuchten Bestellung.</param>
    /// <param name="ct">Abbruch-Token fuer asynchrone Operationen.</param>
    /// <returns>Die gefundene Bestellung oder null.</returns>
    public async Task<Order?> GetByIdAsync(int orderId, CancellationToken ct = default)
    {
        if (orderId <= 0)
            throw new ArgumentOutOfRangeException(nameof(orderId), "Die Id muss groesser als 0 sein.");

        return await _db.Orders
            .Include(o => o.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == orderId, ct);
    }

    /// <summary>
    /// Storniert eine bestehende Bestellung.
    /// </summary>
    /// <param name="orderId">Die Id der zu stornierenden Bestellung.</param>
    /// <param name="ct">Abbruch-Token fuer asynchrone Operationen.</param>
    /// <exception cref="OrderException">Wenn die Bestellung nicht gefunden oder nicht stornierbar ist.</exception>
    public async Task CancelAsync(int orderId, CancellationToken ct = default)
    {
        if (orderId <= 0)
            throw new ArgumentOutOfRangeException(nameof(orderId), "Die Id muss groesser als 0 sein.");

        var order = await _db.Orders.FindAsync(new object[] { orderId }, ct);

        if (order is null)
            throw new OrderException($"Bestellung {orderId} wurde nicht gefunden.");

        if (order.Status == OrderStatus.Shipped)
            throw new OrderException($"Bestellung {orderId} wurde bereits versandt und kann nicht storniert werden.");

        if (order.Status == OrderStatus.Cancelled)
            throw new OrderException($"Bestellung {orderId} ist bereits storniert.");

        order.Status      = OrderStatus.Cancelled;
        order.CancelledAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        _logger.LogInformation("Bestellung {OrderId} wurde storniert.", orderId);
    }
}
