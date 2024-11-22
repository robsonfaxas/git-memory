using GitMemory.Domain.Entities;

namespace GitMemory.Domain.Service.Unstage
{
    public interface IUnstageCommandService
    {
        Task<CommandResponse> ExecuteCommand(List<string> commands);
    }
}