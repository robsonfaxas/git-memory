using GitMemory.Application.Commands;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Service;
using GitMemory.Domain.Service.Unpick;
using MediatR;

namespace GitMemory.Application.Handlers
{
    public class UnpickCommandHandler : IRequestHandler<UnpickCommand, CommandResponse>
    {
        private readonly IMemoryPoolService _memoryPoolService;
        private readonly IUnpickCommandService _unpickCommandService;
        public UnpickCommandHandler(IMemoryPoolService memoryPoolService, IUnpickCommandService unpickCommandService)
        {
            _memoryPoolService = memoryPoolService;
            _unpickCommandService = unpickCommandService;
        }

        public async Task<CommandResponse> Handle(UnpickCommand request, CancellationToken cancellationToken)
        {
            return await _unpickCommandService.ExecuteCommand(request.Parameters);
        }

        
    }
}
