using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.UI;

namespace GitMemory.ConsoleApp.IntegrationTests.Configuration
{
    public class InteractionWindowTest : IInteractionWindow
    {
        public string Title { get; set; } = string.Empty;

        public string Read()
        {
            if (Interactions.StringRequest.Any())
                return Interactions.StringRequest.Dequeue();
            else
                throw new Exception("No inputs were configured for this test. Please, in \"Arrange\" section of this xunit test, add the expected user inputs for this test.");
        }

        public DialogResultEnum Read(DialogButtonsEnum buttons, Command command)
        {
            if (Interactions.DialogResultRequest.Any())
            {
                Interactions.Output.Enqueue(command);
                return Interactions.DialogResultRequest.Dequeue();
            }
            else
                throw new Exception("No inputs were configured for this test. Please, in \"Arrange\" section of this xunit test, add the expected dialog user inputs for this test.");            
        }

        public void Write(Command command)
        {
            Interactions.Output.Enqueue(command);
        }
    }
}
