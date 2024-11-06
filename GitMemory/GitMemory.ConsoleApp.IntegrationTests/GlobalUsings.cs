global using Xunit;
// multiple integration tests running in parallel might cause issues due to tests dependency
[assembly: CollectionBehavior(DisableTestParallelization = true)]