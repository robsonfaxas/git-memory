using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.Domain.UI
{
    public interface ICommandUI
    {
        Task Run();
    }
}
