using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DemoShop.Application.Orders;

/// <summary>
/// Definiert den Datenzugriff für Bestellungen.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Gibt eine Bestellung anhand ihrer Id zurück.
    /// </summary>
    Task<Order?> GetByIdAsync(int orderId, CancellationToken ct = default);

    Task<IReadOnlyList<Order>> GetByCustomerIdAsync(int customerId, CancellationToken ct = default);

    Task<Order> AddAsync(Order order, CancellationToken ct = default);

    /// <summary>
    /// Aktualisiert eine bestehende Bestellung.
    /// </summary>
    Task UpdateAsync(Order order, CancellationToken ct = default);

    Task<bool> ExistsAsync(int orderId, CancellationToken ct = default);
}
