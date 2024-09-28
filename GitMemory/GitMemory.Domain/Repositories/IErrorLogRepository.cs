using GitMemory.Domain.Entities.Memories;

namespace GitMemory.Domain.Repositories
{
    public interface IErrorLogRepository
    {
        void Log(string message);
        void Log(Exception ex);
    }
}
