namespace GitMemory.Application.Interfaces
{
    public interface IGitCommandFactory
    {
        IGitCommandRequest GetCommand(string commandName, List<string> parameters);
    }
}
