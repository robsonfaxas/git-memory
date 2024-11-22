using GitMemory.Domain.Entities;

namespace GitMemory.Domain.Service.Stage
{
    public interface IStageCommandService
    {
        Task<CommandResponse> ExecuteCommand(List<string> commands);
    }
}
