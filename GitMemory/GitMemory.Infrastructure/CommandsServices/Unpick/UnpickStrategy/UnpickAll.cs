using GitMemory.CultureConfig;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Entities.Memories;
using GitMemory.Domain.Service;
using LibGit2Sharp;

namespace GitMemory.Infrastructure.CommandsServices.Unpick.UnpickStrategy
{
    internal class UnpickAll : IUnpickStrategy
    {
        private readonly IMemoryPoolService _memoryPoolService;
        private readonly IErrorLogService _errorLogService;
        public UnpickAll(IMemoryPoolService memoryPoolService, IErrorLogService errorLogService)
        {
            _memoryPoolService = memoryPoolService;
            _errorLogService = errorLogService;
        }
        
        public Task<Command> Execute(List<string> arguments, MemoryPool memoryPool)
        {
            try
            {
                var currentDirectory = CommandContextAccessor.Current.CurrentDirectory;
                string repoPath = Repository.Discover(CommandContextAccessor.Current.CurrentDirectory);

                if (string.IsNullOrEmpty(repoPath))
                    throw new InvalidOperationException(ResourceMessages.Services_UnpickAll_GitRepositoryNotFound);

                foreach (var repository in memoryPool.GitRepositories.Where(p => p.GitRepositoryPath.ToLower().StartsWith(repoPath.ToLower())))
                {
                    repository.Staged.Clear();
                    repository.Unstaged.Clear();
                }
                _memoryPoolService.WriteMemoryPool(memoryPool);
                return Task.FromResult(new Command(ResourceMessages.Services_UnpickAll_Success, ResponseTypeEnum.Info));
            }
            catch (ArgumentException ex)
            {
                return Task.FromResult(new Command(ex.Message, ResponseTypeEnum.Error));
            }
            catch (Exception ex)
            {
                _errorLogService.Log(ex);
                return Task.FromResult(new Command(ResourceMessages.Services_UnpickAll_UnhandledException, ResponseTypeEnum.Error));
            }
                
        }
    }
}
