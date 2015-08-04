using System.Collections.Generic;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class NUnitReport
  {
    readonly NUnitResultSummary _summary;
    readonly NUnitEnvironment _nUnitEnvironment;
    readonly NUnitCulture _nUnitCulture;
    readonly NUnitAssemblies _assemblies;

    public NUnitReport(NUnitResultSummary summary, NUnitEnvironment nUnitEnvironment, NUnitCulture nUnitCulture, NUnitAssemblies assemblies)
    {
      _summary = summary;
      _nUnitEnvironment = nUnitEnvironment;
      _nUnitCulture = nUnitCulture;
      _assemblies = assemblies;
    }

    public XElement MergeAsXml()
    {
      //bug refactor further
      var results = Summary.Xml();
      var outResultsBuilder = new OutResultsBuilder(results);
      NUnitEnvironment.AddTo(results);
      NUnitCulture.AddTo(results);
      Assemblies.AddTo(outResultsBuilder);
      return results;
    }


    NUnitResultSummary Summary
    {
      get { return _summary; }
    }

    NUnitEnvironment NUnitEnvironment
    {
      get { return _nUnitEnvironment; }
    }

    NUnitCulture NUnitCulture
    {
      get { return _nUnitCulture; }
    }

    NUnitAssemblies Assemblies
    {
      get { return _assemblies; }
    }

  }
}