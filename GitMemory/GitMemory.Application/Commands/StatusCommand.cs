using GitMemory.Application.Interfaces;

namespace GitMemory.Application.Commands
{
    public class StatusCommand : IGitCommandRequest
    {
        public List<string> Parameters { get; set; } = new List<string>();

        public StatusCommand Initialize(List<string> commands)
        {
            Parameters = commands;            
            return this;
        }
    }
}
