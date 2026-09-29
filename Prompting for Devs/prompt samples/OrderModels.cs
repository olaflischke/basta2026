using System;
using System.Collections.Generic;

namespace DemoShop.Application.Orders;

public class Order
{
    public int              Id          { get; set; }
    public int              CustomerId  { get; set; }
    public OrderStatus      Status      { get; set; }
    public DateTime         CreatedAt   { get; set; }
    public DateTime?        CancelledAt { get; set; }
    public List<OrderItem>  Items       { get; set; } = new();
}

public class OrderItem
{
    public int     Id        { get; set; }
    public int     OrderId   { get; set; }
    public string  ProductId { get; set; } = string.Empty;
    public int     Quantity  { get; set; }
    public decimal UnitPrice { get; set; }
}

public enum OrderStatus
{
    Pending,
    Confirmed,
    Shipped,
    Cancelled
}

public class OrderException : Exception
{
    public OrderException(string message) : base(message) { }
    public OrderException(string message, Exception inner) : base(message, inner) { }
}
