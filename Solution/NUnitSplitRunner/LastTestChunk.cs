using System;
using AtmaFileSystem;

namespace NUnitSplitRunner
{
  public class LastTestChunk : ITestChunk
  {
    private readonly ITestChunk _chunk;

    public LastTestChunk(ITestChunk chunk)
    {
      _chunk = chunk;
    }

    public void PerformNunitRun(PathWithFileName thirdPartyRunnerPath, RealRunnerInvocationOptions realRunnerInvocationOptions)
    {
      Console.WriteLine("One last run...");
      _chunk.PerformNunitRun(thirdPartyRunnerPath, realRunnerInvocationOptions);
    }
  }
}