using System;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  public static class Merge
  {
    private static XElement ApplyTo(NUnitResultSummary summary, NUnitEnvironment nUnitEnvironment, NUnitCulture nUnitCulture, NUnitAssemblies assemblies)
    {
      var results = summary.Xml();
      results.Add(nUnitEnvironment.Xml(), nUnitCulture.Xml(), assemblies.Xml());
      return results;
    }

    public static XElement ApplyTo(Tuple<NUnitResultSummary, NUnitEnvironment, NUnitCulture, NUnitAssemblies> state)
    {
      return ApplyTo(state.Item1, state.Item2, state.Item3, state.Item4);
    }
  }
}