using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using AtmaFileSystem;

namespace NUnitSplitRunner
{
  public class TestChunk : ITestChunk
  {
    public int RunId { get; set; }
    readonly DirectoryName _partialDirName;
    readonly List<AnyPathWithFileName> _dlls = new List<AnyPathWithFileName>();
    readonly int _allowedAssemblyCount;
    private readonly OutputBuilder _outputBuilder;

    public TestChunk(int runId, DirectoryName partialDirName, int allowedAssemblyCount, OutputBuilder outputBuilder)
    {
      RunId = runId;
      _partialDirName = partialDirName;
      _allowedAssemblyCount = allowedAssemblyCount;
      _outputBuilder = outputBuilder;
    }

    public void Add(AnyPathWithFileName element)
    {
      _dlls.Add(element);
    }

    public override string ToString()
    {
      return String.Join(" ", _dlls);
    }

    public bool IsFilled()
    {
      
      return _dlls.Count >= _allowedAssemblyCount;
    }

    public bool IsEmpty()
    {
      return _dlls.Count == 0;
    }

    public void PerformNunitRun(PathWithFileName thirdPartyRunnerPath, RealRunnerInvocationOptions realRunnerInvocationOptions)
    {
      var exitCode = RunCommand(thirdPartyRunnerPath, realRunnerInvocationOptions);
      Handle(exitCode);
    }

    private static void Handle(int exitCode)
    {
      Console.WriteLine("Exit code: " + exitCode);
      if (IsError(exitCode) && ThereWasNoErrorYet())
      {
        Console.WriteLine("Assigning process exit code: " + exitCode);
        Environment.ExitCode = exitCode;
      }
    }

    public static bool ThereWasNoErrorYet()
    {
      return Environment.ExitCode == 0;
    }

    public static bool IsError(int exitCode)
    {
      return exitCode != 0;
    }

    public int RunCommand(PathWithFileName processName, RealRunnerInvocationOptions remainingTargetCommandline)
    {
      var arguments = remainingTargetCommandline + " " + this.ToString();
      Console.WriteLine("Running " + processName + " with: " + arguments);

      using (var process = CreateNUnitProcess(processName, arguments))
      {
        process.Start();
        process.WaitForExit();
        _outputBuilder.Add(arguments, process.StandardOutput, process.StandardError);
        return process.ExitCode;
      }
    }

    public Process CreateNUnitProcess(PathWithFileName processName, string arguments)
    {
      Directory.CreateDirectory(_partialDirName.ToString());

      return new Process
        {
          StartInfo =
            {
              FileName = processName.ToString(),
              Arguments = arguments + " " + "/xml:" + _partialDirName + "\\nunit-partial-run-" + this.RunId + ".xml",
              UseShellExecute = false,
              Domain = Process.GetCurrentProcess().StartInfo.Domain,
              RedirectStandardError = true,
              RedirectStandardOutput = true
            },
        };
    }
  }
}