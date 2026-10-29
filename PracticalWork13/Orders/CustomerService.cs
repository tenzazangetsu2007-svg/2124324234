using System;
using System.Collections.Generic;
using System.Linq;
using PracticalWork13.Common;

namespace PracticalWork13.Orders
{
    /// <summary>Регистрация клиентов (клиентская часть).</summary>
    public class CustomerService
    {
        private readonly List<Customer> _customers = new List<Customer>();
        private int _nextId = 1;

        /// <summary>
        /// Регистрирует клиента (TC-OR-01). При успехе клиент сохраняется
        /// в «базу» и выполняется автоматический вход (возврат объекта).
        /// </summary>
        public Customer Register(string email, string password, string phone)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || email.StartsWith("@") || email.EndsWith("@"))
                throw new ValidationException("Некорректный формат email.");

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                throw new ValidationException("Пароль должен содержать не менее 6 символов.");

            if (string.IsNullOrWhiteSpace(phone))
                throw new ValidationException("Телефон не должен быть пустым.");

            if (_customers.Any(c => string.Equals(c.Email, email, StringComparison.OrdinalIgnoreCase)))
                throw new ValidationException("Пользователь с таким email уже зарегистрирован.");

            var customer = new Customer
            {
                Id = _nextId++,
                Email = email,
                Password = password,
                Phone = phone
            };
            _customers.Add(customer);
            return customer;
        }

        public IReadOnlyList<Customer> GetAll()
        {
            return _customers;
        }
    }
}
