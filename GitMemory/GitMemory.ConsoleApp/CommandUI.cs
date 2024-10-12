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
        private readonly IGitCommandFactory _commandFactory;
        private CommandContextConfiguration _contextConfiguration;

        /// <summary>
        /// 1º item is the command
        /// All the other items are arguments/parameters to the method
        /// </summary>
        public List<string> Args { get; set; } = new List<string>();

        public CommandUI(IMediator mediator, IInteractionWindow interactionWindow, IGitCommandFactory commandFactory,
                        CommandContextConfiguration contextConfiguration)
        {
            _mediator = mediator;            
            _interactionWindow = interactionWindow;
            _commandFactory = commandFactory;
            _contextConfiguration = contextConfiguration;
            BuildContext();
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
                // Using the factory to get the appropriate command
                return _commandFactory.GetCommand(Args.First(), Args.Skip(1).ToList());
            }
            else if (Args.Count == 1)
            {
                throw new ArgumentException("No arguments provided.");
            }
            else
            {
                throw new ArgumentException("Command not provided.");
            }
        }

        private CommandContextConfiguration BuildContext()
        {
            _contextConfiguration.UserProfileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) ?? "";
            _contextConfiguration.CurrentDirectory = Directory.GetCurrentDirectory() ?? "";
            CommandContextAccessor.Current = _contextConfiguration;
            return _contextConfiguration;
        }
    }
}
