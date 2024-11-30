using GitMemory.CultureConfig;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Service;
using GitMemory.Domain.Service.Status;
using LibGit2Sharp;

namespace GitMemory.Infrastructure.CommandsServices.Status
{
    public class StatusCommandService : IStatusCommandService
    {
        private readonly IMemoryPoolService _memoryPoolService;

        public StatusCommandService(IMemoryPoolService memoryPoolService)
        {
            this._memoryPoolService=memoryPoolService;
        }

        public Task<Command> ExecuteCommand(List<string> commands)
        {
            var memoryPool = _memoryPoolService.ReadMemoryPool();
            string repoPath = Repository.Discover(CommandContextAccessor.Current.CurrentDirectory);            
            foreach (var repository in memoryPool.GitRepositories.Where(p => p.GitRepositoryPath.StartsWith(repoPath, StringComparison.OrdinalIgnoreCase)))
            {
                if (repository.Staged.Count == 0 && repository.Unstaged.Count == 0)
                    return Task.FromResult(new Command(ResourceMessages.Services_Status_NoCommits));

                if (repository.Staged.Count > 0)
                {
                    CommandContextAccessor.Current.InteractionWindow.Write(
                        new Command(ResourceMessages.Services_Status_StagedHeader,
                                    Domain.Entities.Enums.ResponseTypeEnum.Info,
                                    ConsoleColor.Green));
                    foreach (var staged in repository.Staged)
                    {
                        CommandContextAccessor.Current.InteractionWindow.Write(
                        new Command(string.Format(ResourceMessages.Services_Status_StagedItem, staged.CommitDescription, staged.CommitHash),
                                    Domain.Entities.Enums.ResponseTypeEnum.Info,
                                    ConsoleColor.Green));
                    }
                }

                if (repository.Unstaged.Count > 0)
                {
                    CommandContextAccessor.Current.InteractionWindow.Write(
                        new Command(ResourceMessages.Services_Status_UnstagedHeader,
                                    Domain.Entities.Enums.ResponseTypeEnum.Info,
                                    ConsoleColor.Red));
                    foreach (var unstaged in repository.Unstaged)
                    {
                        CommandContextAccessor.Current.InteractionWindow.Write(
                        new Command(string.Format(ResourceMessages.Services_Status_UnstagedItem, unstaged.CommitDescription, unstaged.CommitHash),
                                    Domain.Entities.Enums.ResponseTypeEnum.Info,
                                    ConsoleColor.Red));
                    }
                }
            }
            return Task.FromResult(new Command(ResourceMessages.Services_Status_End));
        }
    }
}
