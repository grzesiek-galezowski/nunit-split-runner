using System.IO;

namespace NUnitSplitRunner
{
  public interface OutputBuilder
  {
    void Add(string runArguments, StreamReader standardOutput, StreamReader standardError);
  }
}