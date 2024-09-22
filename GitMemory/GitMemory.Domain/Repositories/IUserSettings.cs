using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.Domain.Repositories
{
    public interface IUserSettings
    {
        FileInfo CreateUserSettingsJson(string folder);
        DirectoryInfo CreateUserSettingsFolder(string folderPath);
        void HideFile(string folderPath);
    }
}
