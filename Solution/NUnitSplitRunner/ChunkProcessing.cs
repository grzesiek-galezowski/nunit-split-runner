using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using NUnitReportMerge;

namespace NUnitSplitRunner
{
  internal class ChunkProcessing
  {
    readonly string _processPath;
    private readonly TestChunkFactory _testChunkFactory;
    private readonly int _maxDegreeOfParallelism;

    public ChunkProcessing(string processPath, TestChunkFactory testChunkFactory, int maxDegreeOfParallelism)
    {
      _processPath = processPath;
      _testChunkFactory = testChunkFactory;
      _maxDegreeOfParallelism = maxDegreeOfParallelism;
    }

    public const string PartialDirName = "partial";
    public const string InputPattern = "*.xml";
    public const string OutputPath = "TestResult.xml";

    public void Execute(IEnumerable<string> dlls, RealRunnerInvocationOptions remainingTargetCommandline)
    {
      RunAllChunks(dlls, remainingTargetCommandline);
    }

    private void RunAllChunks(IEnumerable<string> dlls, RealRunnerInvocationOptions remainingTargetCommandline)
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

    public void MergeReports(string partialDirName, string searchPattern, string testresultXml)
    {
      try
      {
        var list = XmlReportFiles.LoadFrom(partialDirName, searchPattern);
        var portedResult = CreateMergedReport(list);
        portedResult.Save(testresultXml);
        Console.WriteLine("Merge successful");
      }
      catch (Exception e)
      {
        Console.Error.WriteLine("ERROR: Merge failed due to: " + e);
        throw;
      }
    }

    private static XElement CreateMergedReport(IEnumerable<ReportDocument> list)
    {
      var tuple = NUnitReport.Fold(list);
      var result = Merge.ApplyTo(tuple);
      return result;
    }
  }
}