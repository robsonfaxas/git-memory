using GitMemory.Domain.Entities;

namespace GitMemory.Domain.Service.SetRepo
{
    public interface ISetRepoCommandService
    {
        Task<Command> ExecuteCommand(List<string> commands);
    }
}
