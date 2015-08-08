using System;
using System.Linq;
using NUnitReportMerge.Model;
using NUnitReportMerge.Out;

namespace NUnitReportMerge
{
  public class SingleRunReport
  {
    readonly NUnitEnvironment _nUnitEnvironment;
    readonly NUnitCulture _nUnitCulture;
    readonly NUnitResultSummary _nUnitResultSummary;
    readonly NUnitAssemblies _nUnitAssemblies;

    public SingleRunReport(
      NUnitEnvironment nUnitEnvironment, 
      NUnitCulture nUnitCulture, 
      NUnitResultSummary nUnitResultSummary, 
      NUnitAssemblies nUnitAssemblies)
    {
      _nUnitEnvironment = nUnitEnvironment;
      _nUnitCulture = nUnitCulture;
      _nUnitResultSummary = nUnitResultSummary;
      _nUnitAssemblies = nUnitAssemblies;
    }

    public NUnitResultSummary NUnitSummary()
    {
      return _nUnitResultSummary;
    }

    public void AnnounceMergeWith(NUnitResultSummary resultSummary)
    {
      Console.WriteLine(
        "Merging " + _nUnitResultSummary.Total +
        " existing tests with  " + resultSummary.Total);
    }

    public void AssertIsFromTheSameRunAs(SingleRunReport firstReport)
    {
      // Sanity check!
      if (WasRunInDifferentEnvironmentThan(firstReport) || WasRunInDifferentCultureThan(firstReport))
      {
        Console.WriteLine(
          "Unmatched environment and/or cultures detected: some of theses results files are not from the same test run.");
      }
    }

    public void AddAssembliesTo(NUnitAssemblies nUnitAssemblies)
    {
      nUnitAssemblies.JoinWith(_nUnitAssemblies);
    }

    public void AddCultureAndEnvironmentInfoTo(OutResultsBuilder outResultsBuilder)
    {
      _nUnitEnvironment.AddTo(outResultsBuilder);
      _nUnitCulture.AddTo(outResultsBuilder);
    }

    bool WasRunInDifferentCultureThan(SingleRunReport firstReport)
    {
      return firstReport._nUnitCulture != _nUnitCulture;
    }

    bool WasRunInDifferentEnvironmentThan(SingleRunReport firstReport)
    {
      return firstReport._nUnitEnvironment != _nUnitEnvironment;
    }


  }
}