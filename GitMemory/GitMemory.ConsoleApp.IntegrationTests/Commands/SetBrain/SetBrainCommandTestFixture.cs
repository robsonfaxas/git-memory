namespace GitMemory.ConsoleApp.IntegrationTests.Commands.SetBrain
{
    public class SetBrainCommandTestFixture : CommandTestFixture
    {
        public override async Task DisposeAsync()
        {
            await base.DisposeAsync();
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            Console.WriteLine($"[SetBrain] Temp Directory: {TempDirectory}");
        }
    }
}
