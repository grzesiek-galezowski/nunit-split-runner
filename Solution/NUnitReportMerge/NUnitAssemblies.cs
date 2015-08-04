using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class NUnitAssemblies
  {
    IEnumerable<XElement> _assemblyResults;

    public NUnitAssemblies(IEnumerable<XElement> assemblyResults)
    {
      _assemblyResults = assemblyResults;
    }

    public void JoinWith(NUnitAssemblies nUnitAssemblies)
    {
      _assemblyResults = _assemblyResults.Union(nUnitAssemblies._assemblyResults);
    }

    public void AddTo(XElement results)
    {
      var testSuiteBuilder = new TestSuiteBuilder("Success", 0.0, 0);

      foreach (var assemblyResults in _assemblyResults)
      {
        testSuiteBuilder.Add(assemblyResults);
      }

      var projectXml = testSuiteBuilder.Build(_assemblyResults);
      results.Add(projectXml);
    }
  }
}