using GitMemory.Domain.Entities;

namespace GitMemory.Domain.Service.Stage
{
    public interface IStageCommandService
    {
        Task<Command> ExecuteCommand(List<string> commands);
    }
}
