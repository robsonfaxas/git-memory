using GitMemory.ConsoleApp.IntegrationTests.Configuration;

namespace GitMemory.ConsoleApp.IntegrationTests.Commands
{
    public abstract class CommandTestFixture : IAsyncLifetime
    {
        public string TempDirectory { get; set; } = string.Empty;
        public string GlobalSettingsDirectory { get; set; } = string.Empty;
        public string RepoDirectory { get; set; } = string.Empty;
        public string CurrentDirectoryFolder { get; set; } = string.Empty;
        public Interactions Interactions { get; set; } = new Interactions();

        public virtual Task DisposeAsync()
        {
            if (Directory.Exists(TempDirectory))
            {
                Directory.Delete(TempDirectory, true);
            }
            Interactions.Output.Clear();
            Interactions.DialogResultRequest.Clear();
            Interactions.StringRequest.Clear();
            return Task.CompletedTask;
        }

        public virtual Task InitializeAsync()
        {
            TempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(TempDirectory);

            GlobalSettingsDirectory = Path.Combine(TempDirectory, "globalconfig");
            Directory.CreateDirectory(GlobalSettingsDirectory);

            RepoDirectory = Path.Combine(TempDirectory, "repo");
            Directory.CreateDirectory(RepoDirectory);

            CurrentDirectoryFolder = Directory.GetCurrentDirectory() ?? "";
            return Task.CompletedTask;
        }
    }
}
