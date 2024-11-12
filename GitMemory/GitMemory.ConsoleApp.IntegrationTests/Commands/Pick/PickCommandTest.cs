using GitMemory.ConsoleApp.IntegrationTests.Configuration;
using GitMemory.CultureConfig;
using Xunit.Priority;

namespace GitMemory.ConsoleApp.IntegrationTests.Commands.Pick
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class PickCommandTest : IClassFixture<PickCommandTestFixture>
    {
        private readonly PickCommandTestFixture _commandTestFixture;

        public PickCommandTest(PickCommandTestFixture pickCommandTestFixture)
        {
            this._commandTestFixture=pickCommandTestFixture;
        }

        [Fact, Priority(1)]
        public async void TestPickCommit_Success()
        {
            //Arrange
            var expectedResult = ResourceMessages.Services_Pick_Success;

            //Act
            await ProgramTest.MainTestAsync(new string[7]{ "pick", _commandTestFixture.Hash1, _commandTestFixture.Hash2,
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
        public async void TestPickCommit_WithUnstagedCommits_SaysYes_Success()
        {
            //Arrange
            var warnsAlreadyAddedCommits = ResourceMessages.Handlers_Pick_WarningCommitsInList;
            var warnCommit1 = string.Format(ResourceMessages.Handlers_Pick_NotStagedItem, _commandTestFixture.Hash1);
            var warnCommit2 = string.Format(ResourceMessages.Handlers_Pick_NotStagedItem, _commandTestFixture.Hash2);
            var asksToClearList = "\n" + ResourceMessages.Handlers_Pick_RequestClearList;
            Interactions.DialogResultRequest.Enqueue(Domain.Entities.Enums.DialogResultEnum.Yes);
            var expectedResult = ResourceMessages.Services_Pick_Success;
            

            //Act
            await ProgramTest.MainTestAsync(new string[7]{ "pick", _commandTestFixture.Hash1, _commandTestFixture.Hash2,
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder});

            //Assert
            Assert.Equal(warnsAlreadyAddedCommits, Interactions.Output.Dequeue().Message);
            Assert.Equal(warnCommit1, Interactions.Output.Dequeue().Message);
            Assert.Equal(warnCommit2, Interactions.Output.Dequeue().Message);
            Assert.Equal(asksToClearList, Interactions.Output.Dequeue().Message);
            var actualResult = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, actualResult.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, actualResult.ResponseType);
            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);
            //TODO: add check for current commits in stage when status command get created
        }

        [Fact, Priority(3)]
        public async void TestPickCommit_WithUnstagedCommits_SaysNo_Success()
        {
            //Arrange
            var warnsAlreadyAddedCommits = ResourceMessages.Handlers_Pick_WarningCommitsInList;
            var warnCommit1 = string.Format(ResourceMessages.Handlers_Pick_NotStagedItem, _commandTestFixture.Hash1);
            var warnCommit2 = string.Format(ResourceMessages.Handlers_Pick_NotStagedItem, _commandTestFixture.Hash2);
            var asksToClearList = "\n" + ResourceMessages.Handlers_Pick_RequestClearList;
            Interactions.DialogResultRequest.Enqueue(Domain.Entities.Enums.DialogResultEnum.No);
            var expectedResult = ResourceMessages.Services_Pick_Success;


            //Act
            await ProgramTest.MainTestAsync(new string[7]{ "pick", _commandTestFixture.Hash1, _commandTestFixture.Hash2,
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder});

            //Assert
            Assert.Equal(warnsAlreadyAddedCommits, Interactions.Output.Dequeue().Message);
            Assert.Equal(warnCommit1, Interactions.Output.Dequeue().Message);
            Assert.Equal(warnCommit2, Interactions.Output.Dequeue().Message);
            Assert.Equal(asksToClearList, Interactions.Output.Dequeue().Message);
            var actualResult = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, actualResult.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, actualResult.ResponseType);
            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);
            //TODO: add check for current commits in stage when status command get created
        }

        [Fact, Priority(4)]
        public async void TestPickCommitByHash_WithUnstagedCommits_Success()
        {
            //Arrange
            var numberOfCommitsToGet = "12";
            var warnsAlreadyAddedCommits = ResourceMessages.Handlers_Pick_WarningCommitsInList;
            var warnCommit1 = string.Format(ResourceMessages.Handlers_Pick_NotStagedItem, _commandTestFixture.Hash1);
            var warnCommit2 = string.Format(ResourceMessages.Handlers_Pick_NotStagedItem, _commandTestFixture.Hash2);
            var asksToClearList = "\n" + ResourceMessages.Handlers_Pick_RequestClearList;
            Interactions.DialogResultRequest.Enqueue(Domain.Entities.Enums.DialogResultEnum.Yes);
            var expectedResult = ResourceMessages.Services_Pick_Success;


            //Act
            await ProgramTest.MainTestAsync(new string[6]{ "pick", numberOfCommitsToGet,
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder});

            //Assert
            Assert.Equal(warnsAlreadyAddedCommits, Interactions.Output.Dequeue().Message);
            Assert.Equal(warnCommit1, Interactions.Output.Dequeue().Message);
            Assert.Equal(warnCommit2, Interactions.Output.Dequeue().Message);
            Assert.Equal(asksToClearList, Interactions.Output.Dequeue().Message);
            var actualResult = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, actualResult.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, actualResult.ResponseType);
            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);
            //TODO: add check for current commits in stage when status command get created
        }
    }
}
