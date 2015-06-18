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

    public void PerformNunitRun(string processName, CommandlineArguments commandline)
    {
      Console.WriteLine("One last run...");
      _chunk.PerformNunitRun(processName, commandline);
    }
  }
}