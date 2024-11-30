using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Service.Unpick;
using GitMemory.Infrastructure.CommandsServices.Unpick.UnpickStrategy;
using GitMemory.Domain.Entities.Memories;
using GitMemory.Domain.Service;
using GitMemory.CultureConfig;
namespace GitMemory.Infrastructure.CommandsServices.Unpick
{
    public class UnpickCommandService : IUnpickCommandService
    {
        private readonly IMemoryPoolService _memoryPoolService;
        private readonly IErrorLogService _errorLogService;
        private IUnpickStrategy _pickStrategy = null!;
        public UnpickCommandService(IMemoryPoolService memoryPoolService, IErrorLogService errorLogService)
        {
            _memoryPoolService = memoryPoolService;
            _errorLogService = errorLogService;
        }

        public Task<Command> ExecuteCommand(List<string> commands)
        {
            try
            {
                if (commands == null || commands.Count == 0)
                    return Task.FromResult(new Command(ResourceMessages.Services_Unpick_MissingArgument, ResponseTypeEnum.Error));
                var memoryPool = _memoryPoolService.ReadMemoryPool() ?? new MemoryPool();
                if (commands.FirstOrDefault()!.Equals(".") || commands.FirstOrDefault()!.ToLower().Equals("--all"))
                    _pickStrategy = new UnpickAll(_memoryPoolService, _errorLogService);
                else
                    _pickStrategy = new UnpickByList(_memoryPoolService, _errorLogService);                
                return _pickStrategy.Execute(commands, memoryPool);
            }
            catch (Exception ex)
            {
                return Task.FromResult(new Command(ex.Message, ResponseTypeEnum.Error));
            }
        }       
    }
}
