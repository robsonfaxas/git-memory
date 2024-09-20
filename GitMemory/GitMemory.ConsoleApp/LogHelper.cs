using GitMemory.Domain.UI;

namespace GitMemory.ConsoleApp
{
    public static class LogHelper
    {
        public static void Log(CommandResponse command)
        {
            switch (command.ResponseType)
            {
                case ResponseType.Info:
                    LogSuccess(command.Message);
                    break;
                case ResponseType.Warning:
                    LogWarning(command.Message);
                    break;
                case ResponseType.Error:
                    LogError(command.Message);
                    break;
                default: 
                    return;
            }
        }
        // Log a normal success message
        private static void LogSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        // Log a warning in yellow
        private static void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        // Log an error in red
        private static void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(message);
            Console.ResetColor();
        }
    }
}
