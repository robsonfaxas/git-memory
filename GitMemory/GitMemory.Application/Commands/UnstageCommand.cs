using GitMemory.Application.Interfaces;

namespace GitMemory.Application.Commands
{
    public class UnstageCommand : IGitCommandRequest
    {
        public List<string> Parameters { get; set; } = new List<string>();

        public UnstageCommand Initialize(List<string> commands)
        {
            Parameters = commands;            
            return this;
        }
    }
}
