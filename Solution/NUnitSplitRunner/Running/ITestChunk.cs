using AtmaFileSystem;
using NUnitSplitRunner.Input;

namespace NUnitSplitRunner.Running
{
  public interface ITestChunk
  {
    void PerformNunitRun(AbsoluteFilePath thirdPartyRunnerPath, RealRunnerInvocationOptions realRunnerInvocationOptions);
  }
}