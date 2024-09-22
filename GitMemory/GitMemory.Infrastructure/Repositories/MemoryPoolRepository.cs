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
        public MemoryPoolRepository(IGitMemoryGlobalSettings globalSettings)
        {
            _filePath = globalSettings.ReadGlobalSettings().RepositoryLocation;
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
                return JsonSerializer.Deserialize<MemoryPool>(jsonContent);
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
                throw new Exception("Unable to read git-memory.json");
            }
        }
    }
}
