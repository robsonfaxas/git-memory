using GitMemory.ConsoleApp.IntegrationTests.Configuration;
using System.Security.AccessControl;
using System.Security.Principal;

namespace GitMemory.ConsoleApp.IntegrationTests.Commands.ErrorLog
{
    public class ErrorLogCommandTestFixture : CommandTestFixture
    {
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
            Console.WriteLine($"[ErrorLog] Temp Directory: {TempDirectory}");
        }
    }
}
