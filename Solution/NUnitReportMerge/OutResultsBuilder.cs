using System;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class OutResultsBuilder
  {
    readonly XElement _results;

    public OutResultsBuilder(XElement results)
    {
      _results = results;
    }

    public void AddTestSuite(TestSuiteBuilder testSuiteBuilder)
    {
      _results.Add(testSuiteBuilder.Build());
    }

    public void AddEnvironmentInformation(string nUnitVersion, string clrVersion, string osVersion, string platform, string cwd, string machineName, string user, string userDomain)
    {
      _results.Add(XElement.Parse(XmlCulture.Format(
        "<environment nunit-version=\"{0}\" clr-version=\"{1}\" os-version=\"{2}\" platform=\"{3}\" cwd=\"{4}\" machine-name=\"{5}\" user=\"{6}\" user-domain=\"{7}\" />",
        nUnitVersion,
        clrVersion,
        osVersion,
        platform,
        cwd,
        machineName,
        user,
        userDomain)));
    }

    public void AddCultureInfo(string currentCulture, string currentUiCulture)
    {
      _results.Add(XElement.Parse(XmlCulture.Format(
        "<culture-info current-culture=\"{0}\" current-uiculture=\"{1}\" />",
        currentCulture,
        currentUiCulture)));
    }

    public XElement Build()
    {
      return _results;
    }

    public static OutResultsBuilder New(int total, int errors, int failures, int notRun, int inconclusive, int skipped, int invalid, DateTime dateTime)
    {
      return new OutResultsBuilder(XElement.Parse(XmlCulture.Format(
        "<test-results name=\"Merged results\" total=\"{0}\" errors=\"{1}\" failures=\"{2}\" not-run=\"{3}\" inconclusive=\"{4}\" skipped=\"{5}\" invalid=\"{6}\" date=\"{7}\" time=\"{8}\" />",
        total,
        errors,
        failures,
        notRun,
        inconclusive,
        skipped,
        invalid,
        (dateTime.ToString("yyyy-MM-dd")),
        (dateTime.ToString("HH:mm:ss")))));
    }
  }
}