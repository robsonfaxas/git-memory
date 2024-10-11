using GitMemory.Application.Interfaces;

namespace GitMemory.Application.Commands
{
    public class ErrorLogCommand : IGitCommandRequest
    {
        public List<string> Parameters { get; set; } = new List<string>();                

        public ErrorLogCommand Initialize(List<string> commands)
        {
            Parameters = commands;            
            return this;
        }
    }
}
