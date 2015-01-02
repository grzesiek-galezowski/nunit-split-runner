using System;
using System.Collections.Generic;
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

    private static void RunNotFullAndNotEmptyChunk(
      TestChunk currentChunk, 
      string processName, 
      CommandlineArguments commandline)
    {
      if (!currentChunk.IsEmpty())
      {
        Console.WriteLine("One last run...");
        currentChunk.PerformNunitRun(processName, commandline);
      }
    }

    private TestChunk RunFullTestAssemblyChunks(IEnumerable<string> dlls, string processName, CommandlineArguments commandline)
    {
      var currentChunk = _testChunkFactory.CreateInitialChunk();
      foreach (var dll in dlls)
      {
        currentChunk.Add(dll);

        if (currentChunk.IsFilled())
        {
          currentChunk.PerformNunitRun(processName, commandline);
          currentChunk = _testChunkFactory.CreateChunkFollowing(currentChunk);
        }
      }
      return currentChunk;
    }

    public const string PartialDirName = "partial";
    public const string InputPattern = "*.xml";
    public const string OutputPath = "TestResult.xml";

    public void Execute(IEnumerable<string> dlls, CommandlineArguments commandline)
    {
      var currentChunk = RunFullTestAssemblyChunks(dlls, _processPath, commandline);
      RunNotFullAndNotEmptyChunk(currentChunk, _processPath, commandline);
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

    private static XElement CreateMergedReport(IEnumerable<XDocument> list)
    {
      var tuple = NUnitReport.Fold(list);
      var result = Merge.ApplyTo(tuple);
      return result;
    }
  }
}