using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.UI;

namespace GitMemory.ConsoleApp
{
    public class LogHelper : IInteractionWindow
    {
        public void Write(CommandResponse command)
        {
            switch (command.ResponseType)
            {
                case ResponseTypeEnum.Info:
                    LogSuccess(command.Message);
                    break;
                case ResponseTypeEnum.Warning:
                    LogWarning(command.Message);
                    break;
                case ResponseTypeEnum.Error:
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

        public string Read()
        {
            return Console.ReadLine() ?? "";
        }
    }
}
