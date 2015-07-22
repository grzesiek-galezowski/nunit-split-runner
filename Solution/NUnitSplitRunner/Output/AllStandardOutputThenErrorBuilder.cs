using System;
using System.IO;

namespace NUnitSplitRunner.Output
{
  public class AllStandardOutputThenErrorBuilder : OutputBuilder
  {
    private string _stdOut = string.Empty;
    private string _stdErr = string.Empty;

    public void Add(string runArguments, StreamReader standardOutput, StreamReader standardError)
    {
      _stdOut += "===>" + Environment.NewLine;
      _stdOut += "===> NEW RUN: " + runArguments + Environment.NewLine;
      _stdOut += "===>" + Environment.NewLine;
      _stdOut += standardOutput.ReadToEnd();
      _stdErr += standardError.ReadToEnd();
    }

    public string Output()
    {
      return _stdOut;
    }

    public string Errors()
    {
      return _stdErr;
    }
  }
}