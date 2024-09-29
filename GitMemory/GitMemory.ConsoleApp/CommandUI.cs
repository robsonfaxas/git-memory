using GitMemory.Application.Commands;
using GitMemory.Application.Interfaces;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.UI;
using MediatR;

namespace GitMemory.ConsoleApp
{
    public class CommandUI : ICommandUI
    {
        private readonly IMediator _mediator;
        private readonly IInteractionWindow _interactionWindow;
        public List<string> Args { get; set; } = new List<string>();
        public CommandUI(IMediator mediator, IInteractionWindow interactionWindow)
        {
            _mediator = mediator;
            _interactionWindow = interactionWindow;
        }

        public async Task Run()
        {
            try
            {
                var commandResponse = await _mediator.Send(ParseRequest());
                _interactionWindow.Write(commandResponse);
            }
            catch (ArgumentException ex) 
            {
                _interactionWindow.Write(new CommandResponse(ex.Message, ResponseTypeEnum.Error));
            }            
        }

        private IGitCommandRequest ParseRequest()
        {
            if (Args.Count > 1)
            {
                if (Args.First().ToLower().Equals("set-repo"))
                    return new SetRepoCommand(Args.Skip(1).ToList(), _interactionWindow);
                if (Args.First().ToLower().Equals("pick"))
                    return new PickCommand(Args.Skip(1).ToList(), _interactionWindow);
                if (Args.First().ToLower().Equals("unpick"))
                    return new UnpickCommand(Args.Skip(1).ToList(), _interactionWindow);
                if (Args.First().ToLower().Equals("errorlog"))
                    return new ErrorLogCommand(Args.Skip(1).ToList(), _interactionWindow);
                else
                    throw new ArgumentException("Invalid command.");
            }
            else if (Args.Count == 1)
                throw new ArgumentException(String.Format("No arguments provided."));
            else
                throw new ArgumentException(String.Format("Command not provided."));
        }
    }
}
