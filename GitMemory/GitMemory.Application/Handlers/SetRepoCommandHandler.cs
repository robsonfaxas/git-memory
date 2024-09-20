using GitMemory.Application.Commands;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Service;
using MediatR;

namespace GitMemory.Application.Handlers
{
    public class SetRepoCommandHandler : IRequestHandler<SetRepoCommand, CommandResponse>
    {
        private readonly ISetRepoCommandService _setRepoCommandService;
        private readonly IGlobalSettingsService _globalSettingsService;        

        public SetRepoCommandHandler(ISetRepoCommandService commandService, IGlobalSettingsService globalSettingsService)
        {
            _setRepoCommandService = commandService;
            _globalSettingsService = globalSettingsService;
        }

        public async Task<CommandResponse> Handle(SetRepoCommand request, CancellationToken cancellationToken)
        {
            var globalSettings = _globalSettingsService.ReadGlobalSettings();
            if (!string.IsNullOrEmpty(globalSettings.RepositoryLocation) && 
                !globalSettings.RepositoryLocation.Equals(request.Commands.FirstOrDefault()))
            {   
                request.InteractionWindow.WriteInfo(new CommandResponse($"Current Location: {globalSettings.RepositoryLocation}", ResponseTypeEnum.Info));
                request.InteractionWindow.WriteInfo(new CommandResponse("Warning: A repository is already set for this user. Do you want to overwrite the current set location? Y/N", ResponseTypeEnum.Warning));
                var answer = "";
                do
                {
                    answer = request.InteractionWindow.ReadInfo();
                    if (answer != null)
                        if (answer.ToUpper().Equals("Y"))
                            break;
                        else if (answer.ToUpper().Equals("N"))
                            return await Task.FromResult(new CommandResponse("No repository changes.", ResponseTypeEnum.Info));
                        else
                            request.InteractionWindow.WriteInfo(new CommandResponse($"Invalid Command. Answer 'Y' (yes) or 'N' (no) to proceed.", ResponseTypeEnum.Info));

                } while (answer == null || !(answer != null && (answer.ToUpper().Equals("Y") || answer.ToUpper().Equals("N"))));
            }
            return await _setRepoCommandService.ExecuteCommand(request.Commands);
             

        }

        
    }
}
