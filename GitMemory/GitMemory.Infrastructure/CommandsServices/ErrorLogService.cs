using GitMemory.Domain.Entities.Memories;
using GitMemory.Domain.Repositories;
using GitMemory.Domain.Service;

namespace GitMemory.Infrastructure.CommandsServices
{
    public class ErrorLogService : IErrorLogService
    {
        private readonly IErrorLogRepository _errorLogRepository;
        public ErrorLogService(IErrorLogRepository repository)
        {
            _errorLogRepository = repository;
        }

        public void Log(string message)
        {
            _errorLogRepository.Log(message);
        }

        public void Log(Exception ex)
        {
            _errorLogRepository.Log(ex);
        }
    }
}
