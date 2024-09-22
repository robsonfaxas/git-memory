using GitMemory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.Domain.Repositories
{
    public interface ISettingsService
    {
        void HideFile(string folderPath);

        // global settings
        FileInfo CreateGlobalSettingsJson();
        string GetGlobalSettingsFilePath();
        void WriteGlobalSettingsValue(string section, string property, string value, string defaultValue);
        string? ReadGlobalSettingsValue(string section, string property);
        GlobalSettings ReadGlobalSettings();

        // user settings
        FileInfo CreateUserSettingsJson(string folder);
        DirectoryInfo CreateUserSettingsFolder(string folderPath);        
    }
}
