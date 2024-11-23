using GitMemory.CultureConfig;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Service;
using LibGit2Sharp;
using GitMemory.Domain.Service.Stage;
namespace GitMemory.Infrastructure.CommandsServices.Stage
{
    public class StageCommandService : IStageCommandService
    {
        private readonly IMemoryPoolService _memoryPoolService;

        public StageCommandService(IMemoryPoolService memoryPoolService)
        {
            this._memoryPoolService=memoryPoolService;
        }

        public Task<CommandResponse> ExecuteCommand(List<string> commands)
        {
            try
            {
                if (commands == null || commands.Count == 0)
                    return Task.FromResult(new CommandResponse(ResourceMessages.Services_Stage_MissingArguments, ResponseTypeEnum.Error));
                var memoryPool = _memoryPoolService.ReadMemoryPool();
                string repoPath = Repository.Discover(CommandContextAccessor.Current.CurrentDirectory);
                int totalStaged = 0;
                foreach (var repository in memoryPool.GitRepositories.Where(p => p.GitRepositoryPath.StartsWith(repoPath, StringComparison.OrdinalIgnoreCase)))
                {
                    foreach(var command in commands)
                    {
                        if (command.Equals(".") || command.Equals("--all", StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (var unstaged in repository.Unstaged.ToList())
                            {
                                if (!repository.Staged.Select(p => p.CommitHash.ToLower()).Contains(unstaged.CommitHash.ToLower()))
                                {
                                    repository.Staged.Add(unstaged);
                                    repository.Unstaged.Remove(unstaged);
                                    CommandContextAccessor.Current.InteractionWindow
                                        .Write(new CommandResponse(string.Format(ResourceMessages.Services_Stage_StagedCommit, unstaged.CommitHash)));
                                    totalStaged++;
                                }
                            }
                            break;
                        }
                        else if (repository.Unstaged.Select(p => p.CommitHash.ToLower()).Contains(command.ToLower()))
                        {
                            var commit = repository.Unstaged.FirstOrDefault(p => p.CommitHash.Equals(command, StringComparison.OrdinalIgnoreCase));
                            if (commit is not null)
                            {
                                repository.Staged.Add(commit);
                                repository.Unstaged.Remove(commit);
                                CommandContextAccessor.Current.InteractionWindow
                                    .Write(new CommandResponse(string.Format(ResourceMessages.Services_Stage_StagedCommit, command)));
                                totalStaged++;
                            }                               
                        }
                        else if (!repository.Staged.Select(p => p.CommitHash.ToLower()).Contains(command.ToLower()))
                        {
                            CommandContextAccessor.Current.InteractionWindow
                                .Write(new CommandResponse(string.Format(ResourceMessages.Services_Stage_InvalidHash, command), ResponseTypeEnum.Error));
                        }
                    }
                }
                _memoryPoolService.WriteMemoryPool(memoryPool);
                if (totalStaged > 0)
                {
                    return Task.FromResult(new CommandResponse(ResourceMessages.Services_Stage_Success));
                }
                return Task.FromResult(new CommandResponse(ResourceMessages.Services_Stage_SuccessZeroCommits));
            }
            catch (Exception ex)
            {
                return Task.FromResult(new CommandResponse(ex.Message, ResponseTypeEnum.Error));
            }
        }
    }

}
