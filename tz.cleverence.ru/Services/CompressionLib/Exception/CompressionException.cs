
namespace tz.cleverence.ru.Services.CompressionLib.Exception
{
    /// <summary>
    /// Исключение для ошибок компрессии/декомпрессии
    /// </summary>
    public class CompressionException : System.Exception
    {
        /// <summary>
        /// Исключение для ошибок компрессии/декомпрессии с сообщением
        /// </summary>
        public CompressionException() : base()
        {
        }

        /// <summary>
        /// Исключение для ошибок компрессии/декомпрессии с сообщением
        /// </summary>
        public CompressionException(string message) : base(message)
        {
        }
        /// <summary>
        /// Исключение для ошибок компрессии/декомпрессии с сообщением и внутренней ошибкой
        /// </summary>
        public CompressionException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}
