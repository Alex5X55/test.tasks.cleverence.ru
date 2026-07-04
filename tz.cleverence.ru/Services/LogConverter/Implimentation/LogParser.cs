using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using tz.cleverence.ru.Services.LogConverter.Abstraction;
using tz.cleverence.ru.Services.LogConverter.Contracts;

namespace tz.cleverence.ru.Services.LogConverter.Implimentation
{
    public class LogParser : ILogParser
    {
        /* private readonly Regex Format1Regex = new(
             @"^(?<Date>\d{2}\.\d{2}\.\d{4})\s+(?<Time>\d{2}:\d{2}:\d{2}\.\d{3})\s+(?<Level>INFORMATION|INFO|WARNING|WARN|ERROR|DEBUG)\s+(?<Message>.+)$",
             RegexOptions.Compiled);

         private readonly Regex Format2Regex = new(
             @"^(?<Date>\d{4}-\d{2}-\d{2})\s+(?<Time>\d{2}:\d{2}:\d{2}\.\d{4})\|\s*(?<Level>INFORMATION|INFO|WARNING|WARN|ERROR|DEBUG)\|\d+\|(?<Method>[^|]+)\|\s*(?<Message>.+)$",
             RegexOptions.Compiled);

         */
        private readonly tz.cleverence.ru.Options.RegexOptions _regex;

        private Regex? Format1Regex = null;
        private Regex? Format2Regex = null;

        public LogParser(IOptions<tz.cleverence.ru.Options.RegexOptions> regex)
            {
             _regex = (Options.RegexOptions)regex.Value;

             Format1Regex = new(_regex.Format1Regex, System.Text.RegularExpressions.RegexOptions.Compiled);

             Format2Regex = new(_regex.Format2Regex, System.Text.RegularExpressions.RegexOptions.Compiled);
        }

        public async Task<OutputFormatDto?> ParseAsync(string line, CancellationToken cancellationToken)
        {
            // Пробуем Формат 2 (более специфичный — сначала его)
            var match = Format2Regex.Match(line);
            if (match.Success)
            {
                return BuildDto(match, "yyyy-MM-dd HH:mm:ss.ffff");
            }

            // Пробуем Формат 1
            match = Format1Regex.Match(line);
            if (match.Success)
            {
                return BuildDto(match, "dd.MM.yyyy HH:mm:ss.fff");
            }

            return null; 
        }

        private static string NormalizeLevel(string level) => level.ToUpperInvariant() switch
        {
            "INFORMATION" => "INFO",
            "WARNING" => "WARN",
            _ => level.ToUpperInvariant()
        };

        public OutputFormatDto BuildDto(Match match, string dateTimeFormat)
        {
            var date = match.Groups["Date"].Value;
            var time = match.Groups["Time"].Value;
            var timestamp = DateTime.ParseExact(
                $"{date} {time}",
                dateTimeFormat,
                CultureInfo.InvariantCulture);

            var methodGroup = match.Groups["Method"];
            var method = methodGroup.Success && !string.IsNullOrWhiteSpace(methodGroup.Value)
                ? methodGroup.Value.Trim()
                : "DEFAULT";

            return new OutputFormatDto
            {
                Timestamp = timestamp,
                Level = NormalizeLevel(match.Groups["Level"].Value),  //match.Groups["Level"].Value.ToUpperInvariant(),
                Method = method,
                Message = match.Groups["Message"].Value.Trim()
            };
        }
    }
}
