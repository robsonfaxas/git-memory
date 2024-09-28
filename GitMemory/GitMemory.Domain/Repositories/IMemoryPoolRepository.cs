using GitMemory.Domain.Entities.Memories;

namespace GitMemory.Domain.Repositories
{
    public interface IMemoryPoolRepository
    {
        MemoryPool? ReadMemoryPool();
        void WriteMemoryPool(MemoryPool memoryPool);
        string FileName { get; set; }
    }
}
