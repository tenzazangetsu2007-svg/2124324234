using System;
using PracticalWork13.Orders;
using PracticalWork13.University;
using PracticalWork13.Warehouse;

namespace PracticalWork13
{
    /// <summary>
    /// Практическое занятие №13. Тестирование проекта.
    /// Одно консольное приложение — три информационные системы:
    /// Вариант 1 — Учет товаров на складе,
    /// Вариант 2 — Учет заказов интернет-магазина,
    /// Вариант 3 — Автоматизация работы учебной части (деканат).
    /// </summary>
    internal class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Практическое занятие №13. Тестирование проекта";

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine();
                Console.WriteLine("===================================================");
                Console.WriteLine("   ПРАКТИЧЕСКОЕ ЗАНЯТИЕ №13. ТЕСТИРОВАНИЕ ПРОЕКТА");
                Console.WriteLine("===================================================");
                Console.WriteLine("  1 - Вариант 1. Учет товаров на складе");
                Console.WriteLine("  2 - Вариант 2. Учет заказов интернет-магазина");
                Console.WriteLine("  3 - Вариант 3. Автоматизация работы деканата");
                Console.WriteLine("  0 - Выход");
                Console.Write("  > ");

                switch (Console.ReadLine())
                {
                    case "1": RunWarehouseDemo(); break;
                    case "2": RunOrdersDemo(); break;
                    case "3": RunUniversityDemo(); break;
                    case "0": exit = true; break;
                    default: Console.WriteLine("Неверный пункт меню."); break;
                }
            }
        }

        private static string Read(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? string.Empty;
        }

        // ==================== ВАРИАНТ 1. СКЛАД ====================
        private static void RunWarehouseDemo()
        {
            Console.WriteLine();
            Console.WriteLine("========= ВАРИАНТ 1. УЧЕТ ТОВАРОВ НА СКЛАДЕ =========");

            var auth = new AuthService();
            User current = null;
            while (current == null)
            {
                try
                {
                    current = auth.Login(
                        Read("Логин (кладовщик store / 12345): "),
                        Read("Пароль: "));
                    Console.WriteLine("Добро пожаловать, {0}! Ваша роль: {1}.",
                        current.Login, current.Role);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка авторизации: " + ex.Message);
                }
            }

            var catalog = new ProductCatalog();
            var stock = new StockManager(catalog);
            catalog.AddProduct("NB-001", "Ноутбук", "75000", "10"); // тестовые данные

            bool back = false;
            while (!back)
            {
                Console.WriteLine();
                Console.WriteLine("  1 - Добавить товар");
                Console.WriteLine("  2 - Поиск товара по артикулу");
                Console.WriteLine("  3 - Приход товара");
                Console.WriteLine("  4 - Расход товара");
                Console.WriteLine("  5 - Отчет по остаткам");
                Console.WriteLine("  0 - Назад");
                Console.Write("  > ");

                try
                {
                    switch (Console.ReadLine())
                    {
                        case "1":
                            var p = catalog.AddProduct(
                                Read("Артикул: "), Read("Название: "),
                                Read("Цена: "), Read("Количество: "));
                            Console.WriteLine("Товар сохранен: [{0}] {1} - {2}, цена {3}, остаток {4}.",
                                p.Id, p.Article, p.Name, p.Price, p.Quantity);
                            break;
                        case "2":
                            var found = catalog.SearchByArticle(Read("Артикул: "));
                            Console.WriteLine(found == null
                                ? "Товар не найден."
                                : string.Format("{0}: {1}, цена {2}, остаток {3}.",
                                    found.Article, found.Name, found.Price, found.Quantity));
                            break;
                        case "3":
                            stock.Receive(int.Parse(Read("ID товара: ")), int.Parse(Read("Количество: ")));
                            Console.WriteLine("Приход оформлен.");
                            break;
                        case "4":
                            stock.Issue(int.Parse(Read("ID товара: ")), int.Parse(Read("Количество: ")));
                            Console.WriteLine("Расход оформлен.");
                            break;
                        case "5":
                            Console.WriteLine(StockReport.Generate(catalog.GetAll()));
                            break;
                        case "0": back = true; break;
                        default: Console.WriteLine("Неверный пункт меню."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }
        }

        // ==================== ВАРИАНТ 2. ЗАКАЗЫ ====================
        private static string StatusRu(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.New: return "Новый";
                case OrderStatus.Processing: return "В обработке";
                default: return "Выполнен";
            }
        }

        private static void RunOrdersDemo()
        {
            Console.WriteLine();
            Console.WriteLine("====== ВАРИАНТ 2. УЧЕТ ЗАКАЗОВ ИНТЕРНЕТ-МАГАЗИНА ======");

            var customers = new CustomerService();
            var orders = new OrderService();

            Console.WriteLine("--- Регистрация клиента ---");
            var customer = customers.Register(
                Read("Email: "),
                Read("Пароль (мин. 6 символов): "),
                Read("Телефон: "));
            Console.WriteLine("Клиент зарегистрирован: ID={0}, {1}. Выполнен автоматический вход.",
                customer.Id, customer.Email);

            var cart = new Cart();
            bool back = false;
            while (!back)
            {
                Console.WriteLine();
                Console.WriteLine("  1 - Добавить товар в корзину");
                Console.WriteLine("  2 - Показать корзину и итог со скидкой");
                Console.WriteLine("  3 - Оформить заказ");
                Console.WriteLine("  4 - Сменить статус заказа (администратор)");
                Console.WriteLine("  0 - Назад");
                Console.Write("  > ");

                try
                {
                    switch (Console.ReadLine())
                    {
                        case "1":
                            cart.AddItem(Read("Название товара: "),
                                decimal.Parse(Read("Цена: ")),
                                int.Parse(Read("Количество: ")));
                            Console.WriteLine("Товар добавлен. Сумма корзины: {0}", cart.Total);
                            break;
                        case "2":
                            foreach (var item in cart.Items)
                                Console.WriteLine("{0} x{1} = {2}", item.ProductName, item.Quantity, item.Sum);
                            var discount = DiscountService.CalculateDiscount(cart.Total);
                            Console.WriteLine("Итого: {0}, скидка: {1}, к оплате: {2}",
                                cart.Total, discount, cart.Total - discount);
                            break;
                        case "3":
                            var order = orders.CreateOrder(cart);
                            Console.WriteLine("Заказ №{0} оформлен. Статус: {1}. Сумма: {2} (скидка {3}).",
                                order.Number, StatusRu(order.Status), order.TotalWithDiscount, order.Discount);
                            cart = new Cart();
                            break;
                        case "4":
                            int number = int.Parse(Read("Номер заказа: "));
                            Console.Write("Новый статус (0-Новый, 1-В обработке, 2-Выполнен): ");
                            var status = (OrderStatus)int.Parse(Console.ReadLine() ?? "0");
                            orders.ChangeStatus(number, status,
                                Read("Вы администратор? (y/n): ").Trim().ToLower() == "y");
                            Console.WriteLine("Статус заказа №{0} обновлен: {1}.",
                                number, StatusRu(orders.GetByNumber(number).Status));
                            break;
                        case "0": back = true; break;
                        default: Console.WriteLine("Неверный пункт меню."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }
        }

        // ==================== ВАРИАНТ 3. ДЕКАНАТ ====================
        private static void RunUniversityDemo()
        {
            Console.WriteLine();
            Console.WriteLine("===== ВАРИАНТ 3. АВТОМАТИЗАЦИЯ РАБОТЫ ДЕКАНАТА =====");

            var university = new UniversityService();
            var group = university.AddGroup(Read("Название группы: "));
            Console.WriteLine("Создана группа: ID={0}, {1}.", group.Id, group.Name);

            bool back = false;
            while (!back)
            {
                Console.WriteLine();
                Console.WriteLine("  1 - Добавить студента");
                Console.WriteLine("  2 - Проставить оценку");
                Console.WriteLine("  3 - Средний балл студента");
                Console.WriteLine("  4 - Список группы");
                Console.WriteLine("  0 - Назад");
                Console.Write("  > ");

                try
                {
                    switch (Console.ReadLine())
                    {
                        case "1":
                            var student = university.AddStudent(
                                Read("ФИО студента: "), group.Id);
                            Console.WriteLine("Студент добавлен: ID={0}, {1}, группа {2}.",
                                student.Id, student.FullName, group.Name);
                            break;
                        case "2":
                            university.AddGrade(
                                int.Parse(Read("ID студента: ")),
                                Read("Дисциплина: "),
                                int.Parse(Read("Оценка (2-5): ")));
                            Console.WriteLine("Оценка сохранена.");
                            break;
                        case "3":
                            double avg = university.GetAverage(int.Parse(Read("ID студента: ")));
                            Console.WriteLine("Средний балл: {0:F2}.", avg);
                            break;
                        case "4":
                            foreach (var s in group.Students)
                                Console.WriteLine("[{0}] {1} (ср. балл {2:F2})",
                                    s.Id, s.FullName, university.GetAverage(s.Id));
                            break;
                        case "0": back = true; break;
                        default: Console.WriteLine("Неверный пункт меню."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }
        }
    }
}
