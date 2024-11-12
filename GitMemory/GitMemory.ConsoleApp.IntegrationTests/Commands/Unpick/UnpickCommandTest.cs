using GitMemory.ConsoleApp.IntegrationTests.Configuration;
using GitMemory.CultureConfig;
using Xunit.Priority;

namespace GitMemory.ConsoleApp.IntegrationTests.Commands.Unpick
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class UnpickCommandTest : IClassFixture<UnpickCommandTestFixture>
    {
        private readonly UnpickCommandTestFixture _commandTestFixture;

        public UnpickCommandTest(UnpickCommandTestFixture UnpickCommandTestFixture)
        {
            this._commandTestFixture=UnpickCommandTestFixture;
        }

        [Fact, Priority(1)]
        public async void TestUnpickCommit_ByHash_Success()
        {
            //Arrange
            var expectedResult = ResourceMessages.Services_Unpick_Success;

            //Act
            await ProgramTest.MainTestAsync(new string[7]{ "Unpick", _commandTestFixture.Hash1, _commandTestFixture.Hash2,
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder});

            //Assert
            var output = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, output.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, output.ResponseType);
            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);
            //TODO: add check for current commits in stage when status command get created
        }

        [Fact, Priority(2)]
        public async void TestUnpickCommit_ByAllOperator_Success()
        {
            //Arrange
            var expectedResult = ResourceMessages.Services_UnpickAll_Success;
            await _commandTestFixture.PickDefaultCommitsAsync();
            //Act
            await ProgramTest.MainTestAsync(new string[6]{ "Unpick", ".",
                                                            "--GlobalSettingsFolder", _commandTestFixture.GlobalSettingsDirectory,
                                                            "--CurrentDirectory", _commandTestFixture.CurrentDirectoryFolder});

            //Assert
            var output = Interactions.Output.Dequeue();
            Assert.Equal(expectedResult, output.Message);
            Assert.Equal(Domain.Entities.Enums.ResponseTypeEnum.Info, output.ResponseType);
            Assert.Empty(Interactions.Output);
            Assert.Empty(Interactions.StringRequest);
            Assert.Empty(Interactions.DialogResultRequest);
            //TODO: add check for current commits in stage when status command get created
        }

    }
}
