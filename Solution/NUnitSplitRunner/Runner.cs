using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AtmaFileSystem;

namespace NUnitSplitRunner
{
  public class Runner
  {
    private readonly ProgramArguments _programArguments;
    private readonly RealRunnerInvocationOptions _realRunnerInvocationOptions;
    private readonly AllStandardOutputThenErrorBuilder _outputBuilder;
    private readonly ChunkProcessing _chunkProcessing;

    public Runner(
      ProgramArguments programArguments, 
      RealRunnerInvocationOptions realRunnerInvocationOptions, 
      AllStandardOutputThenErrorBuilder outputBuilder, 
      ChunkProcessing chunkProcessing)
    {
      _programArguments = programArguments;
      _realRunnerInvocationOptions = realRunnerInvocationOptions;
      _outputBuilder = outputBuilder;
      _chunkProcessing = chunkProcessing;
    }

    public static Runner Create(string[] args, int maxAllowedAssemblyCountPerRun, string thirdPartyRunnerPath, int maxDegreeOfParallelism)
    {
      AllStandardOutputThenErrorBuilder outputBuilder = new AllStandardOutputThenErrorBuilder();
      TestChunkFactory testChunkFactory = new TestChunkFactory(
        maxAllowedAssemblyCountPerRun, 
        ChunkProcessing.PartialDirName, 
        outputBuilder);
      return new Runner(new ProgramArguments(args), new RealRunnerInvocationOptions(), outputBuilder, 
        new ChunkProcessing(
          AbsoluteFilePath.Value(thirdPartyRunnerPath), 
          testChunkFactory, maxDegreeOfParallelism));
    }

    public void Run()
    {
      var dlls = new List<AnyFilePath>();

      _programArguments.SplitInto(dlls, _realRunnerInvocationOptions);
      _chunkProcessing.Execute(dlls, _realRunnerInvocationOptions);
      
      Console.WriteLine(_outputBuilder.Output());
      Console.Error.WriteLine(_outputBuilder.Errors());

      _chunkProcessing.MergeReports(ChunkProcessing.PartialDirName, ChunkProcessing.InputPattern, ChunkProcessing.OutputFileName);
    }
  }
}
