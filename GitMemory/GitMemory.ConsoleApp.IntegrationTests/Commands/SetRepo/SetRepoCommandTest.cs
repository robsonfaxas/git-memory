using GitMemory.ConsoleApp.IntegrationTests.Configuration;
using Xunit.Priority;

namespace GitMemory.ConsoleApp.IntegrationTests.Commands.SetRepo
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class SetRepoCommandTest : IClassFixture<SetRepoCommandTestFixture>
    {
        private SetRepoCommandTestFixture _commandTestFixture { get; set; }
        public SetRepoCommandTest(SetRepoCommandTestFixture commandTestFixture)
        {
            _commandTestFixture = commandTestFixture;
        }

        [Fact, Priority(1)]
        public async void TestSetRepoWithoutArguments_ReturnErrorMessage()
        {
            //Arrange
            var expectedResult = "No arguments provided.";

            //Act
            await ProgramTest.MainTestAsync(new string[5]{ "set-repo",
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder});

            //Assert
            var output = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, output.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Error, output.ResponseType);
            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);
        }

        [Fact, Priority(2)]
        public async void TestSetRepo_CreatesRepo()
        {
            //Arrange
            var expectedResult = "Folder created successfully.";

            //Act
            await ProgramTest.MainTestAsync(new string[6]{ "set-repo", _commandTestFixture.RepoDirectory,
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

        [Fact, Priority(3)]
        public async void TestSetAnotherRepo_UserSaysYes_CreatesRepo()
        {
            // Arrange
            var repoDirectory2 = Path.Combine(_commandTestFixture.TempDirectory, "repo2");
            Directory.CreateDirectory(repoDirectory2);
            string expectedQuestion = "Warning: A repository is already set in another location. Do you want to overwrite the current location set?";
            Interactions.DialogResultRequest.Enqueue(Domain.Entities.Enums.DialogResultEnum.Yes);
            string expectedAnswer = $"Current Repository Location: {_commandTestFixture.RepoDirectory}"; // current applied repo check
            string expectedResult = "Folder created successfully.";

            // Act
            await ProgramTest.MainTestAsync(new string[6] { "set-repo", repoDirectory2,
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder });

            // Assert
            var actualQuestion = Interactions.Output.Dequeue();
            Assert.Equal(expectedQuestion, actualQuestion.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Warning, actualQuestion.ResponseType);

            var actualAnswer = Interactions.Output.Dequeue();
            Assert.Equal(expectedAnswer, actualAnswer.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, actualAnswer.ResponseType);

            var actualResult = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, actualResult.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, actualResult.ResponseType);

            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);

            // reassign repo location
            _commandTestFixture.RepoDirectory = repoDirectory2;
        }

        [Fact, Priority(4)]
        public async void TestSetAnotherRepo_UserSaysNo_Cancels()
        {
            // Arrange
            var repoDirectory3 = Path.Combine(_commandTestFixture.TempDirectory, "repo3");
            Directory.CreateDirectory(repoDirectory3);
            string expectedQuestion = "Warning: A repository is already set in another location. Do you want to overwrite the current location set?";
            Interactions.DialogResultRequest.Enqueue(Domain.Entities.Enums.DialogResultEnum.No); // User Says No
            string expectedAnswer = $"Current Repository Location: {_commandTestFixture.RepoDirectory}"; // current applied repo check
            string expectedResult = "No repository changes.";

            // Act
            await ProgramTest.MainTestAsync(new string[6] { "set-repo", repoDirectory3,
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder });

            // Assert
            var actualQuestion = Interactions.Output.Dequeue();
            Assert.Equal(expectedQuestion, actualQuestion.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Warning, actualQuestion.ResponseType);

            var actualAnswer = Interactions.Output.Dequeue();
            Assert.Equal(expectedAnswer, actualAnswer.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, actualAnswer.ResponseType);

            var actualResultOutput = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, actualResultOutput.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, actualResultOutput.ResponseType);

            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);
        }

        [Fact, Priority(5)]
        public async void TestSetAnotherRepoUsingDot_UserSaysYes_CreatesRepo()
        {
            // Arrange
            var repoDirectoryDot = ".";
            Directory.CreateDirectory(repoDirectoryDot);
            string expectedQuestion = "Warning: A repository is already set in another location. Do you want to overwrite the current location set?";
            Interactions.DialogResultRequest.Enqueue(Domain.Entities.Enums.DialogResultEnum.Yes);
            string expectedAnswer = $"Current Repository Location: {_commandTestFixture.RepoDirectory}"; // current applied repo check
            string expectedResult = "Folder created successfully.";

            // Act
            await ProgramTest.MainTestAsync(new string[6] { "set-repo", repoDirectoryDot,
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder });

            // Assert
            var actualQuestion = Interactions.Output.Dequeue();
            Assert.Equal(expectedQuestion, actualQuestion.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Warning, actualQuestion.ResponseType);

            var actualCurrentRepoWarning = Interactions.Output.Dequeue();
            Assert.Equal(expectedAnswer, actualCurrentRepoWarning.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, actualCurrentRepoWarning.ResponseType);

            var actualResult = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, actualResult.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, actualResult.ResponseType);

            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);

            // clean up
            _commandTestFixture.RepoDirectory = repoDirectoryDot;
        }
    }
}