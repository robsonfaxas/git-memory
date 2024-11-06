using GitMemory.ConsoleApp.IntegrationTests.Configuration;
using Xunit.Priority;


namespace GitMemory.ConsoleApp.IntegrationTests.Commands.ErrorLog
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class ErrorLogCommandTest : IClassFixture<ErrorLogCommandTestFixture>
    {
        private ErrorLogCommandTestFixture _commandTestFixture { get; set; }
        public ErrorLogCommandTest(ErrorLogCommandTestFixture commandTestFixture)
        {
            _commandTestFixture = commandTestFixture;
        }

        [Fact, Priority(0)]
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

        [Fact, Priority(2)]
        public async void TestErrorLogCommandWithEnableArgument_ReturnsEnabledStatus()
        {
            //Arrange
            var expectedOutput = "Error Logs enabled.";

            var expectedResult = $"Error Log is currently ENABLED. Use `--enable` or `--disable` to change the status.";

            //Act
            await ProgramTest.MainTestAsync(new string[6]{ "errorLog","--enable",
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder});

            await ProgramTest.MainTestAsync(new string[5]{ "errorLog",
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder});

            //Assert
            var output = Interactions.Output.Dequeue();
            Assert.Equal(expectedOutput, output.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, output.ResponseType);

            var result = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, result.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, result.ResponseType);

            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);
        }

        [Fact, Priority(3)]
        public async void TestErrorLogCommandWithDisableArgument_ReturnsDisabledStatus()
        {
            //Arrange
            var expectedOutput = "Error Logs disabled.";

            var expectedResult = $"Error Log is currently DISABLED. Use `--enable` or `--disable` to change the status.";

            //Act
            await ProgramTest.MainTestAsync(new string[6]{ "errorLog","--disable",
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder});

            await ProgramTest.MainTestAsync(new string[5]{ "errorLog",
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder});

            //Assert
            var output = Interactions.Output.Dequeue();
            Assert.Equal(expectedOutput, output.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, output.ResponseType);

            var result = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, result.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, result.ResponseType);

            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);
        }

        [Fact, Priority(4)]
        public async void TestErrorLogCommandWithInvalidArgument_ReturnsError()
        {
            //Arrange
            var expectedOutput = "Invalid parameter --test_invalid_argument";

            //Act
            await ProgramTest.MainTestAsync(new string[6]{ "errorLog","--test_invalid_argument",
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder});

            //Assert
            var output = Interactions.Output.Dequeue();
            Assert.Equal(expectedOutput, output.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Error, output.ResponseType);
            
            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);
        }
    }
}