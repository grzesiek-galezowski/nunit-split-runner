using System;
using System.Linq;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class NUnitResultSummary
  {
    private int Total { get; set; }
    private int Errors { get; set; }
    private int Failures { get; set; }
    private int NotRun { get; set; }
    private int Inconclusive { get; set; }
    private int Ignored { get; set; }
    private int Skipped { get; set; }
    private int Invalid { get; set; }
    private DateTime DateTime { get; set; }

    public static NUnitResultSummary From(XDocument xdoc)
    {
      var nUnitTestResults = ResultSummary.ExtractFrom(xdoc);
      
      var resultSummary = new NUnitResultSummary
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
      };
      return resultSummary;
    }

    public NUnitResultSummary MergeWith(NUnitResultSummary summary)
    {
      Total += summary.Total;
      Errors += summary.Errors;
      Failures += summary.Failures;
      NotRun += summary.NotRun;
      Inconclusive += summary.Inconclusive;
      Ignored += summary.Ignored;
      Skipped += summary.Skipped;
      Invalid += summary.Invalid;
      DateTime = (new[] {DateTime, summary.DateTime}).Min();

      return this;
    }

    public XElement Xml()
    {
      return XElement.Parse(XmlCulture.Format(
        "<test-results name=\"Merged results\" total=\"{0}\" errors=\"{1}\" failures=\"{2}\" not-run=\"{3}\" inconclusive=\"{4}\" skipped=\"{5}\" invalid=\"{6}\" date=\"{7}\" time=\"{8}\" />",
        Total,
        Errors,
        Failures,
        NotRun,
        Inconclusive,
        Skipped,
        Invalid,
        (DateTime.ToString("yyyy-MM-dd")),
        (DateTime.ToString("HH:mm:ss"))));
    }
  }
}