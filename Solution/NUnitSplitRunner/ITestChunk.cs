using AtmaFileSystem;

namespace NUnitSplitRunner
{
  public interface ITestChunk
  {
    void PerformNunitRun(AbsoluteFilePath thirdPartyRunnerPath, RealRunnerInvocationOptions realRunnerInvocationOptions);
  }
}