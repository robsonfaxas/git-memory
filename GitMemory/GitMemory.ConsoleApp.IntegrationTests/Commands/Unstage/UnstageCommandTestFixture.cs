using GitMemory.ConsoleApp.IntegrationTests.Configuration;

namespace GitMemory.ConsoleApp.IntegrationTests.Commands.Unstage
{
    public class UnstageCommandTestFixture : CommandTestFixture
    {
        public readonly string Hash1 = "2181e36dda5a84947fb98656cd2a78810bfbf85a";
        public readonly string Hash2 = "0693eb213621d7014858f6537376efb1e62e7c29";
        public override async Task DisposeAsync()
        {
            await base.DisposeAsync();
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await ProgramTest.MainTestAsync(new string[6]{ "set-brain", RepoDirectory,
                                                            "--GlobalSettingsFolder", GlobalSettingsDirectory,
                                                            "--CurrentDirectory", CurrentDirectoryFolder});

            await ProgramTest.MainTestAsync(new string[7]{ "pick", Hash1, Hash2,
                                                            "--GlobalSettingsFolder", GlobalSettingsDirectory,
                                                            "--CurrentDirectory", CurrentDirectoryFolder});

            await ProgramTest.MainTestAsync(new string[7]{ "stage", Hash1, Hash2,
                                                            "--GlobalSettingsFolder", GlobalSettingsDirectory,
                                                            "--CurrentDirectory", CurrentDirectoryFolder});

            Interactions.Output.Clear();
            Interactions.DialogResultRequest.Clear();
            Interactions.StringRequest.Clear();
            Console.WriteLine($"[Unstage] Temp Directory: {TempDirectory}");
        }
    }
}
