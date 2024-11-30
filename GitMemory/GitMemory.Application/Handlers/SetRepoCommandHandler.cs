using GitMemory.Application.Commands;
using GitMemory.CultureConfig;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Repositories;
using GitMemory.Domain.Service.SetBrain;
using MediatR;

namespace GitMemory.Application.Handlers
{
    public class SetBrainCommandHandler : IRequestHandler<SetBrainCommand, Command>
    {
        private readonly ISetBrainCommandService _setRepoCommandService;
        private readonly ISettingsService _settingsService;        

        public SetBrainCommandHandler(ISetBrainCommandService commandService, ISettingsService settingsService)
        {
            _setRepoCommandService = commandService;
            _settingsService = settingsService;
        }

        public async Task<Command> Handle(SetBrainCommand request, CancellationToken cancellationToken)
        {
            var globalSettings = _settingsService.ReadGlobalSettings();
            if (!string.IsNullOrEmpty(globalSettings.RepositoryLocation) && 
                !globalSettings.RepositoryLocation.Equals(request.Parameters.FirstOrDefault()))
            {
                CommandContextAccessor.Current.InteractionWindow.Write(new Command(string.Format(ResourceMessages.Handlers_SetBrain_CurrentRepoInfo,globalSettings.RepositoryLocation), ResponseTypeEnum.Info));
                var dialogResult = CommandContextAccessor.Current.InteractionWindow.Read(DialogButtonsEnum.YesNo, new Command(ResourceMessages.Handlers_SetBrain_Warning, ResponseTypeEnum.Warning));                
                if (dialogResult == DialogResultEnum.No)
                    return await Task.FromResult(new Command(ResourceMessages.Handlers_SetBrain_Cancel, ResponseTypeEnum.Info));
            }
            return await _setRepoCommandService.ExecuteCommand(request.Parameters);
        }
    }
}
