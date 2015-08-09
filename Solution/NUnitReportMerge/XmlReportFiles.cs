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

      return files
        .Select(File.ReadAllText)
        .Select(XDocument.Parse)
        .Select(NewSingleRunReport)
        .ToArray();
    }

    static string[] GetFiles(DirectoryName directory, string filter)
    {
      return Directory.GetFiles(directory.ToString(), filter, SearchOption.AllDirectories);
    }

    static SingleRunReport NewSingleRunReport(XDocument d)
    {
      var testResultsEnvironment = TestResultsEnvironmentFrom(d);
      var testResultsCulture = TestResultsCultureFrom(d);
      var testResultsSummary = NUnitTestResults(d);
      var assemblyResults = AssemblyElements(d);
      var nUnitAssemblies = assemblyResults.Select(ToNUnitAssembly);

      return new SingleRunReport(
        NUnitEnvironment.From(testResultsEnvironment), 
        NUnitCulture.From(testResultsCulture), 
        NUnitResultSummary.From(testResultsSummary), 
        NUnitAssemblies.From(nUnitAssemblies));
    }

    static NUnitAssembly ToNUnitAssembly(XElement r)
    {
      var testResultsForAssembly = new TestResultsForAssembly(r);
      return NUnitAssembly.From(r, testResultsForAssembly);
    }

    static TestResultsEnvironment TestResultsEnvironmentFrom(XDocument d)
    {
      var xElement = d.Element("test-results").Element("environment");
      return new TestResultsEnvironment(xElement);
    }

    static ResultSummary NUnitTestResults(XDocument d)
    {
      var testResultsElement = d.Element("test-results");

      var nUnitTestResults = new ResultSummary(testResultsElement);
      return nUnitTestResults;
    }

    static TestResultsCulture TestResultsCultureFrom(XDocument d)
    {
      var cultureElement = d.Element("test-results").Element("culture-info");
      var testResultsCulture = new TestResultsCulture(cultureElement);
      return testResultsCulture;
    }

    static IEnumerable<XElement> AssemblyElements(XContainer d)
    {
      return d.Descendants().Where(el => 
        el.Name == "test-suite" && el.Attribute("type").Value == "Assembly");
    }
  }
}