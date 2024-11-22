using GitMemory.Application.Interfaces;

namespace GitMemory.Application.Commands
{
    public class StageCommand : IGitCommandRequest
    {
        public List<string> Parameters { get; set; } = new List<string>();

        public StageCommand Initialize(List<string> commands)
        {
            Parameters = commands;            
            return this;
        }
    }
}
