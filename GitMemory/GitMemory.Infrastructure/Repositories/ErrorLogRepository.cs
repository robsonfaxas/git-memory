using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Memories;
using GitMemory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GitMemory.Infrastructure.Repositories
{
    public class ErrorLogRepository : IErrorLogRepository
    {
        private readonly string _filePath;
        private readonly bool _errorLogsEnabled;
        public ErrorLogRepository(IGitMemoryGlobalSettings globalSettings)
        {
            _filePath = globalSettings.ReadGlobalSettings().RepositoryLocation ?? CommandContextAccessor.Current.CurrentDirectory;
            _filePath += $"\\{DateTime.Now:yyyy-MM-dd}-errors.log";
            _errorLogsEnabled = globalSettings.ReadGlobalSettings().IsErrorLogsEnabled;
        }

        public void Log(string message)
        {
            if (!_errorLogsEnabled)
                return;
            if (String.IsNullOrEmpty(_filePath))
                return;
            try
            {
                var directory = Path.GetDirectoryName(_filePath);
                if (!Directory.Exists(directory))
                {
                    if (directory == null)
                        return;
                    Directory.CreateDirectory(directory);
                }                
                using (StreamWriter writer = new StreamWriter(_filePath, true))
                {
                    writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                }
            }
            catch (Exception)
            {
                throw new Exception("Unable to write in error log file. Execute the command 'git memory errorlog --disable' to turn error logs off");
            }
        }

        public void Log(Exception ex)
        {
            if (!_errorLogsEnabled)
                return;
            string exceptionMessage = FormatException(ex);
            Log(exceptionMessage);
        }

        private string FormatException(Exception ex)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Exception: {ex.GetType().FullName}");
            sb.AppendLine($"Message: {ex.Message}");
            sb.AppendLine($"Source: {ex.Source}");
            sb.AppendLine($"Stack Trace: {ex.StackTrace}");

            if (ex.InnerException != null)
            {
                sb.AppendLine("Inner Exception:");
                sb.AppendLine($"Exception: {ex.InnerException.GetType().FullName}");
                sb.AppendLine($"Message: {ex.InnerException.Message}");
                sb.AppendLine($"Source: {ex.InnerException.Source}");
                sb.AppendLine($"Stack Trace: {ex.InnerException.StackTrace}");
            }
            return sb.ToString();
        }
    }
}
