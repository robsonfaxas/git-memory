using GitMemory.Domain.Entities;

namespace GitMemory.Domain.Service.SetRepo
{
    public interface ISetRepoCommandService
    {
        Task<CommandResponse> ExecuteCommand(List<string> commands);
    }
}
