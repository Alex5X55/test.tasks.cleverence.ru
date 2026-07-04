namespace tz.cleverence.ru.Services.CompressionLib.Abstraction
{
    /// <summary>
    /// Интерфейс для компрессии и декомпрессии строк
    /// </summary>
    public interface IStringCompressor
    {
        /// <summary>
        /// Сжимает строку
        /// </summary>
        /// <param name="input">Исходная строка (маленькие латинские буквы)</param>
        /// <returns>Сжатая строка</returns>
        string Compress(string input);

        /// <summary>
        /// Распаковывает строку
        /// </summary>
        /// <param name="compressed">Сжатая строка</param>
        /// <returns>Исходная строка</returns>
        string Decompress(string compressed);

        /// <summary>
        /// Сжимает строку, вариант для ассинхронных операций
        /// </summary>
        /// <param name="input">Исходная строка (маленькие латинские буквы)</param>
        /// <returns>Сжатая строка</returns>
        Task<string> CompressAsync(string input, CancellationToken cancellationToken);

        /// <summary>
        /// Распаковывает строку, вариант для ассинхронных операций
        /// </summary>
        /// <param name="compressed">Сжатая строка</param>
        /// <returns>Исходная строка</returns>
        Task<string> DecompressAsync(string compressed, CancellationToken cancellationToken);
    }
}
