using GitMemory.CultureConfig;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Entities.Memories;
using GitMemory.Domain.Service;
using LibGit2Sharp;

namespace GitMemory.Infrastructure.CommandsServices.Unpick.UnpickStrategy
{
    internal class UnpickByList : IUnpickStrategy
    {
        private readonly IMemoryPoolService _memoryPoolService;
        private readonly IErrorLogService _errorLogService;
        public UnpickByList(IMemoryPoolService memoryPoolRepository, IErrorLogService errorLogRepository)
        {
            _memoryPoolService = memoryPoolRepository;
            _errorLogService = errorLogRepository;
        }

        public Task<CommandResponse> Execute(List<string> hashes, MemoryPool memoryPool)
        {
            try
            {
                var currentDirectory = CommandContextAccessor.Current.CurrentDirectory;
                string repoPath = Repository.Discover(CommandContextAccessor.Current.CurrentDirectory);
                
                if (string.IsNullOrEmpty(repoPath))
                    throw new InvalidOperationException(ResourceMessages.Services_UnpickByList_GitRepositoryNotFound);

                foreach (var repository in memoryPool.GitRepositories.Where(p => p.GitRepositoryPath.ToLower().StartsWith(repoPath.ToLower())))
                    foreach(var hash in hashes)
                        RemoveIfExists(repository.Unstaged, repository.Staged, hash);
                
                _memoryPoolService.WriteMemoryPool(memoryPool);
                return Task.FromResult(new CommandResponse(ResourceMessages.Services_Unpick_Success, ResponseTypeEnum.Info));
            }
            catch (ArgumentException ex)
            {
                return Task.FromResult(new CommandResponse(ex.Message, ResponseTypeEnum.Error));
            }
            catch (Exception ex)
            {
                _errorLogService.Log(ex);
                return Task.FromResult(new CommandResponse(ResourceMessages.Services_UnpickByList_UnhandledException, ResponseTypeEnum.Error));
            }
                
        }

        private void RemoveIfExists(List<MemoryCommit> unstaged, List<MemoryCommit> staged, string hash)
        {
            staged.RemoveAll(commit => commit.CommitHash.Equals(hash));
            unstaged.RemoveAll(commit => commit.CommitHash.Equals(hash));
        }
    }
}
