using NUnit.Framework;
using PracticalWork13.Common;
using PracticalWork13.Orders;

namespace OrderTests
{
    /// <summary>Тесты регистрации клиентов (TC-OR-01).</summary>
    [TestFixture]
    public class CustomerServiceTests
    {
        [Test]
        public void TC_OR_01_ValidRegistration_CreatesAccountInDatabase()
        {
            // Шаги: 1. Заполнить поля регистрации (Email, Пароль, Телефон). 2. «Зарегистрироваться».
            var service = new CustomerService();
            var customer = service.Register("ivan@mail.ru", "pass123", "89991234567");

            // Ожидаемый результат: аккаунт создан, в БД появилась запись, автоматический вход.
            Assert.AreEqual(1, service.GetAll().Count);
            Assert.AreSame(customer, service.GetAll()[0]);
            Assert.AreEqual("ivan@mail.ru", customer.Email);
        }

        [Test]
        public void Register_InvalidEmailFormat_ThrowsException()
        {
            var service = new CustomerService();
            Assert.Throws<ValidationException>(
                () => service.Register("ivan-mail.ru", "pass123", "89991234567"));
        }

        [Test]
        public void Register_ShortPassword_ThrowsException()
        {
            var service = new CustomerService();
            Assert.Throws<ValidationException>(
                () => service.Register("ivan@mail.ru", "123", "89991234567"));
        }

        [Test]
        public void Register_EmptyPhone_ThrowsException()
        {
            var service = new CustomerService();
            Assert.Throws<ValidationException>(
                () => service.Register("ivan@mail.ru", "pass123", ""));
        }

        [Test]
        public void Register_DuplicateEmail_ThrowsException()
        {
            var service = new CustomerService();
            service.Register("ivan@mail.ru", "pass123", "89991234567");
            Assert.Throws<ValidationException>(
                () => service.Register("ivan@mail.ru", "pass456", "89997654321"));
        }
    }
}
