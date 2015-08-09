using System.Collections.Generic;
using System.Linq;
using NUnitReportMerge.Out;

namespace NUnitReportMerge.Model
{
  public class NUnitAssemblies
  {
    IEnumerable<NUnitAssembly> _assemblyResults;

    public NUnitAssemblies(IEnumerable<NUnitAssembly> nUnitAssemblies)
    {
      _assemblyResults = nUnitAssemblies;
    }

    public void JoinWith(NUnitAssemblies nUnitAssemblies)
    {
      _assemblyResults = _assemblyResults.Union(nUnitAssemblies._assemblyResults);
    }

    public void AddTo(OutResultsBuilder results)
    {
      var testSuiteBuilder = new TestSuiteBuilder("Success", 0.0, 0);
      
      foreach (var assemblyResult in _assemblyResults)
      {
        assemblyResult.AddTo(testSuiteBuilder);
      }

      results.AddTestSuite(testSuiteBuilder);
    }

    public static NUnitAssemblies None()
    {
      return new NUnitAssemblies(new List<NUnitAssembly>());
    }

    public static NUnitAssemblies From(IEnumerable<NUnitAssembly> nUnitAssemblies)
    {
      return new NUnitAssemblies(nUnitAssemblies);
    }
  }
}