using AtmaFileSystem;

namespace NUnitSplitRunner
{
  public interface ITestChunk
  {
    void PerformNunitRun(PathWithFileName thirdPartyRunnerPath, RealRunnerInvocationOptions realRunnerInvocationOptions);
  }
}