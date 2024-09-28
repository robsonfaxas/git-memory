using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Entities.Memories;
using GitMemory.Domain.Repositories;
using LibGit2Sharp;
using System.Collections.Generic;

namespace GitMemory.Infrastructure.CommandsServices.Pick.PickStrategy
{
    internal class PickByNumber : IPickStrategy
    {
        private readonly IMemoryPoolRepository _memoryPoolRepository;
        public PickByNumber(IMemoryPoolRepository memoryPoolRepository)
        {
            _memoryPoolRepository = memoryPoolRepository;
        }
        public Task<CommandResponse> Execute(List<string> values, MemoryPool memoryPool)
        {
            var hashes = new List<string>();
            try
            {                
                var parsed = int.TryParse(values.FirstOrDefault(), out int numberOfLastCommits);
                if (!parsed)
                    throw new Exception("Invalid Number in parameter <N>");
                hashes = GetCommitHashes(numberOfLastCommits);                
            }
            catch (Exception ex)
            {
                return Task.FromResult(new CommandResponse(ex.Message, ResponseTypeEnum.Error));
            }
            return new PickByList(_memoryPoolRepository).Execute(hashes, memoryPool);
        }

        private List<string> GetCommitHashes(int numberOfLastCommits)
        {
            var commitHashes = new List<string>();
            string repoPath = Repository.Discover(".");

            if (string.IsNullOrEmpty(repoPath))
                throw new InvalidOperationException("No Git repository found in the current directory.");            

            using (var repo = new Repository(repoPath))
            {
                if (repo.Head.Tip == null)
                    throw new InvalidOperationException("The repository does not have any commits.");
                
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
