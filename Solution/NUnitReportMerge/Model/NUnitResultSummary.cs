using System;
using System.Linq;
using NUnitReportMerge.Out;

namespace NUnitReportMerge.Model
{
  public class NUnitResultSummary
  {
    public int Total { get; set; }
    public int Errors { private get; set; }
    public int Failures { private get; set; }
    public int NotRun { private get; set; }
    public int Inconclusive { private get; set; }
    public int Ignored { private get; set; }
    public int Skipped { private get; set; }
    public int Invalid { private get; set; }
    public DateTime DateTime { private get; set; }

    public void Add(NUnitResultSummary summary)
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
    }

    public OutResultsBuilder Builder()
    {
      return OutResultsBuilder.New(Total, Errors, Failures, NotRun, Inconclusive, Skipped, Invalid, DateTime);
    }
  }
}