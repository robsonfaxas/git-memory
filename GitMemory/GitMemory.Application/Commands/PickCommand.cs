using GitMemory.Application.Interfaces;

namespace GitMemory.Application.Commands
{
    public class PickCommand : IGitCommandRequest
    {
        public List<string> Parameters { get; set; } = new List<string>();

        public PickCommand Initialize(List<string> commands)
        {
            Parameters = commands;            
            return this;
        }
    }
}
