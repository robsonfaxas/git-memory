using GitMemory.Domain.Service;
using GitMemory.Domain.UI;

namespace GitMemory.Infrastructure.Services
{
    public class SetRepoCommandService : ISetRepoCommandService
    {
        /// <summary>
        /// Commands expected:
        /// 1) set-repo .  
        /// 2) set-repo <full-path>
        /// </summary>
        /// <param name="commands"> expects 1 argument for the set-repo command </param>
        /// <returns>Always the completion of the tasks. All errors are displayed to the terminal</returns>
        public Task<CommandResponse> ExecuteCommand(List<string> commands)
        {
            try
            {
                if (commands == null || commands.Count == 0)
                    return Task.FromResult(new CommandResponse("No arguments provided.", ResponseType.Error));
                string directory = "";
                if (commands.First().Equals("."))
                    directory = Directory.GetCurrentDirectory();
                else
                    directory = commands.First();
                if (directory != null && Directory.Exists(directory))
                {
                    var directoryInfo = CreateFolder(directory);
                    if (directoryInfo != null)
                    {
                        HideFolder(directoryInfo.FullName);
                        var jsonFile = CreateJson(directoryInfo.FullName);
                        return Task.FromResult(new CommandResponse($"Folder created successfully."));
                    }
                    else
                        return Task.FromResult(new CommandResponse($"Error creating/reading directory in {directory}", ResponseType.Error));                    
                }
                else
                    return Task.FromResult(new CommandResponse("Directory not found.", ResponseType.Error));                
            }
            catch (Exception ex)
            {
                return Task.FromResult(new CommandResponse(ex.Message, ResponseType.Error));                
            }
        }

        private FileInfo CreateJson(string folder)
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

        private DirectoryInfo CreateFolder(string folderPath)
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

        private void HideFolder(string folderPath)
        {
            FileAttributes attributes = File.GetAttributes(folderPath);
            File.SetAttributes(folderPath, attributes | FileAttributes.Hidden);
        }
    }
}
