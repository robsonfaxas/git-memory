using GitMemory.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.Infrastructure.Services
{
    public class RepositorySettingsService : IRepositorySettingsService
    {
        public FileInfo CreateRepositorySettingsJson(string folder)
        {
            try
            {
                var filePath = String.Concat(folder, "\\git-memory.json");
                if (!File.Exists(filePath))
                    File.Create(filePath).Close();
                return new FileInfo(filePath);
            }
            catch (Exception)
            {
                throw new Exception($"Error creating file in git-memory.json in {folder}");
            }
        }

        public DirectoryInfo CreateRepositorySettingsFolder(string folderPath)
        {
            try
            {
                string gitMemoryFolder = string.Concat(folderPath, "\\.gitmemory");
                if (!Directory.Exists(gitMemoryFolder))
                    return Directory.CreateDirectory(gitMemoryFolder);
                return new DirectoryInfo(gitMemoryFolder);
            }
            catch (Exception)
            {
                throw new Exception($"Error creating directory in {folderPath}");
            }
        }

        public void HideFile(string folderPath)
        {
            FileAttributes attributes = File.GetAttributes(folderPath);
            File.SetAttributes(folderPath, attributes | FileAttributes.Hidden);
        }
    }
}
