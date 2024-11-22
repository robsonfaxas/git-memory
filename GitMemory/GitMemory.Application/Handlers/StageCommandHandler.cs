using GitMemory.Application.Commands;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Service;
using GitMemory.Domain.Service.Stage;
using MediatR;

namespace GitMemory.Application.Handlers
{
    public class StageCommandHandler : IRequestHandler<StageCommand, CommandResponse>
    {
        private readonly IStageCommandService _stageService;
        private readonly IMemoryPoolService _memoryPoolService;
        public StageCommandHandler(IStageCommandService stageService, IMemoryPoolService memoryPoolService)
        {
            _stageService = stageService;
            _memoryPoolService = memoryPoolService;
        }

        public async Task<CommandResponse> Handle(StageCommand request, CancellationToken cancellationToken)
        {
            return await _stageService.ExecuteCommand(request.Parameters);
        }


    }
}
