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
        private readonly IInteractionWindow _logger;
        public List<string> Args { get; set; } = new List<string>();
        public CommandUI(IMediator mediator, IInteractionWindow logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Run()
        {
            try
            {
                var commandResponse = await _mediator.Send(ParseRequest());
                _logger.WriteInfo(commandResponse);
            }
            catch (ArgumentException ex) 
            {
                _logger.WriteInfo(new CommandResponse(ex.Message, ResponseTypeEnum.Error));
            }            
        }

        private IGitCommandRequest ParseRequest()
        {
            if (Args.Count > 1)
            {
                if (Args.First().ToLower().Equals("set-repo"))
                    return new SetRepoCommand(Args.Skip(1).ToList(), _logger);
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
