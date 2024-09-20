using GitMemory.Application.Commands;
using GitMemory.Application.Interfaces;
using GitMemory.Domain.UI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.ConsoleApp
{
    public class CommandUI : ICommandUI
    {
        private readonly IMediator _mediator;
        public List<string> Args { get; set; } = new List<string>();
        public CommandUI(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Run()
        {
            //var input = Console.ReadLine();
            await _mediator.Send(ParseRequest());
            foreach (var arg in Args)
                Console.WriteLine(arg);
        }

        private IGitCommandRequest ParseRequest()
        {
            if (Args.Count > 1)
            {
                if (Args.First().ToLower().Equals("set-repo"))
                    return new SetRepoCommand(Args.Skip(1).ToList());
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
