using GitMemory.Domain.Entities;
using MediatR;

namespace GitMemory.Application.Interfaces
{
    public interface IGitCommandRequest : IRequest<Command>
    {
        List<string> Parameters { get; set; }                
    }
}
