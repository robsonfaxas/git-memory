using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Memories;

namespace GitMemory.Infrastructure.CommandsServices.Pick.PickStrategy
{
    internal interface IPickStrategy
    {
        Task<Command> Execute(List<string> arguments, MemoryPool memoryPool);
    }
}
