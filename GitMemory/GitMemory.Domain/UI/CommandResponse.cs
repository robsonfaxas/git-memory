namespace GitMemory.Domain.UI
{
    public class CommandResponse
    {
        public string Message { get; set; }
        public ResponseType ResponseType { get; set; }
        public CommandResponse(string message, ResponseType responseType = ResponseType.Info)
        {
            this.Message = message;
            this.ResponseType = responseType;
        }        
    }
}
