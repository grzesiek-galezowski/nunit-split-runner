using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AtmaFileSystem;
using NUnitReportMerge;
using NUnitSplitRunner.Input;

namespace NUnitSplitRunner.Running
{
  public class ChunkProcessing
  {
    readonly AbsoluteFilePath _processPath;
    private readonly TestChunkFactory _testChunkFactory;
    private readonly int _maxDegreeOfParallelism;

    public ChunkProcessing(AbsoluteFilePath processPath, TestChunkFactory testChunkFactory, int maxDegreeOfParallelism)
    {
      _processPath = processPath;
      _testChunkFactory = testChunkFactory;
      _maxDegreeOfParallelism = maxDegreeOfParallelism;
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

      Parallel.ForEach(chunks, LoopConfig(), chunk => chunk.PerformNunitRun(_processPath, remainingTargetCommandline));
    }

    private ParallelOptions LoopConfig()
    {
      return new ParallelOptions() { MaxDegreeOfParallelism = _maxDegreeOfParallelism };
    }

    public void MergeReports(DirectoryName partialDirName, string searchPattern, FileName finalXmlResultFileName)
    {
      try
      {
        var list = XmlReportFiles.LoadFrom(partialDirName, searchPattern);
        Console.WriteLine("Loaded " + list.Length + " partial files");
        var result = NUnitReportFactory.CreateFrom(list).Xml();
        result.Save(finalXmlResultFileName.ToString());
        Console.WriteLine("Merge successful");
      }
      catch (Exception e)
      {
        Console.Error.WriteLine("ERROR: Merge failed due to: " + e);
        throw;
      }
    }
  }
}