namespace GitMemory.Domain.Entities.Memories
{
    public class MemoryPool
    {
        public List<GitRepository> GitRepositories { get; set; } = new List<GitRepository>();
    }
}
