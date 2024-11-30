using GitMemory.CultureConfig;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Repositories;

namespace GitMemory.Infrastructure.CommandsServices
{
    public class SettingsService : ISettingsService
    {
        private readonly IGitMemoryGlobalSettings _globalSettingsRepository;
        private readonly IUserSettings _userSettingsRepository;
        public SettingsService(IGitMemoryGlobalSettings globalSettingsRepository, IUserSettings userSettingsRepository)
        {
            _globalSettingsRepository = globalSettingsRepository;
            _userSettingsRepository = userSettingsRepository;
        }
        public void HideFile(string folderPath)
        {
            _userSettingsRepository.HideFolder(folderPath);
        }

        // global settings

        public FileInfo CreateGlobalSettingsJson()
        {
            return _globalSettingsRepository.CreateGlobalSettingsJson();
        }

        public Domain.Entities.GlobalSettings ReadGlobalSettings()
        {
            return _globalSettingsRepository.ReadGlobalSettings();
        }

        public string? ReadGlobalSettingsValue(string section, string property)
        {
            return _globalSettingsRepository.ReadValue(section, property);
        }

        public void WriteGlobalSettingsValue(string section, string property, string value, string defaultValue)
        {
            _globalSettingsRepository.WriteValue(section, property, value, defaultValue);
        }
        public string GetGlobalSettingsFilePath()
        {
            return _globalSettingsRepository.GetGlobalSettingsFilePath();
        }
        public Task<Command> EnableErrorLogs()
        {
            try
            {
                WriteGlobalSettingsValue(GlobalSettingsSections.UserSectionKey, GlobalSettingsItems.ErrorLogItemKey, "TRUE", "");
                return Task.FromResult(new Command(ResourceMessages.Services_Settings_EnableErrorLogResult, ResponseTypeEnum.Info));
            }
            catch {
                return Task.FromResult(new Command("EnableErrorLogs: Unable to enable error logs", ResponseTypeEnum.Error));
            }
        }


        public Task<Command> DisableErrorLogs()
        {
            try
            {
                WriteGlobalSettingsValue(GlobalSettingsSections.UserSectionKey, GlobalSettingsItems.ErrorLogItemKey, "FALSE", "");
                return Task.FromResult(new Command(ResourceMessages.Services_Settings_DisableErrorLogResult, ResponseTypeEnum.Info));
            }
            catch
            {
                return Task.FromResult(new Command("EnableErrorLogs: Unable to disable error logs", ResponseTypeEnum.Error));
            }
            
        }

        // user settings

        public DirectoryInfo CreateUserSettingsFolder(string folderPath)
        {
            return _userSettingsRepository.CreateUserSettingsFolder(folderPath);
        }

        public FileInfo CreateUserSettingsJson(string folder)
        {
            return _userSettingsRepository.CreateUserSettingsJson(folder);
        }

        public void WriteValue(string section, string property, string value, string defaultValue)
        {
            _globalSettingsRepository.WriteValue(section, property, value, defaultValue);
        }

        public string? ReadValue(string section, string property)
        {
            return _globalSettingsRepository.ReadValue(section, property);
        }
    }
}
