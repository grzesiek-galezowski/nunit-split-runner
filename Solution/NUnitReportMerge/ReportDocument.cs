using System.Linq;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class ReportDocument
  {
    private readonly XDocument _doc;

    public ReportDocument(XDocument doc)
    {
      _doc = doc;
    }

    public NUnitCulture Culture()
    {
      var cultureElement = _doc.Element("test-results").Element("culture-info");

      var culture = new NUnitCulture
      {
        CurrentCulture = cultureElement.Attribute("current-culture").Value,
        CurrentUiCulture = cultureElement.Attribute("current-uiculture").Value
      };

      return culture;
    }

    public NUnitEnvironment Environment()
    {
      var env = TestResultsEnvironment();
      var actualEnv = new NUnitEnvironment
      {
        NUnitVersion = env.NUnitVersion(),
        ClrVersion = env.ClrVersion(),
        OsVersion = env.OsVersion(),
        Platform = env.PlatformVersion(),
        Cwd = env.Cwd(),
        MachineName = env.MachineName(),
        User = env.User(),
        UserDomain = env.UserDomain()
      };

      return actualEnv;
    }

    public NUnitResultSummary NUnitSummary()
    {
      var nUnitTestResults = Summary();
      
      var resultSummary = new NUnitResultSummary
      {
        //bug shouldn't be 0 everywhere?
        Total = 0,//nUnitTestResults.Total(),
        Errors = 0,//nUnitTestResults.Errors(),
        Failures = 0,//nUnitTestResults.Failures(),
        NotRun = 0,//nUnitTestResults.NotRun(),
        Inconclusive = 0,//nUnitTestResults.Inconclusive(),
        Ignored = 0,//nUnitTestResults.Ignored(),
        Skipped = 0,//nUnitTestResults.Skipped(),
        Invalid = 0,//nUnitTestResults.Invalid(),
        DateTime = nUnitTestResults.DateTimeValue()
      };
      return resultSummary;
    }

    public NUnitAssemblies Assemblies()
    {
      return new NUnitAssemblies(_doc.Descendants().Where(el => el.Name == "test-suite" && el.Attribute("type").Value == "Assembly"));
    }

    private ResultSummary Summary()
    {
      var element = _doc.Element("test-results");

      var nUnitTestResults = new ResultSummary(element);
      return nUnitTestResults;
    }

    private TestResultsEnvironment TestResultsEnvironment()
    {
      return new TestResultsEnvironment(_doc.Element("test-results").Element("environment"));
    }
  }
}