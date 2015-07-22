using AtmaFileSystem;
using NUnitSplitRunner.Output;

namespace NUnitSplitRunner.Running
{
  public class TestChunkFactory
  {
    readonly DirectoryName _partialDirName;
    readonly int _allowedAssemblyCount;
    readonly OutputBuilder _outputBuilder;

    public TestChunkFactory(int allowedAssemblyCount, DirectoryName partialDirName, OutputBuilder outputBuilder)
    {
      _allowedAssemblyCount = allowedAssemblyCount;
      _partialDirName = partialDirName;
      _outputBuilder = outputBuilder;
    }

    public TestChunk CreateChunkFollowing(TestChunk currentChunk)
    {
      return new TestChunk(currentChunk.RunId + 1, _partialDirName, _allowedAssemblyCount, _outputBuilder);
    }

    public TestChunk CreateInitialChunk()
    {
      return new TestChunk(1, _partialDirName, _allowedAssemblyCount, _outputBuilder);
    }
    
  }
}