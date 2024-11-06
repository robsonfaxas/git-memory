using GitMemory.Application.Commands;
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
                var dialogResult = CommandContextAccessor.Current.InteractionWindow.Read(DialogButtonsEnum.YesNo, new CommandResponse("Warning: A repository is already set in another location. Do you want to overwrite the current location set?", ResponseTypeEnum.Warning));
                CommandContextAccessor.Current.InteractionWindow.Write(new CommandResponse($"Current Repository Location: {globalSettings.RepositoryLocation}", ResponseTypeEnum.Info));
                if (dialogResult == DialogResultEnum.No)
                    return await Task.FromResult(new CommandResponse("No repository changes.", ResponseTypeEnum.Info));
            }
            return await _setRepoCommandService.ExecuteCommand(request.Parameters);
        }
    }
}
