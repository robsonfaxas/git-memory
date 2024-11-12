using GitMemory.Application.Commands;
using GitMemory.CultureConfig;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Repositories;
using GitMemory.Domain.Service.SetRepo;
using MediatR;

namespace GitMemory.Application.Handlers
{
    public class SetRepoCommandHandler : IRequestHandler<SetRepoCommand, CommandResponse>
    {
        private readonly ISetRepoCommandService _setRepoCommandService;
        private readonly ISettingsService _settingsService;        

        public SetRepoCommandHandler(ISetRepoCommandService commandService, ISettingsService settingsService)
        {
            _setRepoCommandService = commandService;
            _settingsService = settingsService;
        }

        public async Task<CommandResponse> Handle(SetRepoCommand request, CancellationToken cancellationToken)
        {
            var globalSettings = _settingsService.ReadGlobalSettings();
            if (!string.IsNullOrEmpty(globalSettings.RepositoryLocation) && 
                !globalSettings.RepositoryLocation.Equals(request.Parameters.FirstOrDefault()))
            {
                CommandContextAccessor.Current.InteractionWindow.Write(new CommandResponse(string.Format(ResourceMessages.Handlers_SetRepo_CurrentRepoInfo,globalSettings.RepositoryLocation), ResponseTypeEnum.Info));
                var dialogResult = CommandContextAccessor.Current.InteractionWindow.Read(DialogButtonsEnum.YesNo, new CommandResponse(ResourceMessages.Handlers_SetRepo_Warning, ResponseTypeEnum.Warning));                
                if (dialogResult == DialogResultEnum.No)
                    return await Task.FromResult(new CommandResponse(ResourceMessages.Handlers_SetRepo_Cancel, ResponseTypeEnum.Info));
            }
            return await _setRepoCommandService.ExecuteCommand(request.Parameters);
        }
    }
}
