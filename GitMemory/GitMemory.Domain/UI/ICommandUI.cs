namespace GitMemory.Domain.UI
{
    public interface ICommandUI
    {
        Task Run();
        List<string> Args { get; set; }
    }
}
