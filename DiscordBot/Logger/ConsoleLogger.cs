namespace Discord.Logger
{
    internal class ConsoleLogger
    {
        public static Task LogAsync(string systemName, string message, LogSeverityLevels logSeverity)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = logSeverity switch
            {
                LogSeverityLevels.INFO => ConsoleColor.DarkGreen,
                LogSeverityLevels.OK => ConsoleColor.Green,
                LogSeverityLevels.FATAL => ConsoleColor.Red,
                LogSeverityLevels.ERROR => ConsoleColor.DarkRed,
                LogSeverityLevels.WARNING => ConsoleColor.Yellow,
                LogSeverityLevels.LOADING => ConsoleColor.Magenta,
                LogSeverityLevels.DEBUG => ConsoleColor.DarkRed,
                _ => ConsoleColor.Gray
            };
            
            var timestamp = DateTime.Now.ToString("D: dd-MM-yyyy T: HH:mm:ss");
            Console.WriteLine($"[{timestamp}] ({logSeverity}) [{systemName}] {message}");
            
            Console.ForegroundColor = originalColor;

            return Task.CompletedTask;
        }

        public enum LogSeverityLevels
        {
            WARNING,
            ERROR,
            FATAL,
            OK,
            INFO,
            LOADING,
            DEBUG
        }
    }
}