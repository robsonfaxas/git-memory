using GitMemory.ConsoleApp.IntegrationTests.Configuration;

namespace GitMemory.ConsoleApp.IntegrationTests.Commands.Status
{
    public class StatusCommandTestFixture : CommandTestFixture
    {
        public readonly string Hash1 = "9330a54ba335a2431268a7ccb05f43975ef06b71";
        public readonly string DescriptionCommit1 = "#4: added Stage and Unstage commands\n";

        public readonly string Hash2 = "68a86a59093fd7b671a08fdf0052debbc9a6fb95";
        public readonly string DescriptionCommit2 = "#21 - Multi-language support added and all messages adapted. New tests added using Resource messages\n";

        public override async Task DisposeAsync()
        {
            await base.DisposeAsync();
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await ProgramTest.MainTestAsync(new string[6]{ "set-repo", RepoDirectory,
                                                            "--GlobalSettingsFolder", GlobalSettingsDirectory,
                                                            "--CurrentDirectory", CurrentDirectoryFolder});
            Interactions.Output.Clear();
            Interactions.DialogResultRequest.Clear();
            Interactions.StringRequest.Clear();
            Console.WriteLine($"[Status] Temp Directory: {TempDirectory}");
        }

        public async Task PickCommit(string hash)
        {
            await ProgramTest.MainTestAsync(new string[6]{ "pick", hash,
                                                            "--GlobalSettingsFolder", GlobalSettingsDirectory,
                                                            "--CurrentDirectory", CurrentDirectoryFolder});
            Interactions.Output.Clear();
            Interactions.DialogResultRequest.Clear();
            Interactions.StringRequest.Clear();
        }

        public async Task UnpickCommit(string hash)
        {
            await ProgramTest.MainTestAsync(new string[6]{ "unpick", hash,
                                                            "--GlobalSettingsFolder", GlobalSettingsDirectory,
                                                            "--CurrentDirectory", CurrentDirectoryFolder});
            Interactions.Output.Clear();
            Interactions.DialogResultRequest.Clear();
            Interactions.StringRequest.Clear();
        }

        public async Task StageCommit(string hash)
        {
            await ProgramTest.MainTestAsync(new string[6]{ "stage", hash,
                                                            "--GlobalSettingsFolder", GlobalSettingsDirectory,
                                                            "--CurrentDirectory", CurrentDirectoryFolder});
            Interactions.Output.Clear();
            Interactions.DialogResultRequest.Clear();
            Interactions.StringRequest.Clear();
        }

        public async Task UnstageCommit(string hash)
        {
            await ProgramTest.MainTestAsync(new string[6]{ "unstage", hash,
                                                            "--GlobalSettingsFolder", GlobalSettingsDirectory,
                                                            "--CurrentDirectory", CurrentDirectoryFolder});
            Interactions.Output.Clear();
            Interactions.DialogResultRequest.Clear();
            Interactions.StringRequest.Clear();
        }

       

    }
}
