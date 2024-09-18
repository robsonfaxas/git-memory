using GitMemory.Application.Commands;
using GitMemory.Domain.Interfaces;
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

        public CommandUI(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Run()
        {
            var input = Console.ReadLine();
            await _mediator.Send(new ProcessCommand(input ?? ""));
        }
    }
}
