using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Entities.Memories;
using GitMemory.Domain.Repositories;
using LibGit2Sharp;

namespace GitMemory.Infrastructure.CommandsServices.Pick.PickStrategy
{
    internal class PickByList : IPickStrategy
    {
        private readonly IMemoryPoolRepository _memoryPoolRepository;
        private readonly IErrorLogRepository _errorLogRepository;
        public PickByList(IMemoryPoolRepository memoryPoolRepository, IErrorLogRepository errorLogRepository)
        {
            _memoryPoolRepository = memoryPoolRepository;
            _errorLogRepository = errorLogRepository;
        }
        // <hash> <hash>: read all the hashes, one by one, and check if they're valid commits. 
        //                if any of them are not valid, return an error with the wrong hash
        //                  saying it was not found for the current git repository
        //                check the dates and add them to the unstaged list (don't add duplicates, check both lits) following the dates order
        //                write the file
        public Task<CommandResponse> Execute(List<string> hashes, MemoryPool memoryPool)
        {
            try
            {
                var commits = new List<Commit>();
                foreach (var hash in hashes)
                {
                    var commit = GetCommit(hash);
                    if (commit == null)
                        throw new ArgumentException($"Invalid hash: {hash}");
                    else
                        commits.Add(commit);
                }

                foreach (var repository in memoryPool.GitRepositories.Where(p => p.GitRepositoryPath.ToLower().StartsWith(CommandContextAccessor.Current.CurrentDirectory.ToLower())))
                {
                    foreach (var commit in commits)
                    {
                        var memoryCommit = new MemoryCommit()
                        {
                            CommitDate = commit.Committer.When.DateTime,
                            CommitHash = commit.Sha
                        };
                        AddIfNotExists(repository.Unstaged, repository.Staged, memoryCommit);                        
                    }
                }
                _memoryPoolRepository.WriteMemoryPool(memoryPool);
                return Task.FromResult(new CommandResponse("Commits added.", ResponseTypeEnum.Info));
            }
            catch (ArgumentException ex)
            {
                return Task.FromResult(new CommandResponse(ex.Message, ResponseTypeEnum.Error));
            }
            catch (Exception ex)
            {
                _errorLogRepository.Log(ex);
                return Task.FromResult(new CommandResponse("An error occurred while attempting to retrieve commits. Please refer to the error log for more details.", ResponseTypeEnum.Error));
            }
                
        }

        private Commit? GetCommit(string hash)
        {
            var currentDirectory = CommandContextAccessor.Current.CurrentDirectory;
            if (!Repository.IsValid(currentDirectory))
            {
                throw new ArgumentException("Not a valid git repository.");                                
            }
            using var repo = new Repository(currentDirectory);
            try
            {
                return repo.Lookup<Commit>(hash);                
            }
            catch (LibGit2SharpException)
            {
                throw new ArgumentException($"Error looking up commit {hash}");
            }
        }

        private void AddIfNotExists(List<MemoryCommit> unstaged, List<MemoryCommit> staged, MemoryCommit newCommit)
        {
            if (!staged.Any(commit => commit.CommitHash.Equals(newCommit.CommitHash)) && 
                !unstaged.Any(commit => commit.CommitHash.Equals(newCommit.CommitHash)))
                unstaged.Add(newCommit);          
        }
    }
}
