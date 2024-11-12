namespace GitMemory.ConsoleApp.IntegrationTests.Commands.SetRepo
{
    public class SetRepoCommandTestFixture : CommandTestFixture
    {
        public override async Task DisposeAsync()
        {
            await base.DisposeAsync();
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            Console.WriteLine($"[SetRepo] Temp Directory: {TempDirectory}");
        }
    }
}
