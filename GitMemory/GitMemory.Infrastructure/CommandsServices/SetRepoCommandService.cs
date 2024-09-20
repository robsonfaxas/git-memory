using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Service;

namespace GitMemory.Infrastructure.Services
{
    public class SetRepoCommandService : ISetRepoCommandService
    {
        private readonly IGlobalSettingsService _globalSettingsService;
        private readonly IRepositorySettingsService _repositorySettingsService;
        public SetRepoCommandService(IGlobalSettingsService globalSettingsService, IRepositorySettingsService repositorySettingsService)
        {
            _globalSettingsService = globalSettingsService;
            _repositorySettingsService = repositorySettingsService;
        }

        /// <summary>
        /// Commands expected:
        /// 1) set-repo .  
        /// 2) set-repo <full-path>
        /// </summary>
        /// <param name="commands"> expects 1 argument for the set-repo command </param>
        /// <returns>Always the completion of the tasks. All errors are displayed to the terminal</returns>
        public Task<CommandResponse> ExecuteCommand(List<string> commands)
        {
            try
            {
                if (commands == null || commands.Count == 0)
                    return Task.FromResult(new CommandResponse("No arguments provided.", ResponseTypeEnum.Error));
                string repositoryFolder = "";
                if (commands.First().Equals("."))
                    repositoryFolder = Directory.GetCurrentDirectory();
                else
                    repositoryFolder = commands.First();
                if (repositoryFolder != null && Directory.Exists(repositoryFolder))
                {
                    var settingsDirectoryInnerFolder = _repositorySettingsService.CreateRepositorySettingsFolder(repositoryFolder);
                    if (settingsDirectoryInnerFolder != null)
                    {
                        _repositorySettingsService.HideFile(settingsDirectoryInnerFolder.FullName);
                        var configurationJsonFile = _repositorySettingsService.CreateRepositorySettingsJson(settingsDirectoryInnerFolder.FullName);
                        _globalSettingsService.CreateGlobalSettingsJson();
                        _globalSettingsService.WriteValue(GlobalSettingsSections.UserSectionKey, GlobalSettingsItems.RepositoryLocationItemKey, repositoryFolder, "");
                        _globalSettingsService.WriteValue(GlobalSettingsSections.UserSectionKey, GlobalSettingsItems.ConfigurationFileLocationItemKey, configurationJsonFile.FullName, "");
                        return Task.FromResult(new CommandResponse($"Folder created successfully."));
                    }
                    else
                        return Task.FromResult(new CommandResponse($"Error creating/reading directory in {repositoryFolder}", ResponseTypeEnum.Error));                    
                }
                else
                    return Task.FromResult(new CommandResponse("Directory not found.", ResponseTypeEnum.Error));                
            }
            catch (Exception ex)
            {
                return Task.FromResult(new CommandResponse(ex.Message, ResponseTypeEnum.Error));                
            }
        }
    }
}
