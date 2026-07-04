
using System.Text.RegularExpressions;
using tz.cleverence.ru.Services.LogConverter.Contracts;

namespace tz.cleverence.ru.Services.LogConverter.Abstraction
{
    public interface ILogParser
    {
        Task<OutputFormatDto?> ParseAsync(string line, CancellationToken cancellationToken);

        OutputFormatDto BuildDto(Match match, string dateTimeFormat);

    }
}
