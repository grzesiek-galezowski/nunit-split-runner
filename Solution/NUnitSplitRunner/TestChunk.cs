using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace NUnitSplitRunner
{
  public class TestChunk
  {
    public int RunId { get; set; }
    readonly string _partialDirName;
    readonly List<string> _dlls = new List<string>();
    readonly int _allowedAssemblyCount;

    public TestChunk(int runId, string partialDirName, int allowedAssemblyCount)
    {
      RunId = runId;
      _partialDirName = partialDirName;
      _allowedAssemblyCount = allowedAssemblyCount;
    }

    public void Add(string element)
    {
      _dlls.Add(element);
    }

    public override string ToString()
    {
      return String.Join(" ", _dlls);
    }

    public bool IsFilled()
    {
      
      return _dlls.Count > _allowedAssemblyCount;
    }

    public bool IsEmpty()
    {
      return _dlls.Count == 0;
    }

    public void PerformNunitRun(string processName, CommandlineArguments commandline)
    {
      var exitCode = RunCommand(processName, commandline);
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

    public int RunCommand(string processName, CommandlineArguments commandline)
    {
      var arguments = commandline + " " + this;
      Console.WriteLine("Running " + processName + " with: " + arguments);

      using (var process = CreateNUnitProcess(processName, arguments))
      {
        process.Start();
        process.WaitForExit();
        return process.ExitCode;
      }
    }

    public Process CreateNUnitProcess(string processName, string arguments)
    {
      Directory.CreateDirectory(_partialDirName);

      return new Process
        {
          StartInfo =
            {
              FileName = processName,
              Arguments = arguments + " " + "/xml:" + _partialDirName + "\\nunit-partial-run-" + this.RunId + ".xml",
              UseShellExecute = false,
              Domain = Process.GetCurrentProcess().StartInfo.Domain
            }
        };
    }
  }
}