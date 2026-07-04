
namespace tz.cleverence.ru.Services.LogConverter.Contracts
{
    public record OutputFormatDto
    {
        public DateTime Timestamp { get; set; }

        public string Level { get; set; }

        public string Method { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            /*return $"{Timestamp:yyyy-MM-dd}\n" +
                   $"{Timestamp:HH:mm:ss.fff} {Level} {Method}\n" +
                   $"{Message}";*/
            return $"{Timestamp:yyyy-MM-dd HH:mm:ss.fff}\t{Level}\t{Method}\t{Message}";
        }

    }
}
