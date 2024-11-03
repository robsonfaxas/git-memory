using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.ConsoleApp.IntegrationTests.Fixtures
{
    public class CommandTestFixture : IAsyncLifetime
    {
        public string TempDirectory { get; set; } = string.Empty;
        public string GlobalSettingsDirectory { get; set; } = string.Empty;
        public string RepoDirectory { get; set; } = string.Empty;
        public string CurrentDirectoryFolder { get; set; } = string.Empty;

        public Task DisposeAsync()
        {
            if (Directory.Exists(TempDirectory))
            {
                Directory.Delete(TempDirectory, true);
            }
            return Task.CompletedTask;
        }

        public Task InitializeAsync()
        {
            TempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(TempDirectory);

            GlobalSettingsDirectory = Path.Combine(TempDirectory, "globalconfig");
            Directory.CreateDirectory(GlobalSettingsDirectory);

            RepoDirectory = Path.Combine(TempDirectory, "repo");
            Directory.CreateDirectory(RepoDirectory);

            CurrentDirectoryFolder = Directory.GetCurrentDirectory() ?? "";
            return Task.CompletedTask;
        }

        // helper method to create a folder and deny creating subfolders
        public void CreateFolderWithPermissionToCreateSubFolder(string folderPath)
        {
            // Create the directory if it does not exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Create a DirectoryInfo object for the folder
            var directoryInfo = new DirectoryInfo(folderPath);

            // Get the current security settings
            var security = directoryInfo.GetAccessControl();

            // Define the 'Everyone' group for permissions
            var everyoneSID = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);

            // Remove existing permissions for 'Everyone'
            security.RemoveAccessRuleAll(new FileSystemAccessRule(everyoneSID, FileSystemRights.FullControl, AccessControlType.Allow));

            // Add permissions to allow deleting the folder but restrict changes and creation of subfolders
            security.AddAccessRule(new FileSystemAccessRule(everyoneSID,
                FileSystemRights.Delete | FileSystemRights.ReadAttributes | FileSystemRights.ReadPermissions,
                AccessControlType.Allow));

            // Deny permission to create subfolders
            security.AddAccessRule(new FileSystemAccessRule(everyoneSID,
                FileSystemRights.CreateDirectories,
                AccessControlType.Deny));

            // Get the owner and group for the folder
            var owner = security.GetOwner(typeof(NTAccount));
            var group = security.GetGroup(typeof(NTAccount));

            // Grant specific permissions to the owner and group
            security.AddAccessRule(new FileSystemAccessRule(owner,
                FileSystemRights.Read | FileSystemRights.ReadAndExecute,
                AccessControlType.Allow));

            security.AddAccessRule(new FileSystemAccessRule(group,
                FileSystemRights.Read | FileSystemRights.ReadAndExecute,
                AccessControlType.Allow));

            // Set the modified access rules back to the folder
            directoryInfo.SetAccessControl(security);

            Console.WriteLine($"Folder '{folderPath}' created with restricted permissions (no subfolders allowed).");
        }
    }
}
