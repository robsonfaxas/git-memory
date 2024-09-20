using GitMemory.Domain.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.Infrastructure.Services
{
    public class SetRepoCommandService : ISetRepoCommandService
    {
        public Task ExecuteCommand(List<string> commands)
        {
            if (commands == null) throw new ArgumentNullException(nameof(commands));
            if (commands.Count == 0) throw new ArgumentException("No arguments provided.");
            if (commands.First().Equals("."))
            {
                var directory = Directory.GetCurrentDirectory();
                if (directory != null && Directory.Exists(directory)) 
                {
                    var directoryInfo = CreateFolder(directory);
                    if (directoryInfo != null)
                        CreateJson(directoryInfo.FullName);
                }
                else
                    throw new Exception("Directory not found.");
            }
            return Task.CompletedTask;
        }

        private void CreateJson(string folder)
        {
            try
            {
                var filePath = String.Concat(folder, "\\git-memory.json");
                if (!File.Exists(filePath))
                    File.Create(filePath).Close();               
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating file in git-memory.json in {folder}");
            }
        }

        private DirectoryInfo CreateFolder(string folderPath)
        {
            try
            {
                string gitMemoryFolder = string.Concat(folderPath, "\\.gitmemory");
                return Directory.CreateDirectory(gitMemoryFolder);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating directory in {folderPath}");
            }
        }
    }
}
