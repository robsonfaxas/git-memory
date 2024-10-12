using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Entities.Memories;
using GitMemory.Domain.Repositories;
using LibGit2Sharp;

namespace GitMemory.Infrastructure.CommandsServices.Unpick.UnpickStrategy
{
    internal class UnpickAll : IUnpickStrategy
    {
        private readonly IMemoryPoolRepository _memoryPoolRepository;
        private readonly IErrorLogRepository _errorLogRepository;
        public UnpickAll(IMemoryPoolRepository memoryPoolRepository, IErrorLogRepository errorLogRepository)
        {
            _memoryPoolRepository = memoryPoolRepository;
            _errorLogRepository = errorLogRepository;
        }
        
        public Task<CommandResponse> Execute(List<string> arguments, MemoryPool memoryPool)
        {
            try
            {
                var currentDirectory = CommandContextAccessor.Current.CurrentDirectory;
                string repoPath = Repository.Discover(CommandContextAccessor.Current.CurrentDirectory);

                if (string.IsNullOrEmpty(repoPath))
                    throw new InvalidOperationException("No Git repository found in the current directory.");

                foreach (var repository in memoryPool.GitRepositories.Where(p => p.GitRepositoryPath.ToLower().StartsWith(repoPath.ToLower())))
                {
                    repository.Staged.Clear();
                    repository.Unstaged.Clear();
                }                
                _memoryPoolRepository.WriteMemoryPool(memoryPool);
                return Task.FromResult(new CommandResponse("All commits unpicked.", ResponseTypeEnum.Info));
            }
            catch (ArgumentException ex)
            {
                return Task.FromResult(new CommandResponse(ex.Message, ResponseTypeEnum.Error));
            }
            catch (Exception ex)
            {
                _errorLogRepository.Log(ex);
                return Task.FromResult(new CommandResponse("An error occurred while attempting to unpick commits. Please refer to the error log for more details.", ResponseTypeEnum.Error));
            }
                
        }
    }
}
