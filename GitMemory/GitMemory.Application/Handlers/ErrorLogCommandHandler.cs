using GitMemory.Application.Commands;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Repositories;
using GitMemory.Domain.Service.SetRepo;
using MediatR;

namespace GitMemory.Application.Handlers
{
    public class ErrorLogCommandHandler : IRequestHandler<ErrorLogCommand, CommandResponse>
    {
        private readonly ISettingsService _settingsService;        

        public ErrorLogCommandHandler(ISetRepoCommandService commandService, ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public async Task<CommandResponse> Handle(ErrorLogCommand request, CancellationToken cancellationToken)
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
                    return await Task.FromResult(new CommandResponse($"Invalid parameter {request.Parameters.FirstOrDefault()}", ResponseTypeEnum.Error));
            }
            
            var errorLogStatus = _settingsService.ReadGlobalSettings().IsErrorLogsEnabled ? "ENABLED" : "DISABLED";            
            return await Task.FromResult(new CommandResponse($"Error Log is currently {errorLogStatus}. Use `--enable` or `--disable` to change the status.", ResponseTypeEnum.Info));
        }
    }
}
