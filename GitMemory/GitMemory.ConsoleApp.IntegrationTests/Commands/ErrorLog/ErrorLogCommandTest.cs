using GitMemory.ConsoleApp.IntegrationTests.Configuration;
using Xunit.Priority;


namespace GitMemory.ConsoleApp.IntegrationTests.Commands.SetRepo
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class ErrorLogCommandTest : IClassFixture<ErrorLogCommandTestFixture>
    {
        private ErrorLogCommandTestFixture _commandTestFixture { get; set; }
        public ErrorLogCommandTest(ErrorLogCommandTestFixture commandTestFixture)
        {
            _commandTestFixture = commandTestFixture;
        }

        [Fact, Priority(101)]
        public async void TestErrorLogCommandWithoutArguments_ReturnsCurrentStatus()
        {
            //Arrange
            var expectedResult = $"Error Log is currently DISABLED. Use `--enable` or `--disable` to change the status.";

            //Act
            await ProgramTest.MainTestAsync(new string[5]{ "errorLog",
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder});

            //Assert
            var output = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, output.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, output.ResponseType);
            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);
        }        
    }
}