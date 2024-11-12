namespace GitMemory.Domain.Repositories
{
    public interface IUserSettings
    {
        FileInfo CreateUserSettingsJson(string folder);
        DirectoryInfo CreateUserSettingsFolder(string folderPath);
        void HideFolder(string folderPath);
    }
}
