using GitMemory.CultureConfig;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Repositories;
using GitMemory.Domain.Service.SetBrain;

namespace GitMemory.Infrastructure.CommandsServices.SetBrain
{
    public class SetBrainCommandService : ISetBrainCommandService
    {
        private readonly ISettingsService _settingsService;
        public SetBrainCommandService(ISettingsService repositorySettingsService)
        {
            _settingsService = repositorySettingsService;
        }

        /// <summary>
        /// Commands expected:
        /// 1) set-brain .  
        /// 2) set-brain <full-path>
        /// </summary>
        /// <param name="commands"> expects 1 argument for the set-brain command </param>
        /// <returns>Always the completion of the tasks. All errors are displayed to the terminal</returns>
        public Task<Command> ExecuteCommand(List<string> commands)
        {
            try
            {
                if (commands == null || commands.Count == 0)
                    return Task.FromResult(new Command(ResourceMessages.Services_SetBrain_MissingArgument, ResponseTypeEnum.Error));
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
                        return Task.FromResult(new Command(ResourceMessages.Services_SetBrain_CreationSuccess));
                    }
                    else
                        return Task.FromResult(new Command(string.Format(ResourceMessages.Services_SetBrain_ErrorHandlingDirectory, repositoryFolder), ResponseTypeEnum.Error));
                }
                else
                    return Task.FromResult(new Command(ResourceMessages.Services_SetBrain_DirectoryNotFound, ResponseTypeEnum.Error));
            }
            catch (Exception ex)
            {
                return Task.FromResult(new Command(ex.Message, ResponseTypeEnum.Error));
            }
        }
    }
}
