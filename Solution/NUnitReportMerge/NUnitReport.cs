using System.Collections.Generic;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class NUnitReport
  {
    readonly NUnitResultSummary _summary;
    readonly SingleRunReport _firstRunReport;
    readonly NUnitAssemblies _assemblies;

    public NUnitReport(SingleRunReport firstRunReport, NUnitAssemblies assemblies)
    {
      _summary = firstRunReport.NUnitSummary();
      _firstRunReport = firstRunReport;
      _assemblies = assemblies;
    }

    public XElement Xml()
    {
      var outResultsBuilder = _summary.Builder();
      _firstRunReport.AddCultureAndEnvironmentTo(outResultsBuilder);
      _assemblies.AddTo(outResultsBuilder);
      return outResultsBuilder.Build();
    }

    public void Add(SingleRunReport nextRunReport)
    {
      nextRunReport.AddAssembliesTo(_assemblies);
      _summary.Add(nextRunReport.NUnitSummary());
    }

    public void AnnounceMergeWith(SingleRunReport nextRunReport)
    {
      nextRunReport.AnnounceMergeWith(_summary);
    }
  }
}