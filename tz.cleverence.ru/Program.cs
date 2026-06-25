using Microsoft.Extensions.DependencyInjection;
using tz.cleverence.ru.App;
using tz.cleverence.ru.Services.CompressionLib.Abstraction;
using tz.cleverence.ru.Services.CompressionLib.Implimentation;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Тестовое задание для ООО 'Клеверенс Софт' !");

        var services = new ServiceCollection();
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
        }

    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IStringCompressor, CharacterLengthCompressor>();

        services.AddScoped<IAppRunner, AppRunner>();
    }
}

