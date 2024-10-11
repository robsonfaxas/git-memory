using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.Domain.Entities
{
    public static class CommandContextAccessor
    {
        private static AsyncLocal<CommandContextConfiguration> _context = new AsyncLocal<CommandContextConfiguration>();
        
        public static CommandContextConfiguration Current
        {
            get => _context.Value ?? throw new InvalidOperationException("CommandContextConfiguration is not set.");
            set => _context.Value = value;
        }
    }

}
