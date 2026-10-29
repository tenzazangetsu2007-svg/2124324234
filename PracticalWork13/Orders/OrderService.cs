using System.Collections.Generic;
using System.Linq;
using PracticalWork13.Common;

namespace PracticalWork13.Orders
{
    /// <summary>Оформление заказов и смена статусов (административная часть).</summary>
    public class OrderService
    {
        private readonly List<Order> _orders = new List<Order>();
        private int _nextNumber = 1000;

        /// <summary>
        /// Оформляет заказ из корзины (TC-OR-02): присваивается уникальный
        /// номер, рассчитывается скидка, статус «Новый».
        /// </summary>
        public Order CreateOrder(Cart cart)
        {
            if (cart == null || cart.Items.Count == 0)
                throw new ValidationException("Корзина пуста — невозможно оформить заказ.");

            var discount = DiscountService.CalculateDiscount(cart.Total);
            var order = new Order(_nextNumber++, cart.Items, cart.Total, discount);
            _orders.Add(order);
            return order;
        }

        /// <summary>
        /// Смена статуса заказа (TC-OR-04). Доступна только администратору.
        /// </summary>
        public void ChangeStatus(int orderNumber, OrderStatus newStatus, bool isAdministrator)
        {
            if (!isAdministrator)
                throw new System.UnauthorizedAccessException(
                    "Изменять статус заказа может только администратор.");

            var order = _orders.FirstOrDefault(o => o.Number == orderNumber);
            if (order == null)
                throw new ValidationException(string.Format("Заказ №{0} не найден.", orderNumber));

            order.Status = newStatus;
        }

        public Order GetByNumber(int orderNumber)
        {
            return _orders.FirstOrDefault(o => o.Number == orderNumber);
        }

        public IReadOnlyList<Order> GetAll()
        {
            return _orders;
        }
    }
}
