using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Repositories;
using GitMemory.Domain.Service.Pick;
using GitMemory.Infrastructure.CommandsServices.Pick.PickStrategy;
using GitMemory.Domain.Entities.Memories;
namespace GitMemory.Infrastructure.CommandsServices.Pick
{
    public class PickCommandService : IPickCommandService
    {
        private readonly IMemoryPoolRepository _memoryPoolRepository;
        private IPickStrategy _pickStrategy = null!;
        public PickCommandService(IMemoryPoolRepository memoryPoolRepository)
        {
            _memoryPoolRepository = memoryPoolRepository;
        }

        public Task<CommandResponse> ExecuteCommand(List<string> commands, bool clearPoolList)
        {
            try
            {
                if (commands == null || commands.Count == 0)
                    return Task.FromResult(new CommandResponse("No arguments provided.", ResponseTypeEnum.Error));
                var memoryPool = _memoryPoolRepository.ReadMemoryPool() ?? new MemoryPool();
                if (clearPoolList)
                    memoryPool = ClearPoolList(memoryPool);
                var isInteger = int.TryParse(commands.FirstOrDefault(), out int result);
                _pickStrategy = isInteger ? new PickByNumber(_memoryPoolRepository) : new PickByList(_memoryPoolRepository);
                return _pickStrategy.Execute(commands, memoryPool);
            }
            catch (Exception ex)
            {
                return Task.FromResult(new CommandResponse(ex.Message, ResponseTypeEnum.Error));
            }
        }

        private MemoryPool ClearPoolList(MemoryPool memoryPool)
        {
            var currentDirectory = Directory.GetCurrentDirectory() ?? "";
            foreach (var repository in memoryPool.GitRepositories.Where(p => p.GitRepositoryPath.ToLower().StartsWith(currentDirectory.ToLower())))
            {
                repository.Staged.Clear();
                repository.Unstaged.Clear();
            }
            return memoryPool;
        }
    }
}
