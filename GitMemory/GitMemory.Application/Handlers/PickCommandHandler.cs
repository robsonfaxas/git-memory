using GitMemory.Application.Commands;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Repositories;
using GitMemory.Domain.Service;
using GitMemory.Domain.Service.Pick;
using GitMemory.Infrastructure.Services;
using LibGit2Sharp;
using MediatR;
using System.Reflection.Metadata.Ecma335;

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
            var currentDirectory = Directory.GetCurrentDirectory() ?? "";
            bool clearPoolList = false;
            foreach(var repository in memoryPool.GitRepositories.Where(p => p.GitRepositoryPath.ToLower().StartsWith(currentDirectory.ToLower())))
            {
                if (repository.Unstaged.Any() || repository.Unstaged.Any())
                {
                    request.InteractionWindow.Write(new CommandResponse("Warning: you have commits set in your list.", ResponseTypeEnum.Warning));
                    foreach (var file in repository.Unstaged)
                        request.InteractionWindow.Write(new CommandResponse(file.CommitHash + " - Staged",ResponseTypeEnum.Info));
                    foreach (var file in repository.Staged)
                        request.InteractionWindow.Write(new CommandResponse(file.CommitHash + " - Not Staged\n", ResponseTypeEnum.Info));
                    request.InteractionWindow.Write(new CommandResponse("Do you want to clear the list before add new? Y/N", ResponseTypeEnum.Info));
                    var answer = "";
                    do
                    {
                        answer = request.InteractionWindow.Read();
                        if (answer != null)
                            if (answer.ToUpper().Equals("Y"))
                            {
                                clearPoolList = true;
                                break;
                            }
                            else if (answer.ToUpper().Equals("N"))
                                break;
                            else
                                request.InteractionWindow.Write(new CommandResponse($"Invalid Command. Answer 'Y' (yes) or 'N' (no) to proceed.", ResponseTypeEnum.Info));

                    } while (answer == null || !(answer != null && (answer.ToUpper().Equals("Y") || answer.ToUpper().Equals("N"))));
                }
            }
            return await _pickCommandService.ExecuteCommand(request.Parameters, clearPoolList);
        }

        
    }
}
