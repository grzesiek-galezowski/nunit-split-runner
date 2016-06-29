using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using AtmaFileSystem;
using NUnitReportMerge;
using NUnitReportMerge.Out;
using NUnitSplitRunner.Input;

namespace NUnitSplitRunner.Running
{
  public class ChunkProcessing
  {
    private readonly TestChunkFactory _testChunkFactory;
    private readonly IChunksExecution _chunksExecution;
    private readonly ILogger _logger;

    public ChunkProcessing(TestChunkFactory testChunkFactory, IChunksExecution chunksExecution, ILogger logger)
    {
      _testChunkFactory = testChunkFactory;
      _chunksExecution = chunksExecution;
      _logger = logger;
    }

    public static readonly DirectoryName PartialDirName = DirectoryName.Value("partial");
    public const string InputPattern = "*.xml";
    public static readonly FileName OutputFileName = FileName.Value("TestResult.xml");

    public void Execute(IEnumerable<AnyFilePath> dlls, RealRunnerInvocationOptions remainingTargetCommandline)
    {
      RunAllChunks(dlls, remainingTargetCommandline);
    }

    private void RunAllChunks(IEnumerable<AnyFilePath> dlls, RealRunnerInvocationOptions remainingTargetCommandline)
    {
      var chunks = new List<ITestChunk>();
      var currentChunk = _testChunkFactory.CreateInitialChunk();
      foreach (var dll in dlls)
      {
        currentChunk.Add(dll);

        if (currentChunk.IsFilled())
        {
          chunks.Add(currentChunk);
          currentChunk = _testChunkFactory.CreateChunkFollowing(currentChunk);
        }
      }

      if (!currentChunk.IsEmpty())
      {
        chunks.Add(new LastTestChunk(currentChunk));
      }

      _chunksExecution.Perform(chunks, remainingTargetCommandline);
    }

    public void MergeReports(DirectoryName partialDirName, string searchPattern, FileName finalXmlResultFileName)
    {
      try
      {
        var list = XmlReportFiles.LoadFrom(partialDirName, searchPattern);
        var resultBuilder = new OutResultsBuilder();
        _logger.Info("Loaded " + list.Length + " partial files");
        NUnitReportFactory.CreateFrom(list).Xml(resultBuilder);
        var finalXmlOutput = resultBuilder.Build();
        finalXmlOutput.Save(finalXmlResultFileName.ToString());
        Console.WriteLine("Merge successful");
      }
      catch (Exception e)
      {
        _logger.Error("ERROR: Merge failed due to: " + e);
        throw;
      }
    }
  }

  public interface IChunksExecution
  {
    void Perform(IEnumerable<ITestChunk> chunks, RealRunnerInvocationOptions realRunngInvocationOptions);
  }

  public class ChunksExecution : IChunksExecution
  {
    private readonly AbsoluteFilePath _processPath;
    private readonly int _maxDegreeOfParallelism;
    private readonly ILoggerFactory _loggerFactory;

    public ChunksExecution(AbsoluteFilePath processPath, int maxDegreeOfParallelism, ILoggerFactory loggerFactory)
    {
      _processPath = processPath;
      _maxDegreeOfParallelism = maxDegreeOfParallelism;
      _loggerFactory = loggerFactory;
    }

    public void Perform(IEnumerable<ITestChunk> chunks, RealRunnerInvocationOptions realRunngInvocationOptions)
    {
      if (_maxDegreeOfParallelism > 1)
      {
        var logger = _loggerFactory.GetBuffered();
        Parallel.ForEach(chunks, LoopConfig(),
          chunk =>
          {
            try
            {
              chunk.PerformNunitRun(_processPath, realRunngInvocationOptions, logger);
            }
            finally
            {
              logger.Flush();
            }
          });
      }
      else
      {
        foreach (var chunk in chunks)
        {
          chunk.PerformNunitRun(_processPath, realRunngInvocationOptions, _loggerFactory.GetInstant());
        }
      }
    }

    private ParallelOptions LoopConfig()
    {
      return new ParallelOptions { MaxDegreeOfParallelism = _maxDegreeOfParallelism };
    }
  }

  public interface ILoggerFactory
  {
    ILogger GetInstant();
    ILogger GetBuffered();
  }

  public class LoggerFactory : ILoggerFactory
  {
    readonly ILogger _instantLogger;

    public LoggerFactory(ILogger instantLogger)
    {
      _instantLogger = instantLogger;
    }

    public ILogger GetInstant()
    {
      return _instantLogger;
    }

    public ILogger GetBuffered()
    {
      return new BufferedConsoleLogger(_instantLogger);
    }

  }
}