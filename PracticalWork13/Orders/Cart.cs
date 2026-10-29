using System.Collections.Generic;
using System.Linq;
using PracticalWork13.Common;

namespace PracticalWork13.Orders
{
    /// <summary>Позиция заказа.</summary>
    public class OrderItem
    {
        public string ProductName { get; }
        public decimal Price { get; }
        public int Quantity { get; }
        public decimal Sum => Price * Quantity;

        public OrderItem(string productName, decimal price, int quantity)
        {
            ProductName = productName;
            Price = price;
            Quantity = quantity;
        }
    }

    /// <summary>Корзина покупателя.</summary>
    public class Cart
    {
        private readonly List<OrderItem> _items = new List<OrderItem>();

        public IReadOnlyList<OrderItem> Items => _items;

        public decimal Total => _items.Sum(i => i.Sum);

        public void AddItem(string productName, decimal price, int quantity)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new ValidationException("Название товара не должно быть пустым.");
            if (price <= 0)
                throw new ValidationException("Цена товара должна быть положительной.");
            if (quantity <= 0)
                throw new ValidationException("Количество товара должно быть положительным.");
            _items.Add(new OrderItem(productName, price, quantity));
        }
    }
}
