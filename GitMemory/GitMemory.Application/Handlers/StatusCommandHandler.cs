using GitMemory.Application.Commands;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Service;
using GitMemory.Domain.Service.Stage;
using GitMemory.Domain.Service.Status;
using MediatR;

namespace GitMemory.Application.Handlers
{
    public class StatusCommandHandler : IRequestHandler<StatusCommand, Command>
    {
        private readonly IStatusCommandService _statusService;
        private readonly IMemoryPoolService _memoryPoolService;
        public StatusCommandHandler(IStatusCommandService stageService, IMemoryPoolService memoryPoolService)
        {
            _statusService = stageService;
            _memoryPoolService = memoryPoolService;
        }

        public async Task<Command> Handle(StatusCommand request, CancellationToken cancellationToken)
        {
            return await _statusService.ExecuteCommand(request.Parameters);
        }


    }
}
