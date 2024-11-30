using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Service.Pick;
using GitMemory.Infrastructure.CommandsServices.Pick.PickStrategy;
using GitMemory.Domain.Entities.Memories;
using GitMemory.Domain.Service;
using GitMemory.CultureConfig;
namespace GitMemory.Infrastructure.CommandsServices.Pick
{
    public class PickCommandService : IPickCommandService
    {
        private readonly IMemoryPoolService _memoryPoolService;
        private readonly IErrorLogService _errorLogService;
        private IPickStrategy _pickStrategy = null!;
        public PickCommandService(IMemoryPoolService memoryPoolService, IErrorLogService errorLogService)
        {
            _memoryPoolService = memoryPoolService;
            _errorLogService = errorLogService;
        }

        public Task<Command> ExecuteCommand(List<string> commands, bool clearPoolList)
        {
            try
            {
                if (commands == null || commands.Count == 0)
                    return Task.FromResult(new Command(ResourceMessages.Services_Pick_MissingArgument, ResponseTypeEnum.Error));
                var memoryPool = _memoryPoolService.ReadMemoryPool() ?? new MemoryPool();
                if (clearPoolList)
                    memoryPool = ClearPoolList(memoryPool);
                var isInteger = int.TryParse(commands.FirstOrDefault(), out int result);
                _pickStrategy = isInteger ? new PickByNumber(_memoryPoolService, _errorLogService) : new PickByList(_memoryPoolService, _errorLogService);
                return _pickStrategy.Execute(commands, memoryPool);
            }
            catch (Exception ex)
            {
                return Task.FromResult(new Command(ex.Message, ResponseTypeEnum.Error));
            }
        }

        private MemoryPool ClearPoolList(MemoryPool memoryPool)
        {
            var currentDirectory = CommandContextAccessor.Current.CurrentDirectory;
            foreach (var repository in memoryPool.GitRepositories.Where(p => p.GitRepositoryPath.ToLower().StartsWith(currentDirectory.ToLower())))
            {
                repository.Staged.Clear();
                repository.Unstaged.Clear();
            }
            return memoryPool;
        }
    }
}
