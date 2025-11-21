using System;
using System.Collections.Generic;

namespace Domain.Services;

using Domain.Entities;

public static class OrderService
{
    private static readonly List<Order> lastOrders = new List<Order>();
    public static IReadOnlyList<Order> LastOrders => lastOrders.AsReadOnly();

    public static Order CreateOrder(string customer, string product, int qty, decimal price)
    {
        var order = new Order
        {
            Id = new Random().Next(1, 9999999),
            CustomerName = customer,
            ProductName = product,
            Quantity = qty,
            UnitPrice = price
        };
        lastOrders.Add(order);
        return order;
    }
}
