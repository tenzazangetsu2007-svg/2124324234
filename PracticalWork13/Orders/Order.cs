using System.Collections.Generic;

namespace PracticalWork13.Orders
{
    /// <summary>Статусы заказа.</summary>
    public enum OrderStatus
    {
        New,        // Новый
        Processing, // В обработке
        Completed   // Выполнен
    }

    /// <summary>Заказ интернет-магазина.</summary>
    public class Order
    {
        public int Number { get; }
        public IReadOnlyList<OrderItem> Items { get; }
        public decimal Total { get; }
        public decimal Discount { get; }
        public decimal TotalWithDiscount { get; }
        public OrderStatus Status { get; internal set; }

        public Order(int number, IReadOnlyList<OrderItem> items, decimal total, decimal discount)
        {
            Number = number;
            Items = items;
            Total = total;
            Discount = discount;
            TotalWithDiscount = total - discount;
            Status = OrderStatus.New;
        }
    }
}
