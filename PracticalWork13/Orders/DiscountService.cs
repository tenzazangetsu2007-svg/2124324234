namespace PracticalWork13.Orders
{
    /// <summary>
    /// Расчет скидки (TC-OR-03): при сумме заказа от 5000 руб. скидка 5%.
    /// </summary>
    public static class DiscountService
    {
        public const decimal DiscountThreshold = 5000m;
        public const decimal DiscountRate = 0.05m;

        public static decimal CalculateDiscount(decimal total)
        {
            return total >= DiscountThreshold ? total * DiscountRate : 0m;
        }
    }
}
