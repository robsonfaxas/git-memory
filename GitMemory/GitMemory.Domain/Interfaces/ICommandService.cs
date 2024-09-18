using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.Domain.Interfaces
{
    public interface ICommandService
    {
        Task ExecuteCommand(string command);
    }
}
