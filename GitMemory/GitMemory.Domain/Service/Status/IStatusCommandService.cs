using GitMemory.Domain.Entities;

namespace GitMemory.Domain.Service.Status
{
    public interface IStatusCommandService
    {
        Task<Command> ExecuteCommand(List<string> commands);
    }
}
