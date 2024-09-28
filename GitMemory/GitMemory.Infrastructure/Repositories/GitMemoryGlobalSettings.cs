using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMemory.Infrastructure.Services
{
    public class GitMemoryGlobalSettings : IGitMemoryGlobalSettings
    {
        public string FileName { get; set; } = ".git-memoryconfig";
        public GitMemoryGlobalSettings()
        {

        }

        public FileInfo CreateGlobalSettingsJson()
        {
            try
            {
                var filePath = GetGlobalSettingsFilePath();
                if (!File.Exists(filePath))
                    File.Create(filePath).Close();
                return new FileInfo(filePath);
            }
            catch (Exception)
            {
                throw new Exception($"Error creating {FileName} file");
            }
        }

        public Domain.Entities.GlobalSettings ReadGlobalSettings()
        {
            return new Domain.Entities.GlobalSettings()
            {
                ConfigurationFileLocation = ReadValue(GlobalSettingsSections.UserSectionKey, GlobalSettingsItems.ConfigurationFileLocationItemKey) ?? "",
                RepositoryLocation = ReadValue(GlobalSettingsSections.UserSectionKey, GlobalSettingsItems.RepositoryLocationItemKey) ?? "",
                IsErrorLogsEnabled = ReadValue(GlobalSettingsSections.UserSectionKey, GlobalSettingsItems.ErrorLogItemKey)?.ToUpper() == "TRUE"
            };
        }

        public string GetGlobalSettingsFilePath()
        {
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return String.Concat(userFolder, $"\\{FileName}");
        }

        public string? ReadValue(string section, string property)
        {
            var filePath = GetGlobalSettingsFilePath();
            if (!File.Exists(filePath))            
                return null;
            string[] lines = File.ReadAllLines(filePath);

            bool sectionFound = false;

            foreach (var line in lines)
            {
                string trimmedLine = line.Trim();
                if (trimmedLine.Equals($"[{section}]", StringComparison.OrdinalIgnoreCase))                
                    sectionFound = true;
                
                else if (sectionFound && trimmedLine.StartsWith($"{property}=", StringComparison.OrdinalIgnoreCase))
                {
                    var value = trimmedLine.Substring(trimmedLine.IndexOf('=') + 1).Trim();
                    return value;
                }
                else if (sectionFound && trimmedLine.StartsWith("[") && !trimmedLine.Equals($"[{section}]"))
                {
                    break;
                }
            }
            return null;
        }

        public void WriteValue(string section, string property, string value, string defaultValue)
        {
            try
            {
                var filePath = GetGlobalSettingsFilePath();
                if (!File.Exists(filePath))
                    _ = CreateGlobalSettingsJson();
                value ??= defaultValue;
                List<string> lines = new List<string>(File.Exists(filePath) ? File.ReadAllLines(filePath) : new string[0]);

                bool sectionFound = false;
                bool propertyFound = false;
                int sectionIndex = -1;

                for (int i = 0; i < lines.Count; i++)
                {
                    string line = lines[i].Trim();
                    if (line.Equals($"[{section}]", StringComparison.OrdinalIgnoreCase))
                    {
                        sectionFound = true;
                        sectionIndex = i;
                    }

                    if (sectionFound && line.StartsWith($"{property}=", StringComparison.OrdinalIgnoreCase))
                    {
                        lines[i] = $"{property}={value}";
                        propertyFound = true;
                        break;
                    }

                    if (sectionFound && line.StartsWith("[") && !line.Equals($"[{section}]"))
                    {
                        break;
                    }
                }

                if (!sectionFound)
                {
                    lines.Add($"[{section}]");
                    lines.Add($"{property}={value}");
                }
                else if (!propertyFound)
                {
                    lines.Insert(sectionIndex + 1, $"{property}={value}");
                }

                File.WriteAllLines(filePath, lines);
            }
            catch (Exception) 
            {
                throw new Exception("Unable to write value in Global Configuration.");
            }
        }
    }
}
