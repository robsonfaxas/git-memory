using GitMemory.Application.Commands;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Service;
using GitMemory.Domain.Service.Unstage;
using MediatR;

namespace GitMemory.Application.Handlers
{
    public class UnstageCommandHandler : IRequestHandler<UnstageCommand, Command>
    {
        private readonly IUnstageCommandService _unstageService;
        private readonly IMemoryPoolService _memoryPoolService;
        public UnstageCommandHandler(IUnstageCommandService unstage, IMemoryPoolService memoryPoolService)
        {
            _unstageService = unstage;
            _memoryPoolService = memoryPoolService;
        }

        public async Task<Command> Handle(UnstageCommand request, CancellationToken cancellationToken)
        {
            return await _unstageService.ExecuteCommand(request.Parameters);
        }


    }
}
