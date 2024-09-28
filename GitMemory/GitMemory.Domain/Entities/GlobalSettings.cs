namespace GitMemory.Domain.Entities
{
    public class GlobalSettings
    {
        public string RepositoryLocation { get; set; } = String.Empty;
        public string ConfigurationFileLocation { get; set; } = String.Empty;
        public bool IsErrorLogsEnabled { get; set; }
    }
}
