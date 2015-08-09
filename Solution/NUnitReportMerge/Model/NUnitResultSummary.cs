using System;
using System.Linq;
using NUnitReportMerge.Input;
using NUnitReportMerge.Out;

namespace NUnitReportMerge.Model
{
  public class NUnitResultSummary
  {
    NUnitResultSummary()
    {

    }

    public int Total { get; private set; }
    int Errors { get; set; }
    int Failures { get; set; }
    int NotRun { get; set; }
    int Inconclusive { get; set; }
    int Ignored { get; set; }
    int Skipped { get; set; }
    int Invalid { get; set; }
    DateTime DateTime { get; set; }

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

    public static NUnitResultSummary Empty()
    {
      return new NUnitResultSummary()
      {
        DateTime = DateTime.MaxValue
      };
    }

    public static NUnitResultSummary From(ResultSummary nUnitTestResults)
    {
      return new NUnitResultSummary
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
    }

    public void AddTo(OutResultsBuilder outResultsBuilder)
    {
      outResultsBuilder.AddSummary(Total, Errors, Failures, NotRun, Inconclusive, Skipped, Invalid, DateTime);
    }
  }
}