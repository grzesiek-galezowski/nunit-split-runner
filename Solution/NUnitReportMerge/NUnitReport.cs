using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NUnitReportMerge.Model;
using NUnitReportMerge.Out;

namespace NUnitReportMerge
{
  public class NUnitReport
  {
    readonly NUnitResultSummary _summary;
    readonly SingleRunReport _firstRunReport;
    readonly NUnitAssemblies _resultsForEachAssembly;

    public NUnitReport(SingleRunReport firstRunReport, NUnitAssemblies resultsForEachAssembly)
    {
      _summary = NUnitResultSummary.Empty();
      _firstRunReport = firstRunReport;
      _resultsForEachAssembly = resultsForEachAssembly;
    }

    public void Xml(OutResultsBuilder builder)
    {
      _summary.AddTo(builder);
      _firstRunReport.AddCultureAndEnvironmentInfoTo(builder);
      _resultsForEachAssembly.AddTo(builder);
    }

    public void Add(SingleRunReport nextRunReport)
    {
      nextRunReport.AddAssembliesTo(_resultsForEachAssembly);
      _summary.Add(nextRunReport.NUnitSummary());
    }

    public void AnnounceMergeWith(SingleRunReport nextRunReport)
    {
      nextRunReport.AnnounceMergeWith(_summary);
    }

    public static NUnitReport FullReport(SingleRunReport firstRunReport)
    {
      return new NUnitReport(
        firstRunReport, NUnitAssemblies.None());
    }

    public void AssertIsFromTheSameRunAs(SingleRunReport nextRunReport)
    {
      nextRunReport.AssertIsFromTheSameRunAs(_firstRunReport);
    }
  }
}