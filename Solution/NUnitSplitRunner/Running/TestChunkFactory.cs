using System;
using AtmaFileSystem;
using NUnitSplitRunner.Output;

namespace NUnitSplitRunner.Running
{
  public class TestChunkFactory
  {
    private readonly IOptions _options;
    readonly DirectoryName _partialDirName;
    readonly int _allowedAssemblyCount;
    readonly OutputBuilder _outputBuilder;

    public TestChunkFactory(IOptions options, DirectoryName partialDirName, OutputBuilder outputBuilder)
    {
      _options = options;
      _partialDirName = partialDirName;
      _outputBuilder = outputBuilder;
    }

    public TestChunk CreateChunkFollowing(TestChunk currentChunk)
    {
      return new TestChunk(currentChunk.RunId + 1, _partialDirName, _options.AssembliesPerRun, _options.NunitTimeout);
    }

    public TestChunk CreateInitialChunk()
    {
      return new TestChunk(1, _partialDirName, _options.AssembliesPerRun, _options.NunitTimeout);
    }
    
  }
}