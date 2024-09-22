using GitMemory.Domain.Entities;
using GitMemory.Domain.UI;
using MediatR;

namespace GitMemory.Application.Interfaces
{
    public interface IGitCommandRequest : IRequest<CommandResponse>
    {
        List<string> Parameters { get; set; }
        IInteractionWindow InteractionWindow { get; set; }
    }
}
