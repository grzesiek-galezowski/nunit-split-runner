using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace NUnitSplitRunner
{
  public class Runner
  {
    private static void Main(string[] args)
    {
      var runner = new Runner();
      runner.Run(args, LoadMaxAllowedAssemblyCountPerRun());
    }

    private static int LoadMaxAllowedAssemblyCountPerRun()
    {
      return int.Parse(ConfigurationManager.AppSettings["MaxAssemblyCountPerRun"]);
    }

    public void Run(string[] args, int allowedAssemblyCount)
    {
      var processName = args[0];
      var dlls = new List<string>();
      var commandline = new CommandlineArguments();
      var stringStreamOutputBuilder = new AllStandardOutputThenErrorBuilder();
      var chunkProcessing = new ChunkProcessing(processName, new TestChunkFactory(allowedAssemblyCount, ChunkProcessing.PartialDirName, stringStreamOutputBuilder));

      Parse(args, dlls, commandline);
      chunkProcessing.Execute(dlls, commandline);
      Console.WriteLine(stringStreamOutputBuilder.Output());
      Console.Error.WriteLine(stringStreamOutputBuilder.Errors());
      chunkProcessing.MergeReports(ChunkProcessing.PartialDirName, ChunkProcessing.InputPattern, ChunkProcessing.OutputPath);
    }

    private static void Parse(IEnumerable<string> args, List<string> dlls, CommandlineArguments commandline)
    {
      foreach (var arg in args.Skip(1))
      {
        if (IsAssemblyPath(arg))
        {
          dlls.Add(arg);
        }
        else
        {
          commandline.Add(arg);
        }
      }
    }

    private static bool IsAssemblyPath(string arg)
    {
      return arg.EndsWith(".dll");
    }
  }
}
