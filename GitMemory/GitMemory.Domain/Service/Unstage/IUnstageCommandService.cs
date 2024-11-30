using GitMemory.Domain.Entities;

namespace GitMemory.Domain.Service.Unstage
{
    public interface IUnstageCommandService
    {
        Task<Command> ExecuteCommand(List<string> commands);
    }
}