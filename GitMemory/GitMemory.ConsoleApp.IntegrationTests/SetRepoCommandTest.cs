using GitMemory.ConsoleApp.IntegrationTests.Configuration;
using GitMemory.ConsoleApp.IntegrationTests.Fixtures;
using System.Security.AccessControl;
using System.Security.Principal;

[assembly: TestCaseOrderer("GitMemory.ConsoleApp.IntegrationTests.Configuration.PriorityOrderer", "GitMemory.ConsoleApp.IntegrationTests")]
namespace GitMemory.ConsoleApp.IntegrationTests
{
    public class SetRepoCommandTest : IClassFixture<CommandTestFixture>
    {
        private CommandTestFixture _commandTestFixture { get; set; }
        public SetRepoCommandTest(CommandTestFixture commandTestFixture)
        {
            this._commandTestFixture = commandTestFixture;
        }

        [Fact, TestPriority(1)]
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

        [Fact, TestPriority(2)]
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

        [Fact, TestPriority(3)]
        public async void TestSetAnotherRepo_UserSaysYes_CreatesRepo()
        {
            // Arrange
            var repoDirectory2 = Path.Combine(_commandTestFixture.TempDirectory, "repo2");
            Directory.CreateDirectory(repoDirectory2);
            Interactions.DialogResultRequest.Enqueue(Domain.Entities.Enums.DialogResultEnum.Yes);
            string currentRepoWarning = $"Current Repository Location: {_commandTestFixture.RepoDirectory}"; // current applied repo check
            string expectedResult = "Folder created successfully.";

            // Act
            await ProgramTest.MainTestAsync(new string[6] { "set-repo", repoDirectory2, 
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory, 
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder });

            // Assert
            var actualCurrentRepoWarning = Interactions.Output.Dequeue();
            Assert.Equal(currentRepoWarning, actualCurrentRepoWarning.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, actualCurrentRepoWarning.ResponseType);

            var actualResultOutput = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, actualResultOutput.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, actualResultOutput.ResponseType);

            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);

            // clean up
            _commandTestFixture.RepoDirectory = repoDirectory2;
        }        

        [Fact, TestPriority(4)]
        public async void TestSetAnotherRepo_UserSaysNo_Cancels()
        {
            // Arrange
            var repoDirectory3 = Path.Combine(_commandTestFixture.TempDirectory, "repo3");
            Directory.CreateDirectory(repoDirectory3);
            Interactions.DialogResultRequest.Enqueue(Domain.Entities.Enums.DialogResultEnum.No); // User Says No
            string expectedRepoWarning = $"Current Repository Location: {_commandTestFixture.RepoDirectory}"; // current applied repo check
            string expectedResult = "No repository changes.";

            // Act
            await ProgramTest.MainTestAsync(new string[6] { "set-repo", repoDirectory3,
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder });

            // Assert
            var actualRepoWarning = Interactions.Output.Dequeue();
            Assert.Equal(expectedRepoWarning, actualRepoWarning.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, actualRepoWarning.ResponseType);

            var actualResultOutput = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, actualResultOutput.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, actualResultOutput.ResponseType);

            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);
        }

        [Fact, TestPriority(5)]
        public async void TestSetAnotherRepoWithoutPermissionToCreate_UserSaysYes_ReturnsError()
        {
            // Arrange
            var repoDirectory3 = Path.Combine(_commandTestFixture.TempDirectory, "repo3");
            _commandTestFixture.CreateFolderWithPermissionToCreateSubFolder(repoDirectory3);
            Interactions.DialogResultRequest.Enqueue(Domain.Entities.Enums.DialogResultEnum.Yes);
            string currentRepoWarning = $"Current Repository Location: {_commandTestFixture.RepoDirectory}"; // current applied repo check
            string expectedResult = $"Error creating directory in {repoDirectory3}";

            // Act
            await ProgramTest.MainTestAsync(new string[6] { "set-repo", repoDirectory3,
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder });

            // Assert
            var actualCurrentRepoWarning = Interactions.Output.Dequeue();
            Assert.Equal(currentRepoWarning, actualCurrentRepoWarning.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, actualCurrentRepoWarning.ResponseType);

            var actualResultOutput = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, actualResultOutput.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Error, actualResultOutput.ResponseType);

            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);
        }

        [Fact, TestPriority(6)]
        public async void TestSetAnotherRepoUsingDot_UserSaysYes_CreatesRepo()
        {
            // Arrange
            var repoDirectoryDot = ".";
            Directory.CreateDirectory(repoDirectoryDot);
            Interactions.DialogResultRequest.Enqueue(Domain.Entities.Enums.DialogResultEnum.Yes);
            string currentRepoWarning = $"Current Repository Location: {_commandTestFixture.RepoDirectory}"; // current applied repo check
            string expectedResult = "Folder created successfully.";

            // Act
            await ProgramTest.MainTestAsync(new string[6] { "set-repo", repoDirectoryDot,
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder });

            // Assert
            var actualCurrentRepoWarning = Interactions.Output.Dequeue();
            Assert.Equal(currentRepoWarning, actualCurrentRepoWarning.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, actualCurrentRepoWarning.ResponseType);

            var actualResultOutput = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, actualResultOutput.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, actualResultOutput.ResponseType);

            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);

            // clean up
            _commandTestFixture.RepoDirectory = repoDirectoryDot;
        }
    }
}