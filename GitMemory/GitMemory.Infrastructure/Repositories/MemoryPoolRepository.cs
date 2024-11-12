using GitMemory.CultureConfig;
using GitMemory.Domain.Entities.Memories;
using GitMemory.Domain.Repositories;
using System.Text.Json;

namespace GitMemory.Infrastructure.Repositories
{
    public class MemoryPoolRepository : IMemoryPoolRepository
    {
        private readonly string _filePath;
        private readonly IErrorLogRepository _errorLogRepository;
        public string FileName { get; set; } = string.Empty;
        public MemoryPoolRepository(IGitMemoryGlobalSettings globalSettings, IErrorLogRepository errorLogRepository)
        {
            FileName = String.IsNullOrEmpty(FileName)? "git-memory.json" : FileName;
            _filePath = $"{globalSettings.ReadGlobalSettings().RepositoryLocation}\\.gitmemory\\{FileName}";
            _errorLogRepository = errorLogRepository;
        }

        public MemoryPool? ReadMemoryPool()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    throw new FileNotFoundException(string.Format(ResourceMessages.Repository_MemoryPool_Read_JsonNotFound, _filePath));
                }

                string jsonContent = File.ReadAllText(_filePath);
                if (jsonContent == null || string.IsNullOrEmpty(jsonContent))
                    return null;
                else 
                    return JsonSerializer.Deserialize<MemoryPool>(jsonContent);
            }
            catch (Exception ex)
            {
                _errorLogRepository.Log(ex);
                throw new Exception(ResourceMessages.Repository_MemoryPool_Read_UnhandledException);
            }
        }

        public void WriteMemoryPool(MemoryPool memoryPool)
        {
            try
            {
                if (memoryPool == null)
                {
                    throw new ArgumentNullException(nameof(memoryPool), ResourceMessages.Repository_MemoryPool_Write_NullException);
                }

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string jsonContent = JsonSerializer.Serialize(memoryPool, options);
                File.WriteAllText(_filePath, jsonContent);
            }
            catch (Exception ex)
            {
                _errorLogRepository.Log(ex);
                throw new Exception(ResourceMessages.Repository_MemoryPool_Write_UnhandledException);
            }
        }
    }
}
