using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AtmaFileSystem;
using NUnitSplitRunner.Input;
using NUnitSplitRunner.Output;
using NUnitSplitRunner.Running;

namespace NUnitSplitRunner
{
  public class Runner
  {
    private readonly ProgramArguments _programArguments;
    private readonly RealRunnerInvocationOptions _realRunnerInvocationOptions;
    private readonly ChunkProcessing _chunkProcessing;
    private readonly ILogger _logger;
    const string AssemblyNameSeparator = " ";

    public Runner(ProgramArguments programArguments, RealRunnerInvocationOptions realRunnerInvocationOptions, ChunkProcessing chunkProcessing, ILogger logger)
    {
      _programArguments = programArguments;
      _realRunnerInvocationOptions = realRunnerInvocationOptions;
      _chunkProcessing = chunkProcessing;
      _logger = logger;
    }

    public static Runner Create(IOptions options, ILoggerFactory loggerFactory)
    {
      AllStandardOutputThenErrorBuilder outputBuilder = new AllStandardOutputThenErrorBuilder();
      TestChunkFactory testChunkFactory = new TestChunkFactory(
        options,
        ChunkProcessing.PartialDirName,
        outputBuilder);
      return new Runner(new ProgramArguments(options.RunnerArgs.Split(new[] { AssemblyNameSeparator }, StringSplitOptions.RemoveEmptyEntries)), new RealRunnerInvocationOptions(),
        new ChunkProcessing(
          testChunkFactory, new ChunksExecution(AbsoluteFilePath.Value(options.RunnerPath), 
          options.ParallelProcesses, loggerFactory), loggerFactory.GetInstant()), loggerFactory.GetInstant());
    }

    public void Run()
    {
      var dlls = new List<AnyFilePath>();

      _programArguments.SplitInto(dlls, _realRunnerInvocationOptions);

      _logger.Info("Assemblies found: " + dlls.Count);

      _chunkProcessing.Execute(dlls, _realRunnerInvocationOptions);

      _chunkProcessing.MergeReports(
        ChunkProcessing.PartialDirName,
        ChunkProcessing.InputPattern,
        ChunkProcessing.OutputFileName);
    }
  }
}
