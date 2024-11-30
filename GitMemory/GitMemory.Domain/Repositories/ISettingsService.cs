using GitMemory.Domain.Entities;

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
        void WriteValue(string section, string property, string value, string defaultValue);
        string? ReadValue(string section, string property);

        // user settings
        FileInfo CreateUserSettingsJson(string folder);
        DirectoryInfo CreateUserSettingsFolder(string folderPath);
        Task<Command> EnableErrorLogs();
        Task<Command> DisableErrorLogs();
    }
}
