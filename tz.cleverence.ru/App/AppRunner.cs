
using tz.cleverence.ru.Services.CompressionLib.Abstraction;

namespace tz.cleverence.ru.App
{
    public class AppRunner : IAppRunner
    {
        private readonly IStringCompressor _compressor;

        public AppRunner(IStringCompressor compressor)
        {
            _compressor = compressor;
        }

        public void Run(string[] args)
        {
            Console.WriteLine("Упаковка/распаковка строки:");
            Console.WriteLine("Упаковка:");
            Console.WriteLine("Исходная строка: aaabbcccdde");
            var compressStr = _compressor.Compress("aaabbcccdde");
            Console.WriteLine($"Сжатая строка: {compressStr}");

            Console.WriteLine("Распаковка:");
            Console.WriteLine("Исходная строка: a3b2c3d2e");
            var decompressStr = _compressor.Decompress("a3b2c3d2e");
            Console.WriteLine($"Распакованная строка: {decompressStr}");

            Console.ReadKey();
        }
    }
}
