using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using AtmaFileSystem;
using NUnitReportMerge.Input;
using NUnitReportMerge.Model;

namespace NUnitReportMerge
{
  public static class XmlReportFiles
  {
    public static SingleRunReport[] LoadFrom(DirectoryName directory, string filter)
    {
      var files = GetFiles(directory, filter);

      Console.WriteLine("Found reports: " + string.Join(", ", files));

      return Parse(files);
    }

    private static string[] GetFiles(DirectoryName directory, string filter)
    {
      return Directory.GetFiles(directory.ToString(), filter, SearchOption.AllDirectories);
    }

    private static SingleRunReport[] Parse(string[] files)
    {
      return files.Select(fileName => XDocument.Parse(File.ReadAllText(fileName)))
        .Select(NewSingleRunReport).ToArray();
    }

    static SingleRunReport NewSingleRunReport(XDocument d)
    {
      TestResultsEnvironment testResultsEnvironment = new TestResultsEnvironment(EnvironmentElement(d));

      XElement cultureElement = CultureElement(d);

      XElement testResultsElement = TestResultsElement(d);
      ResultSummary nUnitTestResults = new ResultSummary(testResultsElement);
      return new SingleRunReport(new NUnitEnvironment
      {
        NUnitVersion = testResultsEnvironment.NUnitVersion(),
        ClrVersion = testResultsEnvironment.ClrVersion(),
        OsVersion = testResultsEnvironment.OsVersion(),
        Platform = testResultsEnvironment.PlatformVersion(),
        Cwd = testResultsEnvironment.Cwd(),
        MachineName = testResultsEnvironment.MachineName(),
        User = testResultsEnvironment.User(),
        UserDomain = testResultsEnvironment.UserDomain()
      },
      new NUnitCulture
      {
        CurrentCulture = CurrentCulture(cultureElement),
        CurrentUiCulture = CurrentUiCulture(cultureElement)
      }, new NUnitResultSummary
      {
        Total = nUnitTestResults.Total(),
        Errors = nUnitTestResults.Errors(),
        Failures = nUnitTestResults.Failures(),
        NotRun = nUnitTestResults.NotRun(),
        Inconclusive = nUnitTestResults.Inconclusive(),
        Ignored = nUnitTestResults.Ignored(),
        Skipped = nUnitTestResults.Skipped(),
        Invalid = nUnitTestResults.Invalid(),
        DateTime = nUnitTestResults.DateTimeValue()
      },
      new NUnitAssemblies(AssemblyElements(d)));
    }

    private static IEnumerable<XElement> AssemblyElements(XContainer d)
    {
      return d.Descendants().Where(el => el.Name == "test-suite" && el.Attribute("type").Value == "Assembly");
    }

    private static string CurrentCulture(XElement cultureElement)
    {
      return cultureElement.Attribute("current-culture").Value;
    }

    private static string CurrentUiCulture(XElement cultureElement)
    {
      return cultureElement.Attribute("current-uiculture").Value;
    }

    private static XElement EnvironmentElement(XContainer d)
    {
      return d.Element("test-results").Element("environment");
    }

    private static XElement CultureElement(XContainer d)
    {
      return d.Element("test-results").Element("culture-info");
    }

    private static XElement TestResultsElement(XContainer d)
    {
      return d.Element("test-results");
    }
  }
}