using GitMemory.Domain.Entities;

namespace GitMemory.Domain.Service
{
    public interface IPickCommandService
    {
        Task<CommandResponse> ExecuteCommand(List<string> commands, bool clear);
    }
}
