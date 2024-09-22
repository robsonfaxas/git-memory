using GitMemory.Domain.Entities.Memories;
using GitMemory.Domain.Repositories;
using GitMemory.Domain.Service;

namespace GitMemory.Infrastructure.CommandsServices
{
    public class MemoryPoolService : IMemoryPoolService
    {
        private readonly IMemoryPoolRepository _memoryPoolRepository;
        public MemoryPoolService(IMemoryPoolRepository repository)
        {
            _memoryPoolRepository = repository;
        }

        public MemoryPool ReadMemoryPool()
        {
            return _memoryPoolRepository.ReadMemoryPool() ?? new MemoryPool();
        }

        public void WriteMemoryPool(MemoryPool memoryPool)
        {
            _memoryPoolRepository.WriteMemoryPool(memoryPool);
        }
    }
}
