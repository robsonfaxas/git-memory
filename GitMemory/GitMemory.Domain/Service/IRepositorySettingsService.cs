using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.Domain.Service
{
    public interface IRepositorySettingsService
    {
        FileInfo CreateRepositorySettingsJson(string folder);
        DirectoryInfo CreateRepositorySettingsFolder(string folderPath);
        void HideFile(string folderPath);
    }
}
