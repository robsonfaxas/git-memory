using GitMemory.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.Infrastructure.Services
{
    public class CommandService : ICommandService
    {
        public Task ExecuteCommand(string command)
        {
            return Task.CompletedTask;
        }
    }
}
