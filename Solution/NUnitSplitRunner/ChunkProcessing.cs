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

    public ChunkProcessing(string processPath, TestChunkFactory testChunkFactory)
    {
      _processPath = processPath;
      _testChunkFactory = testChunkFactory;
    }

    public const string PartialDirName = "partial";
    public const string InputPattern = "*.xml";
    public const string OutputPath = "TestResult.xml";

    public void Execute(IEnumerable<string> dlls, CommandlineArguments commandline)
    {
      RunAllChunks(dlls, commandline);
    }

    private void RunAllChunks(IEnumerable<string> dlls, CommandlineArguments commandline)
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

      /*foreach (var action in chunks)
      {
        action.PerformNunitRun(_processPath, commandline);
      }*/
      Parallel.ForEach(chunks, chunk => chunk.PerformNunitRun(_processPath, commandline));
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

  class LastTestChunk : ITestChunk
  {
    private readonly ITestChunk _chunk;

    public LastTestChunk(ITestChunk chunk)
    {
      _chunk = chunk;
    }

    public void PerformNunitRun(string processName, CommandlineArguments commandline)
    {
      Console.WriteLine("One last run...");
      _chunk.PerformNunitRun(processName, commandline);
    }
  }
}