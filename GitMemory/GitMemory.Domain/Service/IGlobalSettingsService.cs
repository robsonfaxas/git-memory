using GitMemory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.Domain.Service
{
    public interface IGlobalSettingsService
    {
        FileInfo CreateGlobalSettingsJson();
        string GetGlobalSettingsFilePath();
        void WriteValue(string section, string property, string value, string defaultValue);
        string? ReadValue(string section, string property);
        GlobalSettings ReadGlobalSettings();
    }
}
