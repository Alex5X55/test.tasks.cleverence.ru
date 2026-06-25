
using tz.cleverence.ru.Services.CompressionLib.Abstraction;
using tz.cleverence.ru.Services.CompressionLib.Implimentation;

namespace tz.cleverence.ru.tests
{
    public class CompressorTestsFixture : IDisposable
    {
        public IStringCompressor compressor { get; set; }
        public CompressorTestsFixture()
        {
            compressor = new CharacterLengthCompressor();
        }

        public void Dispose()
        {
        }
    }
}
