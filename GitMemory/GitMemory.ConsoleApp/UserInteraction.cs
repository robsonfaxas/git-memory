using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.UI;
using MediatR;

namespace GitMemory.ConsoleApp
{
    public class UserInteraction : IInteractionWindow
    {
        public string Title { get; set; } = string.Empty;

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

        public DialogResultEnum Read(DialogButtonsEnum buttons, CommandResponse command)
        {
            Func<CommandResponse, DialogResultEnum> Answer = buttons switch
            {
                DialogButtonsEnum.Ok => OkControl,
                DialogButtonsEnum.OkCancel => OkCancelControl,
                DialogButtonsEnum.YesNoCancel => YesNoCancelControl,
                DialogButtonsEnum.YesNo => YesNoControl,
                _ => throw new NotImplementedException()
            };
            return Answer(command);

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

        private DialogResultEnum OkCancelControl(CommandResponse response)
        {
            if (response is not null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message += " Type 'Y' (yes) to proceed or any other key to cancel";
                Write(response);
            }
            string? answer;

            answer = Read();
            if (answer != null)
                if (answer.ToUpper().Equals("Y"))
                    return DialogResultEnum.Ok;
            return DialogResultEnum.Cancel;
        }

        private DialogResultEnum YesNoControl(CommandResponse response)
        {
            if (response is not null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message += "Answer 'Y' (yes) or 'N' (no) to proceed.";
                Write(response);
            }
            string? answer;
            do
            {
                answer = Read();
                if (answer != null)
                    if (answer.ToUpper().Equals("Y"))
                        return DialogResultEnum.Yes;
                    else if (answer.ToUpper().Equals("N"))
                        return DialogResultEnum.No;
                    else
                        Write(new CommandResponse($"Invalid Command. Answer 'Y' (yes) or 'N' (no) to proceed.", ResponseTypeEnum.Info));

            } while (answer == null || !(answer != null && (answer.ToUpper().Equals("Y") || answer.ToUpper().Equals("N"))));

            return DialogResultEnum.No;
        }

        private DialogResultEnum YesNoCancelControl(CommandResponse response)
        {
            if (response is not null && !string.IsNullOrEmpty(response.Message))
            {
                response.Message += "Answer 'Y' (yes), 'N' (no) to proceed or any other key to Cancel.";
                Write(response);
            }
            var answer = Read();
            if (answer != null)
                if (answer.ToUpper().Equals("Y"))
                    return DialogResultEnum.Yes;
                else if (answer.ToUpper().Equals("N"))
                    return DialogResultEnum.No;
            return DialogResultEnum.Cancel;
        }

        private DialogResultEnum OkControl(CommandResponse response)
        {
            if (response is not null && !string.IsNullOrEmpty(response.Message))
                Write(response);
            return DialogResultEnum.Ok;
        }

    }
}
