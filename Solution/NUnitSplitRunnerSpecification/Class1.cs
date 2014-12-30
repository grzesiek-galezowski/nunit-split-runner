using System;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;
using NUnitSplitRunner;

namespace NUnitSplitRunnerSpecification
{
    public class Specification
    {
      [Test, Explicit]
      public void ShouldPassSimpleSanityCheck()
      {
        //GIVEN
        Console.WriteLine(Directory.GetCurrentDirectory());
        var runner = new Runner();
        
        //WHEN
        runner.Run(new[]
        {
          
          @"C:\Users\astral\Documents\GitHub\nunit-split-runner\Solution\packages\NUnit.Runners.2.6.4\tools\nunit-console-x86.exe",
          "ClassLibrary1.dll",
          "ClassLibrary2.dll",
          "ClassLibrary3.dll",
          "ClassLibrary4.dll",
          "ClassLibrary5.dll",
        }, 1);
        

        //THEN
        var goldenMaster = GetGoldenMaster();
        var createdFile = GetMergedReport();
        Assert.AreEqual(goldenMaster, createdFile);
      }

      private static string GetMergedReport()
      {
        var createdFile = File.ReadAllText("TestResult.xml");
        createdFile = RemoveRunSpecificValuesFrom(createdFile);
        return createdFile;
      }

      private static string GetGoldenMaster()
      {
        var goldenMaster = File.ReadAllText("GoldenMasterTestResult.xml");
        goldenMaster = RemoveRunSpecificValuesFrom(goldenMaster);
        return goldenMaster;
      }

      private static string RemoveRunSpecificValuesFrom(string goldenMaster)
      {
        var timePattern1 = "time=\"\\d\\d:\\d\\d:\\d\\d\"";
        var removedTimeValue1 = "time=\"\"";
        var timePattern2 = "time=\"\\d+\\.\\d+\"";
        var removedTimeValue2 = "time=\"\"";
        var datePattern = "date=\"\\d\\d\\d\\d-\\d\\d-\\d\\d\"";
        var removedDateValue = "date=\"\"";
        goldenMaster = Regex.Replace(goldenMaster, timePattern1, removedTimeValue1);
        goldenMaster = Regex.Replace(goldenMaster, timePattern2, removedTimeValue2);
        goldenMaster = Regex.Replace(goldenMaster, datePattern, removedDateValue);
        return goldenMaster;
      }
    }
}
