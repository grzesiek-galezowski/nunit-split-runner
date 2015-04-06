namespace NUnitSplitRunner
{
  public class TestChunkFactory
  {
    readonly string _partialDirName;
    readonly int _allowedAssemblyCount;

    public TestChunkFactory(int allowedAssemblyCount, string partialDirName)
    {
      _allowedAssemblyCount = allowedAssemblyCount;
      _partialDirName = partialDirName;
    }

    public TestChunk CreateChunkFollowing(TestChunk currentChunk)
    {
      return new TestChunk(currentChunk.RunId + 1, _partialDirName, _allowedAssemblyCount);
    }

    public TestChunk CreateInitialChunk()
    {
      return new TestChunk(1, _partialDirName, _allowedAssemblyCount);
    }
    
  }
}