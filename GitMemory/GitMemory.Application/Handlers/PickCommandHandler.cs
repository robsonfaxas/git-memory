using GitMemory.Application.Commands;
using GitMemory.CultureConfig;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Service;
using GitMemory.Domain.Service.Pick;
using LibGit2Sharp;
using MediatR;

namespace GitMemory.Application.Handlers
{
    public class PickCommandHandler : IRequestHandler<PickCommand, CommandResponse>
    {
        private readonly IMemoryPoolService _memoryPoolService;
        private readonly IPickCommandService _pickCommandService;
        public PickCommandHandler(IMemoryPoolService memoryPoolService, IPickCommandService pickCommandService)
        {
            _memoryPoolService = memoryPoolService;
            _pickCommandService=pickCommandService;
        }

        public async Task<CommandResponse> Handle(PickCommand request, CancellationToken cancellationToken)
        {
            var memoryPool = _memoryPoolService.ReadMemoryPool();
            bool clearPoolList = false;
            string repoPath = Repository.Discover(CommandContextAccessor.Current.CurrentDirectory);
            foreach (var repository in memoryPool.GitRepositories.Where(p => p.GitRepositoryPath.ToLower().StartsWith(repoPath.ToLower())))
            {
                if (repository.Unstaged.Any() || repository.Unstaged.Any())
                {
                    CommandContextAccessor.Current.InteractionWindow.Write(new CommandResponse(ResourceMessages.Handlers_Pick_WarningCommitsInList, ResponseTypeEnum.Warning));
                    foreach (var file in repository.Staged)
                        CommandContextAccessor.Current.InteractionWindow.Write(new CommandResponse(string.Format(ResourceMessages.Handlers_Pick_StagedItem, file.CommitHash),ResponseTypeEnum.Info));
                    foreach (var file in repository.Unstaged)
                        CommandContextAccessor.Current.InteractionWindow.Write(new CommandResponse(string.Format(ResourceMessages.Handlers_Pick_NotStagedItem, file.CommitHash), ResponseTypeEnum.Info));

                    var dialogResult = CommandContextAccessor.Current.InteractionWindow.Read(DialogButtonsEnum.YesNo, new CommandResponse("\n" + ResourceMessages.Handlers_Pick_RequestClearList, ResponseTypeEnum.Warning));
                    clearPoolList = dialogResult == DialogResultEnum.Yes;
                }
            }
            return await _pickCommandService.ExecuteCommand(request.Parameters, clearPoolList);
        }

        
    }
}
