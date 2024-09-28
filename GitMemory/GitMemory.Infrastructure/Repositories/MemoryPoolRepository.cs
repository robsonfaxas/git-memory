using GitMemory.Domain.Entities.Memories;
using GitMemory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
                    throw new FileNotFoundException($"Json configuration file not found at {_filePath}");
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
                throw new Exception("Unable to read git-memory.json");
            }
        }

        public void WriteMemoryPool(MemoryPool memoryPool)
        {
            try
            {
                if (memoryPool == null)
                {
                    throw new ArgumentNullException(nameof(memoryPool), "Memory pool cannot be null");
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
                throw new Exception("Unable to read git-memory.json");
            }
        }
    }
}
