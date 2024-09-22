namespace GitMemory.Domain.Entities.Memories
{
    public class MemoryCommit
    {
        public string CommitHash { get; set; } = string.Empty;
        public DateTime CommitDate { get; set; }
    }
}