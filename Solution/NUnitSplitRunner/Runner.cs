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
      var programArguments = new ProgramArguments(args);
      var commandline = new TargetCommandlineArguments();
      var stringStreamOutputBuilder = new AllStandardOutputThenErrorBuilder();
      var testChunkFactory = new TestChunkFactory(allowedAssemblyCount, ChunkProcessing.PartialDirName, stringStreamOutputBuilder);
      var chunkProcessing = new ChunkProcessing(processName, testChunkFactory, 2);

      var dlls = new List<string>();

      Process(programArguments, dlls, commandline, chunkProcessing, stringStreamOutputBuilder);
    }

    private static void Process(ProgramArguments programArguments, List<string> dlls, TargetCommandlineArguments commandline,
      ChunkProcessing chunkProcessing, AllStandardOutputThenErrorBuilder stringStreamOutputBuilder)
    {
      programArguments.SplitInto(dlls, commandline);
      chunkProcessing.Execute(dlls, commandline);
      
      Console.WriteLine(stringStreamOutputBuilder.Output());
      Console.Error.WriteLine(stringStreamOutputBuilder.Errors());

      chunkProcessing.MergeReports(ChunkProcessing.PartialDirName, ChunkProcessing.InputPattern, ChunkProcessing.OutputPath);
    }
  }
}
