using GitMemory.Application.Commands;
using GitMemory.Domain.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.Application.Handlers
{
    public class SetRepoCommandHandler : IRequestHandler<SetRepoCommand>
    {
        private readonly ISetRepoCommandService _setRepoCommandService;

        public SetRepoCommandHandler(ISetRepoCommandService commandService)
        {
            _setRepoCommandService = commandService;
        }

        public async Task Handle(SetRepoCommand request, CancellationToken cancellationToken)
        {
            await _setRepoCommandService.ExecuteCommand(request.Commands);
        }
    }
}
