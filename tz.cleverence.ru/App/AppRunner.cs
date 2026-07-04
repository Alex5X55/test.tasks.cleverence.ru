
using Microsoft.Extensions.Options;
using tz.cleverence.ru.Options;
using tz.cleverence.ru.Services.CompressionLib.Abstraction;
using tz.cleverence.ru.Services.LogConverter.Abstraction;
using tz.cleverence.ru.Services.StaticServer;

namespace tz.cleverence.ru.App
{
    public class AppRunner : IAppRunner
    {
        private readonly IStringCompressor _compressor;
        private readonly ILogConverter _logConverter;
        private readonly IOptions<FilesOptions> _fileOptions;
        private readonly IOptions<RegexOptions> _regexOptions;

        private Task[] RunConsumers(int consumers)
        {
            var tasks = new Task[consumers];

            for (int i = 0; i < consumers; i++)
            {
                int consumerId = i;
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 30; j++)
                    {
                        var val = StaticServer.GetCount();
                        Console.WriteLine($"[Consumer {consumerId}] прочитал: {val}");
                    }
                });
            }

            return tasks;
        }

        private Task[] RunConsumersProd(int consumers)
        {
            var tasks = new Task[consumers];

            for (int i = 0; i < consumers; i++)
            {
                int consumerId = i;
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 30; j++)
                    {
                        var val = StaticServerProd.GetCountProd();
                        Console.WriteLine($"[Consumer prod {consumerId}] прочитал: {val}");
                    }
                });
            }

            return tasks;
        }

        private Task[] RunProducers(int produsers)
        {
            var tasks = new Task[produsers];

            for (int i = 0; i < produsers; i++)
            {
                int produserId = i;
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 20; j++)
                    {
                        StaticServer.AddToCount(15);
                        Console.WriteLine($"[Produser {produserId}] записал +15");
                    }
                });
            }

            return tasks;
        }

        private Task[] RunProducersProd(int produsers)
        {
            var tasks = new Task[produsers];

            for (int i = 0; i < produsers; i++)
            {
                int produserId = i;
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 20; j++)
                    {
                        StaticServerProd.AddToCountProd(15);
                        Console.WriteLine($"[Produser prod {produserId}] записал +15");
                    }
                });
            }

            return tasks;
        }


        public AppRunner(IStringCompressor compressor
                         ,ILogConverter logConverter
                         ,IOptions<FilesOptions> fileOptions
                         ,IOptions<RegexOptions> regexOptions)
        {
            _compressor = compressor;
            _logConverter = logConverter;
            _fileOptions = fileOptions ?? throw new ArgumentNullException(nameof(fileOptions));
            _regexOptions = regexOptions ?? throw new ArgumentNullException(nameof(regexOptions));
        }

        //public void Run(string[] args)
        public async Task RunAsync(string[] args, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("CompressionLib************************");
                Console.WriteLine("Упаковка/распаковка строки:");
                Console.WriteLine("Упаковка:");
                Console.WriteLine("Исходная строка: aaabbcccdde");
                var compressStr = _compressor.Compress("aaabbcccdde");
                Console.WriteLine($"Сжатая строка: {compressStr}");
                Console.WriteLine("Распаковка:");
                Console.WriteLine("Исходная строка: a3b2c3d2e");
                var decompressStr = _compressor.Decompress("a3b2c3d2e");
                Console.WriteLine($"Распакованная строка: {decompressStr}");
                Console.WriteLine("**************************************");
                Console.WriteLine("StaticServer**************************");

                // Запускаем читателей и писателей
                var pTasks = RunProducers(3);
                var cTasks = RunConsumers(5);
                Task.WaitAll(pTasks);
                Task.WaitAll(cTasks);
                Console.WriteLine($"Итоговое значение: {StaticServer.GetCount()}");
                //******
                var pTasksProd = RunProducersProd(3);
                var cTasksProd = RunConsumersProd(5);
                Task.WaitAll(pTasksProd);
                Task.WaitAll(cTasksProd);
                Console.WriteLine($"Итоговое значение: {StaticServerProd.GetCountProd()}");
                Console.WriteLine("**************************************");

                Console.WriteLine("LogConverter**************************");
                Console.WriteLine("Читаем настройки из appsettings.json");
                Console.WriteLine($"Format1Regex {_regexOptions.Value.Format1Regex}");
                Console.WriteLine($"Format2Regex {_regexOptions.Value.Format2Regex}");
                Console.WriteLine($"Исходный файл {_fileOptions.Value.InputFile}");
                Console.WriteLine($"Выходной файл {_fileOptions.Value.OutputFile}");
                Console.WriteLine($"Проблемный файл {_fileOptions.Value.ProblemsFile}");
                await _logConverter.Convert(cancellationToken);
                Console.WriteLine("**************************************");

            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
            Console.ReadKey();
        }
    }
}
