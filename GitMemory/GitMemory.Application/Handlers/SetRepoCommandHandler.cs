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
                request.InteractionWindow.Write(new CommandResponse($"Current Location: {globalSettings.RepositoryLocation}", ResponseTypeEnum.Info));
                request.InteractionWindow.Write(new CommandResponse("Warning: A repository is already set for this user. Do you want to overwrite the current set location? Y/N", ResponseTypeEnum.Warning));
                var answer = "";
                do
                {
                    answer = request.InteractionWindow.Read();
                    if (answer != null)
                        if (answer.ToUpper().Equals("Y"))
                            break;
                        else if (answer.ToUpper().Equals("N"))
                            return await Task.FromResult(new CommandResponse("No repository changes.", ResponseTypeEnum.Info));
                        else
                            request.InteractionWindow.Write(new CommandResponse($"Invalid Command. Answer 'Y' (yes) or 'N' (no) to proceed.", ResponseTypeEnum.Info));

                } while (answer == null || !(answer != null && (answer.ToUpper().Equals("Y") || answer.ToUpper().Equals("N"))));
            }
            return await _setRepoCommandService.ExecuteCommand(request.Parameters);
        }

        
    }
}
