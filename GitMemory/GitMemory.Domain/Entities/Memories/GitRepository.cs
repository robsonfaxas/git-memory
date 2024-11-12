namespace GitMemory.Domain.Entities.Memories
{
    public class GitRepository
    {
        public List<MemoryCommit> Staged { get; set; } = new List<MemoryCommit>();
        public List<MemoryCommit> Unstaged { get; set; } = new List<MemoryCommit>();
        public string GitRepositoryPath { get; set; } = string.Empty;
    }
}
