using GitMemory.Domain.Entities.Enums;

namespace GitMemory.Domain.Entities
{
    public class Command
    {
        public string Message { get; set; }
        public ResponseTypeEnum ResponseType { get; set; }
        public ConsoleColor ResponseColor { get; set; }

        public Command(string message, ResponseTypeEnum responseType = ResponseTypeEnum.Info, ConsoleColor color = ConsoleColor.White)
        {
            Message = message;            
            ResponseType = responseType;
            ResponseColor = color;
        }
    }
}
