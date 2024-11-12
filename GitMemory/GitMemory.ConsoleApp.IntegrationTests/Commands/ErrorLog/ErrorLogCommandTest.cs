using GitMemory.ConsoleApp.IntegrationTests.Configuration;
using GitMemory.CultureConfig;
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

        [Fact, Priority(1)]
        public async void TestErrorLogCommandWithoutArguments_ReturnsCurrentStatus()
        {
            //Arrange
            var expectedResult = string.Format(ResourceMessages.Handlers_ErrorLog_StatusMessage, "DISABLED");

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
        public async void TestErrorLogCommandWithDisableArgument_ReturnsDisabledStatus()
        {
            //Arrange
            var expectedOutput = ResourceMessages.Services_Settings_DisableErrorLogResult;

            var expectedResult = string.Format(ResourceMessages.Handlers_ErrorLog_StatusMessage, "DISABLED");

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

        [Fact, Priority(3)]
        public async void TestErrorLogCommandWithEnableArgument_ReturnsEnabledStatus()
        {
            //Arrange
            var expectedOutput = ResourceMessages.Services_Settings_EnableErrorLogResult;

            var expectedResult = string.Format(ResourceMessages.Handlers_ErrorLog_StatusMessage, "ENABLED");

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

        [Fact, Priority(4)]
        public async void TestErrorLogCommandWithInvalidArgument_ReturnsError()
        {
            //Arrange
            var inputInvalidArgument = "--test_invalid_argument";            
            var expectedOutput = string.Format(ResourceMessages.Handlers_ErrorLog_InvalidParameter, inputInvalidArgument);

            //Act
            await ProgramTest.MainTestAsync(new string[6]{ "errorLog",inputInvalidArgument,
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

        [Fact, Priority(5)]
        public async void TestPickCommit_NoGitRepository_CreatesErrorLogFile()
        {
            //Arrange
            var hash1 = "2181e36dda5a84947fb98656cd2a78810bfbf85a";
            var hash2 = "0693eb213621d7014858f6537376efb1e62e7c29";
            var expectedResult = ResourceMessages.Services_PickByList_InvalidGitRepository;

            //Act
            await ProgramTest.MainTestAsync(new string[7]{ "pick", hash1, hash2,
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.GlobalSettingsDirectory});
            string[] expectedLogFileInRepoFolder = Directory.GetFiles(_commandTestFixture.RepoDirectory, "*.log");

            //Assert
            var output = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, output.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Error, output.ResponseType);
            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);
            Assert.Single(expectedLogFileInRepoFolder);
        }

    }
}