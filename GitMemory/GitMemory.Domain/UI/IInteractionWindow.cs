using GitMemory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.Domain.UI
{
    public interface IInteractionWindow
    {
        void Write(CommandResponse command);
        string Read();
    }
}
