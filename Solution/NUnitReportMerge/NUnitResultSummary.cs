using System;
using System.Linq;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class NUnitResultSummary
  {
    public int Total { get; set; }
    public int Errors { get; set; }
    public int Failures { get; set; }
    public int NotRun { get; set; }
    public int Inconclusive { get; set; }
    public int Ignored { get; set; }
    public int Skipped { get; set; }
    public int Invalid { get; set; }
    public DateTime DateTime { get; set; }

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