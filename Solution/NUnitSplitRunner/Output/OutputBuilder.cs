using System.IO;

namespace NUnitSplitRunner.Output
{
  public interface OutputBuilder
  {
    void Add(string runArguments, StreamReader standardOutput, StreamReader standardError);
  }
}