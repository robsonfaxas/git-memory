using GitMemory.Domain.Entities;

namespace GitMemory.Domain.Service.SetBrain
{
    public interface ISetBrainCommandService
    {
        Task<Command> ExecuteCommand(List<string> commands);
    }
}
