using GitMemory.CultureConfig;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Service;
using LibGit2Sharp;
using GitMemory.Domain.Service.Unstage;
namespace GitMemory.Infrastructure.CommandsServices.Unstage
{
    public class UnstageCommandService : IUnstageCommandService
    {
        private readonly IMemoryPoolService _memoryPoolService;

        public UnstageCommandService(IMemoryPoolService memoryPoolService)
        {
            this._memoryPoolService=memoryPoolService;
        }

        public Task<Command> ExecuteCommand(List<string> commands)
        {
            try
            {
                if (commands == null || commands.Count == 0)
                    return Task.FromResult(new Command(ResourceMessages.Services_Unstage_MissingArguments, ResponseTypeEnum.Error));
                var memoryPool = _memoryPoolService.ReadMemoryPool();
                string repoPath = Repository.Discover(CommandContextAccessor.Current.CurrentDirectory);
                int totalUnstaged = 0;
                foreach (var repository in memoryPool.GitRepositories.Where(p => p.GitRepositoryPath.StartsWith(repoPath, StringComparison.OrdinalIgnoreCase)))
                {
                    foreach(var command in commands)
                    {
                        if (command.Equals(".") || command.Equals("--all", StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (var staged in repository.Staged.ToList())
                            {
                                if (repository.Staged.Select(p => p.CommitHash.ToLower()).Contains(staged.CommitHash.ToLower()))
                                {
                                    repository.Unstaged.Add(staged);
                                    repository.Staged.Remove(staged);
                                    CommandContextAccessor.Current.InteractionWindow
                                        .Write(new Command(string.Format(ResourceMessages.Services_Unstage_UnstagedCommit, staged.CommitHash)));
                                    totalUnstaged++;
                                }
                            }
                            break;
                        }
                        else if (repository.Staged.Select(p => p.CommitHash.ToLower()).Contains(command.ToLower()))
                        {
                            var commit = repository.Staged.FirstOrDefault(p => p.CommitHash.Equals(command, StringComparison.OrdinalIgnoreCase));
                            if (commit is not null)
                            {
                                repository.Unstaged.Add(commit);
                                repository.Staged.Remove(commit);
                                CommandContextAccessor.Current.InteractionWindow
                                    .Write(new Command(string.Format(ResourceMessages.Services_Unstage_UnstagedCommit, command)));
                                totalUnstaged++;
                            }                               
                        }
                        else if (!repository.Unstaged.Select(p => p.CommitHash.ToLower()).Contains(command.ToLower()))
                        {
                            CommandContextAccessor.Current.InteractionWindow
                                .Write(new Command(string.Format(ResourceMessages.Services_Unstage_InvalidHash, command), ResponseTypeEnum.Error));
                        }
                    }
                }
                _memoryPoolService.WriteMemoryPool(memoryPool);
                if (totalUnstaged > 0)
                {
                    return Task.FromResult(new Command(ResourceMessages.Services_Unstage_Success));
                }
                return Task.FromResult(new Command(ResourceMessages.Services_Unstage_SuccessZeroCommits));
            }
            catch (Exception ex)
            {
                return Task.FromResult(new Command(ex.Message, ResponseTypeEnum.Error));
            }
        }
    }

}
