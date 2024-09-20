using GitMemory.Domain.Entities.Enums;

namespace GitMemory.Domain.Entities
{
    public class CommandResponse
    {
        public string Message { get; set; }
        public ResponseTypeEnum ResponseType { get; set; }
        public CommandResponse(string message, ResponseTypeEnum responseType = ResponseTypeEnum.Info)
        {
            Message = message;
            ResponseType = responseType;
        }
    }
}
