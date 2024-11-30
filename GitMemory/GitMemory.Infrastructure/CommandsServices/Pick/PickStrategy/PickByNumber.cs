using GitMemory.CultureConfig;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Entities.Memories;
using GitMemory.Domain.Service;
using LibGit2Sharp;

namespace GitMemory.Infrastructure.CommandsServices.Pick.PickStrategy
{
    internal class PickByNumber : IPickStrategy
    {
        private readonly IMemoryPoolService _memoryPoolService;
        private readonly IErrorLogService _errorLogService;
        public PickByNumber(IMemoryPoolService memoryPoolService, IErrorLogService errorLogService)
        {
            _memoryPoolService = memoryPoolService;
            _errorLogService = errorLogService;
        }
        public Task<Command> Execute(List<string> values, MemoryPool memoryPool)
        {
            var hashes = new List<string>();
            try
            {                
                var parsed = int.TryParse(values.FirstOrDefault(), out int numberOfLastCommits);
                if (!parsed)
                    throw new ArgumentException(ResourceMessages.Services_PickByNumber_Invalid);
                hashes = GetCommitHashes(numberOfLastCommits);     
            }
            catch (ArgumentException ex)
            {
                return Task.FromResult(new Command(ex.Message, ResponseTypeEnum.Error));
            }
            catch (InvalidOperationException ex)
            {
                return Task.FromResult(new Command(ex.Message, ResponseTypeEnum.Error));
            }
            catch (Exception ex)
            {
                _errorLogService.Log(ex);
                return Task.FromResult(new Command(ResourceMessages.Services_PickByNumber_UnhandledException, ResponseTypeEnum.Error));
            }
            return new PickByList(_memoryPoolService, _errorLogService).Execute(hashes, memoryPool);
        }

        private List<string> GetCommitHashes(int numberOfLastCommits)
        {
            var commitHashes = new List<string>();
            string repoPath = Repository.Discover(CommandContextAccessor.Current.CurrentDirectory);

            if (string.IsNullOrEmpty(repoPath))
                throw new InvalidOperationException(ResourceMessages.Services_PickByNumber_InvalidGitRepository);            

            using (var repo = new Repository(repoPath))
            {
                if (repo.Head.Tip == null)
                    throw new InvalidOperationException(ResourceMessages.Services_PickByNumber_NoCommits);
                
                var commits = repo.Commits.Take(numberOfLastCommits).ToList();

                // If there are fewer commits than requested, adjust the count
                if (commits.Count < numberOfLastCommits)
                    numberOfLastCommits = commits.Count;

                // Collect the hashes of the last N commits
                commitHashes = commits.Take(numberOfLastCommits)
                                      .Select(commit => commit.Sha)
                                      .ToList();
            }
            return commitHashes;
        }
    }
}
