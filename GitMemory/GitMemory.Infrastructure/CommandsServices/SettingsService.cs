using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _userSettingsRepository.HideFile(folderPath);
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

        // user settings

        public DirectoryInfo CreateUserSettingsFolder(string folderPath)
        {
            return _userSettingsRepository.CreateUserSettingsFolder(folderPath);
        }

        public FileInfo CreateUserSettingsJson(string folder)
        {
            return _userSettingsRepository.CreateUserSettingsJson(folder);
        }


    }
}
