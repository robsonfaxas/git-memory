using GitMemory.Domain.Entities;

namespace GitMemory.Domain.Service.Pick
{
    public interface IPickCommandService
    {
        Task<Command> ExecuteCommand(List<string> commands, bool clear);
    }
}
