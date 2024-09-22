using GitMemory.Domain.Entities;

namespace GitMemory.Domain.Service.Pick
{
    public interface IPickCommandService
    {
        Task<CommandResponse> ExecuteCommand(List<string> commands, bool clear);
    }
}
