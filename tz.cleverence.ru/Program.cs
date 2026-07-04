using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using tz.cleverence.ru.App;
using tz.cleverence.ru.Options;
using tz.cleverence.ru.Services.CompressionLib.Abstraction;
using tz.cleverence.ru.Services.CompressionLib.Implimentation;
using tz.cleverence.ru.Services.LogConverter.Abstraction;
using tz.cleverence.ru.Services.LogConverter.Implimentation;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Тестовое задание для ООО 'Клеверенс Софт' !");

        var cts = new CancellationTokenSource();
        CancellationToken cancellationToken = cts.Token;

        /*var services = new ServiceCollection();
        ConfigureServices(services);

        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();

        try
        {
            var app = scope.ServiceProvider.GetRequiredService<IAppRunner>();
            app.Run(args);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Ошибка: {ex.Message}");
            Environment.Exit(1);
        }*/

        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.Configure<FilesOptions>(
            builder.Configuration.GetSection(FilesOptions.SectionName));

        builder.Services.Configure<RegexOptions>(
            builder.Configuration.GetSection(RegexOptions.SectionName));

        ConfigureServices(builder.Services);

        var host = builder.Build();

        using var scope = host.Services.CreateScope();
        var app = scope.ServiceProvider.GetRequiredService<IAppRunner>();
        await app.RunAsync(args, cancellationToken);

    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IStringCompressor, CharacterLengthCompressor>();
        services.AddTransient<ILogParser, LogParser>();
        services.AddTransient<ILogConverter, LogConverter>();
        services.AddScoped<IAppRunner, AppRunner>();
    }
}

