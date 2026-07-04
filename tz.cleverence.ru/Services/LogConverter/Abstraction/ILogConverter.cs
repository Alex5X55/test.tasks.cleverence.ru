
namespace tz.cleverence.ru.Services.LogConverter.Abstraction
{
    public interface ILogConverter
    {
        Task Convert(CancellationToken cancellationToken);
    }
}
