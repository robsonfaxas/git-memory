using GitMemory.CultureConfig;
using GitMemory.Domain.Entities;
using GitMemory.Domain.Entities.Enums;
using GitMemory.Domain.Entities.Memories;
using GitMemory.Domain.Service;
using LibGit2Sharp;

namespace GitMemory.Infrastructure.CommandsServices.Pick.PickStrategy
{
    internal class PickByList : IPickStrategy
    {
        private readonly IMemoryPoolService _memoryPoolService;
        private readonly IErrorLogService _errorLogService;
        public PickByList(IMemoryPoolService memoryPoolRepository, IErrorLogService errorLogService)
        {
            _memoryPoolService = memoryPoolRepository;
            _errorLogService = errorLogService;
        }
        // <hash> <hash>: read all the hashes, one by one, and check if they're valid commits. 
        //                if any of them are not valid, return an error with the wrong hash
        //                  saying it was not found for the current git repository
        //                check the dates and add them to the unstaged list (don't add duplicates, check both lits) following the dates order
        //                write the file
        public Task<Command> Execute(List<string> hashes, MemoryPool memoryPool)
        {
            try
            {
                var commits = new List<MemoryCommit>();
                foreach (var hash in hashes)
                {
                    var commit = GetCommit(hash);
                    if (commit == null)
                        throw new ArgumentException(string.Format(ResourceMessages.Services_PickByList_InvalidHash, hash));
                    else
                        commits.Add(commit);
                }

                AddRepositoryIfNotExists(memoryPool);
                var currentRepository = Repository.Discover(CommandContextAccessor.Current.CurrentDirectory);
                
                foreach (var repository in memoryPool.GitRepositories.Where(p => p.GitRepositoryPath.ToLower().Equals(currentRepository.ToLower())))
                {
                    foreach (var commit in commits)
                        AddIfNotExists(repository.Unstaged, repository.Staged, commit);                        
                    
                }
                _memoryPoolService.WriteMemoryPool(memoryPool);
                return Task.FromResult(new Command(ResourceMessages.Services_Pick_Success, ResponseTypeEnum.Info));
            }
            catch (ArgumentException ex)
            {
                _errorLogService.Log(ex);
                return Task.FromResult(new Command(ex.Message, ResponseTypeEnum.Error));
            }
            catch (Exception ex)
            {
                _errorLogService.Log(ex);
                return Task.FromResult(new Command(ResourceMessages.Services_PickByList_UnhandledException, ResponseTypeEnum.Error));
            }
                
        }

        private void AddRepositoryIfNotExists(MemoryPool memoryPool)
        {
            string repoPath = Repository.Discover(CommandContextAccessor.Current.CurrentDirectory);
            if (string.IsNullOrEmpty(repoPath))
            {
                throw new ArgumentException(ResourceMessages.Services_PickByList_InvalidGitRepository);
            }
            if (!memoryPool.GitRepositories
                    .Where(p => p.GitRepositoryPath.ToLower().StartsWith(repoPath.ToLower()))
                    .Any())
            {
                var repository = new GitRepository() { GitRepositoryPath = repoPath};
                memoryPool.GitRepositories .Add(repository);
            }            
        }

        private MemoryCommit? GetCommit(string hash)
        {
            string repoPath = Repository.Discover(CommandContextAccessor.Current.CurrentDirectory);
            if (string.IsNullOrEmpty(repoPath))
            {
                throw new ArgumentException(ResourceMessages.Services_PickByList_InvalidGitRepository);                               
            }
            using var repo = new Repository(repoPath);
            try
            {
                var commit = repo.Commits.FirstOrDefault(p => p.Sha.ToLower().Equals(hash.ToLower()));
                var memoryCommit = new MemoryCommit()
                {
                    CommitDate = commit?.Author?.When.DateTime ?? default,
                    CommitHash = commit?.Sha ?? default!,
                    CommitDescription = commit?.Message ?? default!
                };
                return memoryCommit;
            }
            catch (LibGit2SharpException)
            {
                throw new ArgumentException(string.Format(ResourceMessages.Services_PickByList_GetCommit_Error, hash));
            }
        }

        private void AddIfNotExists(List<MemoryCommit> unstaged, List<MemoryCommit> staged, MemoryCommit newCommit)
        {
            if (!staged.Any(commit => commit.CommitHash.Equals(newCommit.CommitHash, StringComparison.OrdinalIgnoreCase)) && 
                !unstaged.Any(commit => commit.CommitHash.Equals(newCommit.CommitHash, StringComparison.OrdinalIgnoreCase)))
                unstaged.Add(newCommit);          
        }
    }
}
