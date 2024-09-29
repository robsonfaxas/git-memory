using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Memories;

namespace GitMemory.Infrastructure.CommandsServices.Unpick.UnpickStrategy
{
    internal interface IUnpickStrategy
    {
        Task<CommandResponse> Execute(List<string> arguments, MemoryPool memoryPool);
    }
}
