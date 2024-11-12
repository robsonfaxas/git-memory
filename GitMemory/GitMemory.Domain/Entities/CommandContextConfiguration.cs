using GitMemory.Domain.UI;

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
