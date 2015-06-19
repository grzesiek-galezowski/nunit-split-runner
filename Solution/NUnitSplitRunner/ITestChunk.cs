namespace NUnitSplitRunner
{
  public interface ITestChunk
  {
    void PerformNunitRun(string processName, RealRunnerInvocationOptions realRunnerInvocationOptions);
  }
}