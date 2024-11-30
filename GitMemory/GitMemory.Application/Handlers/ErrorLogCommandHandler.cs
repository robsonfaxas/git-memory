using GitMemory.Application.Commands;
using GitMemory.CultureConfig;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Repositories;
using GitMemory.Domain.Service.SetRepo;
using MediatR;

namespace GitMemory.Application.Handlers
{
    public class ErrorLogCommandHandler : IRequestHandler<ErrorLogCommand, Command>
    {
        private readonly ISettingsService _settingsService;        

        public ErrorLogCommandHandler(ISetRepoCommandService commandService, ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public async Task<Command> Handle(ErrorLogCommand request, CancellationToken cancellationToken)
        {
            if (request.Parameters is not null && !String.IsNullOrEmpty(request.Parameters.FirstOrDefault())
                && request.Parameters.Count > 0)
            {
                if (request.Parameters.FirstOrDefault()!.Trim().ToLower().Equals("--enable"))
                {
                    return await _settingsService.EnableErrorLogs();                    
                }
                else
                if (request.Parameters.FirstOrDefault()!.Trim().ToLower().Equals("--disable"))
                {
                    return await _settingsService.DisableErrorLogs();                    
                }
                else
                    return await Task.FromResult(new Command(string.Format(ResourceMessages.Handlers_ErrorLog_InvalidParameter, request.Parameters.FirstOrDefault()), ResponseTypeEnum.Error));
            }
            
            var errorLogStatus = _settingsService.ReadGlobalSettings().IsErrorLogsEnabled ? "ENABLED" : "DISABLED";            
            return await Task.FromResult(new Command(string.Format(ResourceMessages.Handlers_ErrorLog_StatusMessage, errorLogStatus), ResponseTypeEnum.Info));
        }
    }
}
