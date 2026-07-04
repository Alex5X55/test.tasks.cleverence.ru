using tz.cleverence.ru.Services.CompressionLib.Exception;

namespace tz.cleverence.ru.Services.CompressionLib.Abstraction
{
    /// <summary>
    /// Базовый абстрактный класс с валидацией входных данных
    /// </summary>
    public abstract class StringCompressorBase : IStringCompressor
    {
        /// <summary>
        /// Сжимает строку, проверяет на правильность входных данных
        /// </summary>
        /// <param name="input">Исходная строка (маленькие латинские буквы)</param>
        /// <returns>Сжатая строка</returns>
        public string Compress(string input)
            {
            if (input == null || input.Length == 0)
                throw new ArgumentNullException(nameof(input));

            ValidateInput(input);

            return CompressData(input);
        }

        /// <summary>
        /// Распаковывает строку
        /// </summary>
        /// <param name="compressed">Сжатая строка</param>
        /// <returns>Исходная строка</returns>
        public string Decompress(string compressed)
        {
            if (compressed == null || compressed.Length == 0)
                throw new ArgumentNullException(nameof(compressed));

            return DecompressData(compressed);
        }
        /// <summary>
        /// Сжимает строку, проверяет на правильность входных данных, версия для асинхронных операций
        /// </summary>
        /// <param name="input">Исходная строка (маленькие латинские буквы)</param>
        /// <returns>Сжатая строка</returns>
        public virtual Task<string> CompressAsync(string input, CancellationToken cancellationToken)
        {
            return Task.Run(() => Compress(input));
        }

        /// <summary>
        /// Распаковывает строку, вариант для ассинхронных операций
        /// </summary>
        /// <param name="compressed">Сжатая строка</param>
        /// <returns>Исходная строка</returns>
        public virtual Task<string> DecompressAsync(string compressed, CancellationToken cancellationToken)
        {
            return Task.Run(() => Decompress(compressed));
        }


        /// <summary>
        /// Валидация входной строки (только маленькие латинские буквы)
        /// </summary>
        protected virtual void ValidateInput(string input)
        {
            foreach (var c in input)
            {
                if (c < 'a' || c > 'z')
                {
                    throw new CompressionException(
                        $"Недопустимый символ '{c}'. Допускаются только маленькие латинские буквы (a-z).");
                }
            }
        }

        /// <summary>
        /// Логика сжатия (для реализации в наследниках)
        /// </summary>
        protected abstract string CompressData(string input);

        /// <summary>
        /// Логика распаковки (для оеализации в наследниках)
        /// </summary>
        protected abstract string DecompressData(string compressed);

    }
}
