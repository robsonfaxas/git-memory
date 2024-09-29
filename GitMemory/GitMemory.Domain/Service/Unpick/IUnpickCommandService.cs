using GitMemory.Domain.Entities;

namespace GitMemory.Domain.Service.Unpick
{
    public interface IUnpickCommandService
    {
        Task<CommandResponse> ExecuteCommand(List<string> commands);
    }
}
