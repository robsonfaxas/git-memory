using GitMemory.Application.Interfaces;

namespace GitMemory.Application.Commands
{
    public class SetRepoCommand : IGitCommandRequest
    {
        public List<string> Commands { get; set; }
        
        public SetRepoCommand(List<string> commands)
        {
            Commands = commands;
        }
    }
}
