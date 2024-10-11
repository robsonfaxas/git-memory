using GitMemory.Application.Interfaces;

namespace GitMemory.Application.Commands
{
    public class SetRepoCommand : IGitCommandRequest
    {
        public List<string> Parameters { get; set; } = new List<string>();

        public SetRepoCommand Initialize(List<string> commands)
        {
            Parameters = commands;            
            return this;
        }
    }
}
