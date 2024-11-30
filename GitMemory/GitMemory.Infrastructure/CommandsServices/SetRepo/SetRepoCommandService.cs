using GitMemory.CultureConfig;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Repositories;
using GitMemory.Domain.Service.SetRepo;

namespace GitMemory.Infrastructure.CommandsServices.SetRepo
{
    public class SetRepoCommandService : ISetRepoCommandService
    {
        private readonly ISettingsService _settingsService;
        public SetRepoCommandService(ISettingsService repositorySettingsService)
        {
            _settingsService = repositorySettingsService;
        }

        /// <summary>
        /// Commands expected:
        /// 1) set-repo .  
        /// 2) set-repo <full-path>
        /// </summary>
        /// <param name="commands"> expects 1 argument for the set-repo command </param>
        /// <returns>Always the completion of the tasks. All errors are displayed to the terminal</returns>
        public Task<Command> ExecuteCommand(List<string> commands)
        {
            try
            {
                if (commands == null || commands.Count == 0)
                    return Task.FromResult(new Command(ResourceMessages.Services_SetRepo_MissingArgument, ResponseTypeEnum.Error));
                string repositoryFolder = "";
                if (commands.First().Equals("."))
                    repositoryFolder = CommandContextAccessor.Current.CurrentDirectory;
                else
                    repositoryFolder = commands.First();
                if (repositoryFolder != null && Directory.Exists(repositoryFolder))
                {
                    var settingsDirectoryInnerFolder = _settingsService.CreateUserSettingsFolder(repositoryFolder);
                    if (settingsDirectoryInnerFolder != null)
                    {
                        _settingsService.HideFile(settingsDirectoryInnerFolder.FullName);
                        var configurationJsonFile = _settingsService.CreateUserSettingsJson(settingsDirectoryInnerFolder.FullName);
                        _settingsService.CreateGlobalSettingsJson();
                        _settingsService.WriteValue(GlobalSettingsSections.UserSectionKey, GlobalSettingsItems.RepositoryLocationItemKey, repositoryFolder, "");
                        _settingsService.WriteValue(GlobalSettingsSections.UserSectionKey, GlobalSettingsItems.ConfigurationFileLocationItemKey, configurationJsonFile.FullName, "");
                        _settingsService.WriteValue(GlobalSettingsSections.UserSectionKey, GlobalSettingsItems.ErrorLogItemKey, "FALSE", "");
                        return Task.FromResult(new Command(ResourceMessages.Services_SetRepo_CreationSuccess));
                    }
                    else
                        return Task.FromResult(new Command(string.Format(ResourceMessages.Services_SetRepo_ErrorHandlingDirectory, repositoryFolder), ResponseTypeEnum.Error));
                }
                else
                    return Task.FromResult(new Command(ResourceMessages.Services_SetRepo_DirectoryNotFound, ResponseTypeEnum.Error));
            }
            catch (Exception ex)
            {
                return Task.FromResult(new Command(ex.Message, ResponseTypeEnum.Error));
            }
        }
    }
}
