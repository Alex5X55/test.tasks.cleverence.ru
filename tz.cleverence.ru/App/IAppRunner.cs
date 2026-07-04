
namespace tz.cleverence.ru.App
{
    public interface IAppRunner
    {
        //void Run(string[] args);
        Task RunAsync(string[] args, CancellationToken ct = default);
    }
}
