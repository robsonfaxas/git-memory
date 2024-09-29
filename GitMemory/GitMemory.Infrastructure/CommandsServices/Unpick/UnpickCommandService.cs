using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Repositories;
using GitMemory.Domain.Service.Unpick;
using GitMemory.Infrastructure.CommandsServices.Unpick.UnpickStrategy;
using GitMemory.Domain.Entities.Memories;
namespace GitMemory.Infrastructure.CommandsServices.Unpick
{
    public class UnpickCommandService : IUnpickCommandService
    {
        private readonly IMemoryPoolRepository _memoryPoolRepository;
        private readonly IErrorLogRepository _errorLogRepository;
        private IUnpickStrategy _pickStrategy = null!;
        public UnpickCommandService(IMemoryPoolRepository memoryPoolRepository, IErrorLogRepository errorLogRepository)
        {
            _memoryPoolRepository = memoryPoolRepository;
            _errorLogRepository = errorLogRepository;
        }

        public Task<CommandResponse> ExecuteCommand(List<string> commands)
        {
            try
            {
                if (commands == null || commands.Count == 0)
                    return Task.FromResult(new CommandResponse("No arguments provided.", ResponseTypeEnum.Error));
                var memoryPool = _memoryPoolRepository.ReadMemoryPool() ?? new MemoryPool();
                if (commands.FirstOrDefault()!.Equals(".") || commands.FirstOrDefault()!.ToLower().Equals("--all"))
                    _pickStrategy = new UnpickAll(_memoryPoolRepository, _errorLogRepository);
                else
                    _pickStrategy = new UnpickByList(_memoryPoolRepository, _errorLogRepository);                
                return _pickStrategy.Execute(commands, memoryPool);
            }
            catch (Exception ex)
            {
                return Task.FromResult(new CommandResponse(ex.Message, ResponseTypeEnum.Error));
            }
        }       
    }
}
