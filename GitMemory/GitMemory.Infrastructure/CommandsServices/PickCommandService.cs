using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Repositories;
using GitMemory.Domain.Service;

namespace GitMemory.Infrastructure.Services
{
    public class PickCommandService : IPickCommandService
    {
        private readonly IMemoryPoolRepository _memoryPoolRepository;        
        public PickCommandService(IMemoryPoolRepository memoryPoolRepository)
        {
            _memoryPoolRepository = memoryPoolRepository;
        }

        public Task<CommandResponse> ExecuteCommand(List<string> commands, bool clear)
        {
            try
            {
                if (commands == null || commands.Count == 0)
                    return Task.FromResult(new CommandResponse("No arguments provided.", ResponseTypeEnum.Error));
                var memoryPool = _memoryPoolRepository.ReadMemoryPool();
            }
            catch (Exception ex)
            {
                return Task.FromResult(new CommandResponse(ex.Message, ResponseTypeEnum.Error));                
            }
        }
    }
}
