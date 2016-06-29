using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Threading;
using NUnitSplitRunner.Output;
using NUnitSplitRunner.Running;

namespace NUnitSplitRunner
{
  static internal class EntryPoint
  {
    public static void Main(string[] args)
    {
      var options = new Options();
      var logger = new ConsoleLogger();
      var loggerFactory = new LoggerFactory(logger);

      CommandLine.Parser.Default.ParseArgumentsStrict(args, options, OnArgumentsParsingFailed);
      options.DumpTo(logger);

      var runner = Runner.Create(options, loggerFactory);
      runner.Run();
    }

    private static void OnArgumentsParsingFailed()
    {
      throw new Exception("The commandline parser has failed. Some required arguments may not have been passed.");
    }
  }

  public class LogEntry
  {
    public string Message { get; set; }
    public bool IsError { get; set; }
  }

  public class BufferedConsoleLogger : ILogger
  {
    private readonly ILogger _standardLogger;
    private static readonly object FlushLock = new object();

    public BufferedConsoleLogger(ILogger standardLogger)
    {
      _standardLogger = standardLogger;
    }

    private readonly ConcurrentQueue<LogEntry> _entries = new ConcurrentQueue<LogEntry>();
    public void Info(string message)
    {
      _entries.Enqueue(new LogEntry { Message = message, IsError = false });
    }

    public void Error(string message)
    {
      _entries.Enqueue(new LogEntry { Message = message, IsError = true });
    }

    public void Flush()
    {
      lock (FlushLock)
      {
        LogEntry entry;
        while(_entries.TryDequeue(out entry))
        {
          if (entry.IsError)
            _standardLogger.Info(entry.Message);
          else
          {
            _standardLogger.Error(entry.Message);
          }
        }
      }
    }
  }

  public interface ILogger
  {
    void Info(string message);
    void Error(string message);
    void Flush();
  }

  public class ConsoleLogger : ILogger
  {
    public void Info(string message)
    {
      WriteIfDebug(message);
      Console.WriteLine(message);
    }

    public void Error(string message)
    {
      WriteIfDebug(message);
      Console.Error.WriteLine(message);
    }

    public void Flush()
    {
    }

    [Conditional("DEBUG")]
    private static void WriteIfDebug(string message)
    {
      Debug.WriteLine(message);
    }
  }
}