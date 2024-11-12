using GitMemory.CultureConfig;

namespace GitMemory.Domain.Entities
{
    public static class CommandContextAccessor
    {
        private static AsyncLocal<CommandContextConfiguration> _context = new AsyncLocal<CommandContextConfiguration>();
        
        public static CommandContextConfiguration Current
        {
            get => _context.Value ?? throw new InvalidOperationException(ResourceMessages.CommandContextAccessor_Configuration_NotSet);
            set => _context.Value = value;
        }
    }

}
