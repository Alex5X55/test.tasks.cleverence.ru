using Microsoft.Extensions.Options;
using tz.cleverence.ru.Options;
using tz.cleverence.ru.Services.LogConverter.Abstraction;

namespace tz.cleverence.ru.Services.LogConverter.Implimentation
{
    public class LogConverter : ILogConverter
    {
        private readonly FilesOptions _files;

        private readonly ILogParser _logParser;
        public LogConverter(IOptions<FilesOptions> files, ILogParser logParser)
        {
            _files = files.Value;
            _logParser = logParser;
        }

        public async Task Convert(CancellationToken cancellationToken)
        {
            StreamWriter? writerOut = null;
            StreamWriter? writerProblem = null;

            if (!File.Exists(_files.InputFile))
                throw new Exception($"Файла {_files.InputFile} не существует");

            Console.WriteLine($"Проверяем, существет ли файл {_files.InputFile}");

            try
            {
                try
                {
                    writerOut = new StreamWriter(_files.OutputFile);
                    Console.WriteLine($"Создаем, файл {_files.OutputFile}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Не удалось создать файл {_files.InputFile}");
                }

                try
                {
                    writerProblem = new StreamWriter(_files.ProblemsFile);
                    Console.WriteLine($"Создаем, файл {_files.ProblemsFile}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Не удалось создать файл {_files.ProblemsFile}");
                }

                await foreach (string line in File.ReadLinesAsync(_files.InputFile, cancellationToken))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var outputFormat = await _logParser.ParseAsync(line, cancellationToken);

                    Console.WriteLine($"Парсим строку {line}");

                    if (outputFormat != null)
                    {
                        await writerOut.WriteLineAsync(outputFormat.ToString());
                        Console.WriteLine($"Удовлетворяет регулярному вражению, пишем в файл {_files.OutputFile}");
                    }
                    else
                    {
                        await writerProblem.WriteLineAsync(line);
                        Console.WriteLine($"Неудовлетворяет регулярному вражению, пишем в файл {_files.ProblemsFile}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                throw new Exception("Операция отменена");
            }
            catch (Exception ex)
            {
                throw new Exception($"Критическая ошибка: {ex.Message}");
            }
            finally
            {
                if (writerOut != null)
                {
                    await writerOut.FlushAsync();
                    await writerOut.DisposeAsync();
                    Console.WriteLine($"Освобождаем ресурсы {_files.OutputFile}");
                }

                if (writerProblem != null)
                {
                    await writerProblem.FlushAsync();
                    await writerProblem.DisposeAsync();
                    Console.WriteLine($"Освобождаем ресурсы {_files.ProblemsFile}");
                }
            }
        }
    }
}
