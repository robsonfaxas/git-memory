using GitMemory.Application.Interfaces;
using GitMemory.Domain.UI;

namespace GitMemory.Application.Commands
{
    public class UnpickCommand : IGitCommandRequest
    {
        public List<string> Parameters { get; set; }
        public IInteractionWindow InteractionWindow { get; set; }

        public UnpickCommand(List<string> parameters, IInteractionWindow logger)
        {
            Parameters = parameters;
            InteractionWindow = logger;
        }
    }
}
