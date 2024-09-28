using GitMemory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.Infrastructure.Services
{
    public class UserSettings : IUserSettings
    {
        private readonly IErrorLogRepository _errorLogRepository;
        public UserSettings(IErrorLogRepository errorLogRepository)
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
                throw new Exception($"Error creating file in git-memory.json in {folder}");
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
                throw new Exception($"Error creating directory in {folderPath}");
            }
        }

        public void HideFile(string folderPath)
        {
            try
            {
                FileAttributes attributes = File.GetAttributes(folderPath);
                File.SetAttributes(folderPath, attributes | FileAttributes.Hidden);
            }
            catch (Exception ex)
            {
                _errorLogRepository.Log(ex);
                throw new Exception($"Error trying to hide folder {folderPath}");
            }
        }
    }
}
