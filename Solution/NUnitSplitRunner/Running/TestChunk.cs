using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AtmaFileSystem;
using NUnitSplitRunner.Input;
using NUnitSplitRunner.Output;

namespace NUnitSplitRunner.Running
{
  public class TestChunk : ITestChunk
  {
    public int RunId { get; set; }
    readonly DirectoryName _partialDirName;
    readonly List<AnyFilePath> _dlls = new List<AnyFilePath>();
    readonly int _allowedAssemblyCount;
    private ILogger _logger;
    private readonly TimeSpan _timeout;

    public TestChunk(int runId, DirectoryName partialDirName, int allowedAssemblyCount, TimeSpan timeout)
    {
      RunId = runId;
      _partialDirName = partialDirName;
      _allowedAssemblyCount = allowedAssemblyCount;
      _timeout = timeout;
    }

    public void Add(AnyFilePath element)
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

    public void PerformNunitRun(AbsoluteFilePath thirdPartyRunnerPath, RealRunnerInvocationOptions realRunnerInvocationOptions, ILogger logger)
    {
      _logger = logger;
      var exitCode = RunCommand(thirdPartyRunnerPath, realRunnerInvocationOptions);
      Handle(exitCode);
    }

    private void Handle(int exitCode)
    {
      _logger.Info("Exit code: " + exitCode);
      if (IsError(exitCode) && ThereWasNoErrorYet())
      {
        _logger.Info("Assigning process exit code: " + exitCode);
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

    public int RunCommand(AbsoluteFilePath processName, RealRunnerInvocationOptions remainingTargetCommandline)
    {
      var arguments = remainingTargetCommandline + " " + this;
      _logger.Info("Running " + processName + " with: " + arguments);
      _logger.Flush();

      using (var process = CreateNUnitProcess(processName, arguments))
      {
        using (var outputWaitHandle = new AutoResetEvent(false))
        using (var errorWaitHandle = new AutoResetEvent(false))
        {
          process.OutputDataReceived += ProcessOnOutputDataReceived(outputWaitHandle);
          process.ErrorDataReceived += ProcessOnErrorDataReceived(errorWaitHandle);

          process.Start();

          process.BeginOutputReadLine();
          process.BeginErrorReadLine();

          if (process.WaitForExit((int)_timeout.TotalMilliseconds) &&
              outputWaitHandle.WaitOne(_timeout) &&
              errorWaitHandle.WaitOne(_timeout))
          {
            return process.ExitCode;
          }
          else
          {
            if (!process.HasExited)
              process.Kill();
            throw new TimeoutException("The external process did not finish within expected time: " + _timeout);
          }
        }
      }
    }

    private DataReceivedEventHandler ProcessOnOutputDataReceived(AutoResetEvent outputWaitHandle)
    {
      return (sender, e) =>
      {
        if (e.Data == null)
        {
          SetIgnoreDisposed(outputWaitHandle);
        }
        else
        {
          _logger.Info(e.Data);
        }
      };
    }

    private DataReceivedEventHandler ProcessOnErrorDataReceived(AutoResetEvent errorWaitHandle)
    {
      return (sender, e) =>
      {
        if (e.Data == null)
        {
          SetIgnoreDisposed(errorWaitHandle);
        }
        else
        {
          _logger.Error(e.Data);
        }
      };
    }

    private static void SetIgnoreDisposed(AutoResetEvent errorWaitHandle)
    {
      try
      {
        errorWaitHandle.Set();
      }
      catch (ObjectDisposedException)
      {
      }
    }

    public Process CreateNUnitProcess(AbsoluteFilePath processName, string arguments)
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