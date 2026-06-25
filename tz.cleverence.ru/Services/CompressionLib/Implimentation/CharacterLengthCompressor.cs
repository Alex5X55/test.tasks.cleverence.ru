
using System.Text;
using tz.cleverence.ru.Services.CompressionLib.Abstraction;
using tz.cleverence.ru.Services.CompressionLib.Exception;

namespace tz.cleverence.ru.Services.CompressionLib.Implimentation
{
    /// <summary>
    /// Character-Length Encoding/Decodong компрессор
    /// Алгоритм: группы одинаковых символов заменяются на "символ + количество"
    ///           если символ один — количество не пишется
    /// </summary>
    public class CharacterLengthCompressor : StringCompressorBase
    {
        /// <summary>
        /// Упаковка(компрессия) строки
        /// Пример:
        /// "aaabbcccdde" -> "a3b2c3d2e"
        /// </summary>
        protected override string CompressData(string input)
        {
            var result = new StringBuilder(input.Length);
            int i = 0;

            while (i < input.Length)
            {
                char currentChar = input[i];
                int count = 1;

                while (i + count < input.Length && input[i + count] == currentChar)
                {
                    count++;
                }

                result.Append(currentChar);

                if (count > 1)
                {
                    result.Append(count);
                }

                i += count;
            }

            return result.ToString();

        }

        /// <summary>
        /// Распаковка(декомпрессия) строки
        /// Пример:
        /// "a3b2c3d2e" -> "aaabbcccdde"
        /// </summary>
        protected override string DecompressData(string compressed)
        {
            var result = new StringBuilder();
            int i = 0;

            while (i < compressed.Length)
            {
                char currentChar = compressed[i];

                if (currentChar < 'a' || currentChar > 'z')
                {
                    throw new CompressionException(
                        $"Ожидалась буква в позиции {i}, но получен символ '{currentChar}'");
                }

                i++;

                int count = 0;
                bool hasNumber = false;

                while (i < compressed.Length && char.IsDigit(compressed[i]))
                {
                    count = count * 10 + (compressed[i] - '0');
                    hasNumber = true;
                    i++;
                }

                if (!hasNumber)
                {
                    count = 1;
                }

                if (count <= 0)
                {
                    throw new CompressionException(
                        $"Недопустимое количество '{count}' для символа '{currentChar}'");
                }

                for (int j = 0; j < count; j++)
                {
                    result.Append(currentChar);
                }
            }

            return result.ToString();
        }
    }
}
