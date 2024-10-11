using GitMemory.Domain.Entities;
using MediatR;

namespace GitMemory.Application.Interfaces
{
    public interface IGitCommandRequest : IRequest<CommandResponse>
    {
        List<string> Parameters { get; set; }                
    }
}
