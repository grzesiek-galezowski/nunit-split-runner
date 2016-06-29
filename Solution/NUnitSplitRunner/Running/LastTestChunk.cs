using System;
using AtmaFileSystem;
using NUnitSplitRunner.Input;

namespace NUnitSplitRunner.Running
{
  public class LastTestChunk : ITestChunk
  {
    private readonly ITestChunk _chunk;

    public LastTestChunk(ITestChunk chunk)
    {
      _chunk = chunk;
    }

    public void PerformNunitRun(AbsoluteFilePath thirdPartyRunnerPath, RealRunnerInvocationOptions realRunnerInvocationOptions, ILogger logger)
    {
      Console.WriteLine("One last run...");
      _chunk.PerformNunitRun(thirdPartyRunnerPath, realRunnerInvocationOptions, logger);
    }
  }
}