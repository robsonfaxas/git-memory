using GitMemory.Application.Commands;
using GitMemory.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.Application.Handlers
{
    public class ProcessCommandHandler : IRequestHandler<ProcessCommand>
    {
        private readonly ICommandService _commandService;

        public ProcessCommandHandler(ICommandService commandService)
        {
            _commandService = commandService;
        }

        public async Task Handle(ProcessCommand request, CancellationToken cancellationToken)
        {
            await _commandService.ExecuteCommand(request.Command);
        }
    }
}
