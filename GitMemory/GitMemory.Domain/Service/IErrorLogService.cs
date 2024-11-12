namespace GitMemory.Domain.Service
{
    public interface IErrorLogService
    {
        void Log(string message);
        void Log(Exception ex);
    }
}
