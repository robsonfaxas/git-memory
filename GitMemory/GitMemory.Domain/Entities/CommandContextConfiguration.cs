using GitMemory.Domain.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.Domain.Entities
{
    public class CommandContextConfiguration
    {
        public CommandContextConfiguration(IInteractionWindow interactionWindow)
        {
            InteractionWindow = interactionWindow;
        }

        public string CurrentDirectory { get; set; } = string.Empty;
        public string GlobalSettingsDirectory { get; set; } = string.Empty;
        public IInteractionWindow InteractionWindow { get; set; }
    }
}
