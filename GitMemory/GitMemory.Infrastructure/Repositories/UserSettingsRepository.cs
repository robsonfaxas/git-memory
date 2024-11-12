using GitMemory.CultureConfig;
using GitMemory.Domain.Repositories;

namespace GitMemory.Infrastructure.Services
{
    public class UserSettingsRepository : IUserSettings
    {
        private readonly IErrorLogRepository _errorLogRepository;
        public UserSettingsRepository(IErrorLogRepository errorLogRepository)
        {
            _errorLogRepository = errorLogRepository;
        }
        public FileInfo CreateUserSettingsJson(string folder)
        {
            try
            {
                var filePath = String.Concat(folder, "\\git-memory.json");
                if (!File.Exists(filePath))
                    File.Create(filePath).Close();
                return new FileInfo(filePath);
            }
            catch (Exception ex)
            {
                _errorLogRepository.Log(ex);
                throw new Exception(string.Format(ResourceMessages.Repository_UserSettings_CreateJson_UnhandledException,folder));
            }
        }

        public DirectoryInfo CreateUserSettingsFolder(string folderPath)
        {
            try
            {
                string gitMemoryFolder = string.Concat(folderPath, "\\.gitmemory");
                if (!Directory.Exists(gitMemoryFolder))
                    return Directory.CreateDirectory(gitMemoryFolder);
                return new DirectoryInfo(gitMemoryFolder);
            }
            catch (Exception ex)
            {
                _errorLogRepository.Log(ex);
                throw new Exception(string.Format(ResourceMessages.Repository_UserSettings_CreateFolder_UnhandledException, folderPath));
            }
        }

        public void HideFolder(string folderPath)
        {
            try
            {
                FileAttributes attributes = File.GetAttributes(folderPath);
                File.SetAttributes(folderPath, attributes | FileAttributes.Hidden);
            }
            catch (Exception ex)
            {
                _errorLogRepository.Log(ex);
                throw new Exception(string.Format(ResourceMessages.Repository_UserSettings_HideFolder_UnhandledException, folderPath));
            }
        }
    }
}
