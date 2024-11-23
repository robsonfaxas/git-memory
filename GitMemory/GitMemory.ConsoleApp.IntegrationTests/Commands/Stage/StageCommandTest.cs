using GitMemory.ConsoleApp.IntegrationTests.Configuration;
using GitMemory.CultureConfig;
using Xunit.Priority;

namespace GitMemory.ConsoleApp.IntegrationTests.Commands.Stage
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class StageCommandTest : IClassFixture<StageCommandTestFixture>
    {
        private readonly StageCommandTestFixture _commandTestFixture;

        public StageCommandTest(StageCommandTestFixture pickCommandTestFixture)
        {
            this._commandTestFixture=pickCommandTestFixture;
        }

        [Fact, Priority(1)]
        public async void TestStage_ByHash_Success()
        {
            //Arrange
            var expectedInfoHash1 = string.Format(ResourceMessages.Services_Stage_StagedCommit, _commandTestFixture.Hash1);
            var expectedResultInfo = ResourceMessages.Services_Stage_Success;

            //Act
            await ProgramTest.MainTestAsync(new string[6]{ "stage", _commandTestFixture.Hash1,
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder});

            //Assert
            var hash1Info = Interactions.Output.Dequeue();
            Assert.Equal(expectedInfoHash1, hash1Info.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, hash1Info.ResponseType);

            var resultInfo = Interactions.Output.Dequeue();
            Assert.Equal(expectedResultInfo, resultInfo.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, resultInfo.ResponseType);

            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);
        }

        [Fact, Priority(2)]
        public async void TestStage_AllCommits_SuccessOneRemaining()
        {
            //Arrange
            var expectedInfoHash2 = string.Format(ResourceMessages.Services_Stage_StagedCommit, _commandTestFixture.Hash2);
            var expectedResult = ResourceMessages.Services_Stage_Success;

            //Act
            await ProgramTest.MainTestAsync(new string[6]{ "stage", "--all",
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder});

            //Assert
            var hash2Info = Interactions.Output.Dequeue();
            Assert.Equal(expectedInfoHash2, hash2Info.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, hash2Info.ResponseType);

            var output = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, output.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, output.ResponseType);
            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);
        }

        [Fact, Priority(3)]
        public async void TestStage_AllCommits_WithDot_SuccessNoCommitsAdded()
        {
            //Arrange
            var expectedResult = ResourceMessages.Services_Stage_SuccessZeroCommits;

            //Act
            await ProgramTest.MainTestAsync(new string[6]{ "stage", ".",
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

        [Fact, Priority(4)]
        public async void TestStage_WithInvalidHash_ReturnsError()
        {
            //Arrange
            var invalidHashCode = _commandTestFixture.Hash1.Insert(3, "K");
            var expectedErrorHash = string.Format(ResourceMessages.Services_Stage_InvalidHash, invalidHashCode);
            var expectedResult = ResourceMessages.Services_Stage_SuccessZeroCommits;

            //Act
            await ProgramTest.MainTestAsync(new string[6]{ "stage", invalidHashCode,
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder});

            //Assert
            var invalidHash = Interactions.Output.Dequeue();
            Assert.Equal(expectedErrorHash, invalidHash.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Error, invalidHash.ResponseType);

            var result = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, result.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, result.ResponseType);

            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);
        }

    }
}
