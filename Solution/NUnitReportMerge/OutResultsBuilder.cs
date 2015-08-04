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

    public void Add(TestSuiteBuilder testSuiteBuilder)
    {
      _results.Add(testSuiteBuilder.Build());
    }
  }
}