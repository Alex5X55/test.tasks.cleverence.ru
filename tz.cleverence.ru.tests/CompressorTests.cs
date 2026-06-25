using tz.cleverence.ru.Services.CompressionLib.Abstraction;
using tz.cleverence.ru.Services.CompressionLib.Exception;
using Xunit;

namespace tz.cleverence.ru.tests
{
    /// <summary>
    /// Класс Юнит тестов для компрессии и декомпрессии строк
    /// </summary>
    public class CompressorTests : IClassFixture<CompressorTestsFixture>
    {
        private readonly IStringCompressor _compressor;
        /// <summary>
        /// Конструктор класса Юнит тестов для компрессии и декомпрессии строк
        /// </summary>
        public CompressorTests(CompressorTestsFixture compressorTestsFixture)
            {
            _compressor = compressorTestsFixture.compressor;

            }

        /// <summary>
        /// Тест на компрессию строки
        /// Проверяется исходная фиксированная строка
        /// Тест пройден, если строка преобразована правильно
        /// </summary>
        [Theory]
        [InlineData("aaabbcccdde", "a3b2c3d2e")]
        [InlineData("aaabbc", "a3b2c")]
        [InlineData("abc", "abc")]
        [InlineData("aaaaa", "a5")]
        public void Compress_String_Return_Right_Results(string decompressstr, string compressstr)
        {
            // ARRANGE
            // ACT
            var compressStr = _compressor.Compress(decompressstr);

            // ASSERT
            Assert.Equal(compressstr, compressStr);
        }

        /// <summary>
        /// Тест - негативный на компрессию строки
        /// Проверяется исходная фиксированная строка
        /// Тест пройден, если строка преобразована не правильно
        /// </summary>
        [Fact]
        public void Compress_String_Return_Not_Right_Results()
        {
            // ARRANGE
            var decompressStr = "aaabbcccdde";

            // ACT
            var compressStr = _compressor.Compress(decompressStr);

            // ASSERT
            Assert.NotEqual("a3b2c3d3e", compressStr);
        }

        /// <summary>
        /// Тест на декомпрессию строки
        /// Проверяется исходная фиксированная строка
        /// Тест пройден, если строка преобразована правильно
        /// </summary>
        [Theory]
        [InlineData("a3b2c3d2e", "aaabbcccdde")]
        [InlineData("a3b2c", "aaabbc")]
        [InlineData("abc", "abc")]
        [InlineData("a5", "aaaaa")]
        public void Decompress_String_Return_Right_Results(string compressstr, string decompressstr)
        {
            // ARRANGE
            // ACT
            var decompressStr = _compressor.Decompress(compressstr);

            // ASSERT
            Assert.Equal(decompressstr, decompressStr);

        }

        /// <summary>
        /// Тест - негативный на декомпрессию строки
        /// Проверяется исходная фиксированная строка
        /// Тест пройден, если строка преобразована не правильно
        /// </summary>
        [Fact]
        public void Decompress_String_Return_Not_Right_Results()
        {
            // ARRANGE
            var compressStr = "a3b2c3d2e";
            // ACT
            var decompressStr = _compressor.Decompress(compressStr);

            // ASSERT
            Assert.NotEqual("aaabbcccdd", decompressStr);

        }

        /// <summary>
        /// Тест - негативный на компрессию строки
        /// Проверяется исходная фиксированная строка (null или пустая строка)
        /// Тест пройден, если возбуждено исключение ArgumentNullException
        /// </summary>
        [Theory]
        [InlineData(null, "a3b2c3d2e")]
        [InlineData("", "a3b2c")]
        public void Compress_String_Null_Or_Empty_Return_Throw_Exception(string decompressstr, string compressstr)
        {
            // ARRANGE
            // ACT
            // ASSERT
            Assert.Throws<ArgumentNullException>(() => _compressor.Compress(decompressstr));

        }

        /// <summary>
        /// Тест - негативный на компрессию строки
        /// Проверяется исходная фиксированная строка с недопустимым символом
        /// Тест пройден, если возбуждено исключение CompressionException
        /// </summary>
        [Fact]
        public void Compress_String_Not_Right_Character_Return_Throw_Exception()
        {
            // ARRANGE
            var decompressStr = "aaabBcccdde";

            // ACT
            // ASSERT
            Assert.Throws<CompressionException>(() => _compressor.Compress(decompressStr));

        }

        /// <summary>
        /// Тест - негативный на декомпрессию строки
        /// Проверяется исходная фиксированная строка когда ожидалась определенная буква
        /// а был получен символ не входящий в множество правильных
        /// Тест пройден, если возбуждено исключение CompressionException
        /// </summary>
        [Fact]
        public void Decompress_String_Not_Right_Character_Return_Throw_Exception()
        {
            // ARRANGE
            var compressStr = "a3b2c!3d2e";

            // ACT
            // ASSERT
            Assert.Throws<CompressionException>(() => _compressor.Decompress(compressStr));
        }

        /// <summary>
        /// Тест - негативный на декомпрессию строки
        /// Проверяется исходная фиксированная строка когда цифра <= 0
        /// а был получен символ не входящий в множество правильных
        /// Тест пройден, если возбуждено исключение CompressionException
        /// </summary>
        [Fact]
        public void Decompress_String_Digit_Less_Or_Equal_Zero_Return_Throw_Exception()
        {
            // ARRANGE
            var compressStr = "a3b2c0d2e";

            // ACT
            // ASSERT
            Assert.Throws<CompressionException>(() => _compressor.Decompress(compressStr));
        }
    }
}
