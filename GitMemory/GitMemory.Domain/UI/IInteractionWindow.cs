using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;

namespace GitMemory.Domain.UI
{
    public interface IInteractionWindow
    {
        string Title { get; set; }
        void Write(Command command);
        string Read();
        DialogResultEnum Read(DialogButtonsEnum buttons, Command command);
    }
}
