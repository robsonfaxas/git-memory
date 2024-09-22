using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.Domain.Entities.Memories
{
    public class MemoryPool
    {
        public List<GitRepository> GitRepositories { get; set; } = new List<GitRepository>();
    }
}
