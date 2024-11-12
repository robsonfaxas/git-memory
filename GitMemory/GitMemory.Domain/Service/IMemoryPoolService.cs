using GitMemory.Domain.Entities.Memories;

namespace GitMemory.Domain.Service
{
    public interface IMemoryPoolService
    {
        MemoryPool ReadMemoryPool();
        void WriteMemoryPool(MemoryPool memoryPool);
    }
}
