using GitMemory.ConsoleApp.IntegrationTests.Configuration;
using GitMemory.CultureConfig;
using Xunit.Priority;

namespace GitMemory.ConsoleApp.IntegrationTests.Commands.Status
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class StatusCommandTest : IClassFixture<StatusCommandTestFixture>
    {
        private readonly StatusCommandTestFixture _commandTestFixture;

        public StatusCommandTest(StatusCommandTestFixture statusCommandTestFixture)
        {
            this._commandTestFixture=statusCommandTestFixture;
        }

        [Fact, Priority(1)]
        public async void TestStatus_NoCommits()
        {
            //Arrange
            await _commandTestFixture.PickCommit(_commandTestFixture.Hash1);
            await _commandTestFixture.UnpickCommit(_commandTestFixture.Hash1);

            var expectedResult = ResourceMessages.Services_Status_NoCommits;            

            //Act
            await ProgramTest.MainTestAsync(new string[5]{ "status",
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
        public async void TestStatus_Unstaged_Success()
        {
            //Arrange
            await _commandTestFixture.PickCommit(_commandTestFixture.Hash1); // pick sends directly to unstaged      
            var expectedHeaderUnstaged = ResourceMessages.Services_Status_UnstagedHeader;
            var expectedItemUnstaged = string.Format(ResourceMessages.Services_Status_UnstagedItem, 
                                                            _commandTestFixture.DescriptionCommit1,
                                                            _commandTestFixture.Hash1);
            var expectedResult = ResourceMessages.Services_Status_End;

            //Act
            await ProgramTest.MainTestAsync(new string[5]{ "status",
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder});

            //Assert
            var headerUnstaged = Interactions.Output.Dequeue();
            Assert.Equal(expectedHeaderUnstaged, headerUnstaged.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, headerUnstaged.ResponseType);

            var itemUnstaged = Interactions.Output.Dequeue();
            Assert.Equal(expectedItemUnstaged, itemUnstaged.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, itemUnstaged.ResponseType);

            var actualResult = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, actualResult.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, actualResult.ResponseType);

            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);
        }

        [Fact, Priority(3)]
        public async void TestStatus_Staged_Success()
        {
            //Arrange
            await _commandTestFixture.StageCommit(_commandTestFixture.Hash1); // sends to staged list
            var expectedHeaderStaged = ResourceMessages.Services_Status_StagedHeader;
            var expectedItemStaged = string.Format(ResourceMessages.Services_Status_StagedItem,
                                                            _commandTestFixture.DescriptionCommit1,
                                                            _commandTestFixture.Hash1);
            var expectedResult = ResourceMessages.Services_Status_End;

            //Act
            await ProgramTest.MainTestAsync(new string[5]{ "status",
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder});

            //Assert
            var headerStaged = Interactions.Output.Dequeue();
            Assert.Equal(expectedHeaderStaged, headerStaged.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, headerStaged.ResponseType);

            var itemStaged = Interactions.Output.Dequeue();
            Assert.Equal(expectedItemStaged, itemStaged.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, itemStaged.ResponseType);

            var actualResult = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, actualResult.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, actualResult.ResponseType);

            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);
        }

        [Fact, Priority(4)]
        public async void TestStatus_MixedCommits_Success()
        {
            //ARRANGE     -- [last test kept one in Stage area, so it needs one more commit, now to Unstaged area]
            
            await _commandTestFixture.PickCommit(_commandTestFixture.Hash2); // pick sends directly to unstaged      

                //  expected in STAGED area
            var expectedHeaderStaged = ResourceMessages.Services_Status_StagedHeader;
            var expectedItemStaged = string.Format(ResourceMessages.Services_Status_StagedItem,
                                                            _commandTestFixture.DescriptionCommit1,
                                                            _commandTestFixture.Hash1);

                //  expected in UNSTAGED area
            var expectedHeaderUnstaged = ResourceMessages.Services_Status_UnstagedHeader;
            var expectedItemUnstaged = string.Format(ResourceMessages.Services_Status_UnstagedItem,
                                                            _commandTestFixture.DescriptionCommit2,
                                                            _commandTestFixture.Hash2);
                //  expected RESULT
            var expectedResult = ResourceMessages.Services_Status_End;

            //ACT

            await ProgramTest.MainTestAsync(new string[5]{ "status",
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder});

            //ASSERT
                // assert STAGED
            var headerStaged = Interactions.Output.Dequeue();
            Assert.Equal(expectedHeaderStaged, headerStaged.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, headerStaged.ResponseType);

            var itemStaged = Interactions.Output.Dequeue();
            Assert.Equal(expectedItemStaged, itemStaged.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, itemStaged.ResponseType);
                
                // assert UNSTAGED
            var headerUnstaged = Interactions.Output.Dequeue();
            Assert.Equal(expectedHeaderUnstaged, headerUnstaged.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, headerUnstaged.ResponseType);

            var itemUnstaged = Interactions.Output.Dequeue();
            Assert.Equal(expectedItemUnstaged, itemUnstaged.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, itemUnstaged.ResponseType);

                // assert RESULT
            var actualResult = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, actualResult.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, actualResult.ResponseType);

            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);
        }

    }
}
