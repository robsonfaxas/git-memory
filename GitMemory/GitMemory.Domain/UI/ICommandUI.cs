using GitMemory.Domain.Entities;

namespace GitMemory.Domain.UI
{
    public interface ICommandUI
    {
        Task Run();
        List<string> Args { get; set; }
        CommandContextConfiguration SetContext(string userProfile, string currentDirectory);
    }
}
