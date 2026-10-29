using System;

namespace PracticalWork13.Common
{
    /// <summary>
    /// Исключение, выбрасываемое при ошибках валидации входных данных.
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
}
