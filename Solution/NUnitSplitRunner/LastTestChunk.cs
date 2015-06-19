using System;

namespace NUnitSplitRunner
{
  public class LastTestChunk : ITestChunk
  {
    private readonly ITestChunk _chunk;

    public LastTestChunk(ITestChunk chunk)
    {
      _chunk = chunk;
    }

    public void PerformNunitRun(string processName, RealRunnerInvocationOptions realRunnerInvocationOptions)
    {
      Console.WriteLine("One last run...");
      _chunk.PerformNunitRun(processName, realRunnerInvocationOptions);
    }
  }
}