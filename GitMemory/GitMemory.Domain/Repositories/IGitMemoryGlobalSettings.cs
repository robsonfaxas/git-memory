using GitMemory.Domain.Entities;

namespace GitMemory.Domain.Repositories
{
    public interface IGitMemoryGlobalSettings
    {
        FileInfo CreateGlobalSettingsJson();
        string GetGlobalSettingsFilePath();
        void WriteValue(string section, string property, string value, string defaultValue);
        string? ReadValue(string section, string property);
        GlobalSettings ReadGlobalSettings();
        public string FileName { get; set; }
    }
}
