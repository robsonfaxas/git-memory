using GitMemory.Application.Commands;
using GitMemory.Domain.Service;
using GitMemory.Domain.UI;
using MediatR;

namespace GitMemory.Application.Handlers
{
    public class SetRepoCommandHandler : IRequestHandler<SetRepoCommand, CommandResponse>
    {
        private readonly ISetRepoCommandService _setRepoCommandService;

        public SetRepoCommandHandler(ISetRepoCommandService commandService)
        {
            _setRepoCommandService = commandService;
        }

        public async Task<CommandResponse> Handle(SetRepoCommand request, CancellationToken cancellationToken)
        {
            return await _setRepoCommandService.ExecuteCommand(request.Commands);
        }
    }
}
