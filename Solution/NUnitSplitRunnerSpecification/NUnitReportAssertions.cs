using System;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace NUnitSplitRunnerSpecification
{
  static internal class NUnitReportAssertions
  {
    public static void AssertReportsEqual(string goldenMasterFileName, string testResultsFileName)
    {
      var goldenMaster = ReadNUnitReport(goldenMasterFileName);
      var createdFile = ReadNUnitReport(testResultsFileName);
      Assert.AreEqual((int) goldenMaster.Length, (int) createdFile.Length);
      for (int i = 0; i < goldenMaster.Length; ++i)
      {
        StringAssert.AreEqualIgnoringCase(goldenMaster[i], createdFile[i], "==LINE " + i + "==");
      }
    }

    private static string[] ReadNUnitReport(string reportPath)
    {
      var reportFile = File.ReadAllText(reportPath);
      reportFile = RemoveRunSpecificValuesFrom(reportFile);
      return reportFile.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
    }

    private static string RemoveRunSpecificValuesFrom(string goldenMaster)
    {
      const string timePattern1 = "time=\"\\d\\d:\\d\\d:\\d\\d\"";
      const string removedTimeValue1 = "time=\"\"";
      const string timePattern2 = "time=\"\\d+\\.\\d+\"";
      const string removedTimeValue2 = "time=\"\"";
      const string datePattern = "date=\"\\d\\d\\d\\d-\\d\\d-\\d\\d\"";
      const string removedDateValue = "date=\"\"";
      goldenMaster = Regex.Replace(goldenMaster, timePattern1, removedTimeValue1);
      goldenMaster = Regex.Replace(goldenMaster, timePattern2, removedTimeValue2);
      goldenMaster = Regex.Replace(goldenMaster, datePattern, removedDateValue);
      return goldenMaster;
    }
  }
}