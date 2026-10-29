using NUnit.Framework;
using PracticalWork13.Common;
using PracticalWork13.Warehouse;

namespace WarehouseTests
{
    /// <summary>Тесты модуля авторизации склада (TC-SK-01).</summary>
    [TestFixture]
    public class AuthServiceTests
    {
        [Test]
        public void TC_SK_01_ValidStorekeeperLogin_OpensStorekeeperInterfaceWithoutAdminRights()
        {
            // Шаги: 1. Ввести верный логин и пароль кладовщика. 2. Нажать «Войти».
            var auth = new AuthService();
            var user = auth.Login("store", "12345");

            // Ожидаемый результат: успешный вход, роль «Кладовщик», без прав админа.
            Assert.AreEqual(UserRole.Storekeeper, user.Role);
            Assert.IsFalse(user.IsAdmin);
        }

        [Test]
        public void Login_ValidAdminCredentials_ReturnsAdministrator()
        {
            var auth = new AuthService();
            var user = auth.Login("admin", "admin123");
            Assert.AreEqual(UserRole.Administrator, user.Role);
            Assert.IsTrue(user.IsAdmin);
        }

        [Test]
        public void Login_WrongPassword_ThrowsValidationException()
        {
            var auth = new AuthService();
            Assert.Throws<ValidationException>(() => auth.Login("store", "wrong"));
        }

        [Test]
        public void Login_UnknownUser_ThrowsValidationException()
        {
            var auth = new AuthService();
            Assert.Throws<ValidationException>(() => auth.Login("nobody", "12345"));
        }
    }
}
